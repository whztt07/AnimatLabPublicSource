/**
\file	VsStructure.cpp

\brief	Implements the vortex structure class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
VsStructure::VsStructure()
{
	m_lpVsBody = NULL;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
VsStructure::~VsStructure()
{
	m_lpAssembly = NULL;
}

void VsStructure::Body(RigidBody *lpBody)
{
	Structure::Body(lpBody);
	m_lpVsBody = dynamic_cast<VsRigidBody *>(lpBody);
	if(!m_lpVsBody)
		THROW_TEXT_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody, "ID: " + lpBody->Name());
}

void VsStructure::SetThisPointers()
{
	VsMovableItem::SetThisPointers();

	m_lpThisST = dynamic_cast<Structure *>(this);
	if(!m_lpThisST)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisST, " + m_lpThisAB->Name());
}

osg::Group *VsStructure::ParentOSG()
{
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	return lpVsSim->OSGRoot();
}

void VsStructure::Create()
{
	m_osgGeometry = CreateSphereGeometry(15, 15,  m_lpThisST->Size());
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;

	CreateItem();

	Structure::Create();
}

void VsStructure::SetupPhysics()
{
}

void VsStructure::ResetSimulation()
{
	VsMovableItem::Physics_ResetSimulation();

	Structure::ResetSimulation();
}

void VsStructure::UpdatePositionAndRotationFromMatrix()
{
	VsMovableItem::UpdatePositionAndRotationFromMatrix();

	m_lpVsBody->EndGripDrag();

	//m_lpThisST->UpdatePhysicsPosFromGraphics();
}

	}			// Environment
}				//VortexAnimatSim
