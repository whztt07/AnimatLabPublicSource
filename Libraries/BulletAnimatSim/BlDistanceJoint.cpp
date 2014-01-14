/**
\file	BlDistanceJoint.cpp

\brief	Implements the vortex distance joint class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlSphere.h"
#include "BlSimulator.h"
#include "BlDistanceJoint.h"

namespace BulletAnimatSim
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
BlDistanceJoint::BlDistanceJoint()
{
	SetThisPointers();
	m_btDistance = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
BlDistanceJoint::~BlDistanceJoint()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlDistanceJoint/\r\n", "", -1, false, true);}
}

/**
\brief	Sets up the graphics for the joint.

\details This joint shows no graphics, so I am overriding this method and leaving it blank to show nothing.

\author	dcofer
\date	4/16/2011
**/
void BlDistanceJoint::SetupGraphics()
{
}

void BlDistanceJoint::SetupPhysics()
{
    //FIX PHYSICS
	if(m_btJoint)
		DeletePhysics(false);

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	m_lpBlParent = dynamic_cast<BlRigidBody *>(m_lpParent);
	if(!m_lpBlParent)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	m_lpBlChild = dynamic_cast<BlRigidBody *>(m_lpChild);
	if(!m_lpBlChild)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

    m_btParent = m_lpBlParent->Part();
    m_btChild = m_lpBlChild->Part();

    //Need to calculate the matrix transform for the joint relative to the child also.
    osg::Matrix jointMT = this->GetOSGWorldMatrix();
    osg::Matrix parentMT = m_lpBlParent->GetOSGWorldMatrix();
    osg::Matrix osgJointRelParent = jointMT * osg::Matrix::inverse(parentMT);

    btTransform tmJointRelParent = osgbCollision::asBtTransform(osgJointRelParent);
    btTransform tmJointRelChild = osgbCollision::asBtTransform(m_osgMT->getMatrix());

	m_btDistance = new btGeneric6DofConstraint(*m_lpBlParent->Part(), *m_lpBlChild->Part(), tmJointRelParent, tmJointRelChild, true); 

    GetBlSimulator()->DynamicsWorld()->addConstraint(m_btDistance, true);
    m_btDistance->setDbgDrawSize(btScalar(5.f));

    //m_btDistance->set

	m_btJoint = m_btDistance;
}

void BlDistanceJoint::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods


bool BlDistanceJoint::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Joint::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlDistanceJoint::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	Joint::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
