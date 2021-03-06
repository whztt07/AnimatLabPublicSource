Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Forms
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Imaging
Imports AnimatGUI.Framework.UndoSystem

Namespace Forms.Behavior

    Public Class AddFlowDiagram
        Inherits AnimatGUI.Forms.Behavior.Diagram

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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer
        Private WithEvents m_ctrlAddFlow As Lassalle.Flow.AddFlow
        Private WithEvents m_Timer As New Timer
        Friend WithEvents AddFlowToolStrip As AnimatGuiCtrls.Controls.AnimatToolStrip
        Friend WithEvents PrintToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AddFlowMenuStrip As AnimatGuiCtrls.Controls.AnimatMenuStrip
        Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents FitToPageMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOutMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOutBy10MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOutBy20MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut100MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut90MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut80MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut70MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut60MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut50MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut40MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut30MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut20MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomOut10MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomInMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomInBy10MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomInBy20MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn100MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn125MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn150MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn175MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn200MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn250MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn300MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn400MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomIn500MenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShapeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignTopMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignVerticalCenterMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignBottomMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignLeftMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignHorizontalCenterMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignRightMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DistributeMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DistributeVerticalMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DistributeHorizontalMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SizeMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SizeBothMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SizeWidthMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SizeHeightMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PageSetupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowConnectionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ZoomInToolStripItem As System.Windows.Forms.ToolStripButton
        Friend WithEvents ZoomOutToolStripItem As System.Windows.Forms.ToolStripButton
        Friend WithEvents AlignDropDownButton As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents AlignTopToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignVerticalCenterToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignBottomToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignLeftToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignHorizontalCenterToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AlignRightToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DistributeDropDownButton As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents DistributeVerticalToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DistributeHorizontalToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SizeDropDownButton As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents SizeBothToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SizeWidthToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SizeHeightToolStripItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents BringToFrontMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SendToBackMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PasteInPlaceMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents GridMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CutToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents RelabelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents RelabelToolStripButton As System.Windows.Forms.ToolStripButton

        'Friend WithEvents CopyPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents PastePopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents CutPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents DeletePopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents SelectByTypePopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents RelabelPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents RelabelSelectedPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents CompareItemsPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents PasteInPlacePopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents AlignPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents AlignTopPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents AlignVerticalCenterPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents AlignBottomPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents AlignLeftPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents AlignHorizontalCenterPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents AlignRightPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents DistributePopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents DistributeVerticalPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents DistributeHorizontalPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents SizePopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents SizeBothPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents SizeWidthPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents SizeHeightPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents BringToFrontPopupMenuItem As System.Windows.Forms.ToolStripMenuItem
        'Friend WithEvents SendToBackPopupMenuItem As System.Windows.Forms.ToolStripMenuItem

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Lassalle.Flow.AddFlow.LicenseKey = "A010C0600952-NqDrtr9tvzYV66Vnzg491Xm1mVYBZyJ3DUzI563gkFA5vAPvk+2VZV5J+i4jHaU0kyiZp0c3hXDXHhZiMoRNS2Awfhjd7EapukGr2wCP/O/UPoBtyVNxhIpS1aqbazMjbwlQoBsDzizH5GixDAWz9RDvg4vJozrCtS+TFU2Rnyk="
            Me.m_ctrlAddFlow = New Lassalle.Flow.AddFlow
            Me.AddFlowToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip
            Me.PrintToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.ZoomInToolStripItem = New System.Windows.Forms.ToolStripButton()
            Me.ZoomOutToolStripItem = New System.Windows.Forms.ToolStripButton()
            Me.AlignDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
            Me.AlignTopToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignVerticalCenterToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignBottomToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignLeftToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignHorizontalCenterToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignRightToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DistributeDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
            Me.DistributeVerticalToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DistributeHorizontalToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SizeDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
            Me.SizeBothToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SizeWidthToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SizeHeightToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddFlowMenuStrip = New AnimatGuiCtrls.Controls.AnimatMenuStrip
            Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PageSetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowConnectionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
            Me.FitToPageMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOutMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOutBy10MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOutBy20MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut100MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut90MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut80MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut70MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut60MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut50MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut40MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut30MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut20MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomOut10MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomInMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomInBy10MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomInBy20MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn100MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn125MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn150MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn175MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn200MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn250MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn300MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn400MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ZoomIn500MenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShapeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignTopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignVerticalCenterMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignBottomMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignLeftMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignHorizontalCenterMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AlignRightMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DistributeMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DistributeVerticalMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DistributeHorizontalMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SizeMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SizeBothMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SizeWidthMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SizeHeightMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.BringToFrontMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SendToBackMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PasteInPlaceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.GridMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CutToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RelabelToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.RelabelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()

            'Me.CopyPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem
            'Me.PastePopupMenuItem = New System.Windows.Forms.ToolStripMenuItem
            'Me.CutPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem
            'Me.DeletePopupMenuItem = New System.Windows.Forms.ToolStripMenuItem
            'Me.SelectByTypePopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.RelabelPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.RelabelSelectedPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.CompareItemsPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.PasteInPlacePopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.AlignPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.AlignTopPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.AlignVerticalCenterPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.AlignBottomPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.AlignLeftPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.AlignHorizontalCenterPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.AlignRightPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.DistributePopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.DistributeVerticalPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.DistributeHorizontalPopupMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.SizeMenuPopupItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.SizeBothMenuPopupItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.SizeWidthMenuPopupItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.SizeHeightMenuPopupItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.BringToFrontMenuPopupItem = New System.Windows.Forms.ToolStripMenuItem()
            'Me.SendToBackMenuPopupItem = New System.Windows.Forms.ToolStripMenuItem()

            Me.AddFlowToolStrip.SuspendLayout()
            Me.AddFlowMenuStrip.SuspendLayout()
            Me.SuspendLayout()
            '
            'm_ctrlAddFlow
            '
            Me.m_ctrlAddFlow.Dock = System.Windows.Forms.DockStyle.Fill
            'Me.m_ctrlAddFlow.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            '            Or System.Windows.Forms.AnchorStyles.Left) _
            '            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.m_ctrlAddFlow.Location = New System.Drawing.Point(8, 8)
            Me.m_ctrlAddFlow.Name = "m_ctrlAddFlow"
            Me.m_ctrlAddFlow.Size = New System.Drawing.Size(192, 200)
            Me.m_ctrlAddFlow.TabIndex = 0

            '
            'AddFlowToolStrip
            '
            Me.AddFlowToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintToolStripButton, Me.PasteToolStripButton, Me.CopyToolStripButton, _
                                                                                         Me.CutToolStripButton, Me.ZoomInToolStripItem, Me.ZoomOutToolStripItem, _
                                                                                         Me.AlignDropDownButton, Me.DistributeDropDownButton, Me.SizeDropDownButton, Me.RelabelToolStripButton})
            Me.AddFlowToolStrip.Location = New System.Drawing.Point(0, 24)
            Me.AddFlowToolStrip.Name = "AddFlowToolStrip"
            Me.AddFlowToolStrip.Size = New System.Drawing.Size(351, 25)
            Me.AddFlowToolStrip.TabIndex = 0
            Me.AddFlowToolStrip.Text = "Add flow Tool stip"
            Me.AddFlowToolStrip.Visible = False
            '
            'PrintToolStripButton
            '
            Me.PrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.PrintToolStripButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Print.gif")
            Me.PrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PrintToolStripButton.Name = "PrintToolStripButton"
            Me.PrintToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.PrintToolStripButton.Text = "&Print"
            '
            'ZoomInDropDownButton
            '
            Me.ZoomInToolStripItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.ZoomInToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.ZoomIn.gif")
            Me.ZoomInToolStripItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ZoomInToolStripItem.Name = "ZoomInToolStripItem"
            Me.ZoomInToolStripItem.Size = New System.Drawing.Size(29, 22)
            Me.ZoomInToolStripItem.Text = "ZoomInToolStripItem"
            Me.ZoomInToolStripItem.ToolTipText = "Zoom in to diagram"
            '
            'ZoomOutDropDownButton
            '
            Me.ZoomOutToolStripItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.ZoomOutToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.ZoomOut.gif")
            Me.ZoomOutToolStripItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ZoomOutToolStripItem.Name = "ZoomOutDropDownButton"
            Me.ZoomOutToolStripItem.Size = New System.Drawing.Size(29, 22)
            Me.ZoomOutToolStripItem.Text = "ZoomOutDropDownButton"
            Me.ZoomOutToolStripItem.ToolTipText = "Zoom out of diagram"
            '
            'AlignDropDownButton
            '
            Me.AlignDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AlignDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AlignTopToolStripItem, Me.AlignVerticalCenterToolStripItem, Me.AlignBottomToolStripItem, Me.AlignLeftToolStripItem, Me.AlignHorizontalCenterToolStripItem, Me.AlignRightToolStripItem})
            Me.AlignDropDownButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Align.gif")
            Me.AlignDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AlignDropDownButton.Name = "AlignDropDownButton"
            Me.AlignDropDownButton.Size = New System.Drawing.Size(29, 22)
            Me.AlignDropDownButton.Text = "AlignDropDownButton"
            Me.AlignDropDownButton.ToolTipText = "Align selected shapes"
            '
            'AlignTopToolStripItem
            '
            Me.AlignTopToolStripItem.Name = "AlignTopToolStripItem"
            Me.AlignTopToolStripItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignTopToolStripItem.Text = "Top"
            Me.AlignTopToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignTop.gif")
            '
            'AlignVerticalCenterToolStripItem
            '
            Me.AlignVerticalCenterToolStripItem.Name = "AlignVerticalCenterToolStripItem"
            Me.AlignVerticalCenterToolStripItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignVerticalCenterToolStripItem.Text = "Vertical Center"
            Me.AlignVerticalCenterToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignVerticalCenter.gif")
            '
            'AlignBottomToolStripItem
            '
            Me.AlignBottomToolStripItem.Name = "AlignBottomToolStripItem"
            Me.AlignBottomToolStripItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignBottomToolStripItem.Text = "Bottom"
            Me.AlignBottomToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignBottom.gif")
            '
            'AlignLeftToolStripItem
            '
            Me.AlignLeftToolStripItem.Name = "AlignLeftToolStripItem"
            Me.AlignLeftToolStripItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignLeftToolStripItem.Text = "Left"
            Me.AlignLeftToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignLeft.gif")
            '
            'AlignHorizontalCenterToolStripItem
            '
            Me.AlignHorizontalCenterToolStripItem.Name = "AlignHorizontalCenterToolStripItem"
            Me.AlignHorizontalCenterToolStripItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignHorizontalCenterToolStripItem.Text = "Horizontal Center"
            Me.AlignHorizontalCenterToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignHorizontalCenter.gif")
            '
            'AlignRightToolStripItem
            '
            Me.AlignRightToolStripItem.Name = "AlignRightToolStripItem"
            Me.AlignRightToolStripItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignRightToolStripItem.Text = "Right"
            Me.AlignRightToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignRight.gif")
            '
            'DistributeDropDownButton
            '
            Me.DistributeDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.DistributeDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DistributeVerticalToolStripItem, Me.DistributeHorizontalToolStripItem})
            Me.DistributeDropDownButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Distribute.gif")
            Me.DistributeDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.DistributeDropDownButton.Name = "DistributeDropDownButton"
            Me.DistributeDropDownButton.Size = New System.Drawing.Size(29, 22)
            Me.DistributeDropDownButton.Text = "DistributeDropDownButton"
            Me.DistributeDropDownButton.ToolTipText = "Distribute selected shapes"
            '
            'DistributeVerticalToolStripItem
            '
            Me.DistributeVerticalToolStripItem.Name = "DistributeVerticalToolStripItem"
            Me.DistributeVerticalToolStripItem.Size = New System.Drawing.Size(129, 22)
            Me.DistributeVerticalToolStripItem.Text = "Vertical"
            Me.DistributeVerticalToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.DistributeVertical.gif")
            '
            'DistributeHorizontalToolStripItem
            '
            Me.DistributeHorizontalToolStripItem.Name = "DistributeHorizontalToolStripItem"
            Me.DistributeHorizontalToolStripItem.Size = New System.Drawing.Size(129, 22)
            Me.DistributeHorizontalToolStripItem.Text = "Horizontal"
            Me.DistributeHorizontalToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.DistributeHorizontal.gif")
            '
            'SizeDropDownButton
            '
            Me.SizeDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SizeDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SizeBothToolStripItem, Me.SizeWidthToolStripItem, Me.SizeHeightToolStripItem})
            Me.SizeDropDownButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Size.gif")
            Me.SizeDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SizeDropDownButton.Name = "SizeDropDownButton"
            Me.SizeDropDownButton.Size = New System.Drawing.Size(29, 22)
            Me.SizeDropDownButton.Text = "ToolStripDropDownButton1"
            '
            'SizeBothToolStripItem
            '
            Me.SizeBothToolStripItem.Name = "SizeBothToolStripItem"
            Me.SizeBothToolStripItem.Size = New System.Drawing.Size(110, 22)
            Me.SizeBothToolStripItem.Text = "Both"
            Me.SizeBothToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SizeBoth.gif")
            '
            'SizeWidthToolStripItem
            '
            Me.SizeWidthToolStripItem.Name = "SizeWidthToolStripItem"
            Me.SizeWidthToolStripItem.Size = New System.Drawing.Size(110, 22)
            Me.SizeWidthToolStripItem.Text = "Width"
            Me.SizeWidthToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SizeWidth.gif")
            '
            'SizeHeightToolStripItem
            '
            Me.SizeHeightToolStripItem.Name = "SizeHeightToolStripItem"
            Me.SizeHeightToolStripItem.Size = New System.Drawing.Size(110, 22)
            Me.SizeHeightToolStripItem.Text = "Height"
            Me.SizeHeightToolStripItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SizeHeight.gif")
            '
            'RelabelToolStripButton
            '
            Me.RelabelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelToolStripButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Relabel.gif")
            Me.RelabelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.RelabelToolStripButton.Name = "RelabelToolStripButton"
            Me.RelabelToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.RelabelToolStripButton.Text = "RelabelToolStripButton"
            Me.RelabelToolStripButton.ToolTipText = "Relabel Items."
            '
            'AddFlowMenuStrip
            '
            Me.AddFlowMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ViewToolStripMenuItem, Me.ShapeToolStripMenuItem})
            Me.AddFlowMenuStrip.Location = New System.Drawing.Point(0, 0)
            Me.AddFlowMenuStrip.Name = "AddFlowMenuStrip"
            Me.AddFlowMenuStrip.Size = New System.Drawing.Size(351, 24)
            Me.AddFlowMenuStrip.TabIndex = 1
            Me.AddFlowMenuStrip.Text = "AddFlowMenuStrip"
            Me.AddFlowMenuStrip.Visible = False
            '
            'FileToolStripMenuItem
            '
            Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripSeparator1, Me.ExportToolStripMenuItem, Me.PageSetupToolStripMenuItem, Me.PrintPreviewToolStripMenuItem, Me.PrintToolStripMenuItem})
            Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
            Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
            Me.FileToolStripMenuItem.Text = "&File"
            Me.FileToolStripMenuItem.MergeAction = MergeAction.MatchOnly
            '
            'toolStripSeparator1
            '
            Me.toolStripSeparator1.Name = "toolStripSeparator1"
            Me.toolStripSeparator1.Size = New System.Drawing.Size(140, 6)
            Me.toolStripSeparator1.MergeAction = System.Windows.Forms.MergeAction.Append
            '
            'ExportToolStripMenuItem
            '
            Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
            Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
            Me.ExportToolStripMenuItem.Text = "Export"
            Me.ExportToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Export.gif")
            Me.ExportToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Append
            '
            'PageSetupToolStripMenuItem
            '
            Me.PageSetupToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.PageSetup.gif")
            Me.PageSetupToolStripMenuItem.Name = "PageSetupToolStripMenuItem"
            Me.PageSetupToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
            Me.PageSetupToolStripMenuItem.Text = "Page Setup"
            Me.PageSetupToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Append
            '
            'PrintPreviewToolStripMenuItem
            '
            Me.PrintPreviewToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.PrintPreview.gif")
            Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
            Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
            Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
            Me.PrintPreviewToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Append
            '
            'PrintToolStripMenuItem
            '
            Me.PrintPreviewToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Print.gif")
            Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
            Me.PrintToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
            Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
            Me.PrintToolStripMenuItem.Text = "&Print"
            Me.PrintToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Append
            '
            'EditToolStripMenuItem
            '
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasteInPlaceMenuItem, Me.PasteToolStripMenuItem, Me.CopyToolStripMenuItem, _
                                                                                                      Me.CutToolStripMenuItem, Me.ShowConnectionsToolStripMenuItem, Me.GridMenuItem, Me.RelabelToolStripMenuItem})
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
            Me.EditToolStripMenuItem.MergeAction = MergeAction.MatchOnly
            '
            'ShowConnectionsToolStripMenuItem
            '
            Me.ShowConnectionsToolStripMenuItem.Name = "ShowConnectionsToolStripMenuItem"
            Me.ShowConnectionsToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
            Me.ShowConnectionsToolStripMenuItem.Text = "Show Connections"
            Me.ShowConnectionsToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Connections.gif")
            Me.ShowConnectionsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Append
            '
            'ViewToolStripMenuItem
            '
            Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator3, Me.FitToPageMenuItem, Me.ZoomOutMenuItem, Me.ZoomInMenuItem})
            Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
            Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
            Me.ViewToolStripMenuItem.Text = "&View"
            Me.ViewToolStripMenuItem.MergeAction = MergeAction.MatchOnly
            '
            'ToolStripSeparator3
            '
            Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
            Me.ToolStripSeparator3.Size = New System.Drawing.Size(149, 6)
            Me.ToolStripSeparator3.MergeAction = MergeAction.Append
            '
            'FitToPageMenuItem
            '
            Me.FitToPageMenuItem.Name = "FitToPageMenuItem"
            Me.FitToPageMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.FitToPageMenuItem.Text = "Fit to Page"
            Me.FitToPageMenuItem.MergeAction = MergeAction.Append
            '
            'RelabelToolStripMenuItem
            '
            Me.RelabelToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Relabel.gif")
            Me.RelabelToolStripMenuItem.Name = "RelabelToolStripMenuItem"
            Me.RelabelToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelToolStripMenuItem.Text = "Relabel"
            Me.RelabelToolStripMenuItem.ToolTipText = "Relabel items using a regular expression"
            '
            'ZoomOutMenuItem
            '
            Me.ZoomOutMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ZoomOutBy10MenuItem, Me.ZoomOutBy20MenuItem, Me.ZoomOut100MenuItem, Me.ZoomOut90MenuItem, Me.ZoomOut80MenuItem, Me.ZoomOut70MenuItem, Me.ZoomOut60MenuItem, Me.ZoomOut50MenuItem, Me.ZoomOut40MenuItem, Me.ZoomOut30MenuItem, Me.ZoomOut20MenuItem, Me.ZoomOut10MenuItem})
            Me.ZoomOutMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.ZoomOut.gif")
            Me.ZoomOutMenuItem.Name = "ZoomOutMenuItem"
            Me.ZoomOutMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.ZoomOutMenuItem.Text = "Zoom Out"
            Me.ZoomOutMenuItem.MergeAction = MergeAction.Append
            '
            'ZoomOutBy10MenuItem
            '
            Me.ZoomOutBy10MenuItem.Name = "ZoomOutBy10MenuItem"
            Me.ZoomOutBy10MenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
            Me.ZoomOutBy10MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOutBy10MenuItem.Text = "Out by 10%"
            '
            'ZoomOutBy20MenuItem
            '
            Me.ZoomOutBy20MenuItem.Name = "ZoomOutBy20MenuItem"
            Me.ZoomOutBy20MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOutBy20MenuItem.Text = "Out by 20%"
            '
            'ZoomOut100MenuItem
            '
            Me.ZoomOut100MenuItem.Name = "ZoomOut100MenuItem"
            Me.ZoomOut100MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut100MenuItem.Text = "100%"
            '
            'ZoomOut90MenuItem
            '
            Me.ZoomOut90MenuItem.Name = "ZoomOut90MenuItem"
            Me.ZoomOut90MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut90MenuItem.Text = "90%"
            '
            'ZoomOut80MenuItem
            '
            Me.ZoomOut80MenuItem.Name = "ZoomOut80MenuItem"
            Me.ZoomOut80MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut80MenuItem.Text = "80%"
            '
            'ZoomOut70MenuItem
            '
            Me.ZoomOut70MenuItem.Name = "ZoomOut70MenuItem"
            Me.ZoomOut70MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut70MenuItem.Text = "70%"
            '
            'ZoomOut60MenuItem
            '
            Me.ZoomOut60MenuItem.Name = "ZoomOut60MenuItem"
            Me.ZoomOut60MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut60MenuItem.Text = "60%"
            '
            'ZoomOut50MenuItem
            '
            Me.ZoomOut50MenuItem.Name = "ZoomOut50MenuItem"
            Me.ZoomOut50MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut50MenuItem.Text = "50%"
            '
            'ZoomOut40MenuItem
            '
            Me.ZoomOut40MenuItem.Name = "ZoomOut40MenuItem"
            Me.ZoomOut40MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut40MenuItem.Text = "40%"
            '
            'ZoomOut30MenuItem
            '
            Me.ZoomOut30MenuItem.Name = "ZoomOut30MenuItem"
            Me.ZoomOut30MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut30MenuItem.Text = "30%"
            '
            'ZoomOut20MenuItem
            '
            Me.ZoomOut20MenuItem.Name = "ZoomOut20MenuItem"
            Me.ZoomOut20MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut20MenuItem.Text = "20%"
            '
            'ZoomOut10MenuItem
            '
            Me.ZoomOut10MenuItem.Name = "ZoomOut10MenuItem"
            Me.ZoomOut10MenuItem.Size = New System.Drawing.Size(175, 22)
            Me.ZoomOut10MenuItem.Text = "10%"
            '
            'ZoomInMenuItem
            '
            Me.ZoomInMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ZoomInBy10MenuItem, Me.ZoomInBy20MenuItem, Me.ZoomIn100MenuItem, Me.ZoomIn125MenuItem, Me.ZoomIn150MenuItem, Me.ZoomIn175MenuItem, Me.ZoomIn200MenuItem, Me.ZoomIn250MenuItem, Me.ZoomIn300MenuItem, Me.ZoomIn400MenuItem, Me.ZoomIn500MenuItem})
            Me.ZoomInMenuItem.Name = "ZoomInMenuItem"
            Me.ZoomInMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.ZoomInMenuItem.Text = "Zoom In"
            Me.ZoomInMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.ZoomIn.gif")
            Me.ZoomInMenuItem.MergeAction = MergeAction.Append
            '
            'ZoomInBy10MenuItem
            '
            Me.ZoomInBy10MenuItem.Name = "ZoomInBy10MenuItem"
            Me.ZoomInBy10MenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
            Me.ZoomInBy10MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomInBy10MenuItem.Text = "In by 10%"
            '
            'ZoomInBy20MenuItem
            '
            Me.ZoomInBy20MenuItem.Name = "ZoomInBy20MenuItem"
            Me.ZoomInBy20MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomInBy20MenuItem.Text = "In by 20%"
            '
            'ZoomIn100MenuItem
            '
            Me.ZoomIn100MenuItem.Name = "ZoomIn100MenuItem"
            Me.ZoomIn100MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn100MenuItem.Text = "100%"
            '
            'ZoomIn125MenuItem
            '
            Me.ZoomIn125MenuItem.Name = "ZoomIn125MenuItem"
            Me.ZoomIn125MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn125MenuItem.Text = "125%"
            '
            'ZoomIn150MenuItem
            '
            Me.ZoomIn150MenuItem.Name = "ZoomIn150MenuItem"
            Me.ZoomIn150MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn150MenuItem.Text = "150%"
            '
            'ZoomIn175MenuItem
            '
            Me.ZoomIn175MenuItem.Name = "ZoomIn175MenuItem"
            Me.ZoomIn175MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn175MenuItem.Text = "175%"
            '
            'ZoomIn200MenuItem
            '
            Me.ZoomIn200MenuItem.Name = "ZoomIn200MenuItem"
            Me.ZoomIn200MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn200MenuItem.Text = "200%"
            '
            'ZoomIn250MenuItem
            '
            Me.ZoomIn250MenuItem.Name = "ZoomIn250MenuItem"
            Me.ZoomIn250MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn250MenuItem.Text = "250%"
            '
            'ZoomIn300MenuItem
            '
            Me.ZoomIn300MenuItem.Name = "ZoomIn300MenuItem"
            Me.ZoomIn300MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn300MenuItem.Text = "300%"
            '
            'ZoomIn400MenuItem
            '
            Me.ZoomIn400MenuItem.Name = "ZoomIn400MenuItem"
            Me.ZoomIn400MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn400MenuItem.Text = "400%"
            '
            'ZoomIn500MenuItem
            '
            Me.ZoomIn500MenuItem.Name = "ZoomIn500MenuItem"
            Me.ZoomIn500MenuItem.Size = New System.Drawing.Size(166, 22)
            Me.ZoomIn500MenuItem.Text = "500%"
            '
            'ShapeToolStripMenuItem
            '
            Me.ShapeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AlignMenuItem, Me.DistributeMenuItem, Me.SizeMenuItem, Me.BringToFrontMenuItem, Me.SendToBackMenuItem})
            Me.ShapeToolStripMenuItem.Name = "ShapeToolStripMenuItem"
            Me.ShapeToolStripMenuItem.Size = New System.Drawing.Size(51, 20)
            Me.ShapeToolStripMenuItem.Text = "Shape"
            '
            'AlignMenuItem
            '
            Me.AlignMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AlignTopMenuItem, Me.AlignVerticalCenterMenuItem, Me.AlignBottomMenuItem, Me.AlignLeftMenuItem, Me.AlignHorizontalCenterMenuItem, Me.AlignRightMenuItem})
            Me.AlignMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Align.gif")
            Me.AlignMenuItem.Name = "AlignMenuItem"
            Me.AlignMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.AlignMenuItem.Text = "Align"
            '
            'AlignTopMenuItem
            '
            Me.AlignTopMenuItem.Name = "AlignTopMenuItem"
            Me.AlignTopMenuItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignTopMenuItem.Text = "Top"
            Me.AlignTopMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignTop.gif")
            '
            'AlignVerticalCenterMenuItem
            '
            Me.AlignVerticalCenterMenuItem.Name = "AlignVerticalCenterMenuItem"
            Me.AlignVerticalCenterMenuItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignVerticalCenterMenuItem.Text = "Vertical Center"
            Me.AlignVerticalCenterMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignVerticalCenter.gif")
            '
            'AlignBottomMenuItem
            '
            Me.AlignBottomMenuItem.Name = "AlignBottomMenuItem"
            Me.AlignBottomMenuItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignBottomMenuItem.Text = "Bottom"
            Me.AlignBottomMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignBottom.gif")
            '
            'AlignLeftMenuItem
            '
            Me.AlignLeftMenuItem.Name = "AlignLeftMenuItem"
            Me.AlignLeftMenuItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignLeftMenuItem.Text = "Left"
            Me.AlignLeftMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignLeft.gif")
            '
            'AlignHorizontalCenterMenuItem
            '
            Me.AlignHorizontalCenterMenuItem.Name = "AlignHorizontalCenterMenuItem"
            Me.AlignHorizontalCenterMenuItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignHorizontalCenterMenuItem.Text = "Horizontal Center"
            Me.AlignHorizontalCenterMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignHorizontalCenter.gif")
            '
            'AlignRightMenuItem
            '
            Me.AlignRightMenuItem.Name = "AlignRightMenuItem"
            Me.AlignRightMenuItem.Size = New System.Drawing.Size(167, 22)
            Me.AlignRightMenuItem.Text = "Right"
            Me.AlignRightMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.AlignRight.gif")
            '
            'DistributeMenuItem
            '
            Me.DistributeMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DistributeVerticalMenuItem, Me.DistributeHorizontalMenuItem})
            Me.DistributeMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Distribute.gif")
            Me.DistributeMenuItem.Name = "DistributeMenuItem"
            Me.DistributeMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.DistributeMenuItem.Text = "Distribute"
            '
            'DistributeVerticalMenuItem
            '
            Me.DistributeVerticalMenuItem.Name = "DistributeVerticalMenuItem"
            Me.DistributeVerticalMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.DistributeVerticalMenuItem.Text = "Vertical"
            Me.DistributeVerticalMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.DistributeVertical.gif")
            '
            'DistributeHorizontalMenuItem
            '
            Me.DistributeHorizontalMenuItem.Name = "DistributeHorizontalMenuItem"
            Me.DistributeHorizontalMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.DistributeHorizontalMenuItem.Text = "Horizontal"
            Me.DistributeHorizontalMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.DistributeHorizontal.gif")
            '
            'SizeMenuItem
            '
            Me.SizeMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SizeBothMenuItem, Me.SizeWidthMenuItem, Me.SizeHeightMenuItem})
            Me.SizeMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Size.gif")
            Me.SizeMenuItem.Name = "SizeMenuItem"
            Me.SizeMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.SizeMenuItem.Text = "Size"
            '
            'SizeBothMenuItem
            '
            Me.SizeBothMenuItem.Name = "SizeBothMenuItem"
            Me.SizeBothMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.SizeBothMenuItem.Text = "Both"
            Me.SizeBothMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SizeBoth.gif")
            '
            'SizeWidthMenuItem
            '
            Me.SizeWidthMenuItem.Name = "SizeWidthMenuItem"
            Me.SizeWidthMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.SizeWidthMenuItem.Text = "Width"
            Me.SizeWidthMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SizeWidth.gif")
            '
            'SizeHeightMenuItem
            '
            Me.SizeHeightMenuItem.Name = "SizeHeightMenuItem"
            Me.SizeHeightMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.SizeHeightMenuItem.Text = "Height"
            Me.SizeHeightMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SizeHeight.gif")
            '
            'SelectByTypeToolStripMenuItem
            '
            Me.SendToBackMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SendToBack.gif")
            Me.SendToBackMenuItem.Name = "SendToBackToolStripMenuItem"
            Me.SendToBackMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.SendToBackMenuItem.Text = "Send to back"
            Me.SendToBackMenuItem.ToolTipText = "Send to back"
            '
            'SelectByTypeToolStripMenuItem
            '
            Me.BringToFrontMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.BringToFront.gif")
            Me.BringToFrontMenuItem.Name = "BringToFrontToolStripMenuItem"
            Me.BringToFrontMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.BringToFrontMenuItem.Text = "Bring to front"
            Me.BringToFrontMenuItem.ToolTipText = "Bring to front"
            '
            'GridMenuItem
            '
            Me.GridMenuItem.Name = "GridMenuItem"
            Me.GridMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.GridMenuItem.Text = "Grid"
            Me.GridMenuItem.MergeAction = MergeAction.Append
            Me.GridMenuItem.CheckOnClick = True
            Me.GridMenuItem.Checked = True
            '
            'CopyToolStripButton
            '
            Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CopyToolStripButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Copy.gif")
            Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyToolStripButton.Name = "CopyToolStripButton"
            Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CopyToolStripButton.Text = "&Copy"
            Me.CopyToolStripButton.MergeAction = MergeAction.Insert
            Me.CopyToolStripButton.MergeIndex = 4
            '
            'CutToolStripButton
            '
            Me.CutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CutToolStripButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Cut.gif")
            Me.CutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CutToolStripButton.Name = "CutToolStripButton"
            Me.CutToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CutToolStripButton.Text = "C&ut"
            Me.CutToolStripButton.MergeAction = MergeAction.Insert
            Me.CutToolStripButton.MergeIndex = 4
            '
            'PasteToolStripButton
            '
            Me.PasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.PasteToolStripButton.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.CopyClipboard.gif")
            Me.PasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteToolStripButton.Name = "PasteToolStripButton"
            Me.PasteToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.PasteToolStripButton.Text = "&Paste"
            Me.PasteToolStripButton.MergeAction = MergeAction.Insert
            Me.PasteToolStripButton.MergeIndex = 4
            '
            'CopyToolStripMenuItem
            '
            Me.CopyToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Copy.gif")
            Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
            Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
            Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CopyToolStripMenuItem.Text = "&Copy"
            Me.CopyToolStripMenuItem.MergeAction = MergeAction.Insert
            Me.CopyToolStripMenuItem.MergeIndex = 4
            '
            'CutToolStripMenuItem
            '
            Me.CutToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.Cut.gif")
            Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
            Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
            Me.CutToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CutToolStripMenuItem.Text = "Cu&t"
            Me.CutToolStripMenuItem.MergeAction = MergeAction.Insert
            Me.CutToolStripMenuItem.MergeIndex = 4
            '
            'PasteToolStripMenuItem
            '
            Me.PasteToolStripMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.CopyClipboard.gif")
            Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
            Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
            Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.PasteToolStripMenuItem.Text = "&Paste"
            Me.PasteToolStripMenuItem.MergeAction = MergeAction.Insert
            Me.PasteToolStripMenuItem.MergeIndex = 4
            '
            'PasteInPlaseToolStripMenuItem
            '
            Me.PasteInPlaceMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.CopyClipboard.gif")
            Me.PasteInPlaceMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteInPlaceMenuItem.Name = "PasteInPlaceToolStripMenuItem"
            Me.PasteInPlaceMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.PasteInPlaceMenuItem.Text = "Paste in place"
            Me.PasteInPlaceMenuItem.MergeAction = MergeAction.Insert
            Me.PasteInPlaceMenuItem.MergeIndex = 4


            'AddFlowDiagram_ToolStrips
            '
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.AddFlowToolStrip)
            Me.Controls.Add(Me.AddFlowMenuStrip)
            Me.Controls.Add(Me.m_ctrlAddFlow)
            Me.MainMenuStrip = Me.AddFlowMenuStrip
            Me.Name = "Behavioral Editor"
            Me.Text = "BehavioralLayout"
            Me.AddFlowToolStrip.ResumeLayout(False)
            Me.AddFlowToolStrip.PerformLayout()
            Me.AddFlowMenuStrip.ResumeLayout(False)
            Me.AddFlowMenuStrip.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Enums "

        Public Enum enumGridStyle
            Pixels
            Lines
            DottedLines
        End Enum

        Public Enum enumJumpSize
            Small
            Medium
            Large
        End Enum
