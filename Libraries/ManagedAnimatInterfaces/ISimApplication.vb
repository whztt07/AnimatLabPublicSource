﻿
Public Interface ISimApplication

    Event ProjectLoaded()
    Event ProjectSaved()
    Event ProjectClosed()
    Event ProjectCreated()
    Event ApplicationExiting()
    Event SimulationStarting()
    Event SimulationResuming()
    Event SimulationStarted()
    Event SimulationPaused()
    Event SimulationStopped()

    Event BeforeAddNode(ByVal doNode As Object)
    Event AfterAddNode(ByVal doNode As Object)
    Event BeforeRemoveNode(ByVal doNode As Object)
    Event AfterRemoveNode(ByVal doNode As Object)

    Event BeforeAddLink(ByVal doLink As Object)
    Event AfterAddLink(ByVal doLink As Object)
    Event BeforeRemoveLink(ByVal doLink As Object)
    Event AfterRemoveLink(ByVal doLink As Object)

    Event BeforeAddBody(ByVal doBody As Object)
    Event AfterAddBody(ByVal doBody As Object)

End Interface