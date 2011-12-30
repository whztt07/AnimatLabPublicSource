﻿Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System
Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.WinControls
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse
Imports MouseButtons = System.Windows.Forms.MouseButtons
Imports AnimatTesting.Framework

Namespace UITests
    Namespace BehavioralEditorTests

        <CodedUITest()>
        Public MustInherit Class NeuralUITest
            Inherits AnimatUITest

#Region "Attributes"

            Protected m_strPartType As String = "Box"
            Protected m_strSecondaryPartType As String = ""
            Protected m_strJointType As String = "Hinge"

            Protected m_ptAddArmClick As New Point(751, 362)
            Protected m_ptZoomStart As New Point(877, 100)
            Protected m_iZoom1 As Integer = 300
            Protected m_iZoom2 As Integer = 300

            Protected m_ptAddRootAttach As New Point(757, 330)
            Protected m_ptAddArmAttach As New Point(990, 545)

            Protected m_ptRotateArmStart As New Point(100, 420)
            Protected m_iRotateArm1 As Integer = 700
            Protected m_iRotateArm2 As Integer = 700

            Protected m_ptRotateArm2Start As New Point(1196, 135)
            Protected m_ptRotateArm2End As New Point(1064, 288)

            Protected m_ptTranslateZAxisStart As New Point(781, 642)  'Point(790, 634)
            Protected m_ptTranslateZAxisEnd As New Point(700, 725) 'Point(741, 669)

            Protected m_ptRotateJoint1Start As New Point(687, 428)
            Protected m_ptRotatejoint1End As New Point(652, 608)

            Protected m_ptMoveJoint1Start As New Point(687, 428)
            Protected m_ptMovejoint1End As New Point(652, 608)

            Protected m_ptTransJointYAxisStart As New Point(790, 634)
            Protected m_ptTransJointYAxisEnd As New Point(741, 669)

            Protected m_strInitialJointXPos As String = "-0.125"
            Protected m_strInitialJointYPos As String = "0"
            Protected m_strInitialJointZPos As String = "0"

            Protected m_strInitialJointXRot As String = "90"
            Protected m_strInitialJointYRot As String = "0"
            Protected m_strInitialJointZRot As String = "90"

#End Region

#Region "Properties"

#End Region