#End Region

#Region " Attributes "

        Protected m_aryAddFlowNodes As New SortedList
        Protected m_aryAddFlowLinks As New SortedList

        'Protected m_aryDeletedNodes As New SortedList
        'Protected m_aryDeletedLinks As New SortedList

        Private m_prnFlow As New Lassalle.PrnFlow.PrnFlow

        Protected m_bSelectingMultiple As Boolean = False

#End Region

#Region " Properties "

        Public Overridable Property DrawGrid() As Boolean
            Get
                Return m_ctrlAddFlow.Grid.Draw
            End Get
            Set(ByVal Value As Boolean)
                m_ctrlAddFlow.Grid.Draw = Value
                Me.GridMenuItem.Checked = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridColor() As System.Drawing.Color
            Get
                Return m_ctrlAddFlow.Grid.Color
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_ctrlAddFlow.Grid.Color = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridSize() As System.Drawing.Size
            Get
                Return m_ctrlAddFlow.Grid.Size
            End Get
            Set(ByVal Value As System.Drawing.Size)
                m_ctrlAddFlow.Grid.Size = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridSnap() As Boolean
            Get
                Return m_ctrlAddFlow.Grid.Snap
            End Get
            Set(ByVal Value As Boolean)
                m_ctrlAddFlow.Grid.Snap = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridStyle() As enumGridStyle
            Get
                Return CType(m_ctrlAddFlow.Grid.Style, enumGridStyle)
            End Get
            Set(ByVal Value As enumGridStyle)
                m_ctrlAddFlow.Grid.Style = CType(Value, Lassalle.Flow.GridStyle)
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property AddFlowBackColor() As System.Drawing.Color
            Get
                Return m_ctrlAddFlow.BackColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_ctrlAddFlow.BackColor = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property JumpSize() As enumJumpSize
            Get
                Return CType(m_ctrlAddFlow.JumpSize, enumJumpSize)
            End Get
            Set(ByVal Value As enumJumpSize)
                m_ctrlAddFlow.JumpSize = CType(Value, Lassalle.Flow.JumpSize)
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overrides Property DiagramName() As String
            Get
                Return Me.TabPageName
            End Get
            Set(ByVal Value As String)
                Me.TabPageName = Value
            End Set
        End Property

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "LicensedAnimatGUI.SubsystemNode.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property FormMenuStrip() As AnimatGuiCtrls.Controls.AnimatMenuStrip
            Get
                Return Me.AddFlowMenuStrip
            End Get
        End Property

        Public Overrides ReadOnly Property FormToolStrip() As AnimatGuiCtrls.Controls.AnimatToolStrip
            Get
                Return Me.AddFlowToolStrip
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSaveOnClose() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try

                MyBase.Initialize(frmParent)

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatGUI.DataGraph.ico")

                Me.m_ctrlAddFlow.Grid.Draw = True
                Me.m_ctrlAddFlow.AutoScroll = True
                Me.m_ctrlAddFlow.CursorSetting = Lassalle.Flow.CursorSetting.ResizeAndDrag
                Me.m_ctrlAddFlow.ScrollbarsDisplayMode = Lassalle.Flow.ScrollbarsDisplayMode.SizeOfDiagramOnly
                Me.m_ctrlAddFlow.BackColor = Color.White
                Me.m_ctrlAddFlow.AllowDrop = True
                Me.m_ctrlAddFlow.CanDrawNode = False
                Me.m_ctrlAddFlow.MouseAction = Lassalle.Flow.MouseAction.Selection

                m_Timer.Enabled = False
                m_Timer.Interval = 100

                Me.AllowDrop = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub UpdateAddFlowNode(ByRef afNode As Lassalle.Flow.Node, _
                                        ByRef bdNode As AnimatGUI.DataObjects.Behavior.Node, _
                                        Optional ByVal bAdding As Boolean = False)

            afNode.Alignment = CType(bdNode.Alignment, Lassalle.Flow.Alignment)
            afNode.AutoSize = CType(bdNode.AutoSize, Lassalle.Flow.AutoSize)
            afNode.BackMode = CType(bdNode.BackMode, Lassalle.Flow.BackMode)
            afNode.DashStyle = bdNode.DashStyle
            afNode.DrawColor = bdNode.DrawColor
            afNode.DrawWidth = bdNode.DrawWidth
            afNode.FillColor = bdNode.FillColor
            afNode.Font = bdNode.Font
            afNode.Gradient = bdNode.Gradient
            afNode.GradientColor = bdNode.GradientColor
            afNode.GradientMode = bdNode.GradientMode
            afNode.ImageIndex = GetDiagramImageIndex(bdNode)
            afNode.ImageLocation = bdNode.ImageLocation
            afNode.ImagePosition = CType(bdNode.ImagePosition, Lassalle.Flow.ImagePosition)
            afNode.InLinkable = bdNode.InLinkable
            afNode.LabelEdit = bdNode.LabelEdit
            afNode.OutLinkable = bdNode.OutLinkable
            afNode.Shadow.Style = CType(bdNode.ShadowStyle, Lassalle.Flow.ShadowStyle)
            afNode.Shadow.Color = bdNode.ShadowColor
            afNode.Shadow.Size = bdNode.ShadowSize
            afNode.Shape.Style = CType(bdNode.Shape, Lassalle.Flow.ShapeStyle)
            afNode.Shape.Orientation = CType(bdNode.ShapeOrientation, Lassalle.Flow.ShapeOrientation)
            afNode.Text = bdNode.Text
            afNode.TextColor = bdNode.TextColor
            afNode.TextMargin = bdNode.TextMargin
            afNode.Tooltip = bdNode.ToolTip
            afNode.Transparent = bdNode.Transparent
            afNode.Trimming = bdNode.Trimming
            afNode.Url = bdNode.Url
            afNode.XMoveable = bdNode.XMoveable
            afNode.XSizeable = bdNode.XSizeable
            afNode.YMoveable = bdNode.YMoveable
            afNode.YSizeable = bdNode.YSizeable
            afNode.Tag = bdNode.ID
            afNode.OwnerDraw = bdNode.IsOwnerDrawn

            If bAdding Then
                afNode.Location = bdNode.Location
                afNode.Size = bdNode.Size
            Else
                bdNode.BeginBatchUpdate()
                bdNode.Location = afNode.Location
                bdNode.Size = afNode.Size
                bdNode.EndBatchUpdate(False)
            End If
        End Sub


        Protected Sub UpdateAddFlowLink(ByRef afLink As Lassalle.Flow.Link, _
                                        ByRef blLink As AnimatGUI.DataObjects.Behavior.Link, _
                                        Optional ByVal bAdding As Boolean = False)

            afLink.AdjustDst = blLink.AdjustDestination
            afLink.AdjustOrg = blLink.AdjustOrigin

            If Not blLink.ArrowDestination Is Nothing Then
                afLink.ArrowDst.Style = CType(blLink.ArrowDestination.Style, Lassalle.Flow.ArrowStyle)
                afLink.ArrowDst.Size = CType(blLink.ArrowDestination.Size, Lassalle.Flow.ArrowSize)
                afLink.ArrowDst.Angle = CType(blLink.ArrowDestination.Angle, Lassalle.Flow.ArrowAngle)
                afLink.ArrowDst.Filled = blLink.ArrowDestination.Filled
            End If

            If Not blLink.ArrowMiddle Is Nothing Then
                afLink.ArrowMid.Style = CType(blLink.ArrowMiddle.Style, Lassalle.Flow.ArrowStyle)
                afLink.ArrowMid.Size = CType(blLink.ArrowMiddle.Size, Lassalle.Flow.ArrowSize)
                afLink.ArrowMid.Angle = CType(blLink.ArrowMiddle.Angle, Lassalle.Flow.ArrowAngle)
                afLink.ArrowMid.Filled = blLink.ArrowMiddle.Filled
            End If

            If Not blLink.ArrowOrigin Is Nothing Then
                afLink.ArrowOrg.Style = CType(blLink.ArrowOrigin.Style, Lassalle.Flow.ArrowStyle)
                afLink.ArrowOrg.Size = CType(blLink.ArrowOrigin.Size, Lassalle.Flow.ArrowSize)
                afLink.ArrowOrg.Angle = CType(blLink.ArrowOrigin.Angle, Lassalle.Flow.ArrowAngle)
                afLink.ArrowOrg.Filled = blLink.ArrowOrigin.Filled
            End If

            afLink.BackMode = CType(blLink.BackMode, Lassalle.Flow.BackMode)
            afLink.CustomEndCap = blLink.CustomEndCap
            afLink.CustomStartCap = blLink.CustomStartCap
            afLink.DashStyle = blLink.DashStyle
            afLink.DrawColor = blLink.DrawColor
            afLink.DrawWidth = blLink.DrawWidth
            afLink.EndCap = blLink.EndCap

            If Not blLink.Font Is Nothing Then
                afLink.Font = DirectCast(blLink.Font.Clone, System.Drawing.Font)
            End If

            afLink.Hidden = blLink.Hidden
            afLink.Jump = CType(blLink.Jump, Lassalle.Flow.Jump)
            afLink.Line.Style = CType(blLink.LineStyle, Lassalle.Flow.LineStyle)
            afLink.Line.RoundedCorner = True
            afLink.Line.OrthogonalDynamic = blLink.OrthogonalDynamic
            afLink.OrientedText = blLink.OrientedText
            afLink.Rigid = False
            afLink.Selectable = blLink.Selectable
            afLink.Stretchable = blLink.Stretchable
            afLink.StartCap = blLink.StartCap
            afLink.Text = blLink.Text
            afLink.TextColor = blLink.TextColor
            afLink.Tooltip = blLink.ToolTip
            afLink.Url = blLink.Url
            afLink.Tag = blLink.ID

        End Sub

        Protected Sub UpdateBehavioralNode(ByRef afNode As Lassalle.Flow.Node, _
                                           ByRef bdNode As AnimatGUI.DataObjects.Behavior.Node, _
                                           Optional ByVal bAdding As Boolean = False, Optional ByVal bSimple As Boolean = True)
            bdNode.BeginBatchUpdate()
            bdNode.Location = afNode.Location
            bdNode.Size = afNode.Size

            If Not bSimple Then
                bdNode.Alignment = CType(afNode.Alignment, AnimatGUI.DataObjects.Behavior.Node.enumAlignment)
                bdNode.AutoSize = CType(afNode.AutoSize, AnimatGUI.DataObjects.Behavior.Node.enumAutoSize)
                bdNode.BackMode = CType(afNode.BackMode, AnimatGUI.DataObjects.Behavior.Node.enumBackmode)
                bdNode.DashStyle = afNode.DashStyle
                bdNode.DrawColor = afNode.DrawColor
                bdNode.DrawWidth = afNode.DrawWidth
                bdNode.FillColor = afNode.FillColor
                bdNode.Font = afNode.Font
                bdNode.Gradient = afNode.Gradient
                bdNode.GradientColor = afNode.GradientColor
                bdNode.GradientMode = afNode.GradientMode
                bdNode.ImageLocation = bdNode.ImageLocation
                bdNode.ImagePosition = CType(afNode.ImagePosition, AnimatGUI.DataObjects.Behavior.Node.enumImagePosition)
                bdNode.InLinkable = afNode.InLinkable
                bdNode.LabelEdit = afNode.LabelEdit
                bdNode.OutLinkable = afNode.OutLinkable
                bdNode.ShadowStyle = CType(afNode.Shadow.Style, AnimatGUI.DataObjects.Behavior.Node.enumShadow)
                bdNode.ShadowColor = afNode.Shadow.Color
                bdNode.ShadowSize = afNode.Shadow.Size
                bdNode.Shape = CType(afNode.Shape.Style, AnimatGUI.DataObjects.Behavior.Node.enumShape)
                bdNode.ShapeOrientation = CType(afNode.Shape.Orientation, AnimatGUI.DataObjects.Behavior.Node.enumShapeOrientation)
                bdNode.Text = afNode.Text
                bdNode.TextColor = afNode.TextColor
                bdNode.TextMargin = afNode.TextMargin
                bdNode.ToolTip = afNode.Tooltip
                bdNode.Transparent = afNode.Transparent
                bdNode.Trimming = afNode.Trimming
                bdNode.Url = afNode.Url
                bdNode.XMoveable = afNode.XMoveable
                bdNode.XSizeable = afNode.XSizeable
                bdNode.YMoveable = afNode.YMoveable
                bdNode.YSizeable = afNode.YSizeable
            End If

            bdNode.EndBatchUpdate(False)
        End Sub

        Protected Overridable Function FindAddFlowNode(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As Lassalle.Flow.Node
            Dim oNode As Object = m_aryAddFlowNodes(strID)
            If oNode Is Nothing Then
                If bThrowError Then
                    Throw New System.Exception("No Addflow node was found with the following id. ID: " & strID)
                Else
                    Return Nothing
                End If
            End If

            Return DirectCast(oNode, Lassalle.Flow.Node)
        End Function

        Protected Overridable Function FindAddFlowLink(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As Lassalle.Flow.Link
            Dim oLink As Object = m_aryAddFlowLinks(strID)
            If oLink Is Nothing Then
                If bThrowError Then
                    Throw New System.Exception("No Addflow link was found with the following id. ID: " & strID)
                Else
                    Return Nothing
                End If
            End If

            Return DirectCast(oLink, Lassalle.Flow.Link)
        End Function

        Protected Overridable Function FindAddFlowItem(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As Lassalle.Flow.Item
            Dim afItem As Lassalle.Flow.Item = FindAddFlowNode(strID, False)
            If Not afItem Is Nothing Then Return afItem

            afItem = FindAddFlowLink(strID, False)
            If Not afItem Is Nothing Then Return afItem

            'If we could not find it in our list then do an exhaustive search of the actual addflow item before choking.
            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                If Not afNode.Tag Is Nothing AndAlso DirectCast(afNode.Tag, String) = strID Then
                    m_aryAddFlowNodes.Add(strID, afNode)
                    Return afNode
                Else

                    'Search through its outlinks
                    For Each afLink As Lassalle.Flow.Link In afNode.OutLinks
                        If Not afLink.Tag Is Nothing AndAlso DirectCast(afLink.Tag, String) = strID Then
                            m_aryAddFlowLinks.Add(strID, afLink)
                            Return afLink
                        End If
                    Next

                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No Addflow item was found with the following id. ID: " & strID)
            Else
                Return Nothing
            End If
        End Function

        Protected Overridable Function GetSelectedAddflowNodes() As ArrayList

            Dim aryList As New ArrayList
            For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                If TypeOf afItem Is Lassalle.Flow.Node Then
                    aryList.Add(afItem)
                End If
            Next

            Return aryList
        End Function

        Public Overrides Sub AddDiagramNode(ByRef bdNode As AnimatGUI.DataObjects.Behavior.Node)
            Dim afNode As New Lassalle.Flow.Node
            UpdateAddFlowNode(afNode, bdNode, True)

            bdNode.Tag = afNode
            afNode.Tag = bdNode.ID

            m_ctrlAddFlow.Nodes.Add(afNode)
            m_aryAddFlowNodes.Add(bdNode.ID, afNode)

            m_Timer.Enabled = True
        End Sub

        Public Overrides Sub BeginEditNode(ByRef bnNode As AnimatGUI.DataObjects.Behavior.Node)
            Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bnNode.ID)
            afNode.BeginEdit()
        End Sub

        Public Overrides Sub EndEditNode(ByRef bnNode As AnimatGUI.DataObjects.Behavior.Node, ByVal bCancel As Boolean)
            Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bnNode.ID)
            afNode.EndEdit(bCancel)
        End Sub

        Public Overrides Sub RemoveDiagramNode(ByRef bnNode As AnimatGUI.DataObjects.Behavior.Node)
            If bnNode Is Nothing Then Return

            Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bnNode.ID, False)

            m_aryAddFlowNodes.Remove(bnNode.ID)
            If Not afNode Is Nothing Then afNode.Remove()

        End Sub

        Public Overrides Function GetChartItemAt(ByVal ptPosition As Point, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Data
            Dim oItem As Lassalle.Flow.Item = m_ctrlAddFlow.GetItemAt(ptPosition)
            If oItem Is Nothing AndAlso bThrowError Then
                Throw New System.Exception("No selectable item was found on the chart at position (" & ptPosition.X & ", " & ptPosition.Y & ")")
            End If

            Dim strID As String = DirectCast(oItem.Tag, String)
            Return FindItem(strID, bThrowError)
        End Function

        Public Overrides Sub AddDiagramLink(ByRef blLink As AnimatGUI.DataObjects.Behavior.Link)

            Dim afLink As New Lassalle.Flow.Link
            UpdateAddFlowLink(afLink, blLink, True)

            blLink.Tag = afLink
            afLink.Tag = blLink.ID

            Dim afOrigin As Lassalle.Flow.Node = FindAddFlowNode(blLink.ActualOrigin.ID)
            Dim afDestination As Lassalle.Flow.Node = FindAddFlowNode(blLink.ActualDestination.ID)

            m_ctrlAddFlow.AddLink(afLink, afOrigin, afDestination)
            m_aryAddFlowLinks.Add(blLink.ID, afLink)

        End Sub

        Public Overrides Sub RemoveDiagramLink(ByRef blLink As AnimatGUI.DataObjects.Behavior.Link)
            If Not blLink Is Nothing Then

                Dim afLink As Lassalle.Flow.Link = FindAddFlowLink(blLink.ID, False)

                If Not afLink Is Nothing Then afLink.Remove()
                m_aryAddFlowLinks.Remove(blLink.ID)
            End If
        End Sub

        Public Overrides Sub UpdateChart(ByRef bdData As AnimatGUI.DataObjects.Behavior.Data)
            If TypeOf (bdData) Is AnimatGUI.DataObjects.Behavior.Node Then
                Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bdData.ID, False)
                If Not afNode Is Nothing Then
                    UpdateAddFlowNode(afNode, DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node))
                Else
                    Dim iVal As Integer = 5
                End If
            ElseIf TypeOf (bdData) Is AnimatGUI.DataObjects.Behavior.Link Then
                Dim afLink As Lassalle.Flow.Link = FindAddFlowLink(bdData.ID.ToLower, False)
                If Not afLink Is Nothing Then
                    UpdateAddFlowLink(afLink, DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Link))
                End If
            End If
        End Sub

        Public Overrides Sub UpdateData(ByRef bdData As AnimatGUI.DataObjects.Behavior.Data, Optional ByVal bSimple As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            If TypeOf (bdData) Is AnimatGUI.DataObjects.Behavior.Node Then
                Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bdData.ID, bThrowError)

                If Not afNode Is Nothing Then
                    UpdateBehavioralNode(afNode, DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node), False, bSimple)
                    'ElseIf TypeOf (bdData) Is DataObjects.Behavior.Link Then
                    '    Dim afLink As Lassalle.Flow.Link = FindAddFlowLink(bdData.ID)
                    '    UpdateAddFlowLink(afLink, DirectCast(bdData, DataObjects.Behavior.Link))
                End If
            End If
        End Sub

        Protected Sub SelectAddFlowItem(ByRef afItem As Lassalle.Flow.Item, ByVal bSelectMultiple As Boolean)
            If Not bSelectMultiple Then
                m_ctrlAddFlow.SelectedItems.Clear()
            End If
            afItem.Selected = True
        End Sub

        Protected Sub DeselectAddFlowItem(ByRef afItem As Lassalle.Flow.Item)
            afItem.Selected = False
        End Sub

        Protected Overrides Function AddImage(ByVal strImageName As String, ByVal oImage As System.Drawing.Image) As Integer

            If m_ctrlAddFlow.Images.Contains(oImage) Then
                Throw New System.Exception("Image is already contained in addflow images collection.")
            End If

            m_ctrlAddFlow.Images.Add(oImage)
            m_hashImages.Add(strImageName, oImage)

            Return m_ctrlAddFlow.Images.IndexOf(oImage)
        End Function

        Protected Overrides Sub RemoveImage(ByVal strImageName As String)

            Dim oImage As System.Drawing.Image = DirectCast(m_hashImages(strImageName), System.Drawing.Image)

            If m_ctrlAddFlow.Images.Contains(oImage) Then
                m_ctrlAddFlow.Images.Remove(oImage)
                m_hashImages.Remove(strImageName)
            End If

        End Sub

        Protected Overrides Function FindDiagramImageIndex(ByVal oImage As System.Drawing.Image, Optional ByVal bThrowError As Boolean = True) As Integer

            Dim iIndex As Integer = 0
            Dim doImage As Lassalle.Flow.FlowImage
            For Each doImage In m_ctrlAddFlow.Images
                If doImage.Image Is oImage Then
                    Return iIndex
                End If
                iIndex = iIndex + 1
            Next

            If bThrowError Then
                Throw New System.Exception("No addflow image found a given image.")
            End If
            Return -1
        End Function

        Protected Overrides Function GetDiagramImageIndex(ByVal bnNode As AnimatGUI.DataObjects.Behavior.Node) As Integer

            If bnNode Is Nothing Then Return -1

            'First check to see if there is an image name. If not then return -1
            If bnNode.DiagramImageName.Trim.Length = 0 AndAlso bnNode.ImageName.Trim.Length = 0 Then
                Return -1
            End If

            Dim strKey As String = ""
            Dim strFile As String = ""
            If bnNode.ImageName.Trim.Length > 0 Then
                strKey = bnNode.ImageName
                strFile = Util.GetFilePath(Util.Application.ProjectPath, bnNode.ImageName)
            Else
                strKey = bnNode.DiagramImageName
                strFile = Util.GetFilePath(Util.Application.ProjectPath, bnNode.DiagramImageName)
            End If

            'Then lets look in the hashtable to see if we have an image with the given name yet.
            If m_hashImages.ContainsKey(strKey) Then
                Dim oImage As System.Drawing.Image = DirectCast(m_hashImages(strKey), System.Drawing.Image)

                Return FindDiagramImageIndex(oImage)
            ElseIf m_hashImages.ContainsKey(strFile) Then
                Dim oImage As System.Drawing.Image = DirectCast(m_hashImages(strFile), System.Drawing.Image)

                Return FindDiagramImageIndex(oImage)
            Else
                'If the file exists then try and load and use it.
                If System.IO.File.Exists(strFile) Then
                    Try
                        Dim bm As New Bitmap(strFile)
                        Return Me.AddImage(strFile, bm)
                    Catch ex As System.Exception
                        'If the file cannot be loaded then try the key.
                        Try
                            Dim oImage As System.Drawing.Image = AnimatGUI.Framework.ImageManager.LoadImage(strKey)
                            Return Me.AddImage(strKey, oImage)
                        Catch ex2 As Exception
                            Throw New System.Exception("Unable to load the image. This does not appear to be a vaild image file. File: " & strFile & ", Key: " & strKey)
                        End Try
                    End Try
                Else
                    'If the file does not exist then load the key
                    Dim oImage As System.Drawing.Image = AnimatGUI.Framework.ImageManager.LoadImage(strKey)
                    Return Me.AddImage(strKey, oImage)
                End If
            End If

        End Function

        Public Overrides Sub FitToPage()
            Dim rc As RectangleF = New RectangleF(New PointF(0, 0), m_ctrlAddFlow.Extent)
            m_ctrlAddFlow.ZoomRectangle(rc, Lassalle.Flow.ZoomType.Isotropic)
        End Sub

        Public Overrides Sub ZoomBy(ByVal fltDelta As Single)

            Dim fltXZoomFactor As Single = CSng(m_ctrlAddFlow.Zoom.X + fltDelta)
            Dim fltYZoomFactor As Single = CSng(m_ctrlAddFlow.Zoom.Y + fltDelta)

            If fltXZoomFactor > 100 Then fltXZoomFactor = 100
            If fltYZoomFactor > 100 Then fltYZoomFactor = 100

            If fltXZoomFactor <= 0 Then fltXZoomFactor = 0.01
            If fltYZoomFactor <= 0 Then fltYZoomFactor = 0.01

            'If we zoom in too far and the grid is on the it looks like the app locked up, but it is really that
            'it is working like a mad dog to draw the grid at such tiny scale.
            If fltXZoomFactor < 0.2 Then
                m_ctrlAddFlow.Grid.Size = New Size(100, m_ctrlAddFlow.Grid.Size.Height)
            End If

            If fltYZoomFactor < 0.2 Then
                m_ctrlAddFlow.Grid.Size = New Size(m_ctrlAddFlow.Grid.Size.Width, 100)
            End If

            'System.Diagnostics.Debug.WriteLine("ZoomFactor: " & fltXZoomFactor & ", " & fltYZoomFactor)

            m_ctrlAddFlow.Zoom.X = fltXZoomFactor
            m_ctrlAddFlow.Zoom.Y = fltYZoomFactor

            'For some reason the diagrams are not sizing correctly once they are zoomed. 
            'I am doing this just to make the editor as a whole call the resize code.
            'This fixes the problem, but it is ugly looking
            m_Timer.Enabled = True
        End Sub

        Public Overrides Sub ZoomTo(ByVal fltZoom As Single)

            Dim fltXZoomFactor As Single = fltZoom
            Dim fltYZoomFactor As Single = fltZoom

            If fltXZoomFactor > 100 Then fltXZoomFactor = 100
            If fltYZoomFactor > 100 Then fltYZoomFactor = 100

            If fltXZoomFactor <= 0 Then fltXZoomFactor = 0.1
            If fltYZoomFactor <= 0 Then fltYZoomFactor = 0.1

            m_ctrlAddFlow.Zoom.X = fltXZoomFactor
            m_ctrlAddFlow.Zoom.Y = fltYZoomFactor

            'For some reason the diagrams are not sizing correctly once they are zoomed. 
            'I am doing this just to make the editor as a whole call the resize code.
            'This fixes the problem, but it is ugly looking
            m_Timer.Enabled = True
        End Sub

        Public Overrides Sub BeginGraphicsUpdate()
            m_ctrlAddFlow.BeginUpdate()
        End Sub

        Public Overrides Sub EndGraphicsUpdate()
            m_ctrlAddFlow.EndUpdate()
        End Sub

        Public Overrides Sub RefreshDiagram()
            m_ctrlAddFlow.Refresh()
        End Sub

