/**
\file	ExternalStimulus.cpp

\brief	Implements the external stimulus class. 
**/

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

namespace AnimatSim
{
	namespace ExternalStimuli
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
ExternalStimulus::ExternalStimulus()
{
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
ExternalStimulus::~ExternalStimulus()
{
}

bool ExternalStimulus::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(ActivatedItem::SetData(strDataType, strValue, bThrowError))
	{
		m_lpSim->GetExternalStimuliMgr()->ReInitialize();
		return true;
	}

	//Value type tells whether this is using an equation or constant. This is determined in the
	//derived class. Lets set this to true here just so we do not generate an exception.
	if(strType == "VALUETYPE")
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void ExternalStimulus::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	ActivatedItem::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ValueType", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

bool ExternalStimulus::operator<(ActivatedItem *lpItem)
{
	ExternalStimulus *lpStimulus = dynamic_cast<ExternalStimulus *>(lpItem);

	if(!lpStimulus)
		THROW_ERROR(Al_Err_lItemNotStimulusType, Al_Err_strItemNotStimulusType);

	if(m_lStartSlice < lpStimulus->m_lStartSlice)
		return true;

	if( (m_lStartSlice == lpStimulus->m_lStartSlice) && (m_lEndSlice < lpStimulus->m_lEndSlice))
		return true;

	return false;
}

	}			//ExternalStimuli
}				//AnimatSim