#Region "Methods"

            Protected Overridable Sub TestNeuron()

                StartNewProject()

                OpenRootBehavioralSubsystem()

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "IntegrateFireGUI.DataObjects.Behavior.Neurons.NonSpiking", New Point(100, 100), "A")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "IntegrateFireGUI.DataObjects.Behavior.Neurons.NonSpiking", New Point(200, 200), "B")

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B", "2", _
                          "Synapses Classes\Non-Spiking Chemical Synapses\Hyperpolarising IPSP", True)


                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "FiringRateGUI.DataObjects.Behavior.Neurons.Normal", New Point(100, 300), "A1")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "FiringRateGUI.DataObjects.Behavior.Neurons.Pacemaker", New Point(200, 400), "B1")

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A1", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B1", "3", _
                          "Normal Synapse", False)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A1", "4", _
                          "", False)

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(400, 400), "OP")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "AnimatGUI.DataObjects.Behavior.Nodes.Joint", New Point(500, 400), "HJ")

                ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\OP", _
                          "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A"})

                'Select the simulation window tab so it is visible now.
                ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name}, 1000)

                'Create the test armature.
                CreateArmature(m_strPartType, m_strSecondaryPartType, m_strJointType, _
                               m_ptAddArmClick, m_ptZoomStart, m_iZoom1, m_iZoom2, _
                                False, "Attachment", m_ptAddRootAttach, m_ptAddArmAttach)

                ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & _
                           "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\HJ", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                           "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1"})


                ''Create the chart for the test armature
                'CreateArmatureChart(False)


            End Sub

            Protected Overrides Sub RecalculatePositionsUsingResolution()
                MyBase.RecalculatePositionsUsingResolution()

                m_ptRotateArmStart.X = CInt(m_ptRotateArmStart.X * m_dblResScaleWidth)
                m_ptRotateArmStart.Y = CInt(m_ptRotateArmStart.Y * m_dblResScaleHeight)

                m_ptRotateArm2Start.X = CInt(m_ptRotateArm2Start.X * m_dblResScaleWidth)
                m_ptRotateArm2Start.Y = CInt(m_ptRotateArm2Start.Y * m_dblResScaleHeight)

                m_ptRotateArm2End.X = CInt(m_ptRotateArm2End.X * m_dblResScaleWidth)
                m_ptRotateArm2End.Y = CInt(m_ptRotateArm2End.Y * m_dblResScaleHeight)

                m_ptAddRootAttach.X = CInt(m_ptAddRootAttach.X * m_dblResScaleWidth)
                m_ptAddRootAttach.Y = CInt(m_ptAddRootAttach.Y * m_dblResScaleHeight)

                m_ptAddArmAttach.X = CInt(m_ptAddArmAttach.X * m_dblResScaleWidth)
                m_ptAddArmAttach.Y = CInt(m_ptAddArmAttach.Y * m_dblResScaleHeight)

                m_ptTranslateZAxisStart.X = CInt(m_ptTranslateZAxisStart.X * m_dblResScaleWidth)
                m_ptTranslateZAxisStart.Y = CInt(m_ptTranslateZAxisStart.Y * m_dblResScaleHeight)

                m_ptTranslateZAxisEnd.X = CInt(m_ptTranslateZAxisEnd.X * m_dblResScaleWidth)
                m_ptTranslateZAxisEnd.Y = CInt(m_ptTranslateZAxisEnd.Y * m_dblResScaleHeight)

                m_ptRotateJoint1Start.X = CInt(m_ptRotateJoint1Start.X * m_dblResScaleWidth)
                m_ptRotateJoint1Start.Y = CInt(m_ptRotateJoint1Start.Y * m_dblResScaleHeight)

                m_ptRotatejoint1End.X = CInt(m_ptRotatejoint1End.X * m_dblResScaleWidth)
                m_ptRotatejoint1End.Y = CInt(m_ptRotatejoint1End.Y * m_dblResScaleHeight)

                m_ptMoveJoint1Start.X = CInt(m_ptMoveJoint1Start.X * m_dblResScaleWidth)
                m_ptMoveJoint1Start.Y = CInt(m_ptMoveJoint1Start.Y * m_dblResScaleHeight)

                m_ptMovejoint1End.X = CInt(m_ptMovejoint1End.X * m_dblResScaleWidth)
                m_ptMovejoint1End.Y = CInt(m_ptMovejoint1End.Y * m_dblResScaleHeight)

                m_ptMoveJoint1Start.X = CInt(m_ptMoveJoint1Start.X * m_dblResScaleWidth)
                m_ptMoveJoint1Start.Y = CInt(m_ptMoveJoint1Start.Y * m_dblResScaleHeight)

                m_ptTransJointYAxisStart.X = CInt(m_ptTransJointYAxisStart.X * m_dblResScaleWidth)
                m_ptTransJointYAxisEnd.Y = CInt(m_ptTransJointYAxisEnd.Y * m_dblResScaleHeight)

            End Sub


            Protected Overrides Sub RepositionChildPart()

                ZoomInOnPart(m_ptRotateArmStart, m_iRotateArm1, m_iRotateArm2, False, MouseButtons.Left)

                DragMouse(m_ptRotateArm2Start, m_ptRotateArm2End, MouseButtons.Left, ModifierKeys.None, True)

                'Set the root part to be frozen.
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Freeze", "True"})

                'Resize the root part and graphic.
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Height", "0.2"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Width", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Length", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Height", "0.2"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Width", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Length", "0.05"})

                'Resize the child part and graphic.
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Height", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Width", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Length", "0.2"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Height", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Width", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Length", "0.2"})

                'Reposition the child part relative to the parent part
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.X", "0.125"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.Y", "-0.075"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.Z", "0"})

                'Reposition the joint relative to the parent part
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.X", m_strInitialJointXPos})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.Y", m_strInitialJointYPos})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.Z", m_strInitialJointZPos})

                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", m_strInitialJointXRot})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Y", m_strInitialJointYRot})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Z", m_strInitialJointZRot})

            End Sub

            Protected Overrides Sub RepositionBlockerPart()
                'Resize the child part and graphic.
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Height", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Width", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Length", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Height", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Width", "0.05"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Length", "0.05"})

                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.X", "0"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Y", "0.125"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Z", "0"})

                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.X", "0"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.Y", "0"})
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.Z", "0"})
            End Sub

#End Region

        End Class

    End Namespace
End Namespace