#Region " Undo/Redo Synchronization "

        'When we add a link we first let the user draw a link and then remove it and add new links. However, when doing undo/redo operations
        'this causes a problem because there is now a line on the graph that does not have a reference to a tag with any of the behavioral nodes.
        'This method is run after an undo/redo operation and looks for any links with blank Tag values. If it finds them then it does an undo or redo 
        'to get rid of them.
        Protected Overridable Sub RemoveIntermediateLinks(ByVal bUndo As Boolean)

            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                For Each afLink As Lassalle.Flow.Link In afNode.OutLinks
                    If Not TypeOf (afLink.Tag) Is String Then
                        If bUndo Then
                            m_ctrlAddFlow.Undo()
                        Else
                            m_ctrlAddFlow.Redo()
                        End If

                        Return
                    End If
                Next
            Next

        End Sub

        'When the user undo's an action then nodes and links that were deleted could be added back onto the graph. This would present 
        'a problem becuase the underlying behavioral nodes and links would still be deleted. If they try and do anything to those nodes
        'they will get an error message. This method goes back through all items on the graph and makes sure that the id they have listed
        'is really in the valid lists. If it nots then it reconnects them.
        Protected Overridable Sub SynchronizeAddedNodes()
            'TODO
            'Dim bnNode As AnimatGUI.DataObjects.Behavior.Node

            ''First lets go through the nodes on the chart and try and sync them back up
            'For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
            '    If m_aryNodes(DirectCast(afNode.Tag, String)) Is Nothing Then
            '        'We could not find an existing node that matched this addflow node id.
            '        'so we need to try and find a delete node and add it back in.
            '        If Not m_aryDeletedNodes(DirectCast(afNode.Tag, String)) Is Nothing Then
            '            bnNode = DirectCast(m_aryDeletedNodes(DirectCast(afNode.Tag, String)), AnimatGUI.DataObjects.Behavior.Node)
            '            bnNode.BeforeUndoRemove()
            '            m_aryNodes.Add(bnNode.ID, bnNode, True)
            '            m_aryAddFlowNodes.Add(bnNode.ID, afNode)
            '            m_aryDeletedNodes.Remove(bnNode.ID)
            '            AddToOrganism(bnNode)
            '            bnNode.AfterUndoRemove()
            '        Else
            '            Throw New System.Exception("A deleted node was not found while trying to add it back to the diagram.")
            '        End If
            '    End If
            'Next

        End Sub

        Protected Overridable Sub SynchronizeRemovedNodes()
            'TODO
            'Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            'Dim aryRemove As New ArrayList

            ''First lets go through and set all of the nodes to found = false
            'Dim iCount As Integer = m_aryNodes.Count - 1
            'For iIndex As Integer = 0 To iCount
            '    bnNode = DirectCast(m_aryNodes.GetByIndex(iIndex), AnimatGUI.DataObjects.Behavior.Node)
            '    bnNode.Found = False
            'Next

            ''Now go through and mark found = true for all nodes in the diagram.
            'For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
            '    bnNode = FindNode(DirectCast(afNode.Tag, String))
            '    bnNode.Found = True
            'Next

            ''Now we need to go through and any nodes that are marked found = false were in the diagram, but no longer in the list of nodes.
            'For iIndex As Integer = 0 To iCount
            '    bnNode = DirectCast(m_aryNodes.GetByIndex(iIndex), AnimatGUI.DataObjects.Behavior.Node)

            '    If Not bnNode.Found Then
            '        'If it was not found in the chart then we need to remove the sucker. 
            '        'Add it to the list of links to remove
            '        aryRemove.Add(bnNode)
            '    End If
            'Next

            ''Now loop through all of the items in the remove list and remove them
            'For Each oNode As Object In aryRemove
            '    bnNode = DirectCast(oNode, AnimatGUI.DataObjects.Behavior.Node)

            '    bnNode.BeforeRedoRemove()
            '    m_aryNodes.Remove(bnNode.ID, True)
            '    m_aryAddFlowNodes.Remove(bnNode.ID)
            '    RemoveFromOrganism(bnNode)

            '    If m_aryDeletedNodes(bnNode.ID) Is Nothing Then
            '        m_aryDeletedNodes.Add(bnNode.ID, bnNode)
            '    End If

            '    bnNode.AfterRedoRemove()
            'Next

        End Sub

        Public Overrides Sub BeginGroupChange()
            'TODO
            'If Not m_beEditor.InGroupChange Then
            '    m_ctrlAddFlow.BeginUpdate()
            '    m_ctrlAddFlow.BeginAction(m_beEditor.GetNextUndoCode())
            '    m_beEditor.InGroupChange = True
            'End If
            'm_beEditor.GroupChangeCounter = m_beEditor.GroupChangeCounter + 1
        End Sub

        Public Overrides Sub EndGroupChange()
            'TODO
            'm_beEditor.GroupChangeCounter = m_beEditor.GroupChangeCounter - 1
            'If m_beEditor.InGroupChange AndAlso m_beEditor.GroupChangeCounter <= 0 Then
            '    m_ctrlAddFlow.EndAction()
            '    m_ctrlAddFlow.EndUpdate()
            '    m_beEditor.InGroupChange = False
            'End If
        End Sub

