#include "stdafx.h"
#include "Util.h"
#include "Logger.h"
#include "PropertyUpdateException.h"
#include "ISimulatorInterface.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
#include "IDataObjectInterface.h"
#include "DataObjectInterface.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

SimGUICallback::SimGUICallback(AnimatGUI::Interfaces::ISimulatorInterface ^doSim)
{
	m_doSim = doSim;
}

SimGUICallback::~SimGUICallback(void)
{
}

void SimGUICallback::NeedToStopSimulation()
{
	m_doSim->FireNeedToStopSimulationEvent();
}

void SimGUICallback::HandleNonCriticalError(string strError)
{
	System::String ^sError = gcnew String(strError.c_str());

	m_doSim->FireHandleNonCriticalErrorEvent(sError);
}

void SimGUICallback::HandleCriticalError(string strError)
{
	System::String ^sError = gcnew String(strError.c_str());

	m_doSim->FireHandleCriticalErrorEvent(sError);
}

	}
}
