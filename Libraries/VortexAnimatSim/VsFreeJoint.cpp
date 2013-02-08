/**
\file	VsFreeJoint.cpp

\brief	Implements the vortex universal class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"
#include "VsFreeJoint.h"


namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

/**
\brief	Default constructor.

\author	dcofer
\date	4/15/2011
**/
VsFreeJoint::VsFreeJoint()
{
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
VsFreeJoint::~VsFreeJoint()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsFreeJoint/\r\n", "", -1, FALSE, TRUE);}
}

void VsFreeJoint::DeletePhysics()
{
}

void VsFreeJoint::SetupPhysics()
{
}

void VsFreeJoint::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods


BOOL VsFreeJoint::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	if(VsJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(BallSocket::SetData(strDataType, strValue, FALSE))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void VsFreeJoint::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	VsJoint::Physics_QueryProperties(aryNames, aryTypes);
	BallSocket::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