#End Region

#Region " Copy/Paste Methods "

        Public Overrides Sub CutSelected()

            Try
                CopySelected()
                Util.Application.OnDeleteFromWorkspace(Me, Nothing)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub CopySelected()

            Try
                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

                Dim bSave As Boolean = SaveSelected(oXml, True)

                'If there is nothing to save then exit
                If Not bSave Then Return

                'oXml.Save("C:\Projects\bin\Experiments\Copy.txt")
                Dim strXml As String = oXml.Serialize()

                Dim data As New System.Windows.Forms.DataObject
                data.SetData("AnimatLab.Behavior.XMLFormat", strXml)
                Clipboard.SetDataObject(data, True)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub PasteSelected(ByVal bInPlace As Boolean)

            Try
                Dim data As IDataObject = Clipboard.GetDataObject()
                If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Behavior.XMLFormat") Then
                    ' We first unselect the selected items
                    m_ctrlAddFlow.SelectedItems.Clear()

                    Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.GetXmlForPaste(data, "AnimatLab.Behavior.XMLFormat", "Diagram")

                    If Not oXml Is Nothing Then
                        LoadPasted(oXml, bInPlace)
                    End If

                    m_Timer.Enabled = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Public Overrides Sub SendSelectedToBack()

            Try

                For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                    afItem.ZOrder = 0
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub BringSelectedToFront()

            Try

                For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                    afItem.ZOrder = 1
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub OnUndo()
            'TODO
            'If m_ctrlAddFlow.CanUndo Then
            '    m_ctrlAddFlow.Undo()
            '    RemoveIntermediateLinks(True)
            '    SynchronizeAddedNodes()
            '    SynchronizeAddedLinks()
            '    SynchronizeRemovedLinks()
            '    SynchronizeRemovedNodes()

            '    'If the node/link that was currently selected is no longer on the form then deselect it from the properties bar
            '    If Not m_beEditor.SelectedObject Is Nothing AndAlso TypeOf m_beEditor.SelectedObject Is AnimatGUI.DataObjects.Behavior.Data Then
            '        Dim bdData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(m_beEditor.SelectedObject, AnimatGUI.DataObjects.Behavior.Data)
            '        If Me.FindItem(bdData.ID, False) Is Nothing Then
            '            m_beEditor.SelectedObject = Nothing
            '            'Me.Editor.PropertiesBar.PropertyData = Nothing
            '        End If
            '    End If
            'End If
        End Sub

        Public Overrides Sub OnRedo()
            'TODO
            'If m_ctrlAddFlow.CanRedo Then
            '    m_ctrlAddFlow.Redo()
            '    RemoveIntermediateLinks(False)
            '    SynchronizeAddedNodes()
            '    SynchronizeAddedLinks()
            '    SynchronizeRemovedLinks()
            '    SynchronizeRemovedNodes()

            '    'If the node/link that was currently selected is no longer on the form then deselect it from the properties bar
            '    If Not m_beEditor.SelectedObject Is Nothing AndAlso TypeOf m_beEditor.SelectedObject Is AnimatGUI.DataObjects.Behavior.Data Then
            '        Dim bdData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(m_beEditor.SelectedObject, AnimatGUI.DataObjects.Behavior.Data)
            '        If Me.FindItem(bdData.ID, False) Is Nothing Then
            '            m_beEditor.SelectedObject = Nothing
            '            'Me.Editor.PropertiesBar.PropertyData = Nothing
            '        End If
            '    End If
            'End If
        End Sub

        Protected Function FindHalfwayLocation(ByVal bnOrigin As AnimatGUI.DataObjects.Behavior.Node, _
                                               ByVal bnDestination As AnimatGUI.DataObjects.Behavior.Node, _
                                               ByVal szAdapterSize As SizeF) As PointF
            Dim ptPoint As New PointF
            Dim ptOriginCenter As PointF
            Dim ptDestCenter As PointF

            ptOriginCenter.X = bnOrigin.Location.X + bnOrigin.Size.Width / 2
            ptOriginCenter.Y = bnOrigin.Location.Y + bnOrigin.Size.Height / 2
            ptDestCenter.X = bnDestination.Location.X + bnDestination.Size.Width / 2
            ptDestCenter.Y = bnDestination.Location.Y + bnDestination.Size.Height / 2

            If ptOriginCenter.X > ptDestCenter.X Then
                ptPoint.X = ptDestCenter.X + ((ptOriginCenter.X - ptDestCenter.X) / 2)
            Else
                ptPoint.X = ptOriginCenter.X + ((ptDestCenter.X - ptOriginCenter.X) / 2)
            End If

            If ptOriginCenter.Y > ptDestCenter.Y Then
                ptPoint.Y = ptDestCenter.Y + ((ptOriginCenter.Y - ptDestCenter.Y) / 2)
            Else
                ptPoint.Y = ptOriginCenter.Y + ((ptDestCenter.Y - ptOriginCenter.Y) / 2)
            End If

            ptPoint.X = ptPoint.X - (szAdapterSize.Width / 2)
            ptPoint.Y = ptPoint.Y - (szAdapterSize.Height / 2)

            Return ptPoint
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Draw Grid", GetType(Boolean), "DrawGrid", _
                                        "Diagram Properties", "Determines whether the grid is drawn or not.", Me.DrawGrid))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Color", GetType(System.Drawing.Color), "GridColor", _
                                        "Diagram Properties", "Sets the color of the grid.", Me.GridColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Size", GetType(System.Drawing.Size), "GridSize", _
                                        "Diagram Properties", "Sets the size of the space (width or height) between grid lines.", Me.GridSize))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Snap To Grid", GetType(Boolean), "GridSnap", _
                                        "Diagram Properties", "Determines whether the diagram items snap to the grid locations.", Me.GridSnap))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Style", GetType(enumGridStyle), "GridStyle", _
                                        "Diagram Properties", "Sets the style of line used to draw the grid.", Me.GridStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Back Color", GetType(System.Drawing.Color), "AddFlowBackColor", _
                                        "Diagram Properties", "Sets the background color for the control.", Me.AddFlowBackColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Jump Size", GetType(enumJumpSize), "JumpSize", _
                                        "Diagram Properties", "Sets the size of the jumps at the intersection of links.", Me.JumpSize))


        End Sub

        Public Overridable Sub RelabelRegEx()
            Try
                Dim frmRelabel As New AnimatGUI.Forms.Behavior.Relabel

                frmRelabel.Diagram = Me
                frmRelabel.ShowDialog 

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Menu Methods "

        Public Overrides Sub ValidateEditToolStripItemState()
            MyBase.ValidateEditToolStripItemState()

            If m_ctrlAddFlow.SelectedItems.Count = 0 Then
                Me.BringToFrontMenuItem.Enabled = False
                Me.SendToBackMenuItem.Enabled = False
            Else
                Me.BringToFrontMenuItem.Enabled = True
                Me.SendToBackMenuItem.Enabled = True
            End If

        End Sub

        Protected Sub CreateDiagramPopupMenu(ByVal ptScreen As Point)
            '
            'SelectByTypeToolStripMenuItem
            '
            Me.SendToBackMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.SendToBack.gif")
            Me.SendToBackMenuItem.Name = "SendToBackToolStripMenuItem"
            Me.SendToBackMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.SendToBackMenuItem.Text = "Send to back"
            Me.SendToBackMenuItem.ToolTipText = "Send to back"
            '
            'SelectByTypeToolStripMenuItem
            '
            Me.BringToFrontMenuItem.Image = AnimatGUI.Framework.ImageManager.LoadImage("AnimatGUI.BringToFront.gif")
            Me.BringToFrontMenuItem.Name = "BringToFrontToolStripMenuItem"
            Me.BringToFrontMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.BringToFrontMenuItem.Text = "Bring to front"
            Me.BringToFrontMenuItem.ToolTipText = "Bring to front"



            '
            'AddFlowMenuStrip
            '
            Dim tsPopupMenu As New AnimatContextMenuStrip("AddFlowDiagramMenu", Nothing)

            Dim mcCut As New System.Windows.Forms.ToolStripMenuItem("Cut", Util.Application.ToolStripImages.GetImage("AnimatGUI.Cut.gif"), New EventHandler(AddressOf Me.CutToolStripButton_Click))
            Dim mcCopy As New System.Windows.Forms.ToolStripMenuItem("Copy", Util.Application.ToolStripImages.GetImage("AnimatGUI.Copy.gif"), New EventHandler(AddressOf Me.CopyToolStripButton_Click))
            Dim mcPaste As New System.Windows.Forms.ToolStripMenuItem("Paste", Util.Application.ToolStripImages.GetImage("AnimatGUI.CopyClipboard.gif"), New EventHandler(AddressOf Me.PasteInPlaceMenuItem_Click))
            Dim mcPasteInPlace As New System.Windows.Forms.ToolStripMenuItem("Paste in Place", Nothing, New EventHandler(AddressOf Me.PasteInPlaceMenuItem_Click))
            Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
            'mcCut.ImageScaling = ToolStripItemImageScaling.SizeToFit

            Dim mcSelectByType As New System.Windows.Forms.ToolStripMenuItem("Select by type", Util.Application.ToolStripImages.GetImage("AnimatGUI.SelectByType.gif"), New EventHandler(AddressOf Util.Application.OnSelectByType))
            Dim mcRelabel As New System.Windows.Forms.ToolStripMenuItem("Relabel", Util.Application.ToolStripImages.GetImage("AnimatGUI.Relabel.gif"), New EventHandler(AddressOf Me.OnRelabel))
            Dim mcRelabelSelected As New System.Windows.Forms.ToolStripMenuItem("Relable selected", Util.Application.ToolStripImages.GetImage("AnimatGUI.RelabelSelected.gif"), New EventHandler(AddressOf Util.Application.OnRelabelSelected))
            Dim mcCompareItems As New System.Windows.Forms.ToolStripMenuItem("Compare items", Util.Application.ToolStripImages.GetImage("AnimatGUI.Equals.gif"), New EventHandler(AddressOf Util.Application.OnCompareItems))

            Dim mcSendToBack As New System.Windows.Forms.ToolStripMenuItem("Send to back", Util.Application.ToolStripImages.GetImage("AnimatGUI.SendToBack.gif"), New EventHandler(AddressOf Me.SendToBackMenuItem_Click))
            Dim mcBringToFront As New System.Windows.Forms.ToolStripMenuItem("Bring to front", Util.Application.ToolStripImages.GetImage("AnimatGUI.BringToFront.gif"), New EventHandler(AddressOf Me.BringToFrontMenuItem_Click))

            Dim mcAlign As New System.Windows.Forms.ToolStripMenuItem("Align", Util.Application.ToolStripImages.GetImage("AnimatGUI.Align.gif"))
            Dim mcAlignTop As New System.Windows.Forms.ToolStripMenuItem("Top", Util.Application.ToolStripImages.GetImage("AnimatGUI.AlignTop.gif"), New EventHandler(AddressOf Me.AlignTopMenuItem_Click))
            Dim mcAlignVerticalCenter As New System.Windows.Forms.ToolStripMenuItem("Vertical center", Util.Application.ToolStripImages.GetImage("AnimatGUI.AlignVerticalCenter.gif"), New EventHandler(AddressOf Me.AlignVerticalCenterMenuItem_Click))
            Dim mcAlignBottom As New System.Windows.Forms.ToolStripMenuItem("Bottom", Util.Application.ToolStripImages.GetImage("AnimatGUI.AlignBottom.gif"), New EventHandler(AddressOf Me.AlignBottomMenuItem_Click))
            Dim mcAlignLeft As New System.Windows.Forms.ToolStripMenuItem("Left", Util.Application.ToolStripImages.GetImage("AnimatGUI.AlignLeft.gif"), New EventHandler(AddressOf Me.AlignLeftMenuItem_Click))
            Dim mcAlignHorizontalCenter As New System.Windows.Forms.ToolStripMenuItem("Horizontal center", Util.Application.ToolStripImages.GetImage("AnimatGUI.AlignHorizontalCenter.gif"), New EventHandler(AddressOf Me.AlignHorizontalCenterMenuItem_Click))
            Dim mcAlignRight As New System.Windows.Forms.ToolStripMenuItem("Right", Util.Application.ToolStripImages.GetImage("AnimatGUI.AlignRight.gif"), New EventHandler(AddressOf Me.AlignRightMenuItem_Click))
            mcAlign.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {mcAlignTop, mcAlignVerticalCenter, mcAlignBottom, mcAlignLeft, mcAlignHorizontalCenter, mcAlignRight})

            Dim mcDistribute As New System.Windows.Forms.ToolStripMenuItem("Distribute", Util.Application.ToolStripImages.GetImage("AnimatGUI.Distribute.gif"))
            Dim mcDistributeVertical As New System.Windows.Forms.ToolStripMenuItem("Vertical", Util.Application.ToolStripImages.GetImage("AnimatGUI.DistributeVertical.gif"), New EventHandler(AddressOf Me.DistributeVerticalMenuItem_Click))
            Dim mcDistributeHorizontal As New System.Windows.Forms.ToolStripMenuItem("Horizontal", Util.Application.ToolStripImages.GetImage("AnimatGUI.DistributeHorizontal.gif"), New EventHandler(AddressOf Me.DistributeHorizontalMenuItem_Click))
            mcDistribute.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {mcDistributeVertical, mcDistributeHorizontal})

            Dim mcSize As New System.Windows.Forms.ToolStripMenuItem("Size", Util.Application.ToolStripImages.GetImage("AnimatGUI.Size.gif"))
            Dim mcSizeBoth As New System.Windows.Forms.ToolStripMenuItem("Both", Util.Application.ToolStripImages.GetImage("AnimatGUI.SizeBoth.gif"), New EventHandler(AddressOf Me.SizeBothMenuItem_Click))
            Dim mcSizeWidth As New System.Windows.Forms.ToolStripMenuItem("Width", Util.Application.ToolStripImages.GetImage("AnimatGUI.SizeWidth.gif"), New EventHandler(AddressOf Me.SizeWidthMenuItem_Click))
            Dim mcSizeHeight As New System.Windows.Forms.ToolStripMenuItem("Height", Util.Application.ToolStripImages.GetImage("AnimatGUI.SizeHeight.gif"), New EventHandler(AddressOf Me.SizeHeightMenuItem_Click))
            mcSize.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {mcSizeBoth, mcSizeWidth, mcSizeHeight})

            Dim mcAddStimulus As New System.Windows.Forms.ToolStripMenuItem("Add Stimulus", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddStimulus.gif"), New EventHandler(AddressOf Util.Application.OnAddStimulus))

            Dim mcAddToChart As System.Windows.Forms.ToolStripMenuItem
            If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing AndAlso Not Util.Application.LastSelectedChart Is Nothing AndAlso Not Util.Application.LastSelectedChart.LastSelectedAxis Is Nothing Then
                If Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType, GetType(DragObject), False) Then
                    Dim doObj As DragObject = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DragObject)

                    If doObj.CanBeCharted Then
                        ' Create the menu items
                        mcAddToChart = New System.Windows.Forms.ToolStripMenuItem("Add to Chart", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddChartItem.gif"), New EventHandler(AddressOf Util.Application.OnAddToChart))
                        tsPopupMenu.Items.Add(mcAddToChart)
                    End If
                End If

            End If

            Dim mcShowConnections As New System.Windows.Forms.ToolStripMenuItem("Show connections", Util.Application.ToolStripImages.GetImage("AnimatGUI.Connections.gif"), New EventHandler(AddressOf Me.ShowConnectionsToolStripMenuItem_Click))
            Dim mcExport As New System.Windows.Forms.ToolStripMenuItem("Export", Util.Application.ToolStripImages.GetImage("AnimatGUI.Export.gif"), New EventHandler(AddressOf Me.ExportToolStripMenuItem_Click))
            Dim mcPrint As New System.Windows.Forms.ToolStripMenuItem("Print", Util.Application.ToolStripImages.GetImage("AnimatGUI.Print.gif"), New EventHandler(AddressOf Me.PrintToolStripMenuItem_Click))
            Dim mcGrid As New System.Windows.Forms.ToolStripMenuItem("Grid", Nothing, New EventHandler(AddressOf Me.GridMenuItem_Click))
            mcGrid.CheckOnClick = True
            mcGrid.Checked = Me.GridMenuItem.Checked

            Dim bPaste As Boolean = False
            Dim data As IDataObject = Clipboard.GetDataObject()
            If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Behavior.XMLFormat") Then
                Dim strXml As String = DirectCast(data.GetData("AnimatLab.Behavior.XMLFormat"), String)
                If strXml.Trim.Length > 0 Then
                    bPaste = True
                End If
            End If

            Dim toolStripSeparator2 As New ToolStripSeparator()

            If m_ctrlAddFlow.SelectedItems.Count > 0 Then
                tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcCut, mcCopy, mcDelete})

                tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {toolStripSeparator2})
            Else
                If bPaste Then
                    tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcPaste, mcPasteInPlace})
                    tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {toolStripSeparator2})
                End If
            End If


            tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcSelectByType, mcRelabel})

            If m_ctrlAddFlow.SelectedItems.Count > 0 Then
                tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcRelabelSelected})
            End If

            If m_ctrlAddFlow.SelectedItems.Count > 1 Then
                tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcCompareItems})
            End If

            If m_ctrlAddFlow.SelectedItems.Count > 0 Then
                'ToolStripSeparator3
                '
                Dim toolStripSeparator3 As New ToolStripSeparator()

                tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {toolStripSeparator3, mcBringToFront, mcSendToBack})

                If m_ctrlAddFlow.SelectedItems.Count > 1 Then
                    tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcAlign, mcDistribute, mcSize})
                End If
            End If

            'ToolStripSeparator5
            '
            Dim toolStripSeparator5 As New ToolStripSeparator()

            tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {toolStripSeparator5})

            If m_ctrlAddFlow.SelectedItems.Count > 0 Then
                If Not mcAddToChart Is Nothing Then
                    tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcAddStimulus, mcAddToChart, mcShowConnections})
                Else
                    tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcAddStimulus, mcShowConnections})
                End If
            End If

            tsPopupMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExport, mcPrint, mcGrid})

            tsPopupMenu.Show(ptScreen)

        End Sub

