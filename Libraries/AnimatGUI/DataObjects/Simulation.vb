Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects

    Public Class Simulation
        Inherits Framework.DataObject

#Region " Enums "

        Public Enum enumVisualSelectionMode
            SelectGraphics = 1
            SelectCollisions = 2
            SelectJoints = 4
            SelectReceptiveFields = 8
            Simulation = 18   '16 + 2 = 0001 0010
        End Enum

#End Region

#Region " Attributes "

        Protected m_iUpdateDataInterval As Integer = 250
        Protected m_bStartPaused As Boolean = False
        Protected m_bUseReleaseLibraries As Boolean = True
        Protected m_bEnableSimRecording As Boolean = True

        Protected m_doEnvironment As New DataObjects.Physical.Environment(Me)

        Protected m_aryToolHolders As New Collections.SortedToolHolders(Me)
        Protected m_iNewToolHolderIndex As Integer = 0

        Protected m_aryProjectStimuli As New Collections.SortedStimuli(Me)
        Protected m_iNewStimuliIndex As Integer = 0

        Protected m_tnToolViewers As Crownwood.DotNetMagic.Controls.Node
        Protected m_tnExternalStimuli As Crownwood.DotNetMagic.Controls.Node
        Protected m_aryHudItems As New Collections.SortedHudItems(Me)

        Protected m_strFilePath As String = ""
        Protected m_strFileName As String = ""

        Protected m_nodePlaybackControl As Crownwood.DotNetMagic.Controls.Node

        Protected m_strExternalStimuli As String = ""

        Protected m_bSetSimEnd As Boolean = False
        Protected m_snSimEndTime As ScaledNumber
        Protected m_bSimulationAtEndTime As Boolean = False

        Protected m_strAPI_File As String = ""

        Protected m_eVisualSelectionMode As enumVisualSelectionMode
        Protected m_bAddBodiesMode As Boolean = False

#End Region

#Region " Properties "

        Public ReadOnly Property AnimatModule() As String
            Get
#If Not Debug Then
                Return "VortexAnimatSim_VC10.dll"
#Else
                Return "VortexAnimatSim_VC10D.dll"
