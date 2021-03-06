// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#ifdef WIN32
    #define _SCL_SECURE_NO_WARNINGS
    #define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
    // Windows Header Files:
    #include <windows.h>

    #define ROBOTICS_PORT __declspec( dllexport )
    #define ARDUINO_PORT __declspec( dllimport )
#else
    #define ROBOTICS_PORT
    #define ARDUINO_PORT
#endif

//#define STD_TRACING_ON

#include "StdUtils.h"
#include "AnimatSim.h"

//Include the timer code and openthreads code from osg
#include <osg/Timer>
#include <OpenThreads/Thread>

#include "RoboticsAnimatSimConstants.h"
#include "dynamixel.h"

#include "ofArduino.h"

//Simulation Objects
namespace RoboticsAnimatSim
{
	class RbClassFactory;
	class RbSimulator;

	namespace Environment
	{
		class RbJoint;
		class RbMotorizedJoint;
		class RbMaterialType;
        class RbConstraintRelaxation;
        class RbConstraintFriction;
		class RbRigidBody;

		namespace Bodies
		{
			class RbBox;
			class RbCone;
			class RbCylinder;
			class RbLinearHillMuscle;
			class RbLinearHillStretchReceptor;
			class RbPlane;
			class RbSphere;
			class RbSpring;
		}

		namespace Joints
		{
			class RbBallSocket;
			class RbHinge;
			class RbHingeLimit;
			class RbPrismatic;
			class RbPrismaticLimit;
		}
	}

	namespace ExternalStimuli
	{
		class RbForceStimulus;
		class RbMotorVelocityStimulus;
	}

    namespace Robotics
    {
        namespace RobotInterfaces
        {
            class RbLANWirelessInterface;
        }

        namespace RobotIOControls
        {
			class RbDynamixelServo;
			class RbDynamixelMotorUpdateData;

			namespace DynamixelUSB
			{
				class RbDynamixelUSB;
				class RbDynamixelUSBServo;
				class RbDynamixelUSBHinge;
				class RbDynamixelUSBPrismatic;
			}

			namespace Firmata
			{
				class RbFirmataController;
				class RbFirmataPart;
				class RbFirmataAnalogInput;
				class RbFirmataAnalogOutput;
				class RbFirmataDigitalInput;
				class RbFirmataDigitalOutput;
				class RbFirmataHingeServo;
				class RbFirmataPrismaticServo;
				class RbFirmataPWMOutput;
			}

			class RbXBeeCommander;
			class RbAnimatSerial;
		}
    }
}

using namespace RoboticsAnimatSim;
using namespace RoboticsAnimatSim::ExternalStimuli;
using namespace RoboticsAnimatSim::Environment;
using namespace RoboticsAnimatSim::Environment::Bodies;
using namespace RoboticsAnimatSim::Environment::Joints;
using namespace RoboticsAnimatSim::Robotics;
using namespace RoboticsAnimatSim::Robotics::RobotInterfaces;
using namespace RoboticsAnimatSim::Robotics::RobotIOControls;
using namespace RoboticsAnimatSim::Robotics::RobotIOControls::DynamixelUSB;
using namespace RoboticsAnimatSim::Robotics::RobotIOControls::Firmata;
