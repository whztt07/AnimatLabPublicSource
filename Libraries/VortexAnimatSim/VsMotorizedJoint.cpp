/**
\file	VsMotorizedJoint.cpp

\brief	Implements the vs motorized joint class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

VsMotorizedJoint::VsMotorizedJoint()
{
	m_lpThisMotorJoint = NULL;
	m_bMotorOn = false;
}

VsMotorizedJoint::~VsMotorizedJoint()
{
}

void VsMotorizedJoint::SetThisPointers()
{
	VsJoint::SetThisPointers();

	m_lpThisMotorJoint = dynamic_cast<MotorizedJoint *>(this);
	if(!m_lpThisMotorJoint)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisMotorJoint, " + m_lpThisAB->Name());

	m_lpThisMotorJoint->PhysicsMotorJoint(this);
}

//If this is a servo motor then the "velocity" signal is not really a velocity signal in this case. 
//It is the desired position and we must convert it to the velocity needed to reach and maintian that position.
void VsMotorizedJoint::CalculateServoVelocity()
{
	if(!m_vxJoint)
		return;

	float fltTargetPos = m_lpThisJoint->GetPositionWithinLimits(m_lpThisMotorJoint->DesiredPosition());
	//For calculating the error we need to get the actual current position of the joint, not the value that was obtained last
	//time it called CollectData. If the motor is being synched with the robot for sim then this could be delayed. However, in the
	//real system this type of error correction would be taking place within the actual servo's motor loop.
	float fltError = fltTargetPos - GetCurrentVxJointPos();
	m_lpThisMotorJoint->SetPosition(fltTargetPos);

	////Test Code
	//int i=5;
	//if(m_lpThisMotorJoint->ID() == "0085EE18-89F7-4039-8648-EC51114BEEFF" && GetSimulator()->Time() >= 0.5 && fabs(fltTargetPos) > 0)
	//	i=6;

	AnimatSim::Environment::eJointMotorType MotorType = m_lpThisMotorJoint->MotorType();
	if((MotorType == eJointMotorType::PositionControl || MotorType == eJointMotorType::PositionVelocityControl) && m_lpThisMotorJoint->ReachedSetPosition() )
	{
		//Lock this joint position.
		m_lpThisMotorJoint->DesiredVelocity(0); 
	}
	else if(MotorType == eJointMotorType::PositionVelocityControl)
	{
		//If we set the desired velocity and position then make sure the desired velocity is in the right direction
		float fltDesiredVel = fabs(m_lpThisMotorJoint->DesiredVelocity()) * Std_Sign(fltError);

		float fltPosError = fabs(fltError);
		if(fltPosError > 0 && fltPosError < 0.05)
		{
			float fltDesiredVel2 = fltDesiredVel * (fabs(fltError)*m_lpThisMotorJoint->ServoGain());
			//Only do this if the new desired velocity is less than the original one to slow it down. Never speed it up.
			if(fabs(fltDesiredVel2) <= fabs(fltDesiredVel))
				fltDesiredVel = fltDesiredVel2;
		}

		m_lpThisMotorJoint->DesiredVelocity(fltDesiredVel);

		if(fabs(fltError) < 1e-4)
			m_lpThisMotorJoint->ReachedSetPosition(true);
	}
	else if(MotorType == eJointMotorType::PositionControl)
	{
		if(m_lpThisJoint->EnableLimits())
		{
			float fltProp = fltError / m_lpThisJoint->GetLimitRange();
			m_lpThisMotorJoint->DesiredVelocity(fltProp * m_lpThisMotorJoint->ServoGain()); 
		}
		else
			m_lpThisMotorJoint->DesiredVelocity(fltError * m_lpThisMotorJoint->MaxVelocity());	
	}
}


void VsMotorizedJoint::Physics_SetVelocityToDesired()
{
	if(m_lpThisMotorJoint->EnableMotor())
	{			
		if(m_lpThisMotorJoint->MotorType() == eJointMotorType::PositionControl || m_lpThisMotorJoint->MotorType() == eJointMotorType::PositionVelocityControl)
			CalculateServoVelocity();
		
		float fltDesiredVel = m_lpThisMotorJoint->DesiredVelocity();
		float fltMaxVel = m_lpThisMotorJoint->MaxVelocity();
		float fltMaxForce = m_lpThisMotorJoint->MaxForce();

		if(fltDesiredVel>fltMaxVel)
			fltDesiredVel = fltMaxVel;

		if(fltDesiredVel < -fltMaxVel)
			fltDesiredVel = -fltMaxVel;

		float fltSetVelocity = fltDesiredVel;

		m_lpThisMotorJoint->SetVelocity(fltSetVelocity);
		m_lpThisMotorJoint->DesiredVelocity(0);
		m_lpThisMotorJoint->DesiredPosition(0);

		//Only do anything if the velocity value has changed
		if(m_vxJoint && fabs(m_lpThisJoint->JointVelocity() - fltSetVelocity) > 1e-4)
		{
			if(fabs(fltSetVelocity) > 1e-4 && m_vxJoint)
			{
				if(m_vxJoint->getControl(m_iCoordID) == Vx::VxConstraint::CoordinateControlEnum::kControlLocked)
					Physics_EnableMotor(true, fltSetVelocity, fltMaxForce, false);
				else
					m_vxJoint->setMotorDesiredVelocity(m_iCoordID, fltSetVelocity);
			}
			else
			{ 
				if(m_vxJoint->getControl(m_iCoordID) != Vx::VxConstraint::CoordinateControlEnum::kControlLocked)
					m_lpThisMotorJoint->EnableLock(true, m_vxJoint->getCoordinateCurrentPosition(m_iCoordID), fltMaxForce);
			}
		}

		m_lpThisMotorJoint->PrevVelocity(fltSetVelocity);
	}
}

void VsMotorizedJoint::Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce)
{
	if (m_vxJoint)
	{ 		
		if(bOn)
		{
			//set the lock parameters
			m_vxJoint->setLockParameters(m_iCoordID, fltPosition, fltMaxLockForce);
			//turn on the lock (disabling motorized or free mode)
			m_vxJoint->setControl(m_iCoordID, VxConstraint::CoordinateControlEnum::kControlLocked);
		}
		else if (m_bMotorOn)
			Physics_EnableMotor(true, 0, fltMaxLockForce, false);
		else
			m_vxJoint->setControl(m_iCoordID, VxConstraint::CoordinateControlEnum::kControlFree);
	}
}

void VsMotorizedJoint::Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup)
{
	if (m_vxJoint)
	{   
		if(bOn)
		{
			if(m_vxJoint->getControl(m_iCoordID) != Vx::VxConstraint::CoordinateControlEnum::kControlMotorized)
				m_vxJoint->setControl(m_iCoordID, Vx::VxConstraint::CoordinateControlEnum::kControlMotorized);

			m_vxJoint->setMotorMaximumForce(m_iCoordID, fltMaxForce);
			m_vxJoint->setMotorDesiredVelocity(m_iCoordID, fltDesiredVelocity);
		}
		else
			m_vxJoint->setControl(m_iCoordID, Vx::VxConstraint::CoordinateControlEnum::kControlFree);

		m_bMotorOn = bOn;
	}
}

void VsMotorizedJoint::Physics_MaxForce(float fltVal)
{
	if(m_vxJoint)
		m_vxJoint->setMotorMaximumForce(m_iCoordID, fltVal);
}

	}			// Environment
}				//VortexAnimatSim