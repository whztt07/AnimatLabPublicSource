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
Imports System.Xml

Namespace UITests
    Namespace ConversionTests
        Namespace BodyPartTests
            Namespace JointTests

                <CodedUITest()>
                Public Class PrismaticConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"
                    '

                    <TestMethod(), _
                    DataSource("System.Data.OleDb", _
                               "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                               "PrismaticRotationTestData", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_PrismaticJointRotations()
                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim dblJointRotX As Double = CDbl(TestContext.DataRow("X"))
                        Dim dblJointRotY As Double = CDbl(TestContext.DataRow("Y"))
                        Dim dblJointRotZ As Double = CDbl(TestContext.DataRow("Z"))
                        Dim strOrientation As String = CStr(TestContext.DataRow("Orientation"))
                        Dim strDataPrefix As String = CStr(TestContext.DataRow("DataPrefix"))

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\PrismaticTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\PrismaticTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\PrismaticTests\" & m_strProjectName

                        ModifyJointRotationInProjectFile(m_strOldProjectFolder, dblJointRotX, dblJointRotY, dblJointRotZ, strOrientation)

                        TestConversionProject(strDataPrefix, 2000)

                    End Sub

                    <TestMethod(), _
                      DataSource("System.Data.OleDb", _
                                 "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                 "PrismaticLimitTestData", _
                                 DataAccessMethod.Sequential), _
                      DeploymentItem("TestCases.accdb")>
                    Public Sub Test_PrismaticLimits()
                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim dblMin As Single = CSng(TestContext.DataRow("Min"))
                        Dim dblMax As Single = CSng(TestContext.DataRow("Max"))
                        Dim dblDamping As Single = CSng(TestContext.DataRow("Damping"))
                        Dim strRestitution As Single = CSng(TestContext.DataRow("Restitution"))
                        Dim strStiffness As Single = CSng(TestContext.DataRow("Stiffness"))
                        Dim strDataPrefix As String = CStr(TestContext.DataRow("DataPrefix"))

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\PrismaticTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\PrismaticTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\PrismaticTests\" & m_strProjectName

                        ModifyJointConstraintsInProjectFile(m_strOldProjectFolder, dblMin, dblMax, False, dblDamping, strRestitution, strStiffness)

                        TestConversionProject(strDataPrefix, 2000)

                    End Sub

                    <TestMethod()>
                    Public Sub Test_PrismaticMotor()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Arm", 0.03)
                        aryMaxErrors.Add("JointPos", 0.03)
                        aryMaxErrors.Add("JointVel", 0.03)
                        aryMaxErrors.Add("AVm", 0.01)
                        aryMaxErrors.Add("BVm", 0.01)
                        aryMaxErrors.Add("BIa", 0.000000003)
                        aryMaxErrors.Add("default", 0.03)

                        m_strProjectName = "PrismaticMotorTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\PrismaticTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\PrismaticTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\PrismaticTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        'Run the same sim a second time to check for changes between sims.
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "DisableWhenDone", "True"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Motor_Stim_2_DisableWhenDone_")
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "DisableWhenDone", "False"})

                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "Enabled", "False"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Motor_Stim_2_Disabled_")
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "Enabled", "True"})

                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "StartTime", "2.5"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "EndTime", "3.5"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Motor_Stim_2_Times_")
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "StartTime", "2"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "EndTime", "3"})

                        SetMotorVelocityStimulus("Motor_Stim_2", False, True, 2, 3, False, False, 0, "-5*t")
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Motor_Stim_2_Equation_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "ValueType", "Constant"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_1", "Enabled", "False"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Motor_Stim_2", "Enabled", "False"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\A_Stim_1", "Enabled", "True"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\A_Stim_2", "Enabled", "True"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Neural_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_Joint", "Gain.C", "5 "})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AJoint_5_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint_B", "Gain.C", "50 n"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "JointB_50n_")
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_Joint", "Gain.C", "10 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint_B", "Gain.C", "100 n"})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem"}, 2000)

                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                          "FiringRateGUI.DataObjects.Behavior.Neurons.Normal", New Point(100, 250), "C")

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint", "", _
                                          "", False)
                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2"})) Then
                            Throw New System.Exception("2 adapter was not added")
                        End If
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Name", "C_Joint"})

                        AddStimulus("Tonic Current", "Organism_1", "\Behavioral System\Neural Subsystem\C", "C_Stim_1")
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "EndTime", "2.4 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "StartTime", "2.2 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "CurrentOn", "5 n"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C_Joint", "Gain.C", "10 "})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Add_C_")

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C_Joint", "Delete Node")
                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.Joint", New Point(228, 250), "Joint2")

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint2", "", "", False)
                        AssertErrorDialogShown("You must specify a linked body part before you can add an adapter to this node.", enumMatchType.Equals)

                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint2", _
                                             "Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Prismatic"})
                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint2", "", "", False)
                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\5"})) Then
                            Throw New System.Exception("5 adapter was not added")
                        End If
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\5", "Name", "C_Joint"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C_Joint", "Gain.C", "10 "})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Add_C_")

                        'Add subsystem.
                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(316, 30), "S2")

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint2", False})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C_Joint", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint2\C_Joint", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C_Joint\C", True})
                        DeleteSelectedParts("Delete Group", True)
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)
                        ExecuteMethod("ClickMenuItem", New Object() {"PasteInPlaceToolStripMenuItem"})
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Stimuli\C_Stim_1"})) Then
                            Throw New System.Exception("C_Stim_1 was not deleted")
                        End If
                        AddStimulus("Tonic Current", "Organism_1", "\Behavioral System\Neural Subsystem\S2\C", "C_Stim_1")
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "EndTime", "2.4 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "StartTime", "2.2 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "CurrentOn", "5 n"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Add_C_")

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint", "Delete Node")
                        System.Threading.Thread.Sleep(1000)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_Joint"})) Then
                            Throw New System.Exception("A_Joint was not deleted")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Joint_B"})) Then
                            Throw New System.Exception("Joint_B was not deleted")
                        End If
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "EndTime", "3 "})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteJoint_")

                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(316, 114), "OP")
                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP", _
                                             "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B"})
                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Joint2", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP", "", "", False)
                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\8"})) Then
                            Throw New System.Exception("A_B adapter node was not created.")
                        End If
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\8", "Name", "Joint_B"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Joint_B", "Gain.C", "100 n"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteJoint_AddB_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\C", "Vrest", "0"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\C", "Vth", "0"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C_Stim_1", "CurrentOn", "-5 n"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\C_Joint", "DataTypes", "FiringRateGUI.DataObjects.Behavior.Neurons.Normal.DataTypes.MembraneVoltage"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "C_Vm_")

                    End Sub

#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        'This test compares data to that generated from the old version. We never re-generate the data in V2, so this should always be false 
                        'regardless of the setting in app.config.
                        m_bGenerateTempates = False

                    End Sub

                    <TestCleanup()> Public Overrides Sub MyTestCleanup()
                        MyBase.MyTestCleanup()
                    End Sub

                    Protected Overrides Sub SetWindowsToOpen()
                        m_aryWindowsToOpen.Add("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan")
                        m_aryWindowsToOpen.Add("Tool Viewers\JointData")
                    End Sub

#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
