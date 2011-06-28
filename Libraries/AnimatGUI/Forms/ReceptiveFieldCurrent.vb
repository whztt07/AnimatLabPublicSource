Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace Forms

    Public Class ReceptiveFieldCurrent
        'Inherits Windows.Forms.Form
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub
        Friend WithEvents grdGainProps As System.Windows.Forms.PropertyGrid
        'Friend WithEvents chartCurrentGain As System.Windows.Forms.Panel

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.grdGainProps = New System.Windows.Forms.PropertyGrid
            'Me.chartCurrentGain = New System.Windows.Forms.Panel
            Me.SuspendLayout()
            '
            'grdGainProps
            '
            Me.grdGainProps.HelpVisible = False
            Me.grdGainProps.LargeButtons = True
            Me.grdGainProps.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.grdGainProps.Location = New System.Drawing.Point(13, 13)
            Me.grdGainProps.Name = "grdGainProps"
            Me.grdGainProps.Size = New System.Drawing.Size(267, 162)
            Me.grdGainProps.TabIndex = 0
            Me.grdGainProps.ToolbarVisible = False
            '
            'chartCurrentGain
            '
            'Me.chartCurrentGain.Anchor = System.Windows.Forms.AnchorStyles.Left
            'Me.chartCurrentGain.Location = New System.Drawing.Point(13, 182)
            'Me.chartCurrentGain.Name = "chartCurrentGain"
            'Me.chartCurrentGain.Size = New System.Drawing.Size(267, 160)
            'Me.chartCurrentGain.TabIndex = 1
            'Me.chartCurrentGain.BackColor = Color.Black
            '
            'ReceptiveFieldGain
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 354)
            'Me.Controls.Add(Me.chartCurrentGain)
            Me.Controls.Add(Me.grdGainProps)
            Me.Name = "ReceptiveFieldCurrent"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected chartCurrentGain As AnimatGUI.Forms.Gain.GainControl

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.ReceptiveField.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try
                MyBase.Initialize(frmParent)

                Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(Util.GetFilePath(Util.Application.ApplicationDirectory, "LicensedAnimatGUI.dll"))
                chartCurrentGain = DirectCast(oAssembly.CreateInstance("LicensedAnimatGUI.Forms.Charts.GainControl"), AnimatGUI.Forms.Gain.GainControl)

                Me.Controls.Add(Me.chartCurrentGain)
                Me.chartCurrentGain.Anchor = System.Windows.Forms.AnchorStyles.Left
                Me.chartCurrentGain.Location = New System.Drawing.Point(13, 182)
                Me.chartCurrentGain.Name = "chartCurrentGain"
                Me.chartCurrentGain.Size = New System.Drawing.Size(267, 160)
                Me.chartCurrentGain.TabIndex = 1

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Events "

        Private Sub ReceptiveFieldPairs_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

            Try
                grdGainProps.Width = Me.Width - 30
                grdGainProps.Height = CInt(CSng(Me.Height) / 2.0) - 30

                If Not chartCurrentGain Is Nothing Then
                    chartCurrentGain.Width = Me.Width - 30
                    chartCurrentGain.Top = grdGainProps.Top + grdGainProps.Height + 15
                    chartCurrentGain.Height = Me.Height - chartCurrentGain.Top - 15
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub grdGainProps_PropertyValueChanged(ByVal s As System.Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles grdGainProps.PropertyValueChanged

        End Sub

        Protected Overrides Sub AnimatForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
            chartCurrentGain = Nothing
        End Sub

#End Region

    End Class

End Namespace