// RbFirmataAnalogInput.cpp: implementation of the RbFirmataAnalogInput class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbHingeLimit.h"
#include "RbHinge.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbFirmataPart.h"
#include "RbFirmataAnalogInput.h"
#include "RbFirmataController.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbFirmataAnalogInput::RbFirmataAnalogInput() 
{
}

RbFirmataAnalogInput::~RbFirmataAnalogInput()
{
}

void RbFirmataAnalogInput::SetupIO()
{
	if(!m_lpSim->InSimulation())
		m_lpFirmata->sendAnalogPinReporting(m_iIOComponentID, ARD_ANALOG);
}

void RbFirmataAnalogInput::StepIO(int iPartIdx)
{
	if(!m_lpSim->InSimulation())
	{
		int iValue = m_lpFirmata->getAnalog(m_iIOComponentID);
		if(iValue != -1 && m_iIOValue != iValue)
		{
			//std::cout << "Analog In: " << iValue << "\r\n";

			IOValue(iValue);
			m_bChanged = true;
		}
	}
}

void RbFirmataAnalogInput::StepSimulation()
{
    RobotPartInterface::StepSimulation();

	if(m_bChanged)
	{
		m_bChanged = false;

		//Calculate the gain of the IO value.
		float fltValue = m_lpGain->CalculateGain(m_fltIOValue);

		if(fltValue >= 1)
			fltValue = fltValue;

		//Remove any previously added value from the param
		*m_lpProperty -= m_fltIOScaledValue;

		m_fltIOScaledValue = fltValue;

		//Add the value back.
		*m_lpProperty += m_fltIOScaledValue;
	}
}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