#End If
            End Get
        End Property

        <Browsable(False)> _
         Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Simulation.gif"
            End Get
        End Property

        Public Property UpdateDataInterval() As Integer
            Get
                Return m_iUpdateDataInterval
            End Get
            Set(ByVal Value As Integer)
                If Value <= 0 Then
                    Throw New System.Exception("You can not set the update data interval to be less than or equal to zero!")
                End If

                m_iUpdateDataInterval = Value
            End Set
        End Property

        Public Property StartPaused() As Boolean
            Get
                Return m_bStartPaused
            End Get
            Set(ByVal Value As Boolean)
                m_bStartPaused = Value
            End Set
        End Property

        Public Property EnableSimRecording() As Boolean
            Get
                Return m_bEnableSimRecording
            End Get
            Set(ByVal Value As Boolean)
                m_bEnableSimRecording = Value
            End Set
        End Property

        Public Overridable Property LogLevel() As AnimatGUI.Interfaces.Logger.enumLogLevel
            Get
                Return Util.Logger.TraceLevel
            End Get
            Set(ByVal Value As AnimatGUI.Interfaces.Logger.enumLogLevel)
                Util.Logger.TraceLevel = Value
            End Set
        End Property

        Public Overridable ReadOnly Property Environment() As DataObjects.Physical.Environment
            Get
                Return m_doEnvironment
            End Get
        End Property

        Public Overridable ReadOnly Property ToolHolders() As Collections.SortedToolHolders
            Get
                Return m_aryToolHolders
            End Get
        End Property

        Public Overridable Property NewToolHolderIndex() As Integer
            Get
                Return m_iNewToolHolderIndex
            End Get
            Set(ByVal Value As Integer)
                m_iNewToolHolderIndex = Value
            End Set
        End Property

        Public Overridable ReadOnly Property ProjectStimuli() As Collections.SortedStimuli
            Get
                Return m_aryProjectStimuli
            End Get
        End Property

        Public Overridable Property NewStimuliIndex() As Integer
            Get
                Return m_iNewStimuliIndex
            End Get
            Set(ByVal Value As Integer)
                m_iNewStimuliIndex = Value
            End Set
        End Property

        Public ReadOnly Property ToolViewersTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnToolViewers
            End Get
        End Property

        Public ReadOnly Property StimuliTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnExternalStimuli
            End Get
        End Property

        Public ReadOnly Property PlaybackControlTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_nodePlaybackControl
            End Get
        End Property

        Public Overridable Property SetSimulationEnd() As Boolean
            Get
                Return m_bSetSimEnd
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("SetEndSimTime", Value.ToString, True)

                m_bSetSimEnd = Value

                'Refresh the property grid 
                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property SimulationEndTime() As ScaledNumber
            Get
                Return m_snSimEndTime
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The simulation end time must be greater than 0.")
                End If

                SetSimData("EndSimTime", Value.ActualValue.ToString, True)

                m_snSimEndTime.CopyData(Value)
            End Set
        End Property

        ''' \brief  When the simulation end time is set to true then the simulation should end after
        ''' 		the user defined time period. While the simulation is processing it checks these values
        ''' 		and when it reaches the correct time it fires a NeedToStopSimulation event. This event
        ''' 		then sets this boolean to true. Each time the SimulationController timer is called it checks
        ''' 		this value. If it is true then it stops the simulation, and resets the value. This was done this
        ''' 		way because it is not possible to call StopSimulation directly from the NeedToStopSimulation event
        ''' 		because the simulation must be running. This creates a blocking event, locking the system up. We need
        ''' 		for the event code to continue on about its business and let the simulation loop keep running, and then
        ''' 		later this thread can call StopSimulation.
        '''
        ''' \value  .
        Public Overridable Property SimulationAtEndTime() As Boolean
            Get
                Return m_bSimulationAtEndTime
            End Get
            Set(ByVal value As Boolean)
                m_bSimulationAtEndTime = value
            End Set
        End Property

        Public Overridable Property APIFile() As String
            Get
                Return m_strAPI_File
            End Get
            Set(ByVal Value As String)
                m_strAPI_File = Value
            End Set
        End Property

        Public Overridable Property VisualSelectionMode() As enumVisualSelectionMode
            Get
                Return m_eVisualSelectionMode
            End Get
            Set(ByVal value As enumVisualSelectionMode)
                If value <> m_eVisualSelectionMode Then
                    Me.SetSimData("VisualSelectionMode", CType(value, Integer).ToString, True)

                    Dim ePrevMode As enumVisualSelectionMode = m_eVisualSelectionMode

                    m_eVisualSelectionMode = value
                    SetVisualSelectionMode()

                    'If we are switching modes then anything selected must be in the previous mode, and 
                    'thus it should not be selected now in the current mode. So lets clear out any selected items.
                    DeselectBodyPartSelections(ePrevMode)
                End If
            End Set
        End Property

        Public Overridable Property AddBodiesMode() As Boolean
            Get
                Return m_bAddBodiesMode
            End Get
            Set(ByVal value As Boolean)
                Me.SetSimData("AddBodiesMode", value.ToString, True)
                m_bAddBodiesMode = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New()
            MyBase.New(Nothing)

            m_strName = "Simulation"
            m_strID = "Simulator"
            m_snSimEndTime = New AnimatGUI.Framework.ScaledNumber(Me, "SimEndTime", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")

            'These events are called when the simulation is starting or resuming so that we can initialize certain objects like stimuli and data charts.
            AddHandler Util.Application.SimulationStarting, AddressOf Me.OnSimulationStarting
            AddHandler Util.Application.SimulationResuming, AddressOf Me.OnSimulationResuming
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "Simulation"
            m_strID = "Simulator"
            m_snSimEndTime = New AnimatGUI.Framework.ScaledNumber(Me, "SimEndTime", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")

            'These events are called when the simulation is starting or resuming so that we can initialize certain objects like stimuli and data charts.
            AddHandler Util.Application.SimulationStarting, AddressOf Me.OnSimulationStarting
            AddHandler Util.Application.SimulationResuming, AddressOf Me.OnSimulationResuming
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            'Dim oSim As New Simulation(doParent)
            Return Nothing
        End Function

        Public Overridable Overloads Function CreateObject(ByRef oXml As Interfaces.StdXml, ByVal strClassType As String, _
                                                           ByVal doParent As AnimatGUI.Framework.DataObject) As Framework.DataObject
            oXml.IntoElem()

            'If we have a partype listed then it gives the complete assemblyname and classname that we 
            'can use to load with the Util.LoadClass function. Otherwise load things the old, non-modular way.
            If oXml.FindChildElement("PartType", False) Then
                Dim strClass As String = oXml.GetChildString("PartType")
                Dim aryClassName() As String = Split(strClass, ".")
                Dim strAssembly As String = aryClassName(0)
                oXml.OutOfElem()

                Dim doObject As Framework.DataObject = DirectCast(Util.LoadClass(strAssembly, strClass, doParent), Framework.DataObject)
                Return doObject
            End If

        End Function

        Public Overridable Overloads Function CreateObject(ByVal strClassType As String, _
                                                           ByVal doParent As AnimatGUI.Framework.DataObject) As Framework.DataObject

            Dim aryClassName() As String = Split(strClassType, ".")
            Dim strAssembly As String = aryClassName(0)
            Dim doObject As Framework.DataObject = DirectCast(Util.LoadClass(strAssembly, strClassType, doParent), Framework.DataObject)
            Return doObject
        End Function

        Public Overridable Overloads Function CreateForm(ByVal strClassType As String, _
                                                        ByVal doParent As AnimatGUI.Framework.DataObject) As Forms.AnimatForm

            Dim aryClassName() As String = Split(strClassType, ".")
            Dim strAssembly As String = aryClassName(0)
            Dim frmObject As Forms.AnimatForm = DirectCast(Util.LoadClass(strAssembly, strClassType, doParent), Forms.AnimatForm)
            Return frmObject
        End Function

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)

            Util.Application.WorkspaceImages.ImageList.ImageSize = New Size(25, 25)
            Util.ProjectWorkspace.TreeView.ImageList = Util.Application.WorkspaceImages.ImageList

            MyBase.CreateWorkspaceTreeView(doParent, doParentNode)

            m_doEnvironment.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

            'Util.Application.WorkspaceImages.AddImage(myAssembly, "AnimatGUI.DefaultObject.gif")
            'Util.Application.WorkspaceImages.AddImage(myAssembly, "AnimatGUI.Simulation.gif")
            'Util.Application.WorkspaceImages.AddImage(myAssembly, "AnimatGUI.RemoteControl.gif")
            'Util.Application.WorkspaceImages.AddImage(myAssembly, "AnimatGUI.Toolbox.gif")
            'Util.Application.WorkspaceImages.AddImage(myAssembly, "AnimatGUI.ExternalStimulus.gif")

            m_tnToolViewers = Util.ProjectWorkspace.AddTreeNode(Nothing, "Tool Viewers", "AnimatGUI.Toolbox.gif")
            m_tnExternalStimuli = Util.ProjectWorkspace.AddTreeNode(Nothing, "Stimuli", "AnimatGUI.ExternalStimulus.gif")
            m_nodePlaybackControl = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Playback Control", "AnimatGUI.RemoteControl.gif")
            m_nodePlaybackControl.Tag = Util.Application.SimulationController

            Dim doTool As DataObjects.ToolHolder
            For Each deEntry As DictionaryEntry In Util.Simulation.ToolHolders
                doTool = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                doTool.CreateWorkspaceTreeView(Me, m_tnToolViewers)
            Next

            Dim doStimulus As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In Util.Simulation.ProjectStimuli
                doStimulus = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                doStimulus.CreateWorkspaceTreeView(Me, m_tnExternalStimuli)
            Next

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean
            If m_doEnvironment.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then Return True

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Simulation.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            If tnSelectedNode Is m_tnToolViewers Then
                CreateToolViewerPopupMenu(tnSelectedNode, ptPoint)
                Return True
            End If

            If tnSelectedNode Is m_tnExternalStimuli Then
                CreateStimuliPopupMenu(tnSelectedNode, ptPoint)
                Return True
            End If

            Dim doHolder As DataObjects.ToolHolder
            For Each deEntry As DictionaryEntry In Util.Simulation.ToolHolders
                doHolder = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                If doHolder.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                    Return True
                End If
            Next

            Dim doStimulus As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In Util.Simulation.ProjectStimuli
                doStimulus = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                If doStimulus.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                    Return True
                End If
            Next

            Return False
        End Function

        Protected Overridable Sub CreateToolViewerPopupMenu(ByVal tnSelected As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point)

            Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Simulation.CreateToolViewerPopupMenu", Util.SecurityMgr)

            If Util.Application.ToolPlugins.Count > 0 Then
                Dim mcAddTool As New System.Windows.Forms.ToolStripMenuItem("Add Data Tool", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddDataTool.gif"))

                For Each doTool As Forms.Tools.ToolForm In Util.Application.ToolPlugins
                    Dim mcTool As New System.Windows.Forms.ToolStripMenuItem(doTool.Name, doTool.TabImage, New EventHandler(AddressOf Me.OnAddToolViewer))
                    mcAddTool.DropDown.Items.Add(mcTool)
                Next

                popup.Items.Add(mcAddTool)
            End If

            Dim mcSepExpand As New ToolStripSeparator()
            Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
            Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

            mcExpandAll.Tag = tnSelected
            mcCollapseAll.Tag = tnSelected

            ' Create the popup menu object
            popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcSepExpand, mcExpandAll, mcCollapseAll})

            Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

            Return

        End Sub

        Protected Overridable Sub CreateStimuliPopupMenu(ByVal tnSelected As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point)

            Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
            Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

            mcExpandAll.Tag = tnSelected
            mcCollapseAll.Tag = tnSelected

            ' Create the popup menu object
            Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Simulation.CreateStimuliPopupMenu", Util.SecurityMgr)
            popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll})

            Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

            Return

        End Sub


        'This will deselect all body part items. This is used when switching between visual selection modes.
        Protected Sub DeselectBodyPartSelections(ByVal ePrevMode As enumVisualSelectionMode)

            If Not Util.ProjectWorkspace Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView.SelectedNodes Is Nothing Then
                Dim doPart As AnimatGUI.DataObjects.Physical.BodyPart
                Dim aryItems As New ArrayList()
                For Each tvItem As Crownwood.DotNetMagic.Controls.Node In Util.ProjectWorkspace.TreeView.SelectedNodes
                    If Not tvItem.Tag Is Nothing Then
                        If Util.IsTypeOf(tvItem.Tag.GetType, GetType(AnimatGUI.DataObjects.Physical.BodyPart), False) Then
                            doPart = DirectCast(tvItem.Tag, AnimatGUI.DataObjects.Physical.BodyPart)

                            'If the selection mode does not match the current mode then deselect it.
                            If doPart.DefaultVisualSelectionMode <> Me.VisualSelectionMode OrElse ePrevMode = enumVisualSelectionMode.SelectReceptiveFields Then
                                aryItems.Add(doPart)
                            End If
                        End If
                    End If
                Next

                For Each doPart In aryItems
                    doPart.DeselectItem()
                Next
            End If

        End Sub

        Protected Sub SetVisualSelectionMode()

            If m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectCollisions Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = True
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = True
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectGraphics Then
                Util.Application.SelGraphicsToolStripButton.Checked = True
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = True
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectJoints Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = True
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = True
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectReceptiveFields Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = True
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = True
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.Simulation Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = True

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = True

            End If

            If Util.Application.AddPartToolStripButton.Checked AndAlso Util.Simulation.VisualSelectionMode <> Simulation.enumVisualSelectionMode.SelectCollisions Then
                Util.Application.AddPartToolStripButton.Checked = False
            End If

            RaiseEvent VisualSelectionModeChanged()
        End Sub

        ''' \brief  Enables/Disables the visual selection modes while the simulation is running.
        ''' 		
        ''' \details When the simulation is running then <b>ONLY</b> the simulation mode is available.
        ''' 		 You cannot edit or move the other parts. You must pause the simulation if you want to do that.
        ''' 		 This method ensures that only the simulation mode can be selected.
        '''
        ''' \author dcofer
        ''' \date   3/27/2011
        '''
        ''' \param  bSimStarting    true to simulation starting. 
        Public Sub SetVisualSelectionModeForSimStarting(ByVal bSimStarting As Boolean)

            If bSimStarting Then
                Util.Application.SelGraphicsToolStripButton.Enabled = False
                Util.Application.SelCollisionToolStripButton.Enabled = False
                Util.Application.SelJointsToolStripButton.Enabled = False
                Util.Application.SelRecFieldsToolStripButton.Enabled = False
                Util.Application.SelSimToolStripButton.Enabled = True

                Util.Application.GraphicsObjectsToolStripMenuItem.Enabled = False
                Util.Application.CollisionObjectsToolStripMenuItem.Enabled = False
                Util.Application.JointsToolStripMenuItem.Enabled = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Enabled = False
                Util.Application.SimulationToolStripMenuItem.Enabled = True

                Me.VisualSelectionMode = enumVisualSelectionMode.Simulation
            Else
                Util.Application.SelGraphicsToolStripButton.Enabled = True
                Util.Application.SelCollisionToolStripButton.Enabled = True
                Util.Application.SelJointsToolStripButton.Enabled = True
                Util.Application.SelRecFieldsToolStripButton.Enabled = True
                Util.Application.SelSimToolStripButton.Enabled = True

                Util.Application.GraphicsObjectsToolStripMenuItem.Enabled = True
                Util.Application.CollisionObjectsToolStripMenuItem.Enabled = True
                Util.Application.JointsToolStripMenuItem.Enabled = True
                Util.Application.ReceptiveFieldsToolStripMenuItem.Enabled = True
                Util.Application.SimulationToolStripMenuItem.Enabled = True
            End If

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Animat Module", m_strAnimatModule.GetType(), "AnimatModule", _
            '                            "Simulation Modules", "This determines the animat plug-in module that will be used throughout the simulation. " & _
            '                            "This plug-in will control the physics portion of the simulation.", m_strAnimatModule))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Use Release Libraries", m_bUseReleaseLibraries.GetType(), "UseReleaseLibraries", _
            '                            "Playback Control", "Determines if the debug or release libraries should be used for running the simulator.", m_bUseReleaseLibraries))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Settings", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("API File", m_strAPI_File.GetType(), "APIFile", _
                                        "Settings", "APIFile", m_strAPI_File))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Log Level", GetType(AnimatGUI.Interfaces.Logger.enumLogLevel), "LogLevel", _
                                        "Logging", "Sets the level of logging in the application.", Me.LogLevel))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Set Sim To End", m_bSetSimEnd.GetType(), "SetSimulationEnd", _
                                        "Playback Control", "If this is true then the simulation will automatically end at the Sim End Time.", m_bSetSimEnd))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snSimEndTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Sim End Time", pbNumberBag.GetType(), "SimulationEndTime", _
                                        "Playback Control", "Sets the time at which the simulation will end if the SetSimEnd property is true.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), Not m_bSetSimEnd))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            m_doEnvironment.ClearIsDirty()
            m_aryToolHolders.ClearIsDirty()
            m_aryProjectStimuli.ClearIsDirty()
            m_aryHudItems.ClearIsDirty()
        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)
            m_doEnvironment.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)

            Dim doStim As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me.ProjectStimuli
                doStim = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doStim.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next
        End Sub

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.IntoChildElement("Simulation")

            Me.UpdateDataInterval = oXml.GetChildInt("UpdateDataInterval", m_iUpdateDataInterval)
            Me.StartPaused = oXml.GetChildBool("StartPaused", m_bStartPaused)
            'Me.UseReleaseLibraries = oXml.GetChildBool("UseReleaseLibraries", m_bUseReleaseLibraries)
            Me.EnableSimRecording = oXml.GetChildBool("EnableSimRecording", m_bEnableSimRecording)
            'Me.AnimatModule = oXml.GetChildString("AnimatModule", m_strAnimatModule)
            m_strExternalStimuli = oXml.GetChildString("ExternalStimuli", "")
            m_strAPI_File = oXml.GetChildString("APIFile", "")

            If oXml.FindChildElement("SetSimEnd", False) Then
                m_bSetSimEnd = oXml.GetChildBool("SetSimEnd", m_bSetSimEnd)
                m_snSimEndTime.LoadData(oXml, "SimEndTime")
            End If

            m_doEnvironment.LoadData(oXml)

            LoadToolHolders(oXml)
            LoadStimuli(oXml)
            LoadHudItems(oXml)

            oXml.OutOfElem()

        End Sub

        Protected Overridable Sub LoadStimuli(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                Dim doStim As DataObjects.ExternalStimuli.Stimulus
                If oXml.FindChildElement("Stimuli", False) Then
                    oXml.IntoChildElement("Stimuli")
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        doStim = DirectCast(Util.LoadClass(oXml, iIndex, Me), DataObjects.ExternalStimuli.Stimulus)
                        doStim.LoadData(oXml)

                        'Add the stimulus to the projects stim list, but since we are just loading it in here then override
                        'the call to the before/after add methods.
                        If Not doStim.StimulusNoLongerValid Then
                            m_aryProjectStimuli.Add(doStim.ID, doStim, False)
                        End If
                    Next

                    oXml.OutOfElem()
                End If

            Catch ex As System.Exception
                'If we hit an erorr then it most likely happened while adding or loading the stimulus.
                'If that is the case then we need to step out of the two elements we stepped into.
                oXml.OutOfElem()
                oXml.OutOfElem()
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub LoadToolHolders(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                Dim iCount As Integer
                Dim doTool As DataObjects.ToolHolder
                If oXml.FindChildElement("ToolViewers", False) Then
                    oXml.IntoChildElement("ToolViewers")

                    iCount = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)
                        doTool = New DataObjects.ToolHolder(Me)
                        doTool.LoadData(oXml)
                        m_aryToolHolders.Add(doTool.ID, doTool)
                    Next

                    oXml.OutOfElem()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub LoadHudItems(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                'Dim iCount As Integer
                'If oXml.FindChildElement("HudItems", False) Then
                '    oXml.IntoChildElement("HudItems")

                '    iCount = oXml.NumberOfChildren() - 1
                '    For iIndex As Integer = 0 To iCount
                '        oXml.FindChildByIndex(iIndex)
                '        doTool = New DataObjects.ToolHolder(Me)
                '        doTool.LoadData(oXml)
                '        m_aryToolHolders.Add(doTool.ID, doTool)
                '    Next

                '    oXml.OutOfElem()
                'Else
                'Add any initialization after the InitializeComponent() call
                'Add a new hud item to display the time and the axis by default
                'TODO: We need a Hud manager code in the GUI. For now just hard code it to have a Hud Text Item.
                m_aryHudItems.Clear()
                Dim doHudItem As New DataObjects.Visualization.HudItem(Me, "HudText", Color.White, New System.Drawing.Point(10, 10), 30, "Time: %3.3f", "Simulator", "Time")
                m_aryHudItems.Add(doHudItem.ID, doHudItem)
                'End If


            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("Simulation")
            oXml.IntoElem()

            oXml.AddChildElement("ProjectPath", Util.Application.ProjectPath)
            oXml.AddChildElement("AnimatModule", Me.AnimatModule)
            oXml.AddChildElement("UpdateDataInterval", m_iUpdateDataInterval)
            oXml.AddChildElement("StartPaused", m_bStartPaused)
            oXml.AddChildElement("EnableSimRecording", m_bEnableSimRecording)
            oXml.AddChildElement("SetSimEnd", m_bSetSimEnd)
            oXml.AddChildElement("APIFile", m_strAPI_File)
            m_snSimEndTime.SaveData(oXml, "SimEndTime")

            m_doEnvironment.SaveData(oXml)

            oXml.AddChildElement("ToolViewers")
            oXml.IntoElem()

            Dim doTool As DataObjects.ToolHolder
            For Each deEntry As DictionaryEntry In m_aryToolHolders
                doTool = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                doTool.SaveData(oXml)
            Next

            oXml.OutOfElem()   'Outof ToolViewers element

            oXml.AddChildElement("Stimuli")
            oXml.IntoElem()

            Dim aryStimsToDelete As New Collection
            Dim doStim As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                If doStim.StimulusNoLongerValid Then
                    aryStimsToDelete.Add(doStim)
                Else
                    doStim.SaveData(oXml)
                End If
            Next

            oXml.OutOfElem()   'Outof Stimuli element

            oXml.AddChildElement("HudItems")
            oXml.IntoElem()   'Into Hud Items element

            Dim hudItem As DataObjects.Visualization.HudItem
            For Each deItem As DictionaryEntry In m_aryHudItems
                hudItem = DirectCast(deItem.Value, DataObjects.Visualization.HudItem)
                hudItem.SaveData(oXml)
            Next

            oXml.OutOfElem()    'Outof Hud Items element


            oXml.OutOfElem() ' OutOfElem Simulation

            'Now lets delete any stims that are no longer valid
            For Each doStim In aryStimsToDelete
                m_aryProjectStimuli.Remove(doStim.ID)
                doStim.RemoveWorksapceTreeView()
            Next
        End Sub

        Public Overloads Sub SaveSimulationXml(ByVal strFilename As String)

            Dim oXml As New AnimatGUI.Interfaces.StdXml

            SaveSimulationXml(oXml, Nothing)

            oXml.Save(strFilename)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddElement("Simulation")

            'If it is standalone then do not save the project path. This is typically used for grid applicaitons, 
            'and the project path needs to be the current directory the exe is located within.
            If Not Util.ExportForStandAloneSim Then
                oXml.AddChildElement("ProjectPath", Util.Application.ProjectPath)
            Else
                oXml.AddChildElement("ProjectPath", "")
            End If

            oXml.AddChildElement("AnimatModule", Me.AnimatModule)
            oXml.AddChildElement("UpdateDataInterval", m_iUpdateDataInterval)
            oXml.AddChildElement("StartPaused", m_bStartPaused)
            oXml.AddChildElement("EnableSimRecording", m_bEnableSimRecording)
            oXml.AddChildElement("SetSimEnd", m_bSetSimEnd)
            oXml.AddChildElement("APIFile", m_strAPI_File)
            m_snSimEndTime.SaveSimulationXml(oXml, Me, "SimEndTime")

            m_doEnvironment.SaveSimulationXml(oXml)

            Dim doStim As DataObjects.ExternalStimuli.Stimulus

            oXml.AddChildElement("ExternalStimuli")
            oXml.IntoElem()

            For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                doStim.SaveSimulationXml(oXml)
            Next

            oXml.OutOfElem()


            oXml.AddChildElement("DataCharts")
            oXml.IntoElem()
            For Each frmWindow As System.Windows.Forms.Form In Util.Application.ChildForms
                If TypeOf frmWindow Is Forms.Tools.ToolForm Then
                    Dim frmViewer As Forms.Tools.ToolForm = DirectCast(frmWindow, Forms.Tools.ToolForm)
                    frmViewer.SaveSimulationXml(oXml)
                End If
            Next
            oXml.OutOfElem()

            oXml.AddChildElement("WindowMgr")
            oXml.IntoElem()   'Into WindowMgr element

            oXml.AddChildElement("Hud")
            oXml.IntoElem()   'Into Hud element

            oXml.AddChildElement("Type", "Hud")

            oXml.AddChildElement("HudItems")
            oXml.IntoElem()   'Into Hud Items element

            For Each deItem As DictionaryEntry In m_aryHudItems
                Dim hudItem As DataObjects.Visualization.HudItem = DirectCast(deItem.Value, DataObjects.Visualization.HudItem)
                hudItem.SaveSimulationXml(oXml)
            Next

            oXml.OutOfElem()    'Outof Hud Items element
            oXml.OutOfElem()    'Outof Hud element
            oXml.OutOfElem()    'Outof WindowMgr element

            oXml.OutOfElem()
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            m_doEnvironment.InitializeAfterLoad()

            Dim doStim As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                doStim.InitializeAfterLoad()
            Next

        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            If Me.IsInitialized Then
                MyBase.InitializeSimulationReferences()

                m_doEnvironment.InitializeSimulationReferences()

                Dim doStim As DataObjects.ExternalStimuli.Stimulus
                For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                    doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                    doStim.InitializeSimulationReferences()
                Next

                For Each frmWindow As System.Windows.Forms.Form In Util.Application.ChildForms
                    If TypeOf frmWindow Is Forms.Tools.ToolForm Then
                        Dim frmViewer As Forms.Tools.ToolForm = DirectCast(frmWindow, Forms.Tools.ToolForm)
                        frmViewer.InitializeSimulationReferences()
                    End If
                Next
            End If
        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doEnvironment Is Nothing Then doObject = m_doEnvironment.FindObjectByID(strID)
            Return doObject

        End Function

        Public Overridable Function FindStimulusByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.ExternalStimuli.Stimulus

            Dim doStim As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In Me.ProjectStimuli
                doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                If doStim.Name = strName Then
                    Return doStim
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No stimulus with the name '" & strName & "' was found.")
            End If

            Return Nothing
        End Function

#End Region

#End Region

#Region " Events "

        Public Event VisualSelectionModeChanged()

        Protected Overridable Sub OnAddToolViewer(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If TypeOf sender Is ToolStripMenuItem Then
                    Dim mcCommand As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)

                    If Not mcCommand.Tag Is Nothing AndAlso TypeOf mcCommand.Tag Is Forms.Tools.ToolForm Then
                        Dim doTool As Forms.Tools.ToolForm = DirectCast(mcCommand.Tag, Forms.Tools.ToolForm)
                        Util.Application.AddNewTool(doTool)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Simulation Events "

        Protected Sub OnSimulationStarting()

            Try
                'Lets setup the external stimuli
                Dim aryDelete As New Collection
                Dim doStim As DataObjects.ExternalStimuli.Stimulus
                For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                    doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                    'If the stimulus is no longer valid then delete it.
                    If doStim.StimulusNoLongerValid Then
                        aryDelete.Add(doStim)
                    End If
                Next

                For Each doStim In aryDelete
                    doStim.Delete(False)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationResuming()

            Try
                ''Lets setup the external stimuli
                'Dim doStim As DataObjects.ExternalStimuli.Stimulus
                'For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                '    doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                '    doStim.PrepareForSimulation()
                'Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#End Region

#End Region

    End Class

End Namespace
