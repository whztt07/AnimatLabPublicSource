#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "Link.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
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
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"
#include "Gain.h"

#include "RobotInterface.h"
#include "RobotIOControl.h"
#include "RobotPartInterface.h"


namespace AnimatSim
{
	namespace Robotics
	{

RobotPartInterface::RobotPartInterface(void)
{
	m_lpParentInterface = NULL;
	m_lpParentIOControl = NULL;
	m_lpPart = NULL;
	m_lpProperty = NULL;
	m_iIOComponentID = 0;
	m_fltIOValue = 0;
	m_iIOValue = 0;
	m_fltIOScaledValue = 0;
	m_lpGain = NULL;
	m_bChanged= false;
	m_fltStepIODuration = 0;
}

RobotPartInterface::~RobotPartInterface(void)
{

try
{
	if(m_lpPart)
		m_lpPart->RemoveRobotPartInterface(this);

	//We do not own any of these.
	m_lpParentInterface = NULL;
	m_lpParentIOControl = NULL;
	m_lpPart = NULL;
	m_lpProperty = NULL;
	if(m_lpGain) delete m_lpGain;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RobotPartInterface\r\n", "", -1, false, true);}
}

void RobotPartInterface::ParentIOControl(RobotIOControl *lpParent) 
{
	m_lpParentIOControl = lpParent;

	if(m_lpParentIOControl)
		m_lpParentInterface = m_lpParentIOControl->ParentInterface();
}

RobotIOControl *RobotPartInterface::ParentIOControl() {return m_lpParentIOControl;}

int RobotPartInterface::IOComponentID() {return m_iIOComponentID;}

void RobotPartInterface::IOComponentID(int iID) {m_iIOComponentID = iID;}

float RobotPartInterface::IOValue() {return m_fltIOValue;}

void RobotPartInterface::IOValue(float fltVal) 
{
	m_fltIOValue = fltVal;
	m_iIOValue = (int) fltVal;
}
						
int RobotPartInterface::IOValueInt() {return m_iIOValue;}

void RobotPartInterface::IOValueInt(int iVal) 
{
	m_iIOValue = iVal;
	m_fltIOValue = (float) iVal;
}

float RobotPartInterface::IOScaledValue() {return m_fltIOScaledValue;}

void RobotPartInterface::IOScaledValue(float fltVal) {m_fltIOScaledValue = fltVal;}
			
bool RobotPartInterface::Changed() {return m_bChanged;}

void RobotPartInterface::Changed(bool bVal) {m_bChanged = bVal;}

void RobotPartInterface::LinkedPartID(std::string strID)
{
	m_strPartID = strID;
	m_strPropertyName = "";
	Initialize();
}

std::string RobotPartInterface::LinkedPartID() {return m_strPartID;}

void RobotPartInterface::PropertyName(std::string strName)
{
	m_strPropertyName = strName;
	Initialize();
}

std::string RobotPartInterface::PropertyName() {return m_strPropertyName;}

/**
\brief	Returns true if this part interface is for controlling a motor.

\author	dcofer
\date	6/19/2014

\return	Pointer to the gain.
**/
bool RobotPartInterface::IsMotorControl() {return false;}

/**
\brief	If this is a servo controller interface then it will take a continuous positon and return back a valid quantized position for that servo.

\discussion Servos have discreet positions they can obtain based on the bit resolution of the goal position. So they can only be set to move to
those positions. If we are trying to sync the sim to match this behavior then we need to only move the sim to these positions as well.
You will need to override this method in your actual motor control part interface and implement for your motor model.

\author	dcofer
\date	6/19/2014

\return	Quantized position.
**/
float RobotPartInterface::QuantizeServoPosition(float fltPos)
{
	return fltPos;
}

/**
\brief	If this is a servo controller interface then it will take a continuous velocity and return back a valid quantized velocity for that servo.

\discussion Servos have discreet velocity they can obtain based on the bit resolution of the goal velocity. So they can only be set to move at
those velocity. If we are trying to sync the sim to match this behavior then we need to only move the sim to these velociies as well.
You will need to override this method in your actual motor control part interface and implement for your motor model.

\author	dcofer
\date	6/19/2014

\return	Quantized velocity.
**/
float RobotPartInterface::QuantizeServoVelocity(float fltVel)
{
	return fltVel;
}

/**
\brief	Gets the poitner to the gain function.

\author	dcofer
\date	3/18/2011

\return	Pointer to the gain.
**/
Gain *RobotPartInterface::GetGain() {return m_lpGain;}

void RobotPartInterface::SetGain(Gain *lpGain)
{
	if(m_lpGain)
	{
		delete m_lpGain;
		m_lpGain = NULL;
	}

	m_lpGain = lpGain;
	m_lpGain->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, true);
}

/**
\brief	Gets the time duration required to perform one step of the IO for this part.

\author	dcofer
\date	5/7/2014

\return	Step duration.
**/
float RobotPartInterface::StepIODuration() {return m_fltStepIODuration;}

/**
\brief	Creates and adds a gain object. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the gain. 
**/
void RobotPartInterface::AddGain(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");

	SetGain(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
}

#pragma region DataAccesMethods

float *RobotPartInterface::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "IOVALUE")
		return &m_fltIOValue;
	else if(strType == "IOSCALEDVALUE")
		return &m_fltIOScaledValue;
	else if(strType == "STEPIODURATION")
		return &m_fltStepIODuration;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RobotPartInterface::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(strType == "IOCOMPONENTID")
	{
		IOComponentID((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "LINKEDPARTID")
	{
		LinkedPartID(strValue);
		return true;
	}

	if(strType == "PROPERTYNAME")
	{
		PropertyName(strValue);
		return true;
	}
	
	if(strType == "GAIN")
	{
		AddGain(strValue);
		return true;
	}

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}

	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RobotPartInterface::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("IOValue", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("IOScaledValue", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("StepIODuration", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("IOComponentID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PropertyName", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LinkedPartID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

#pragma endregion

/**
\brief	This method is called after all connections to whatever control board have been made. It is meant
to be used to setup connection information specific to this part. For example, We connect to a Firmata
microcontroller like an Arduino, and then do a setup that could take some time. We should not attempt to
setup any of the pins until after the board itself has been setup. After that we need to loop through and setup
all the parts. That is what this method is for.

\author	dcofer
\date	5/1/2014

**/
void RobotPartInterface::SetupIO()
{
}

/**
\brief	This method is used to send/recieve the actual IO. This will often be in a seperate thread than
the StepSimulation. StepSimulation gets/sets the values in the sim and gets it read for the thread that
does the IO. Once that thread is ready to send/receive it uses that value to perform the operation.

\author	dcofer
\date	5/2/2014

**/
void RobotPartInterface::StepIO(int iPartIdx)
{
}

/**
\brief	This method is called just before the IO thread is closed down. It gives the IO objects a chance to do 
any required cleanup.

\author	dcofer
\date	5/12/2014

**/
void RobotPartInterface::ShutdownIO()
{
}

void RobotPartInterface::Initialize()
{
	//We need to find the referenced body part and set its robot part interface to this one.
	if(!Std_IsBlank(m_strPartID))
		m_lpPart = dynamic_cast<BodyPart *>(m_lpSim->FindByID(m_strPartID));
	else
	{
		m_lpPart = NULL;
		m_lpProperty = NULL;
	}

	if(m_lpPart)
	{
		if(!Std_IsBlank(m_strPropertyName))
			m_lpProperty = m_lpPart->GetDataPointer(m_strPropertyName);
		else
			m_lpProperty = NULL;

		m_lpPart->AddRobotPartInterface(this);
	}
	else
		m_lpProperty = NULL;
}

void RobotPartInterface::ResetSimulation()
{
	AnimatBase::ResetSimulation();

	m_fltIOValue = 0;
	m_iIOValue = 0;
	m_fltIOScaledValue = 0;
	m_bChanged= false;
}

void RobotPartInterface::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	
	LinkedPartID(oXml.GetChildString("LinkedPartID", ""));
	PropertyName(oXml.GetChildString("PropertyName", ""));
	IOComponentID(oXml.GetChildInt("IOComponentID", m_iIOComponentID));
	
	SetGain(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

	}
}