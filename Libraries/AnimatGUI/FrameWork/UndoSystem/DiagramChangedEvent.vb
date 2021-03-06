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

Namespace Framework.UndoSystem

    Public Class DiagramChangedEvent
        Inherits AnimatGUI.Framework.UndoSystem.HistoryEvent

#Region " Attributes "

        Protected m_frmDiagram As AnimatGUI.Forms.Behavior.Diagram
        Protected m_bdAlteredData As AnimatGUI.DataObjects.Behavior.Data

#End Region

#Region " Properties "

        Public Overridable Property Diagram() As AnimatGUI.Forms.Behavior.Diagram
            Get
                Return m_frmDiagram
            End Get
            Set(ByVal Value As AnimatGUI.Forms.Behavior.Diagram)
                If Value Is Nothing Then
                    Throw New System.Exception("A Diagram change event must be associated with a diagram.")
                End If

                m_frmDiagram = Value
            End Set
        End Property

        Public Overridable Property AlteredData() As AnimatGUI.DataObjects.Behavior.Data
            Get
                Return m_bdAlteredData
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Data)
                m_bdAlteredData = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal frmDiagram As AnimatGUI.Forms.Behavior.Diagram, Optional ByVal bdAltered As AnimatGUI.DataObjects.Behavior.Data = Nothing)
            MyBase.New(frmDiagram)

            If frmDiagram Is Nothing Then
                Throw New System.Exception("The behavioral diagram must not be null.")
            End If

            Me.Diagram = frmDiagram
            Me.AlteredData = bdAltered
        End Sub

        Protected Overridable Sub RefreshParent(ByVal doObject As AnimatGUI.Framework.DataObject)

            'TODO
            'If Not m_mdiParent Is Nothing Then
            '    m_mdiParent.MakeVisible()
            '    m_mdiParent.UndoRedoRefresh(doObject)
            'ElseIf Not m_frmParent Is Nothing Then
            '    m_frmParent.UndoRedoRefresh(doObject)
            'End If

        End Sub

        Public Overrides Sub Undo()
            If Not m_frmDiagram Is Nothing AndAlso Not m_frmDiagram.TabPage Is Nothing Then
                m_frmDiagram.TabPage.Selected = True
            End If
            m_frmDiagram.OnUndo()

            If Not m_bdAlteredData Is Nothing Then
                m_frmDiagram.UpdateData(m_bdAlteredData, False, False)
            End If

            Util.ProjectWorkspace.RefreshProperties()
        End Sub

        Public Overrides Sub Redo()
            If Not m_frmDiagram Is Nothing AndAlso Not m_frmDiagram.TabPage Is Nothing Then
                m_frmDiagram.TabPage.Selected = True
            End If
            m_frmDiagram.OnRedo()
            Util.ProjectWorkspace.RefreshProperties()

            If Not m_bdAlteredData Is Nothing Then
                m_frmDiagram.UpdateData(m_bdAlteredData, False, False)
            End If
            Util.ProjectWorkspace.RefreshProperties()
        End Sub

#End Region

    End Class

End Namespace

