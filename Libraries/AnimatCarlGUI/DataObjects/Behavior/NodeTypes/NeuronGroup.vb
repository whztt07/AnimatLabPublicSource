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

Namespace DataObjects.Behavior.NodeTypes

    Public Class NeuronGroup
        Inherits AnimatGUI.DataObjects.Behavior.Nodes.Neuron

#Region " Attributes "

        Protected m_iNeuronCount As Integer = 10
        Protected m_eNeuralType As enumNeuralType = enumNeuralType.Excitatory

        Protected m_snA As ScaledNumber
        Protected m_snStdA As ScaledNumber
        Protected m_snB As ScaledNumber
        Protected m_snStdB As ScaledNumber
        Protected m_snC As ScaledNumber
        Protected m_snStdC As ScaledNumber
        Protected m_snD As ScaledNumber
        Protected m_snStdD As ScaledNumber

        Protected m_bEnableCOBA As Boolean = True
        Protected m_snTauAMPA As ScaledNumber
        Protected m_snTauNMDA As ScaledNumber
        Protected m_snTauGABAa As ScaledNumber
        Protected m_snTauGABAb As ScaledNumber

        Protected m_bEnableSTP As Boolean = False
        Protected m_snU As ScaledNumber
        Protected m_snTauDepression As ScaledNumber
        Protected m_snTauFacilitation As ScaledNumber

        Protected m_bEnableSTDP As Boolean = False
        Protected m_snMaxLTP As ScaledNumber
        Protected m_snTauLTP As ScaledNumber
        Protected m_snMaxLTD As ScaledNumber
        Protected m_snTauLTD As ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Neuron Group"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatCarlGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable ReadOnly Property NeuronType() As String
            Get
                Return "NeuronGroup"
            End Get
        End Property

        Public Overrides ReadOnly Property DataColumnModuleName As String
            Get
                Return "AnimatCarlSimCUDA"
            End Get
        End Property

        Public Overrides ReadOnly Property DataColumnClassType As String
            Get
                Return "NeuronDataColumn"
            End Get
        End Property

        Public Overrides ReadOnly Property AllowDataColumnSubData As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property DataColumnSubDataName As String
            Get
                Return "Neuron ID"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return Me.NeuronType
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatCarlGUI.NeuronGroup.gif"
            End Get
        End Property

        Public Overridable Property NeuronCount() As Integer
            Get
                Return m_iNeuronCount
            End Get
            Set(value As Integer)
                If value <= 0 Then
                    Throw New System.Exception("The neuron count cannot be zero or less.")
                End If
                SetSimData("NeuronCount", value.ToString(), True)
                m_iNeuronCount = value
            End Set
        End Property

        Public Overridable Property NeuralType() As enumNeuralType
            Get
                Return m_eNeuralType
            End Get
            Set(value As enumNeuralType)
                SetSimData("NeuralType", value.ToString(), True)
                m_eNeuralType = value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property A() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snA
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("A", Value.ActualValue.ToString, True)
                m_snA.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StdA() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStdA
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The standard deviation of A must be greater than zero.")
                End If

                SetSimData("StdA", Value.ActualValue.ToString, True)
                m_snStdA.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property B() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snB
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("B", Value.ActualValue.ToString, True)
                m_snB.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StdB() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStdB
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The standard deviation of B must be greater than zero.")
                End If

                SetSimData("StdB", Value.ActualValue.ToString, True)
                m_snStdB.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property C() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snC
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("C", Value.ActualValue.ToString, True)
                m_snC.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StdC() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStdC
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The standard deviation of C must be greater than zero.")
                End If

                SetSimData("StdC", Value.ActualValue.ToString, True)
                m_snStdC.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property D() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snD
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("D", Value.ActualValue.ToString, True)
                m_snD.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StdD() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStdD
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The standard deviation of D must be greater than zero.")
                End If

                SetSimData("StdD", Value.ActualValue.ToString, True)
                m_snStdD.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EnableCOBA() As Boolean
            Get
                Return m_bEnableCOBA
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("EnableCOBA", Value.ToString, True)
                m_bEnableCOBA = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauAMPA() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauAMPA
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The AMPA time constant must be greater than zero.")
                End If

                SetSimData("TauAMPA", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauAMPA.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauNMDA() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauNMDA
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The NMDA time constant must be greater than zero.")
                End If

                SetSimData("TauNMDA", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauNMDA.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauGABAa() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauGABAa
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The GABAa time constant must be greater than zero.")
                End If

                SetSimData("TauGABAa", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauGABAa.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauGABAb() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauGABAb
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The GABAb time constant must be greater than zero.")
                End If

                SetSimData("TauGABAb", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauGABAb.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EnableSTP() As Boolean
            Get
                Return m_bEnableSTP
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("EnableSTP", Value.ToString, True)
                m_bEnableSTP = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property U() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snU
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The U increment must be greater than or equal to zero.")
                End If

                SetSimData("U", Value.ActualValue.ToString, True)
                m_snU.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauDepression() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauDepression
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The depression time constant must be greater than zero.")
                End If

                SetSimData("TauD", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauDepression.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauFacilitation() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauFacilitation
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The facilitation time constant must be greater than zero.")
                End If

                SetSimData("TauF", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauFacilitation.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EnableSTDP() As Boolean
            Get
                Return m_bEnableSTDP
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("EnableSTDP", Value.ToString, True)
                m_bEnableSTDP = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxLTP() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxLTP
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The Maximum LTP change must be greater than or equal to zero.")
                End If

                SetSimData("MaxLTP", Value.ActualValue.ToString, True)
                m_snMaxLTP.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauLTP() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauLTP
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The LTP time constant must be greater than zero.")
                End If

                SetSimData("TauLTP", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauLTP.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxLTD() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxLTD
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The maximum LTD change must be greater than zero.")
                End If

                SetSimData("MaxLTD", Value.ActualValue.ToString, True)
                m_snMaxLTD.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TauLTD() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snTauLTP
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The LTD time constant must be greater than or equal to zero.")
                End If

                SetSimData("TauLTD", CInt(Value.ActualValue * 1000).ToString, True)
                m_snTauLTD.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_bEnabled = True

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.MultiDocument
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.LightBlue

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatCarlGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatCarlGUI.NormalNeuron.gif", False)
                Me.Name = "Neuron Group"

                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Description = "A group of Izhikevich spiking neurons."

                m_snA = New AnimatGUI.Framework.ScaledNumber(Me, "A", 0.02, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snB = New AnimatGUI.Framework.ScaledNumber(Me, "B", 0.2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snC = New AnimatGUI.Framework.ScaledNumber(Me, "C", -65, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snD = New AnimatGUI.Framework.ScaledNumber(Me, "D", 8, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

                m_snStdA = New AnimatGUI.Framework.ScaledNumber(Me, "StdA", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snStdB = New AnimatGUI.Framework.ScaledNumber(Me, "StdB", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snStdC = New AnimatGUI.Framework.ScaledNumber(Me, "StdC", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snStdD = New AnimatGUI.Framework.ScaledNumber(Me, "StdD", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

                m_snTauAMPA = New AnimatGUI.Framework.ScaledNumber(Me, "TauAMPA", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")
                m_snTauNMDA = New AnimatGUI.Framework.ScaledNumber(Me, "TauNMDA", 150, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")
                m_snTauGABAa = New AnimatGUI.Framework.ScaledNumber(Me, "TauGABAa", 6, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")
                m_snTauGABAb = New AnimatGUI.Framework.ScaledNumber(Me, "TauGABAb", 150, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")

                m_snU = New AnimatGUI.Framework.ScaledNumber(Me, "U", 0.2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snTauDepression = New AnimatGUI.Framework.ScaledNumber(Me, "TauDepression", 800, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")
                m_snTauFacilitation = New AnimatGUI.Framework.ScaledNumber(Me, "TauFacilitation", 20, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")

                m_snU = New AnimatGUI.Framework.ScaledNumber(Me, "U", 0.2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snMaxLTP = New AnimatGUI.Framework.ScaledNumber(Me, "MaxLTP", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snTauLTP = New AnimatGUI.Framework.ScaledNumber(Me, "TauLTP", 20, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "ms")
                m_snMaxLTD = New AnimatGUI.Framework.ScaledNumber(Me, "MaxLTD", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snTauLTD = New AnimatGUI.Framework.ScaledNumber(Me, "TauLTD", 20, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "ms")

                AddCompatibleLink(New AnimatGUI.DataObjects.Behavior.Links.Adapter(Nothing))
                AddCompatibleLink(New SynapseTypes.OneToOneSynapse(Nothing))
                AddCompatibleLink(New SynapseTypes.FullSynapse(Nothing))
                AddCompatibleLink(New SynapseTypes.RandomSynapse(Nothing))
                AddCompatibleLink(New SynapseTypes.IndividualSynapse(Nothing))

                'Lets add the data types that this node understands.
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("GroupFiringRate", "Group Firing Rate", "Hertz", "Hz", 0, 100, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("GroupTotalSpikes", "Group Total Spikes", "", "", 0, 1000, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Spike", "Neuron Spike Data", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                m_thDataTypes.ID = "GroupFiringRate"

                m_thIncomingDataTypes.DataTypes.Clear()
                m_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FiringRate", "Firing Rate", "Hertz", "Hz", 0, 100, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                m_thIncomingDataTypes.ID = "FiringRate"

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitAfterAppStart()
            MyBase.InitAfterAppStart()
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New NeuronGroup(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As NeuronGroup = DirectCast(doOriginal, NeuronGroup)

            m_bEnabled = bnOrig.m_bEnabled
            m_snA = DirectCast(bnOrig.m_snA.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snB = DirectCast(bnOrig.m_snB.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snC = DirectCast(bnOrig.m_snC.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snD = DirectCast(bnOrig.m_snD.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_snStdA = DirectCast(bnOrig.m_snStdA.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snStdB = DirectCast(bnOrig.m_snStdB.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snStdC = DirectCast(bnOrig.m_snStdC.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snStdD = DirectCast(bnOrig.m_snStdD.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_bEnableCOBA = bnOrig.m_bEnableCOBA

            m_iNeuronCount = bnOrig.m_iNeuronCount
            m_eNeuralType = bnOrig.m_eNeuralType

            m_snTauAMPA = DirectCast(bnOrig.m_snTauAMPA.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTauNMDA = DirectCast(bnOrig.m_snTauNMDA.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTauGABAa = DirectCast(bnOrig.m_snTauGABAa.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTauGABAb = DirectCast(bnOrig.m_snTauGABAb.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_bEnableSTP = bnOrig.m_bEnableSTP
            m_snU = DirectCast(bnOrig.m_snU.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTauDepression = DirectCast(bnOrig.m_snTauDepression.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTauFacilitation = DirectCast(bnOrig.m_snTauFacilitation.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_bEnableSTDP = bnOrig.m_bEnableSTDP
            m_snMaxLTP = DirectCast(bnOrig.m_snMaxLTP.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTauLTP = DirectCast(bnOrig.m_snTauLTP.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snMaxLTD = DirectCast(bnOrig.m_snMaxLTD.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snTauLTD = DirectCast(bnOrig.m_snTauLTD.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

#Region " DataObject Methods "

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not NeuralModule Is Nothing Then
                NeuralModule.VerifyExistsInSim()
                If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    'If we just created this neuralmodule in the sim then this object might already exist now. We should only add it if it does not exist.
                    Util.Application.SimulationInterface.AddItem(NeuralModule.ID(), "NeuronGroup", Me.ID, Me.GetSimulationXml("NeuronGroup"), bThrowError, bDoNotInit)
                End If
            End If
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not NeuralModule Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(NeuralModule.ID(), "NeuronGroup", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'First lets remove the 'Text' property for node base classs
            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Node Type") Then propTable.Properties.Remove("Node Type")
            If propTable.Properties.Contains("Description") Then propTable.Properties.Remove("Description")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strText.GetType(), "Text", _
                                        "Neural Properties", "Sets the name of this neuron.", m_strText, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Neuron Type", GetType(String), "TypeName", _
                                        "Neural Properties", "Returns the type of this neuron.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Neural Type", m_eNeuralType.GetType(), "NeuralType", _
                                        "Neural Properties", "Determines if this is an excitatory or inhibitory neuron.", m_eNeuralType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Neuron Count", m_iNeuronCount.GetType(), "NeuronCount", _
                                        "Neural Properties", "Determines the number of neurons in this population.", m_iNeuronCount))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        "Neural Properties", "Sets the description for this neuron.", m_strToolTip, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Neural Properties", "Determines if this neuron is enabled or not.", m_bEnabled))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snA.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("A", pbNumberBag.GetType(), "A", _
                                        "Neural Properties", "Izhikevich A property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snB.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("B", pbNumberBag.GetType(), "B", _
                                        "Neural Properties", "Izhikevich B property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snC.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("C", pbNumberBag.GetType(), "C", _
                                        "Neural Properties", "Izhikevich C property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snD.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("D", pbNumberBag.GetType(), "D", _
                                        "Neural Properties", "Izhikevich D property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snStdA.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("A StdDev", pbNumberBag.GetType(), "StdA", _
                                        "Neural Properties", "Standard deviation of Izhikevich A property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snStdB.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("B StdDev", pbNumberBag.GetType(), "StdB", _
                                        "Neural Properties", "Standard deviation of Izhikevich B property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snStdC.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("C StdDev", pbNumberBag.GetType(), "StdC", _
                                        "Neural Properties", "Standard deviation of Izhikevich C property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snStdD.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("D StdDev", pbNumberBag.GetType(), "StdD", _
                                        "Neural Properties", "Standard deviation of Izhikevich D property.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable COBA", GetType(Boolean), "EnableCOBA", _
                                        "Neural Properties", "Determines if a conductance based model is used. If false then a current based model is used", m_bEnableCOBA))

            pbNumberBag = m_snTauAMPA.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau AMPA", pbNumberBag.GetType(), "TauAMPA", _
                                        "Neural Properties", "Time constant of AMPA decay.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snTauNMDA.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau NMDA", pbNumberBag.GetType(), "TauNMDA", _
                                        "Neural Properties", "Time constant of NMDA decay.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snTauGABAa.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau GABAa", pbNumberBag.GetType(), "TauGABAa", _
                                        "Neural Properties", "Time constant of GABAa decay.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snTauGABAb.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau GABAb", pbNumberBag.GetType(), "TauGABAb", _
                                        "Neural Properties", "Time constant of GABAb decay.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))


            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable STP", GetType(Boolean), "EnableSTP", _
                                        "Neural Properties", "Determines if this neuron is exhibits short-term plasticity.", m_bEnableSTP))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable STDP", GetType(Boolean), "EnableSTDP", _
                                        "Neural Properties", "Determines if this neuron is exhibits spike timing dependent plasticity.", m_bEnableSTDP))

            If m_bEnableSTP Then
                pbNumberBag = m_snU.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("U", pbNumberBag.GetType(), "U", _
                                            "STP Properties", "The increment of u due to a spike.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snTauDepression.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau Depression", pbNumberBag.GetType(), "TauDepression", _
                                            "STP Properties", "Time constant for depression term .", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snTauFacilitation.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau Facilitation", pbNumberBag.GetType(), "TauFacilitation", _
                                            "STP Properties", "Time constant for facilitation term .", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If


            If m_bEnableSTDP Then
                pbNumberBag = m_snMaxLTP.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max LTP", pbNumberBag.GetType(), "MaxLTP", _
                                            "STDP Properties", "The max magnitude for LTP change.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snTauLTP.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau LTP", pbNumberBag.GetType(), "TauLTP", _
                                            "STDP Properties", "Decay time constant for LTP .", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snMaxLTD.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max LTD", pbNumberBag.GetType(), "MaxLTD", _
                                            "STDP Properties", "The max magnitude for LTD change.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snTauLTD.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tau LTD", pbNumberBag.GetType(), "TauLTD", _
                                            "STDP Properties", "Decay time constant for LTD .", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snA.ClearIsDirty()
            m_snB.ClearIsDirty()
            m_snC.ClearIsDirty()
            m_snD.ClearIsDirty()
            m_snStdA.ClearIsDirty()
            m_snStdB.ClearIsDirty()
            m_snStdC.ClearIsDirty()
            m_snStdD.ClearIsDirty()
            m_snTauAMPA.ClearIsDirty()
            m_snTauNMDA.ClearIsDirty()
            m_snTauGABAa.ClearIsDirty()
            m_snTauGABAb.ClearIsDirty()

            m_snU.ClearIsDirty()
            m_snTauDepression.ClearIsDirty()
            m_snTauFacilitation.ClearIsDirty()

            m_snMaxLTP.ClearIsDirty()
            m_snTauLTP.ClearIsDirty()
            m_snMaxLTD.ClearIsDirty()
            m_snTauLTD.ClearIsDirty()

        End Sub

        Public Overrides Function CreateNewAdapter(ByRef bnOrigin As AnimatGUI.DataObjects.Behavior.Node, ByRef doParent As AnimatGUI.Framework.DataObject) As AnimatGUI.DataObjects.Behavior.Node

            ''If it does require an adapter then lets add the pieces.
            Dim bnAdapter As AnimatGUI.DataObjects.Behavior.Node
            If bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If the origin is physics node and the destination is a regular node
                Throw New System.Exception("You cannot create an adpater coming into a regular neuron group. You must use a Spiking Generator and stimulate it instead.")
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Me.IsPhysicsEngineNode Then
                'If the origin is regular node and the destination is a physics node
                bnAdapter = New AnimatGUI.DataObjects.Behavior.Nodes.NodeToPhysicalAdapter(doParent)
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If both the origin and destination are regular nodes.
                Throw New System.Exception("You cannot create an adpater coming into a regular neuron group. You must use a Spiking Generator and stimulate it instead.")
            Else
                'If both the origin and destination are physics nodes.
                Throw New System.Exception("You can only link two physics nodes using a graphical link.")
            End If

            Return bnAdapter
        End Function

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_bEnabled = oXml.GetChildBool("Enabled", True)

            m_iNeuronCount = oXml.GetChildInt("NeuronCount", m_iNeuronCount)
            m_eNeuralType = DirectCast([Enum].Parse(GetType(enumNeuralType), oXml.GetChildString("NeuralType", "Excitatory"), True), enumNeuralType)

            m_snA.LoadData(oXml, "A", False)
            m_snB.LoadData(oXml, "B", False)
            m_snC.LoadData(oXml, "C", False)
            m_snD.LoadData(oXml, "D", False)
            m_snStdA.LoadData(oXml, "StdA", False)
            m_snStdB.LoadData(oXml, "StdB", False)
            m_snStdC.LoadData(oXml, "StdC", False)
            m_snStdD.LoadData(oXml, "StdD", False)
            m_bEnableCOBA = oXml.GetChildBool("EnableCOBA", m_bEnableCOBA)
            m_snTauAMPA.LoadData(oXml, "TauAMPA", False)
            m_snTauNMDA.LoadData(oXml, "TauNMDA", False)
            m_snTauGABAa.LoadData(oXml, "TauGABAa", False)
            m_snTauGABAb.LoadData(oXml, "TauGABAb", False)

            m_bEnableSTP = oXml.GetChildBool("EnableSTP", m_bEnableSTP)
            m_snU.LoadData(oXml, "U", False)
            m_snTauDepression.LoadData(oXml, "TauD", False)
            m_snTauFacilitation.LoadData(oXml, "TauF", False)

            m_bEnableSTDP = oXml.GetChildBool("EnableSTDP", m_bEnableSTDP)
            m_snMaxLTP.LoadData(oXml, "MaxLTP", False)
            m_snTauLTP.LoadData(oXml, "TauLTP", False)
            m_snMaxLTD.LoadData(oXml, "MaxLTD", False)
            m_snTauLTD.LoadData(oXml, "TauLTD", False)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("NeuronCount", m_iNeuronCount)
            oXml.AddChildElement("NeuralType", m_eNeuralType.ToString())

            m_snA.SaveData(oXml, "A")
            m_snB.SaveData(oXml, "B")
            m_snC.SaveData(oXml, "C")
            m_snD.SaveData(oXml, "D")
            m_snStdA.SaveData(oXml, "StdA")
            m_snStdB.SaveData(oXml, "StdB")
            m_snStdC.SaveData(oXml, "StdC")
            m_snStdD.SaveData(oXml, "StdD")
            oXml.AddChildElement("EnableCOBA", m_bEnableCOBA)
            m_snTauAMPA.SaveData(oXml, "TauAMPA")
            m_snTauNMDA.SaveData(oXml, "TauNMDA")
            m_snTauGABAa.SaveData(oXml, "TauGABAa")
            m_snTauGABAb.SaveData(oXml, "TauGABAb")

            oXml.AddChildElement("EnableSTP", m_bEnableSTP)
            m_snU.SaveData(oXml, "U")
            m_snTauDepression.SaveData(oXml, "TauD")
            m_snTauFacilitation.SaveData(oXml, "TauF")

            oXml.AddChildElement("EnableSTDP", m_bEnableSTDP)
            m_snMaxLTP.SaveData(oXml, "MaxLTP")
            m_snTauLTP.SaveData(oXml, "TauLTP")
            m_snMaxLTD.SaveData(oXml, "MaxLTD")
            m_snTauLTD.SaveData(oXml, "TauLTD")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("NeuronGroup")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("Type", Me.NeuronType)
            oXml.AddChildElement("Enabled", Me.Enabled)

            oXml.AddChildElement("NeuronCount", m_iNeuronCount)
            oXml.AddChildElement("NeuralType", m_eNeuralType.ToString())

            m_snA.SaveSimulationXml(oXml, Me, "A")
            m_snB.SaveSimulationXml(oXml, Me, "B")
            m_snC.SaveSimulationXml(oXml, Me, "C")
            m_snD.SaveSimulationXml(oXml, Me, "D")
            m_snStdA.SaveSimulationXml(oXml, Me, "StdA")
            m_snStdB.SaveSimulationXml(oXml, Me, "StdB")
            m_snStdC.SaveSimulationXml(oXml, Me, "StdC")
            m_snStdD.SaveSimulationXml(oXml, Me, "StdD")

            oXml.AddChildElement("EnableCOBA", m_bEnableCOBA)

            'These are saved off in miliseconds
            oXml.AddChildElement("TauAMPA", CInt(m_snTauAMPA.ActualValue * 1000))
            oXml.AddChildElement("TauNMDA", CInt(m_snTauNMDA.ActualValue * 1000))
            oXml.AddChildElement("TauGABAa", CInt(m_snTauGABAa.ActualValue * 1000))
            oXml.AddChildElement("TauGABAb", CInt(m_snTauGABAb.ActualValue * 1000))

            oXml.AddChildElement("EnableSTP", m_bEnableSTP)
            m_snU.SaveSimulationXml(oXml, Me, "U")
            oXml.AddChildElement("TauD", CInt(m_snTauDepression.ActualValue * 1000))
            oXml.AddChildElement("TauF", CInt(m_snTauFacilitation.ActualValue * 1000))

            oXml.AddChildElement("EnableSTDP", m_bEnableSTDP)
            m_snMaxLTP.SaveSimulationXml(oXml, Me, "MaxLTP")
            oXml.AddChildElement("TauLTP", CInt(m_snTauLTP.ActualValue * 1000))
            m_snMaxLTD.SaveSimulationXml(oXml, Me, "MaxLTD")
            oXml.AddChildElement("TauLTD", CInt(m_snTauLTD.ActualValue * 1000))

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#End Region

#End Region

    End Class

End Namespace

