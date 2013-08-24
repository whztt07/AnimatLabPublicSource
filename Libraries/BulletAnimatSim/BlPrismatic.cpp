/**
\file	BlPrismatic.cpp

\brief	Implements the vs prismatic class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlPrismaticLimit.h"
#include "BlRigidBody.h"
#include "BlPrismatic.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
BlPrismatic::BlPrismatic()
{
	SetThisPointers();
	m_vxPrismatic = NULL;

	m_lpUpperLimit = new BlPrismaticLimit();
	m_lpLowerLimit = new BlPrismaticLimit();
	m_lpPosFlap = new BlPrismaticLimit();

	m_lpUpperLimit->LimitPos(1, false);
	m_lpLowerLimit->LimitPos(-1, false);
	m_lpPosFlap->LimitPos(Prismatic::JointPosition(), false);
	m_lpPosFlap->IsShowPosition(true);

	m_lpUpperLimit->Color(0, 0, 1, 1);
	m_lpLowerLimit->Color(1, 1, 0.333, 1);
	m_lpPosFlap->Color(1, 0, 1, 1);

	m_lpLowerLimit->IsLowerLimit(true);
	m_lpUpperLimit->IsLowerLimit(false);
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
BlPrismatic::~BlPrismatic()
{
	//ConstraintLimits are deleted in the base objects.
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlPrismatic/\r\n", "", -1, false, true);}
}


void BlPrismatic::EnableLimits(bool bVal)
{
	Prismatic::EnableLimits(bVal);

	if(m_vxPrismatic)
		m_vxPrismatic->setLimitsActive(m_vxPrismatic->kLinearCoordinate, m_bEnableLimits);	

	if(m_bEnableLimits)
	{
		if(m_lpLowerLimit) m_lpLowerLimit->SetLimitPos();
		if(m_lpUpperLimit) m_lpUpperLimit->SetLimitPos();
	}
}

void BlPrismatic::JointPosition(float fltPos)
{
	m_fltPosition = fltPos;
	if(m_lpPosFlap)
		m_lpPosFlap->LimitPos(fltPos);
}


void BlPrismatic::SetAlpha()
{
	BlJoint::SetAlpha();

	m_lpUpperLimit->Alpha(m_fltAlpha);
	m_lpLowerLimit->Alpha(m_fltAlpha);
	m_lpPosFlap->Alpha(m_fltAlpha);
}

void BlPrismatic::DeleteJointGraphics()
{
	OsgPrismaticLimit *lpUpperLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpUpperLimit);
	OsgPrismaticLimit *lpLowerLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpLowerLimit);
	OsgPrismaticLimit *lpPosFlap = dynamic_cast<OsgPrismaticLimit *>(m_lpPosFlap);

    OsgPrismatic::DeletePrismaticGraphics(m_osgJointMT, lpUpperLimit, lpLowerLimit, lpPosFlap);

    if(m_lpUpperLimit) m_lpUpperLimit->DeleteGraphics();
    if(m_lpLowerLimit) m_lpLowerLimit->DeleteGraphics();
    if(m_lpPosFlap) m_lpPosFlap->DeleteGraphics();
}

void BlPrismatic::CreateJointGraphics()
{
	OsgPrismaticLimit *lpUpperLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpUpperLimit);
	OsgPrismaticLimit *lpLowerLimit = dynamic_cast<OsgPrismaticLimit *>(m_lpLowerLimit);
	OsgPrismaticLimit *lpPosFlap = dynamic_cast<OsgPrismaticLimit *>(m_lpPosFlap);

	m_lpPosFlap->LimitPos(Prismatic::JointPosition());
	OsgPrismatic::CreatePrismaticGraphics(BoxSize(), CylinderRadius(), 
                                          m_osgJointMT, lpUpperLimit, 
                                          lpLowerLimit, lpPosFlap);
}

void BlPrismatic::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	m_osgParent = ParentOSG();

	if(m_osgParent.valid())
	{
		//Add the parts to the group node.
		CStdFPoint vPos(0, 0, 0), vRot(VX_PI/2, 0, 0); 
		vPos.Set(0, 0, 0); vRot.Set(0, VX_PI/2, 0); 
		m_osgJointMT = new osg::MatrixTransform();
		m_osgJointMT->setMatrix(SetupMatrix(vPos, vRot));

        CreateJointGraphics();

		m_osgNode = m_osgJointMT.get();

		BuildLocalMatrix();

		SetAlpha();
		SetCulling();
		SetVisible(m_lpThisMI->IsVisible());

		//Add it to the scene graph.
		m_osgParent->addChild(m_osgRoot.get());

		//Set the position with the world coordinates.
		Physics_UpdateAbsolutePosition();

		//We need to set the UserData on the OSG side so we can do picking.
		//We need to use a node visitor to set the user data for all drawable nodes in all geodes for the group.
		osg::ref_ptr<OsgUserDataVisitor> osgVisitor = new OsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgMT);
	}
}

void BlPrismatic::DeletePhysics()
{
	if(!m_vxPrismatic)
		return;

	if(GetBlSimulator() && GetBlSimulator()->Universe())
	{
		GetBlSimulator()->Universe()->removeConstraint(m_vxPrismatic);
		delete m_vxPrismatic;

		if(m_lpChild && m_lpParent)
			m_lpChild->EnableCollision(m_lpParent);
	}

	m_vxPrismatic = NULL;
	m_vxJoint = NULL;
}

void BlPrismatic::SetupPhysics()
{
	if(m_vxPrismatic)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	BlRigidBody *lpVsChild = dynamic_cast<BlRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	CStdFPoint vGlobal = this->GetOSGWorldCoords();
	
	Vx::VxReal44 vMT;
	VxOSG::copyOsgMatrix_to_VxReal44(this->GetOSGWorldMatrix(true), vMT);
	Vx::VxTransform vTrans(vMT);
	Vx::VxReal3 vxRot;
	vTrans.getRotationEulerAngles(vxRot);

	CStdFPoint vLocalRot(vxRot[0], vxRot[1], vxRot[2]);

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
	osg::Vec3d vNormAxis = NormalizeAxis(vLocalRot);
	VxVector3 axis((double) vNormAxis[0], (double) vNormAxis[1], (double) vNormAxis[2]);

	m_vxPrismatic = new VxPrismatic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 
	m_vxPrismatic->setName(m_strID.c_str());

	GetBlSimulator()->Universe()->addConstraint(m_vxPrismatic);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	BlPrismaticLimit *lpUpperLimit = dynamic_cast<BlPrismaticLimit *>(m_lpUpperLimit);
	BlPrismaticLimit *lpLowerLimit = dynamic_cast<BlPrismaticLimit *>(m_lpLowerLimit);

	lpUpperLimit->PrismaticRef(m_vxPrismatic);
	lpLowerLimit->PrismaticRef(m_vxPrismatic);

	//Re-enable the limits once we have initialized the joint
	EnableLimits(m_bEnableLimits);

	m_vxJoint = m_vxPrismatic;
	m_iCoordID = m_vxPrismatic->kLinearCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	EnableMotor(m_bEnableMotorInit);

    Prismatic::Initialize();
    BlJoint::Initialize();
}

void BlPrismatic::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods

float *BlPrismatic::GetDataPointer(const string &strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "JOINTROTATION")
		return &m_fltPosition;
	else if(strType == "JOINTPOSITION")
		return &m_fltPosition;
	else if(strType == "JOINTACTUALVELOCITY")
		return &m_fltVelocity;
	else if(strType == "JOINTFORCE")
		return &m_fltForce;
	else if(strType == "JOINTDESIREDVELOCITY")
		return &m_fltReportSetVelocity;
	else if(strType == "JOINTSETVELOCITY")
		return &m_fltReportSetVelocity;
	else if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "CONTACTCOUNT")
		THROW_PARAM_ERROR(Al_Err_lMustBeContactBodyToGetCount, Al_Err_strMustBeContactBodyToGetCount, "JointID", m_strName);
	else
	{
		lpData = Prismatic::GetDataPointer(strDataType);
		if(lpData) return lpData;

		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);
	}

	return lpData;
}

bool BlPrismatic::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Prismatic::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlPrismatic::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	Prismatic::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

void BlPrismatic::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