#End Region

#Region " Load/Save "


        Public Overrides Sub InitializeAfterLoad()
            Dim aryDeleteNodes As New ArrayList
            Dim aryDeleteLinks As New ArrayList

            Try
                aryDeleteNodes.Clear()
                aryDeleteLinks.Clear()

                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralNodes
                    Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    bnNode.ParentDiagram = Me

                    Try
                        Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bnNode.ID)
                        afNode.Tag = bnNode.ID
                        bnNode.Tag = afNode
                        UpdateAddFlowNode(afNode, bnNode)
                    Catch ex As Exception
                        aryDeleteNodes.Add(bnNode)
                    End Try

                Next

                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralLinks
                    Dim blLink As AnimatGUI.DataObjects.Behavior.Link = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    blLink.ParentDiagram = Me

                    Try
                        Dim afLink As Lassalle.Flow.Link = FindAddFlowLink(blLink.ID)
                        afLink.Tag = blLink.ID
                        blLink.Tag = afLink
                        UpdateAddFlowLink(afLink, blLink)
                    Catch ex As Exception
                        aryDeleteLinks.Add(blLink)
                    End Try

                Next

                For Each blLink As AnimatGUI.DataObjects.Behavior.Link In aryDeleteLinks
                    blLink.Delete(False)
                Next

                For Each bnNode As AnimatGUI.DataObjects.Behavior.Node In aryDeleteNodes
                    bnNode.Delete(False)
                Next

                aryDeleteNodes.Clear()
                aryDeleteLinks.Clear()

                'Now go through each adflow node and link and find any that do not have an associated tag and delete them.
                For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                    If afNode.Tag Is Nothing OrElse FindNode(afNode.Tag.ToString, False) Is Nothing Then aryDeleteNodes.Add(afNode)

                    For Each afLink As Lassalle.Flow.Link In afNode.InLinks
                        If afLink.Tag Is Nothing OrElse FindLink(afLink.Tag.ToString, False) Is Nothing Then aryDeleteLinks.Add(afLink)
                    Next
                Next

                For Each afLink As Lassalle.Flow.Link In aryDeleteLinks
                    afLink.Remove()
                Next

                For Each afNode As Lassalle.Flow.Node In aryDeleteNodes
                    afNode.Remove()
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub VerifyData()
            Try

                CheckForInvalidLinks()
                VerifyNodesExist()

                'Now go through and check for errors in all of the nodes and links.
                Dim bdItem As AnimatGUI.DataObjects.Behavior.Data
                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralNodes
                    bdItem = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)
                    bdItem.CheckForErrors()
                Next

                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralLinks
                    bdItem = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)
                    bdItem.CheckForErrors()
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        'This method makes sure that the dataobject nodes and the addflow nodes are synchronized. If for some reason an error occurs
        'and graphical nodes are in addflow that do not have matching dataobject nodes, or vice versa, we need to identify these and 
        'remove them because the user will not be able to and they will continually generate errors.
        Protected Overrides Sub VerifyNodesExist()

            Try
                Dim aryRemove As New ArrayList

                'First lets go through and make sure that all of the addflow nodes have corresponding data object nodes.
                Dim bdData As AnimatGUI.DataObjects.Behavior.Data
                Dim afItem As Lassalle.Flow.Item
                For Each afItem In m_ctrlAddFlow.Items

                    If Not afItem.Tag Is Nothing AndAlso TypeOf afItem.Tag Is String Then
                        bdData = FindItem(DirectCast(afItem.Tag, String), False)

                        'If it is nothing then we are missing a dataobject.
                        If bdData Is Nothing Then
                            Debug.WriteLine("No data item found for an addflow item. Text: " & afItem.Text & " Tag: " & DirectCast(afItem.Tag, String))
                            aryRemove.Add(afItem)
                        End If
                    End If
                Next

                For Each afItem In aryRemove
                    If TypeOf afItem Is Lassalle.Flow.Node Then
                        Dim afNode As Lassalle.Flow.Node = DirectCast(afItem, Lassalle.Flow.Node)
                        afNode.Remove()
                    ElseIf TypeOf afItem Is Lassalle.Flow.Link Then
                        Dim afLink As Lassalle.Flow.Link = DirectCast(afItem, Lassalle.Flow.Link)
                        afLink.Remove()
                    End If
                Next

                'Now we need to make sure that there are not any dataobjects that are not also associated with an
                'addflow node.
                Dim bdNode As AnimatGUI.DataObjects.Behavior.Node
                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralNodes
                    bdNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    afItem = Me.FindAddFlowItem(bdNode.ID, False)

                    If afItem Is Nothing Then
                        aryRemove.Add(bdNode)
                    End If
                Next

                For Each bdNode In aryRemove
                    Debug.WriteLine("Removeing Node: " & bdNode.ID & "  Text: " & bdNode.Text)
                    Me.Subsystem.RemoveNode(bdNode)
                Next

                aryRemove.Clear()
                Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    afItem = Me.FindAddFlowItem(bdLink.ID, False)

                    If afItem Is Nothing Then
                        aryRemove.Add(bdLink)
                    End If
                Next

                For Each bdLink In aryRemove
                    Debug.WriteLine("Removeing Link: " & bdLink.ID & "  Text: " & bdLink.Text)
                    Me.Subsystem.RemoveLink(bdLink)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            'Now find the organism and subsystem we need to load the rest of the chart.
            oXml.IntoElem()
            Dim strOrganismID As String = oXml.GetChildString("OrganismID")
            Dim strSubSystemID As String = oXml.GetChildString("SubSystemID")
            oXml.OutOfElem()

            'Now find that subsystem.
            Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = Util.Environment.FindOrganism(strOrganismID)
            m_bnSubSystem = DirectCast(doOrganism.FindBehavioralNode(strSubSystemID), AnimatGUI.DataObjects.Behavior.Nodes.Subsystem)
            m_bnSubSystem.SubsystemDiagram = Me

            LoadDiagramXml(m_bnSubSystem.DiagramXml)

            InitializeAfterLoad()
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()
            oXml.AddChildElement("OrganismID", m_bnSubSystem.Organism.ID)
            oXml.AddChildElement("SubSystemID", m_bnSubSystem.ID)
            oXml.OutOfElem()

        End Sub

        'AddFlow Diagrams have no sim object
        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
        End Sub

        Public Overrides Sub LoadDiagramXml(ByVal strXml As String)
            'Only attempt to load this if there is something in the string.
            If strXml.Trim.Length > 0 Then
                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
                oXml.Deserialize(strXml)

                oXml.FindElement("Root")
                oXml.FindChildElement("Diagram")
                oXml.IntoElem() 'Into Form Element

                m_ctrlAddFlow.Zoom.X = oXml.GetChildFloat("ZoomX", m_ctrlAddFlow.Zoom.X)
                m_ctrlAddFlow.Zoom.Y = oXml.GetChildFloat("ZoomY", m_ctrlAddFlow.Zoom.Y)

                If oXml.FindChildElement("BackColor", False) Then
                    m_ctrlAddFlow.BackColor = Util.LoadColor(oXml, "BackColor")
                End If

                m_ctrlAddFlow.Grid.Draw = oXml.GetChildBool("ShowGrid", m_ctrlAddFlow.Grid.Draw)

                If oXml.FindChildElement("GridColor", False) Then
                    m_ctrlAddFlow.Grid.Color = Util.LoadColor(oXml, "GridColor")
                End If

                If oXml.FindChildElement("GridSize", False) Then
                    m_ctrlAddFlow.Grid.Size = Util.LoadSize(oXml, "GridSize")
                End If

                Me.GridStyle = DirectCast([Enum].Parse(GetType(enumGridStyle), oXml.GetChildString("GridStyle", Me.GridStyle.ToString), True), enumGridStyle)
                Me.JumpSize = DirectCast([Enum].Parse(GetType(enumJumpSize), oXml.GetChildString("JumpSize", Me.JumpSize.ToString), True), enumJumpSize)

                m_ctrlAddFlow.Grid.Snap = oXml.GetChildBool("SnapToGrid", m_ctrlAddFlow.Grid.Snap)

                oXml.FindChildElement("AddFlow")
                Dim strAddFlowXml As String = oXml.GetChildDoc()
                Dim stringReader As System.IO.StringReader = New System.IO.StringReader(strAddFlowXml)
                Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(stringReader)
                Lassalle.XMLFlow.Serial.XMLToFlow(xmlReader, m_ctrlAddFlow)

                'Now we need to go through and add all of the addflow nodes and links into the dictionaries for them.
                For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                    Dim strNodeID As String = DirectCast(afNode.Tag, String)
                    If Not strNodeID Is Nothing AndAlso strNodeID.Length > 0 Then
                        Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = DirectCast(Me.Subsystem.BehavioralNodes.FindObjectByID(strNodeID), AnimatGUI.DataObjects.Behavior.Node)

                        afNode.ImageIndex = GetDiagramImageIndex(bnNode)

                        m_aryAddFlowNodes.Add(strNodeID, afNode)

                        For Each afLink As Lassalle.Flow.Link In afNode.InLinks
                            m_aryAddFlowLinks.Add(DirectCast(afLink.Tag, String), afLink)
                        Next
                    Else
                        Dim ex As New System.Exception("The node '" & afNode.Text & "' did not have an ID associated with its tag. It will be removed from the subsystem.")
                        Util.DisplayError(ex)
                    End If
                Next

                oXml.OutOfElem()  'Outof Diagram Element

                InitializeAfterLoad()
            End If
        End Sub

        Public Overrides Function SaveDiagramXml() As String
            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

            oXml.AddElement("Root")
            oXml.AddChildElement("Diagram")

            oXml.IntoElem() 'Into Diagram Element

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("PageName", Me.TabPageName)

            oXml.AddChildElement("ZoomX", m_ctrlAddFlow.Zoom.X)
            oXml.AddChildElement("ZoomY", m_ctrlAddFlow.Zoom.Y)

            Util.SaveColor(oXml, "BackColor", m_ctrlAddFlow.BackColor)
            oXml.AddChildElement("ShowGrid", m_ctrlAddFlow.Grid.Draw)
            Util.SaveColor(oXml, "GridColor", m_ctrlAddFlow.Grid.Color)
            Util.SaveSize(oXml, "GridSize", m_ctrlAddFlow.Grid.Size)
            oXml.AddChildElement("GridStyle", Me.GridStyle.ToString)
            oXml.AddChildElement("JumpSize", Me.JumpSize.ToString)
            oXml.AddChildElement("SnapToGrid", m_ctrlAddFlow.Grid.Snap)

            'Save the addflow configuration
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim stringWriter As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim xmlWriter As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(stringWriter)
            Lassalle.XMLFlow.Serial.FlowToXML(xmlWriter, m_ctrlAddFlow, False, False)
            Dim strAddFlowXml As String = sb.ToString & vbCrLf

            'We must remove the Xml header infomration before adding it as a child document
            strAddFlowXml = strAddFlowXml.Remove(0, 69)

            oXml.AddChildDoc(strAddFlowXml)

            oXml.OutOfElem()  'Outof Diagram Element

            Return oXml.Serialize()
        End Function

        Public Overrides Sub ExportDiagram(ByVal strFilename As String, ByVal eFormat As System.Drawing.Imaging.ImageFormat)
            Dim mfFile As Metafile = m_ctrlAddFlow.ExportMetafile(False, True, True)
            mfFile.Save(strFilename, eFormat)
        End Sub

        Public Overrides Function SaveSelected(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal bCopy As Boolean) As Boolean

            If m_ctrlAddFlow.SelectedItems.Count = 0 Then Return False

            oXml.AddElement("Diagram")

            'First lets sort the selected items into nodes and links and generate temp selected ids
            Dim aryReplaceIDs As New ArrayList
            Dim aryNodes As New ArrayList
            Dim aryLinks As New ArrayList
            Dim aryItems As New ArrayList
            Dim bdData As AnimatGUI.DataObjects.Behavior.Data
            Dim aryData As New ArrayList
            Dim aryDeselect As New ArrayList

            'Call BeforeCopy first
            For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                bdData = FindItem(DirectCast(afItem.Tag, String))
                bdData.AddToRecursiveSelectedItemsList(aryItems)
            Next

            For Each bdData In aryItems
                bdData.BeforeCopy(aryItems)
            Next

            'First get a list of all selected items. 
            'We are regenerating this list because the BeforeCopy could deselect some items that should not be copied.
            aryItems.Clear()
            For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                bdData = FindItem(DirectCast(afItem.Tag, String))
                bdData.AddToRecursiveSelectedItemsList(aryItems)
            Next

            'Now check to make sure each object can be copied, and sort into appropriate lists.
            For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                bdData = FindItem(DirectCast(afItem.Tag, String))

                If bdData.CanCopy(aryItems) Then
                    If TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Node Then
                        aryNodes.Add(DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node))
                    ElseIf TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Link Then
                        aryLinks.Add(DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Link))
                    Else
                        Throw New System.Exception("An unkown data type was found in the diagram named '" & bdData.Name & "' of type '" & bdData.GetType.FullName & "'")
                    End If

                    bdData.AddToReplaceIDList(aryReplaceIDs, aryItems)
                Else
                    aryDeselect.Add(afItem)
                End If
            Next

            'If we got here and none of the selected items can be copied then lets leave.
            If aryItems.Count = 0 Then
                Return False
            End If

            'If we can not copy an item on the chart then we MUST deselect it so that it is NOT copied 
            'in the purely addflow xml stuff. Otherwise we will be getting stuff in the diagram
            'that does not have a real link to nodes in the system.
            For Each afItem As Lassalle.Flow.Item In aryDeselect
                afItem.Selected = False
            Next

            'Save the replaceme ID list
            oXml.AddChildElement("ReplaceIDList")
            oXml.IntoElem() 'Into ReplaceIDList Element
            For Each strID As String In aryReplaceIDs
                oXml.AddChildElement("ID", strID)
            Next
            oXml.OutOfElem() 'Outof ReplaceIDList Element

            'Now we save the nodes and links.
            oXml.AddChildElement("Nodes")
            oXml.IntoElem() 'Into Nodes Element
            For Each bnNode As AnimatGUI.DataObjects.Behavior.Node In aryNodes
                bnNode.SaveData(oXml)
            Next
            oXml.OutOfElem() 'Outof Nodes Element

            oXml.AddChildElement("Links")
            oXml.IntoElem() 'Into Links Element
            For Each blLink As AnimatGUI.DataObjects.Behavior.Link In aryLinks
                blLink.SaveData(oXml)
            Next
            oXml.OutOfElem() 'Outof Links Element

            'Save the addflow configuration
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim stringWriter As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim xmlWriter As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(stringWriter)
            Lassalle.XMLFlow.Serial.FlowToXML(xmlWriter, m_ctrlAddFlow, True, False)
            Dim strAddFlowXml As String = sb.ToString & vbCrLf

            'We must remove the Xml header infomration before adding it as a child document
            strAddFlowXml = strAddFlowXml.Remove(0, 69)

            oXml.AddChildDoc(strAddFlowXml)

            For Each bdItem As AnimatGUI.DataObjects.Behavior.Data In aryItems
                bdItem.AfterCopy()
            Next

            Return True
        End Function

        Public Overrides Sub LoadPasted(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal bInPlace As Boolean)

            Dim ptClient As Point = m_ctrlAddFlow.PointToClient(Cursor.Position)
            Dim ptBase As Point = m_ctrlAddFlow.PointToAddFlow(ptClient)

            oXml.FindElement("Diagram")

            'Now lets go through and load each of the child diagrams.
            Dim aryItems As New ArrayList
            Dim aryNodes As New ArrayList
            Dim aryLinks As New ArrayList

            LoadPastedNodes(oXml, aryItems, aryNodes)
            LoadPastedLinks(oXml, aryItems, aryLinks)
            VerifyPastedData(aryItems)

            Dim fltMinX As Single = -1
            Dim fltMinY As Single = -1
            ProcessPastedAddFlowData(oXml, aryItems, fltMinX, fltMinY)

            'we need to go through and initialize all the nodes/links after loading.
            Me.Subsystem.InitializeAfterPasted()
            Me.Subsystem.AfterInitialized()

            AddPastedToSim(aryNodes, aryLinks)
            InitPastedInSim(aryNodes, aryLinks)

            Util.ProjectWorkspace.ClearSelections()
            For Each bdItem As AnimatGUI.DataObjects.Behavior.Data In aryItems
                bdItem.CheckForErrors()
                bdItem.SelectItem(True)
            Next

            'Now lets move the addflow items so that they are positioned near the mouse.
            ' We move a little each pasted node and link so that they do not recover
            ' the original items.
            If Not bInPlace Then
                PositionPastedToInPlace(aryItems, ptBase, fltMinX, fltMinY)
            End If

        End Sub

        Protected Sub LoadPastedNodes(ByVal oXml As ManagedAnimatInterfaces.IStdXml, _
                                      ByVal aryItems As ArrayList, _
                                      ByVal aryNodes As ArrayList)

            Dim strAssemblyFile As String
            Dim strClassName As String
            oXml.IntoChildElement("Nodes")
            Dim iCount As Integer = oXml.NumberOfChildren() - 1
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)
                oXml.IntoElem() 'Into Node element
                strAssemblyFile = oXml.GetChildString("AssemblyFile")
                strClassName = oXml.GetChildString("ClassName")
                oXml.OutOfElem() 'Outof Node element

                bnNode = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me.FormHelper), AnimatGUI.DataObjects.Behavior.Node)
                bnNode.ParentDiagram = Me
                bnNode.ParentSubsystem = Me.Subsystem
                bnNode.Organism = Me.Subsystem.Organism
                bnNode.LoadData(oXml)

                bnNode.BeforeAddNode()
                Me.Subsystem.BehavioralNodes.Add(bnNode.ID, bnNode, False)
                aryItems.Add(bnNode)
                aryNodes.Add(bnNode)
                bnNode.AfterAddNode()
            Next
            oXml.OutOfElem() 'Outof Nodes Element

        End Sub

        Protected Sub LoadPastedLinks(ByVal oXml As ManagedAnimatInterfaces.IStdXml, _
                                      ByVal aryItems As ArrayList, _
                                      ByVal aryLinks As ArrayList)

            Dim strAssemblyFile As String
            Dim strClassName As String
            oXml.IntoChildElement("Links")
            Dim iCount As Integer = oXml.NumberOfChildren() - 1
            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)
                oXml.IntoElem() 'Into Node element
                strAssemblyFile = oXml.GetChildString("AssemblyFile")
                strClassName = oXml.GetChildString("ClassName")
                oXml.OutOfElem() 'Outof Node element

                blLink = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me.FormHelper), AnimatGUI.DataObjects.Behavior.Link)
                blLink.ParentDiagram = Me
                blLink.ParentSubsystem = Me.Subsystem
                blLink.Organism = Me.Subsystem.Organism
                blLink.LoadData(oXml)

                blLink.BeforeAddLink()
                Me.Subsystem.BehavioralLinks.Add(blLink.ID, blLink, False)
                aryItems.Add(blLink)
                aryLinks.Add(blLink)

                blLink.AfterAddLink()
            Next
            oXml.OutOfElem() 'Outof Links Element
        End Sub

        Protected Sub VerifyPastedData(ByVal aryItems As ArrayList)

            For Each doItem As Framework.DataObject In aryItems
                doItem.VerifyAfterPaste(aryItems)
            Next
        End Sub

        Protected Sub ProcessPastedAddFlowData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, _
                                               ByVal aryItems As ArrayList, _
                                               ByRef fltMinX As Single, ByRef fltMinY As Single)

            oXml.FindChildElement("AddFlow")
            Dim strAddFlowXml As String = oXml.GetChildDoc()

            'Now I need to replace any of the old ids with the new ids.
            'Debug.Write(strAddFlowXml)

            Dim stringReader As System.IO.StringReader = New System.IO.StringReader(strAddFlowXml)
            Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(stringReader)
            Lassalle.XMLFlow.Serial.XMLToFlow(xmlReader, m_ctrlAddFlow)

            'Now we need to go through and add all of the addflow nodes and links into the dictionaries for them.
            'Also, lets find the rightmost and topmost element for the reposisioning below.
            Dim aryRemoveItems As New ArrayList
            Dim afNode As Lassalle.Flow.Node
            Dim afItem As Lassalle.Flow.Item
            For Each bdData As AnimatGUI.DataObjects.Behavior.Data In aryItems
                'The find routine should add the addflow item if it finds it.
                afItem = FindAddFlowItem(bdData.ID, False)

                If Not afItem Is Nothing Then
                    If TypeOf afItem Is Lassalle.Flow.Node Then
                        afNode = DirectCast(afItem, Lassalle.Flow.Node)
                        If fltMinX < 0 OrElse afNode.Location.X < fltMinX Then
                            fltMinX = afNode.Location.X
                        End If

                        If fltMinY < 0 OrElse afNode.Location.Y < fltMinY Then
                            fltMinY = afNode.Location.Y
                        End If

                        Dim bnData As AnimatGUI.DataObjects.Behavior.Node = DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node)
                        afNode.ImageIndex = GetDiagramImageIndex(bnData)
                    End If

                    bdData.Tag = afItem
                Else
                    'If it cannot find the associated addflow item then remove it.
                    If TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Node AndAlso Me.Subsystem.BehavioralNodes.Contains(bdData.ID) Then
                        Me.Subsystem.BehavioralNodes.Remove(bdData.ID)
                    ElseIf TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Link AndAlso Me.Subsystem.BehavioralLinks.Contains(bdData.ID) Then
                        Me.Subsystem.BehavioralLinks.Remove(bdData.ID, False)
                    End If

                    bdData.RemoveWorksapceTreeView()
                    aryRemoveItems.Add(bdData)
                End If
            Next

            'Check to see if we need to remove any items.
            For Each bdData As AnimatGUI.DataObjects.Behavior.Data In aryRemoveItems
                If aryItems.Contains(bdData) Then
                    aryItems.Remove(bdData)
                End If
            Next
        End Sub

        Protected Sub AddPastedToSim(ByVal aryNodes As ArrayList, ByVal aryLinks As ArrayList)

            For Each bdNode As AnimatGUI.DataObjects.Behavior.Data In aryNodes
                bdNode.AddToSim(True, True)
                bdNode.AddWorkspaceTreeNode()
            Next

            For Each bdLink As AnimatGUI.DataObjects.Behavior.Data In aryLinks
                bdLink.AddToSim(True, True)
                bdLink.AddWorkspaceTreeNode()
            Next

        End Sub

        Protected Sub InitPastedInSim(ByVal aryNodes As ArrayList, ByVal aryLinks As ArrayList)

            For Each bdNode As AnimatGUI.DataObjects.Behavior.Data In aryNodes
                bdNode.InitPastedInSim()
            Next

            For Each bdLink As AnimatGUI.DataObjects.Behavior.Data In aryLinks
                bdLink.InitPastedInSim()
            Next

        End Sub

        Protected Sub PositionPastedToInPlace(ByVal aryItems As ArrayList, ByVal ptBase As Point, ByVal fltMinX As Single, ByVal fltMinY As Single)

            Dim afNode As Lassalle.Flow.Node
            Dim afLink As Lassalle.Flow.Link
            Dim afItem As Lassalle.Flow.Item

            For Each bdData As AnimatGUI.DataObjects.Behavior.Data In aryItems
                afItem = FindAddFlowItem(bdData.ID)

                If TypeOf (afItem) Is Lassalle.Flow.Node Then
                    afNode = DirectCast(afItem, Lassalle.Flow.Node)
                    afNode.Location = New PointF(ptBase.X + (afNode.Location.X - fltMinX), ptBase.Y + (afNode.Location.Y - fltMinY))
                Else
                    Dim pt As PointF
                    Dim k As Integer
                    afLink = DirectCast(afItem, Lassalle.Flow.Link)

                    If afLink.AdjustOrg Then
                        pt = afLink.Points(0)
                        afLink.Points(0) = New PointF(ptBase.X + (pt.X - fltMinX), ptBase.Y + (pt.Y - fltMinY))
                    End If
                    For k = 1 To afLink.Points.Count - 2
                        pt = afLink.Points(k)
                        afLink.Points(k) = New PointF(ptBase.X + (pt.X - fltMinX), ptBase.Y + (pt.Y - fltMinY))
                    Next
                    If afLink.AdjustDst Then
                        pt = afLink.Points(afLink.Points.Count - 1)
                        afLink.Points(afLink.Points.Count - 1) = New PointF(ptBase.X + (pt.X - fltMinX), ptBase.Y + (pt.Y - fltMinY))
                    End If
                End If
            Next

        End Sub

        Protected Overridable Function CreateNewAdapter(ByRef bnOrigin As AnimatGUI.DataObjects.Behavior.Node, _
                                                        ByRef bnDestination As AnimatGUI.DataObjects.Behavior.Node, _
                                                        ByRef doParent As AnimatGUI.Framework.DataObject) As AnimatGUI.DataObjects.Behavior.Node
            Dim doPair As AnimatGUI.DataObjects.Behavior.LinkPair = Util.Application.FindAdapterPair(bnOrigin.GetType().ToString, bnDestination.GetType().ToString())

            If Not doPair Is Nothing Then
                Dim oAdapter As Object = Util.LoadClass(doPair.m_strLinkType, doParent)

                If Not Util.IsTypeOf(oAdapter.GetType(), GetType(AnimatGUI.DataObjects.Behavior.Node), False) Then
                    Throw New System.Exception("Invalid adapter type specified in adapter pair: " & doPair.ToString())
                End If

                Return DirectCast(oAdapter, AnimatGUI.DataObjects.Behavior.Node)
            Else
                Return bnDestination.CreateNewAdapter(bnOrigin, doParent)
            End If

        End Function

