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

Namespace DataObjects.Gains

    Public Class Bell
        Inherits DataObjects.Gain

#Region " Attributes "

        Protected m_snA As AnimatGUI.Framework.ScaledNumber
        Protected m_snB As AnimatGUI.Framework.ScaledNumber
        Protected m_snC As AnimatGUI.Framework.ScaledNumber
        Protected m_snD As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property GainType() As String
            Get
                Return "Bell Curve"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Bell"
            End Get
        End Property

        Public Overrides ReadOnly Property GainEquation() As String
            Get
                Return "Y = B*e^(-C*(X-A)^2) + D"
            End Get
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the X Offset for the center of the bell curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property XOffset() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snA
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("A", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snA.Clone(m_snA.Parent, False, Nothing), ScaledNumber)
                    m_snA.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snA.Clone(m_snA.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("A", snOrig, snNew, True)
                End If
                'RecalculuateLimits()
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the height of the bell curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Amplitude() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snB
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("B", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snB.Clone(m_snB.Parent, False, Nothing), ScaledNumber)
                    m_snB.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snB.Clone(m_snB.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("B", snOrig, snNew, True)
                End If
                'RecalculuateLimits()
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the width of the bell curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property Width() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snC
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("C", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snC.Clone(m_snC.Parent, False, Nothing), ScaledNumber)
                    m_snC.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snC.Clone(m_snC.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("C", snOrig, snNew, True)
                End If
                'RecalculuateLimits()
            End Set
        End Property

        <Category("Equation Parameters"), _
         Description("Sets the Y offset of the bell curve."), _
         TypeConverter(GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter))> _
        Public Overridable Property YOffset() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snD
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("D", Value.ActualValue.ToString, True)

                    Dim snOrig As ScaledNumber = DirectCast(m_snD.Clone(m_snD.Parent, False, Nothing), ScaledNumber)
                    m_snD.CopyData(Value)

                    Dim snNew As ScaledNumber = DirectCast(m_snD.Clone(m_snD.Parent, False, Nothing), ScaledNumber)
                    Me.ManualAddPropertyHistory("D", snOrig, snNew, True)
                End If
                'RecalculuateLimits()
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snA = New AnimatGUI.Framework.ScaledNumber(Me, "XOffset", 500, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "", "")
            m_snB = New AnimatGUI.Framework.ScaledNumber(Me, "Amplitude", 500, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "", "")
            m_snC = New AnimatGUI.Framework.ScaledNumber(Me, "Width", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snD = New AnimatGUI.Framework.ScaledNumber(Me, "YOffset", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal strGainPropertyName As String, ByVal strIndependentUnits As String, ByVal strDependentUnits As String, _
                       Optional ByVal bLimitsReadOnly As Boolean = False, Optional ByVal bLimitOutputsReadOnly As Boolean = False, Optional ByVal bUseParentIncomingDataType As Boolean = True)
            MyBase.New(doParent)

            m_snA = New AnimatGUI.Framework.ScaledNumber(Me, "XOffset", 500, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "", "")
            m_snB = New AnimatGUI.Framework.ScaledNumber(Me, "Amplitude", 500, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "", "")
            m_snC = New AnimatGUI.Framework.ScaledNumber(Me, "Width", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snD = New AnimatGUI.Framework.ScaledNumber(Me, "YOffset", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_strGainPropertyName = strGainPropertyName
            m_strIndependentUnits = strIndependentUnits
            m_strDependentUnits = strDependentUnits
            m_bLimitsReadOnly = bLimitsReadOnly
            m_bLimitOutputsReadOnly = bLimitOutputsReadOnly
            m_bUseParentIncomingDataType = bUseParentIncomingDataType
        End Sub

        Public Overrides Function CalculateGain(ByVal dblInput As Double) As Double
            Dim dblVal As Double = 0

            If InLimits(dblInput) Then
                If m_snB.ActualValue <> 0 Then
                    dblVal = Math.Pow(dblInput - m_snA.ActualValue, 2)
                    dblVal = Math.Exp(-m_snC.ActualValue * dblVal)
                    dblVal = (m_snB.ActualValue * dblVal) + m_snD.ActualValue
                Else
                    dblVal = CalculateLimitOutput(dblInput)
                End If
            End If

            Return dblVal
        End Function

        Public Overrides Sub RecalculuateLimits()

            'If Not m_bUseLimits Then
            '    Dim dblExtent As Double = (15.88 * Math.Pow(m_snC.ActualValue, -0.8471)) - 1.475
            '    If dblExtent <= 0 Then dblExtent = 1

            '    dblExtent = (dblExtent + (dblExtent * 0.3)) / 2

            '    m_snLowerLimit.ActualValue = m_snA.ActualValue - dblExtent
            '    m_snUpperLimit.ActualValue = m_snA.ActualValue + dblExtent
            'End If

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New DataObjects.Gains.Bell(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
            Return oNew
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim gnOrig As DataObjects.Gains.Bell = DirectCast(doOriginal, DataObjects.Gains.Bell)

            m_snA = DirectCast(gnOrig.m_snA.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snB = DirectCast(gnOrig.m_snB.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snC = DirectCast(gnOrig.m_snC.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snD = DirectCast(gnOrig.m_snD.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Protected Overrides Sub SetAllSimData(ByVal doInterface As ManagedAnimatInterfaces.IDataObjectInterface)
            MyBase.SetAllSimData(doInterface)

            SetSimData("A", m_snA.ActualValue.ToString, True)
            SetSimData("B", m_snB.ActualValue.ToString, True)
            SetSimData("C", m_snC.ActualValue.ToString, True)
            SetSimData("D", m_snD.ActualValue.ToString, True)
        End Sub

        Public Overloads Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String, ByVal strGainPropertyName As String)
            MyBase.LoadData(oXml, strName, strGainPropertyName)

            oXml.IntoElem()

            m_snA.LoadData(oXml, "A")
            m_snB.LoadData(oXml, "B")
            m_snC.LoadData(oXml, "C")
            m_snD.LoadData(oXml, "D")

            oXml.OutOfElem()

        End Sub


        Public Overloads Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String)
            MyBase.SaveData(oXml, strName)

            oXml.IntoElem()

            m_snA.SaveData(oXml, "A")
            m_snB.SaveData(oXml, "B")
            m_snC.SaveData(oXml, "C")
            m_snD.SaveData(oXml, "D")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snA.SaveSimulationXml(oXml, Me, "A")
            m_snB.SaveSimulationXml(oXml, Me, "B")
            m_snC.SaveSimulationXml(oXml, Me, "C")
            m_snD.SaveSimulationXml(oXml, Me, "D")

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snA.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X Offset", pbNumberBag.GetType(), "XOffset", _
                                        "Equation Parameters", "Sets the X Offset for the center of the bell curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snB.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Amplitude", pbNumberBag.GetType(), "Amplitude", _
                                        "Equation Parameters", "Sets the height of the bell curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snC.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Width", pbNumberBag.GetType(), "Width", _
                                        "Equation Parameters", "Sets the width of the bell curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snD.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y Offset", pbNumberBag.GetType(), "YOffset", _
                                        "Equation Parameters", "Sets the Y offset of the bell curve.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snA Is Nothing Then m_snA.ClearIsDirty()
            If Not m_snB Is Nothing Then m_snB.ClearIsDirty()
            If Not m_snC Is Nothing Then m_snC.ClearIsDirty()
            If Not m_snD Is Nothing Then m_snD.ClearIsDirty()
        End Sub

#End Region

#End Region

    End Class

End Namespace
