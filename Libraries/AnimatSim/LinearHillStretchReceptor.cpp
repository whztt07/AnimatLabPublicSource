/**
\file	LinearHillStretchReceptor.cpp

\brief	Implements the linear hill stretch receptor class.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sensor.h"
#include "Attachment.h"
#include "Structure.h"
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

#include "ExternalStimulus.h"

#include "LineBase.h"
#include "Gain.h"
#include "SigmoidGain.h"
#include "LengthTensionGain.h"
#include "MuscleBase.h" 
#include "LinearHillMuscle.h"
#include "LinearHillStretchReceptor.h"

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor.

\author	dcofer
\date	5/23/2011
**/
LinearHillStretchReceptor::LinearHillStretchReceptor()
{
	m_bApplyTension = false;
	m_fltIaDischargeConstant = 100;
	m_fltIIDischargeConstant = 100;
	m_fltIaRate = 0;
	m_fltIIRate = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	5/23/2011
**/
LinearHillStretchReceptor::~LinearHillStretchReceptor()
{
}

/**
\brief	Gets wheter tension is applied by the receptor or not.

\author	dcofer
\date	5/23/2011

\return	true to apply tension, false to not.
**/
bool LinearHillStretchReceptor::ApplyTension() {return m_bApplyTension;}

/**
\brief	Sets wheter tension is applied by the receptor or not.

\author	dcofer
\date	5/23/2011

\param	bVal	true to apply tension.
**/
void LinearHillStretchReceptor::ApplyTension(bool bVal) {m_bApplyTension = bVal;}

/**
\brief	Gets the ia discharge constant.

\author	dcofer
\date	5/23/2011

\return	discharge constant.
**/
float LinearHillStretchReceptor::IaDischargeConstant() {return m_fltIaDischargeConstant;}

/**
\brief	Sets the ia discharge constant.

\author	dcofer
\date	5/23/2011

\param	fltVal	The new value.
**/
void LinearHillStretchReceptor::IaDischargeConstant(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1e11, fltVal, true, "IaDischargeConstant");
	m_fltIaDischargeConstant = fltVal;
}

/**
\brief	Gets the ii discharge constant.

\author	dcofer
\date	5/23/2011

\return	discharge constant.
**/
float LinearHillStretchReceptor::IIDischargeConstant() {return m_fltIIDischargeConstant;}

/**
\brief	Sets the ii discharge constant.

\author	dcofer
\date	5/23/2011

\param	fltVal	The new value.
**/
void LinearHillStretchReceptor::IIDischargeConstant(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1e11, fltVal, true, "IIDischargeConstant");
	m_fltIIDischargeConstant = fltVal;
}

/**
\brief	Gets the ia rate.

\author	dcofer
\date	5/23/2011

\return	rate.
**/
float LinearHillStretchReceptor::IaRate() {return m_fltIaRate;}

/**
\brief	Gets the ii rate.

\author	dcofer
\date	5/23/2011

\return	rate.
**/
float LinearHillStretchReceptor::IIRate() {return m_fltIIRate;}

void LinearHillStretchReceptor::CalculateTension()
{
	LinearHillMuscle::CalculateTension();

	m_fltIaRate = m_fltIaDischargeConstant*m_fltSeLength;
	m_fltIIRate = m_fltIIDischargeConstant*m_fltPeLength;
}

void LinearHillStretchReceptor::ResetSimulation()
{
	LinearHillMuscle::ResetSimulation();

	m_fltIaRate = 0;
	m_fltIIRate = 0;
}

float *LinearHillStretchReceptor::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "IA")
		lpData = &m_fltIaRate;
	else if(strType == "IB")
		lpData = &m_fltIbRate;
	else if(strType == "II")
		lpData = &m_fltIIRate;
	else
		lpData = LinearHillMuscle::GetDataPointer(strDataType);

	return lpData;
}

bool LinearHillStretchReceptor::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(LinearHillMuscle::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "APPLYTENSION")
	{
		ApplyTension(Std_ToBool(strValue));
		return true;
	}

	if(strDataType == "IADISCHARGE")
	{
		IaDischargeConstant((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "IIDISCHARGE")
	{
		IIDischargeConstant((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void LinearHillStretchReceptor::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	LinearHillMuscle::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Ia", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("II", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("ApplyTension", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("IaDischarge", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("IIDischarge", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

void LinearHillStretchReceptor::Load(CStdXml &oXml)
{
	LinearHillMuscle::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	ApplyTension(oXml.GetChildBool("ApplyTension", m_bApplyTension));
	IaDischargeConstant(oXml.GetChildFloat("IaDischarge", m_fltIaDischargeConstant));
	IIDischargeConstant(oXml.GetChildFloat("IIDischarge", m_fltIIDischargeConstant));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