#End Region

#Region " Print "

        Public Overrides Sub GenerateMetafiles(ByVal aryMetaDocs As AnimatGUI.Collections.MetaDocuments)

            'TODO
            'aryMetaDocs.Add(New AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument(m_ctrlAddFlow.ExportMetafile(False, True, True), _
            '                DirectCast(m_ctrlAddFlow.Font.Clone(), System.Drawing.Font), Me.TabPageName))

            'Dim bdDiagram As AnimatGUI.Forms.Behavior.DiagramOld
            'For Each deEntry As DictionaryEntry In m_aryDiagrams
            '    bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.DiagramOld)
            '    bdDiagram.GenerateMetafiles(aryMetaDocs)
            'Next

        End Sub

#End Region

#End Region

#Region " Events "

        Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)

            Try
                Dim iSize As Integer = 15

                If Me.Parent.Width > iSize Then
                    Me.m_ctrlAddFlow.Width = Me.Parent.Width - iSize
                End If

                If Me.Parent.Height > iSize Then
                    Me.m_ctrlAddFlow.Height = Me.Parent.Height - iSize
                End If

            Catch ex As System.Exception

            End Try

        End Sub

        Private Sub m_Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Timer.Tick
            Try
                m_Timer.Enabled = False
                m_ctrlAddFlow.Invalidate(True)
                m_ctrlAddFlow.Update()
                'Debug.WriteLine("Inavlidating addflow control")

                Util.ProjectWorkspace.RefreshProperties()

            Catch ex As System.Exception
            End Try
        End Sub


