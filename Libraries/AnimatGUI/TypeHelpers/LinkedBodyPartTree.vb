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

Namespace TypeHelpers

    Public Class LinkedBodyPartTree
        Inherits TypeHelpers.LinkedBodyPart

#Region " Attributes "

#End Region

#Region " Properties "


#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure, _
                       ByVal bpBodyPart As AnimatGUI.DataObjects.Physical.BodyPart, _
                       ByVal tpBodyPartType As System.Type, Optional ByVal doPruner As TreeListPruner = Nothing)
            MyBase.New(doStructure)
            m_doStructure = doStructure
            m_bpBodyPart = bpBodyPart
            m_tpBodyPartType = tpBodyPartType
            m_doPruner = doPruner
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New LinkedBodyPartTree(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            Return oNew
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)
            If m_doStructure Is Nothing Then Return

            If Not TypeOf (ctrlDropDown) Is Crownwood.DotNetMagic.Controls.TreeControl Then
                Throw New System.Exception("The control passed into BuildPropertyDropDown is not a treeview type")
            End If

            Dim tvTree As Crownwood.DotNetMagic.Controls.TreeControl = DirectCast(ctrlDropDown, Crownwood.DotNetMagic.Controls.TreeControl)

            tvTree.SuspendLayout()
            tvTree.Nodes.Clear()

            If Util.IsTypeOf(m_tpBodyPartType, GetType(AnimatGUI.DataObjects.Physical.Joint), False) Then
                m_doStructure.CreateJointTreeView(tvTree, Nothing, Me)
            Else
                m_doStructure.CreateRigidBodyTreeView(tvTree, Nothing, Me)
            End If

            If Not m_doPruner Is Nothing Then
                m_doPruner.PruneTree(tvTree.Nodes)
            End If

            tvTree.Width = 300
            tvTree.ExpandAll()

            MyBase.FormatDropDownTree(tvTree, 8)

            tvTree.ResumeLayout()
            tvTree.Invalidate()

        End Sub

#End Region

    End Class

End Namespace
