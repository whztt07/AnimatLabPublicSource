/**
\file	MotorStimulus.cpp

\brief	Implements the vs motor velocity stimulus class.
**/

#include "StdAfx.h"

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include "Gain.h"
#include "Node.h"
#include "Link.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "MotorizedJoint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimulus.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

#include "MotorStimulus.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{
/**
\brief	Default constructor.

\author	dcofer
\date	4/3/2011
**/
MotorStimulus::MotorStimulus()
{
	m_lpJoint = NULL;
	m_lpEval = NULL;
	m_fltValue = 0;
	m_fltValueReport = 0;
	m_bDisableMotorWhenDone = false;
	m_lpPosition = NULL;
	m_lpVelocity = NULL;
	m_iTargetID = DESIRED_VELOCITY_TYPE;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/3/2011
**/
MotorStimulus::~MotorStimulus()
{

try
{
	m_lpJoint = NULL;
	if(m_lpEval) delete m_lpEval;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of MotorStimulus\r\n", "", -1, false, true);}
}

/**
\brief	Sets the velocity equation used to control the motor speed over time.

\author	dcofer
\date	4/3/2011

\param	strVal	The post-fix velocity equation string. 
**/
void MotorStimulus::Equation(std::string strVal)
{
	//Initialize the postfix evaluator.
	if(m_lpEval) 
	{delete m_lpEval; m_lpEval = NULL;}

	m_strEquation = strVal;
	m_lpEval = new CStdPostFixEval;

	m_lpEval->AddVariable("t");
	m_lpEval->AddVariable("p");
	m_lpEval->AddVariable("v");
	m_lpEval->Equation(m_strEquation);
}

void MotorStimulus::TargetID(int iID) 
{
	if(iID == DESIRED_VELOCITY_TYPE || iID == DESIRED_POSITION_TYPE)
		m_iTargetID = iID;
	else
		THROW_PARAM_ERROR(Al_Err_lInvalidTargetID, Al_Err_strInvalidTargetID, "ID", iID);
}

void MotorStimulus::TargetID(std::string strID)
{
	std::string strId = Std_CheckString(strID);

	if(strId == "VELOCITY")
		m_iTargetID = DESIRED_VELOCITY_TYPE;
	else if(strId == "POSITION")
		m_iTargetID = DESIRED_POSITION_TYPE;
	else
		THROW_PARAM_ERROR(Al_Err_lInvalidTargetID, Al_Err_strInvalidTargetID, "ID", strID);
}

void MotorStimulus::ResetSimulation()
{
	ExternalStimulus::ResetSimulation();

	m_fltValue = 0;
	m_fltValueReport = 0;
}

void MotorStimulus::Initialize()
{
	ExternalStimulus::Initialize();

	//Lets try and get the joint we will be injecting.
	m_lpJoint = dynamic_cast<MotorizedJoint *>(m_lpSim->FindJoint(m_strStructureID, m_strJointID));
	if(!m_lpJoint)
		THROW_PARAM_ERROR(Al_Err_lJointNotMotorized, Al_Err_strJointNotMotorized, "ID", m_strJointID);

	m_lpPosition = m_lpJoint->GetDataPointer("JOINTPOSITION");
	m_lpVelocity = m_lpJoint->GetDataPointer("JOINTACTUALVELOCITY");
}

void MotorStimulus::Activate()
{
	ExternalStimulus::Activate();

	//int iTest = 0;
	//if(m_lpSim->Time() >= 5)
	//	iTest = iTest;
		
	if(m_bEnabled)
	{
		m_lpJoint->EnableMotor(true);

		if(m_iTargetID == DESIRED_VELOCITY_TYPE)
			m_lpJoint->DesiredVelocity(0);
	}
}


void MotorStimulus::StepSimulation()
{
	//float fltVel=0;
	//int iTest = 0;
	//if(m_lpSim->Time() >= 5.75)
	//	iTest = iTest;

	try
	{
		if(m_bEnabled)
		{
			//IMPORTANT! This stimulus applies a stimulus to the physics engine, so it should ONLY be called once for every time the physcis
			//engine steps. If you do not do this then the you will accumulate forces being applied during the neural steps, and the total
			//for you apply will be greater than what it should be. To get around this we will only call the code in step simulation if
			//the physics step count is equal to the step interval.
			if(m_lpSim->PhysicsStepCount() == m_lpSim->PhysicsStepInterval())
			{
				m_lpEval->SetVariable("t", (m_lpSim->Time()-m_fltStartTime) );
				
				if(m_lpPosition)
					m_lpEval->SetVariable("p",  *m_lpPosition);
				
				if(m_lpVelocity)
					m_lpEval->SetVariable("v", *m_lpVelocity);

				m_fltValueReport = m_fltValue = m_lpEval->Solve();
				//fltVel = -sin(6.28*(m_lpSim->Time()-m_fltStartTime));

				if(!m_lpJoint->UsesRadians())
					m_fltValue *= m_lpSim->InverseDistanceUnits();

				if(m_iTargetID == DESIRED_VELOCITY_TYPE)
					m_lpJoint->DesiredVelocity(m_fltValue);
				else if(m_iTargetID == DESIRED_POSITION_TYPE)
					m_lpJoint->DesiredPosition(m_fltValue);
			}
		}
	}
	catch(...)
	{
		LOG_ERROR("Error Occurred while setting Joint Velocity");
	}
}


void MotorStimulus::Deactivate()
{
	ExternalStimulus::Deactivate();

	if(m_bEnabled)
	{
		if(m_iTargetID == DESIRED_VELOCITY_TYPE)
			m_lpJoint->DesiredVelocity(0);

		if(m_bDisableMotorWhenDone)
			m_lpJoint->EnableMotor(false);
	}
}

float *MotorStimulus::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	if(strType == "VELOCITY" || strType == "VALUE")
		lpData = &m_fltValueReport;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 

bool MotorStimulus::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(ExternalStimulus::SetData(strDataType, strValue, false))
		return true;

	if(strType == "VELOCITY" || strType == "EQUATION")
	{
		Equation(strValue);
		return true;
	}

	if(strType == "DISABLEWHENDONE")
	{
		DisableMotorWhenDone(Std_ToBool(strValue));
		return true;
	}

	if(strType == "TARGETID")
	{
		TargetID(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void MotorStimulus::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	ExternalStimulus::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Velocity", AnimatPropertyType::Float, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("Value", AnimatPropertyType::Float, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("Equation", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("DisableMotorWhenDone", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TargetID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

void MotorStimulus::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	m_strStructureID = oXml.GetChildString("StructureID");
	if(Std_IsBlank(m_strStructureID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strJointID = oXml.GetChildString("JointID");
	if(Std_IsBlank(m_strStructureID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	if(oXml.FindChildElement("Velocity", false))
		Equation(oXml.GetChildString("Velocity"));
	else
		Equation(oXml.GetChildString("Equation"));

	DisableMotorWhenDone(oXml.GetChildBool("DisableMotorWhenDone", m_bDisableMotorWhenDone));
	TargetID(oXml.GetChildString("TargetID", "Velocity"));

	oXml.OutOfElem(); //OutOf Simulus Element
}

	}			//ExternalStimuli
}				//AnimatSim