#Region " PopUp Menu Events "

        Private Sub GridMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridMenuItem.Click

            Try
                If Not sender Is Nothing AndAlso TypeOf sender Is ToolStripMenuItem Then
                    Dim menuItem As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
                    Me.DrawGrid = menuItem.Checked
                    m_Timer.Enabled = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub SendToBackMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SendToBackMenuItem.Click
            Try
                SendSelectedToBack()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub BringToFrontMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BringToFrontMenuItem.Click
            Try
                BringSelectedToFront()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ExportToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExportToolStripMenuItem.Click

        End Sub

        Private Sub PrintToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click, PrintToolStripButton.Click

        End Sub

        Public Sub OnSelectAll(ByVal sender As Object, ByVal e As System.EventArgs)
            SelectAll()
        End Sub

        Private Sub CutToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CutToolStripButton.Click, CutToolStripMenuItem.Click
            CutSelected()
        End Sub

        Private Sub CopyToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CopyToolStripButton.Click, CopyToolStripMenuItem.Click
            CopySelected()
        End Sub

        Private Sub PasteToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PasteToolStripButton.Click, PasteToolStripMenuItem.Click
            PasteSelected(False)
        End Sub

        Private Sub PasteInPlaceMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PasteInPlaceMenuItem.Click
            PasteSelected(True)
        End Sub

        Private Sub OnRelabel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelToolStripMenuItem.Click, RelabelToolStripButton.Click
            RelabelRegEx()
        End Sub

#End Region

#Region " Format Events "

#Region " Align Events "

        Private Sub AlignTopMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlignTopMenuItem.Click, AlignTopToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Location = New PointF(afNode.Location.X, afLastNode.Location.Y)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

        Private Sub AlignVerticalCenterMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlignVerticalCenterMenuItem.Click, AlignVerticalCenterToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF((afLastNode.Location.X + (afLastNode.Size.Width / 2)) - (afNode.Size.Width / 2), afNode.Location.Y)
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

        Private Sub AlignBottomMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlignBottomMenuItem.Click, AlignBottomToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(afNode.Location.X, ((afLastNode.Location.Y + afLastNode.Size.Height) - afNode.Size.Height))
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

        Private Sub AlignLeftMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlignLeftMenuItem.Click, AlignLeftToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(afLastNode.Location.X, afNode.Location.Y)
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

        Private Sub AlignHorizontalCenterMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlignHorizontalCenterMenuItem.Click, AlignHorizontalCenterToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(afNode.Location.X, (afLastNode.Location.Y + (afLastNode.Size.Height / 2)) - (afNode.Size.Height / 2))
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Private Sub AlignRightMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlignRightMenuItem.Click, AlignRightToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(((afLastNode.Location.X + afLastNode.Size.Width) - afNode.Size.Width), afNode.Location.Y)
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

#End Region

#Region " Distribute Events "

        Private Sub DistributeHorizontalMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DistributeHorizontalMenuItem.Click, DistributeHorizontalToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()
                aryList.Sort(New FlowNodeXLocation)

                If aryList.Count <= 1 Then Return

                'Find the first and last selected node.
                Dim afFirstNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                'If afFirstNode.Location.X > afLastNode.Location.X Then
                '    Dim afTemp As Lassalle.Flow.Node = afFirstNode
                '    afFirstNode = afLastNode
                '    afLastNode = afTemp
                'End If

                Dim fltFirstCenter As Single = afFirstNode.Location.X + (afFirstNode.Size.Width / 2)
                Dim fltLastCenter As Single = afLastNode.Location.X + (afLastNode.Size.Width / 2)

                Dim fltWidth As Single = Math.Abs(fltLastCenter - fltFirstCenter)
                fltWidth = fltWidth / (aryList.Count - 1)

                Dim fltDelta As Single = 0

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Location = New PointF((fltFirstCenter + fltDelta - (afNode.Size.Width / 2)), afNode.Location.Y)
                    fltDelta = fltDelta + fltWidth
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

        Private Sub DistributeVerticalMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DistributeVerticalMenuItem.Click, DistributeVerticalToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()
                aryList.Sort(New FlowNodeYLocation)

                If aryList.Count <= 1 Then Return

                'Find the first and last selected node.
                Dim afFirstNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                'If afFirstNode.Location.Y > afLastNode.Location.Y Then
                '    Dim afTemp As Lassalle.Flow.Node = afFirstNode
                '    afFirstNode = afLastNode
                '    afLastNode = afTemp
                'End If

                Dim fltFirstCenter As Single = afFirstNode.Location.Y + (afFirstNode.Size.Height / 2)
                Dim fltLastCenter As Single = afLastNode.Location.Y + (afLastNode.Size.Height / 2)

                Dim fltHeight As Single = Math.Abs(fltLastCenter - fltFirstCenter)
                fltHeight = fltHeight / (aryList.Count - 1)

                Dim fltDelta As Single = 0

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Location = New PointF(afNode.Location.X, (fltFirstCenter + fltDelta - (afNode.Size.Height / 2)))
                    fltDelta = fltDelta + fltHeight
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

#End Region

#Region " Size Events "

        Private Sub SizeBothMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SizeBothMenuItem.Click, SizeBothToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Size = New SizeF(afLastNode.Size.Width, afLastNode.Size.Height)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

        Private Sub SizeHeightMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SizeHeightMenuItem.Click, SizeHeightToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Size = New SizeF(afNode.Size.Width, afLastNode.Size.Height)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

        Private Sub SizeWidthMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SizeWidthMenuItem.Click, SizeWidthToolStripItem.Click

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Size = New SizeF(afLastNode.Size.Width, afNode.Size.Height)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try
        End Sub

#End Region


#End Region

#Region " Zoom Events "

        Private Sub FitToPageMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FitToPageMenuItem.Click
            Try
                FitToPage()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomInBy10MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomInBy10MenuItem.Click, ZoomInToolStripItem.Click
            Try
                ZoomBy(0.1)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomInBy20MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomInBy20MenuItem.Click
            Try
                ZoomBy(0.2)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn100MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn100MenuItem.Click
            Try
                ZoomTo(1)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn125MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn125MenuItem.Click
            Try
                ZoomTo(1.25)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn150MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn150MenuItem.Click
            Try
                ZoomTo(1.5)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn175MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn175MenuItem.Click
            Try
                ZoomTo(1.75)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn200MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn200MenuItem.Click
            Try
                ZoomTo(2)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn250MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn250MenuItem.Click
            Try
                ZoomTo(2.5)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn300MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn300MenuItem.Click
            Try
                ZoomTo(3)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn400MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn400MenuItem.Click
            Try
                ZoomTo(4)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomIn500MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomIn500MenuItem.Click
            Try
                ZoomTo(5)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOutBy10MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOutBy10MenuItem.Click, ZoomOutToolStripItem.Click
            Try
                ZoomBy(-0.1)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOutBy20MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOutBy20MenuItem.Click
            Try
                ZoomBy(-0.2)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut100MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut100MenuItem.Click
            Try
                ZoomTo(1)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut90MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut90MenuItem.Click
            Try
                ZoomTo(0.9)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut80MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut80MenuItem.Click
            Try
                ZoomTo(0.8)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut70MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut70MenuItem.Click
            Try
                ZoomTo(0.7)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut60MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut60MenuItem.Click
            Try
                ZoomTo(0.6)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut50MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut50MenuItem.Click
            Try
                ZoomTo(0.5)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut40MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut40MenuItem.Click
            Try
                ZoomTo(0.4)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut30MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut30MenuItem.Click
            Try
                ZoomTo(0.3)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut20MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut20MenuItem.Click
            Try
                ZoomTo(0.2)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ZoomOut10MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZoomOut10MenuItem.Click
            Try
                ZoomTo(0.1)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

        Private Sub ShowConnectionsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowConnectionsToolStripMenuItem.Click

            Try

                Dim aryList As ArrayList = GetSelectedAddflowNodes()
                If aryList.Count = 1 Then
                    Dim afNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                    Dim doNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(afNode.Tag, String))

                    If TypeOf doNode Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                        Dim doOffpage As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(doNode, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)
                        If Not doOffpage.LinkedNode Is Nothing AndAlso Not doOffpage.LinkedNode.Node Is Nothing Then
                            doNode = doOffpage.LinkedNode.Node
                        End If
                    End If

                    Dim frmConn As New AnimatGUI.Forms.Behavior.Connections

                    frmConn.Node = doNode
                    frmConn.ShowDialog()

                    If Not frmConn.SelectedNode Is Nothing Then
                        frmConn.SelectedNode.SelectItem(False)
                    ElseIf Not frmConn.SelectedLink Is Nothing Then
                        frmConn.SelectedLink.SelectItem(False)
                    End If

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#Region " Item Selection Events "

        Protected Sub OnItemsSelected()

            Try
                If m_ctrlAddFlow.SelectedItems.Count = 1 Then
                    Dim item As Lassalle.Flow.Item = m_ctrlAddFlow.SelectedItem
                    If Not (item Is Nothing) AndAlso Not item.Tag Is Nothing Then
                        Dim bdItem As AnimatGUI.DataObjects.Behavior.Data = FindItem(DirectCast(item.Tag, String))
                        bdItem.SelectItem()
                    Else
                        m_bnSubSystem.SelectItem()
                    End If
                ElseIf m_ctrlAddFlow.SelectedItems.Count > 1 Then
                    m_bSelectingMultiple = True

                    Dim aryItems As New AnimatGUI.Collections.DataObjects(Nothing)
                    'If more than one item is selected then lets get a list of them and pass that it.
                    Dim bdItem As AnimatGUI.DataObjects.Behavior.Data
                    Dim iIndex As Integer = 0
                    For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                        If Not afItem.Tag Is Nothing Then
                            bdItem = FindItem(DirectCast(afItem.Tag, String))
                            aryItems.Add(bdItem)
                        End If
                    Next

                    Util.ProjectWorkspace.SelectMultipleItems(aryItems)

                    m_bSelectingMultiple = False
                Else
                    m_bnSubSystem.SelectItem()
                End If

            Catch ex As Exception
                Throw ex
            Finally
                m_bSelectingMultiple = False
            End Try

        End Sub

        Public Overrides Sub OnItemSelected(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal bSelectMultiple As Boolean)
            If Not m_bSelectingMultiple AndAlso Not doObject.Tag Is Nothing AndAlso Util.IsTypeOf(doObject.Tag.GetType, GetType(Lassalle.Flow.Item), False) Then
                SelectAddFlowItem(DirectCast(doObject.Tag, Lassalle.Flow.Item), bSelectMultiple)
            End If
        End Sub

        Public Overrides Sub OnItemDeselected(ByRef doObject As AnimatGUI.Framework.DataObject)
            If Not m_bSelectingMultiple AndAlso Not doObject.Tag Is Nothing AndAlso Util.IsTypeOf(doObject.Tag.GetType, GetType(Lassalle.Flow.Item), False) Then
                DeselectAddFlowItem(DirectCast(doObject.Tag, Lassalle.Flow.Item))
            End If
        End Sub

#End Region

        Protected Overrides Sub AnimatForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
            MyBase.AnimatForm_FormClosing(sender, e)

            'If we are closing then reset the Parent diagram property of all nodes/links and the subsystem node link
            If e.Cancel = False Then
                Me.Subsystem.DiagramXml = Me.SaveDiagramXml
                Me.Subsystem.SubsystemDiagram = Nothing

                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralNodes
                    Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    bnNode.ParentDiagram = Nothing
                Next

                For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralLinks
                    Dim blLink As AnimatGUI.DataObjects.Behavior.Link = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    blLink.ParentDiagram = Nothing
                Next
            End If

        End Sub

        Private Sub ShapeToolStripMenuItem_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShapeToolStripMenuItem.DropDownOpening
            Try
                If m_ctrlAddFlow.SelectedItems.Count > 1 Then
                    Me.AlignMenuItem.Enabled = True
                    Me.DistributeMenuItem.Enabled = True
                    Me.SizeMenuItem.Enabled = True
                Else
                    Me.AlignMenuItem.Enabled = False
                    Me.DistributeMenuItem.Enabled = False
                    Me.SizeMenuItem.Enabled = False
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#Region " AddFlow Events "

        Private Sub m_ctrlAddFlow_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_ctrlAddFlow.MouseDown

            Try

                If e.Button = MouseButtons.Right Then
                    Dim ctl As Control = CType(sender, System.Windows.Forms.Control)
                    Dim ptScreen As Point = ctl.PointToScreen(New Point(e.X, e.Y))
                    CreateDiagramPopupMenu(ptScreen)
                ElseIf e.Button = MouseButtons.Left Then
                    OnItemsSelected()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_ctrlAddFlow_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_ctrlAddFlow.DragEnter
            Try
                If (e.Data.GetDataPresent(GetType(AnimatGuiCtrls.Controls.PanelIcon))) Then
                    e.Effect = DragDropEffects.Copy
                    Me.Cursor = Cursors.Arrow
                Else
                    e.Effect = DragDropEffects.None
                    Me.Cursor = Cursors.Default
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.DragLeave
            Me.Cursor = Cursors.Default
        End Sub

        Private Sub m_ctrlAddFlow_GiveFeedback(ByVal sender As Object, ByVal e As System.Windows.Forms.GiveFeedbackEventArgs) Handles m_ctrlAddFlow.GiveFeedback
            'e.UseDefaultCursors = False
            'Debug.WriteLine("GiveFeedback")
        End Sub

        Private Sub m_ctrlAddFlow_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_ctrlAddFlow.DragDrop

            Try
                Dim ptClient As Point = m_ctrlAddFlow.PointToClient(New Point(e.X, e.Y))
                Dim ptAddFlow As Point = m_ctrlAddFlow.PointToAddFlow(ptClient)

                'Check if it is a behavioral node, the check if it is a behavioral connector
                If (e.Data.GetDataPresent(GetType(AnimatGuiCtrls.Controls.PanelIcon))) Then
                    Dim pnlIcon As AnimatGuiCtrls.Controls.PanelIcon = DirectCast(e.Data.GetData(GetType(AnimatGuiCtrls.Controls.PanelIcon)), AnimatGuiCtrls.Controls.PanelIcon)
                    Dim bdDropData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(pnlIcon.Data, AnimatGUI.DataObjects.Behavior.Data)

                    Automation_DropNode(bdDropData, ptAddFlow)

                    'Debug.WriteLine("Finishing DragDrop")
                    pnlIcon.DraggingIcon = False
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Automation_DropNode(ByVal bdDropData As AnimatGUI.DataObjects.Behavior.Data, ByVal ptAddFlow As Point) As AnimatGUI.DataObjects.Behavior.Node

            If Not bdDropData Is Nothing And TypeOf (bdDropData) Is AnimatGUI.DataObjects.Behavior.Node Then
                Dim bdData As AnimatGUI.DataObjects.Behavior.Node = DirectCast(bdDropData.Clone(Me.Subsystem.Organism, False, Nothing), AnimatGUI.DataObjects.Behavior.Node)

                ptAddFlow.X = ptAddFlow.X - CInt(bdData.Size.Width / 2)
                ptAddFlow.Y = ptAddFlow.Y - CInt(bdData.Size.Height / 2)

                If ptAddFlow.X < 0 Then ptAddFlow.X = 0
                If ptAddFlow.Y < 0 Then ptAddFlow.Y = 0

                bdData.Location = New PointF(ptAddFlow.X, ptAddFlow.Y)

                Me.Subsystem.Organism.MaxNodeCount = Me.Subsystem.Organism.MaxNodeCount + 1
                bdData.Text = Me.Subsystem.Organism.MaxNodeCount.ToString
                Me.Subsystem.AddNode(bdData)

                Me.IsDirty = True
                Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me))

                Return bdData
            Else
                Return Nothing
            End If

        End Function


        Private Sub m_ctrlAddFlow_AfterAddLink(ByVal sender As Object, ByVal e As Lassalle.Flow.AfterAddLinkEventArgs) Handles m_ctrlAddFlow.AfterAddLink

            Try

                'Lets get rid of the actual link that was drawn and then 
                m_ctrlAddFlow.BeginAction(1002)

                e.Link.Remove()

                Dim bnOrigin As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Link.Org.Tag, String))
                Dim bnDestination As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Link.Dst.Tag, String))

                Automation_DrawLink(bnOrigin, bnDestination)

                m_ctrlAddFlow.EndAction()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Automation_DrawLink(ByVal bnOrigin As AnimatGUI.DataObjects.Behavior.Node, ByVal bnDestination As AnimatGUI.DataObjects.Behavior.Node) As AnimatGUI.DataObjects.Behavior.Link

            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            Dim bRequiresAdapter As Boolean

            'Only do this If the user decides to not cancel
            If SelectLinkType(bnOrigin, bnDestination, blLink, bRequiresAdapter) Then
                If Not bRequiresAdapter Then
                    'If it does not require an adapter then just add the link directly.
                    Me.Subsystem.AddLink(bnOrigin, bnDestination, blLink)
                Else

                    ''If it does require an adapter then lets add the pieces.
                    Dim bnAdapter As AnimatGUI.DataObjects.Behavior.Node = CreateNewAdapter(bnOrigin, bnDestination, Me.FormHelper)
                    bnAdapter = bnOrigin.ValidateDestinationAdapterChosen(bnAdapter)

                    bnAdapter.Location = FindHalfwayLocation(bnOrigin, bnDestination, bnAdapter.Size)
                    Me.Subsystem.Organism.MaxNodeCount = Me.Subsystem.Organism.MaxNodeCount + 1
                    bnAdapter.Text = Me.Subsystem.Organism.MaxNodeCount.ToString

                    Me.Subsystem.AddNode(bnAdapter)

                    Try
                        'Connect the origin to the new adapter using the adapter link.
                        Me.Subsystem.AddLink(bnOrigin, bnAdapter, blLink)

                        'Now we need a new link to go from the adapter to the destination.
                        blLink = DirectCast(blLink.Clone(Me.Subsystem.Organism, False, Nothing), AnimatGUI.DataObjects.Behavior.Link)

                        blLink.BeginBatchUpdate()
                        blLink.ArrowDestination = New AnimatGUI.DataObjects.Behavior.Link.Arrow(blLink, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Arrow, _
                                                                                      AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Small, _
                                                                                      AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg15, False)
                        blLink.EndBatchUpdate(False)

                        Me.Subsystem.AddLink(bnAdapter, bnDestination, blLink)

                        Dim baAdapter As AnimatGUI.DataObjects.Behavior.Nodes.Adapter = DirectCast(bnAdapter, AnimatGUI.DataObjects.Behavior.Nodes.Adapter)
                        baAdapter.InitializeAfterLoad()
                        baAdapter.AddToSim(True)

                    Catch ex As Exception
                        bnAdapter.Delete(False)
                        Throw ex
                    End Try

                    bnAdapter.SelectItem()
                End If
            End If

            Me.IsDirty = True
            Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me))

            Return blLink
        End Function

        Private Sub m_ctrlAddFlow_BeforeEdit(ByVal sender As Object, ByVal e As Lassalle.Flow.BeforeEditEventArgs) Handles m_ctrlAddFlow.BeforeEdit
            Try
                'TODO
                'MenuCommands Needs Fix
                'Dim mcEdit As MenuCommand = m_beEditor.MainMenu.MenuCommands("Edit")
                'Dim mcDelete As MenuCommand = mcEdit.MenuCommands("Delete")
                'mcDelete.Enabled = False
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_AfterEdit(ByVal sender As Object, ByVal e As Lassalle.Flow.AfterEditEventArgs) Handles m_ctrlAddFlow.AfterEdit
            Try

                'TODO
                'MenuCommands Needs Fix
                'Dim mcEdit As MenuCommand = m_beEditor.MainMenu.MenuCommands("Edit")
                'Dim mcDelete As MenuCommand = mcEdit.MenuCommands("Delete")
                'mcDelete.Enabled = True

                'After we have edited the text directly we need to update the behavioral object.
                If Not e.Node Is Nothing Then
                    Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Node.Tag, String))

                    If bnNode.BeforeEdit(e.Text) Then
                        e.Cancel.Cancel = True
                    Else
                        bnNode.BeginBatchUpdate()

                        bnNode.SignalBeforePropertyChanged(bnNode, bnNode.GetType().GetProperty("Text"))

                        bnNode.Text = e.Text

                        bnNode.SignalAfterPropertyChanged(bnNode, bnNode.GetType().GetProperty("Text"))
                        bnNode.EndBatchUpdate(False)
                        bnNode.SelectItem(False)

                        bnNode.AfterEdit()

                        Me.IsDirty = True
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_NodeOwnerDraw(ByVal sender As Object, ByVal e As Lassalle.Flow.NodeOwnerDrawEventArgs) Handles m_ctrlAddFlow.NodeOwnerDraw

            Try
                If e.Node.OwnerDraw Then
                    Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Node.Tag, String))
                    Dim gs As System.Drawing.Drawing2D.GraphicsState = e.Graphics.Save()

                    'Debug.WriteLine("Drawing: (" & e.Node.Rect.Top & ", " & e.Node.Rect.Left & ", " & e.Node.Rect.Width & ", " & e.Node.Rect.Height & ")")
                    bnNode.OwnerDraw(sender, e.Node.Rect, e.Graphics)

                    e.Graphics.Restore(gs)

                    e.Flags = Not Lassalle.Flow.NodeDrawFlags.Fill And Not Lassalle.Flow.NodeDrawFlags.Shape
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_ctrlAddFlow_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.DoubleClick

            Try
                Dim afItem As Lassalle.Flow.Item = m_ctrlAddFlow.PointedItem()

                If Not afItem Is Nothing Then
                    Dim bdItem As AnimatGUI.DataObjects.Behavior.Data = FindItem(DirectCast(afItem.Tag, String), False)
                    If Not bdItem Is Nothing Then
                        bdItem.DoubleClicked()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_AfterSelect(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterSelect
            Try
                OnItemsSelected()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_AfterMove(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterMove
            Me.IsDirty = True
            'TODO
            'Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))
        End Sub

        Private Sub m_ctrlAddFlow_AfterResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterResize
            Me.IsDirty = True
            'TODO
            'Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))
        End Sub

        Private Sub m_ctrlAddFlow_AfterStretch(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterStretch
            Me.IsDirty = True
            'TODO
            'Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))
        End Sub


#End Region

#End Region

#Region " Sorter Classes "

        Protected Class FlowNodeXLocation
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is Lassalle.Flow.Node AndAlso TypeOf y Is Lassalle.Flow.Node) Then Return 0

                Dim afX As Lassalle.Flow.Node = DirectCast(x, Lassalle.Flow.Node)
                Dim afY As Lassalle.Flow.Node = DirectCast(y, Lassalle.Flow.Node)

                If afX.Location.X > afY.Location.X Then
                    Return 1
                ElseIf afX.Location.X = afY.Location.X Then
                    Return 0
                Else
                    Return -1
                End If

            End Function 'IComparer.Compare

        End Class

        Protected Class FlowNodeYLocation
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is Lassalle.Flow.Node AndAlso TypeOf y Is Lassalle.Flow.Node) Then Return 0

                Dim afX As Lassalle.Flow.Node = DirectCast(x, Lassalle.Flow.Node)
                Dim afY As Lassalle.Flow.Node = DirectCast(y, Lassalle.Flow.Node)

                If afX.Location.Y > afY.Location.Y Then
                    Return 1
                ElseIf afX.Location.Y = afY.Location.Y Then
                    Return 0
                Else
                    Return -1
                End If

            End Function 'IComparer.Compare

        End Class

#End Region


    End Class

End Namespace
