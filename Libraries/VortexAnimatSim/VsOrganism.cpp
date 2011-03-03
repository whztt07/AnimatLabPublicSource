// VsMuscle.cpp: implementation of the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsOrganism.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsDragger.h"


namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsOrganism::VsOrganism()
{
	m_lpAssembly = NULL;
}

VsOrganism::~VsOrganism()
{

}

void VsOrganism::Initialize()
{
	//m_lpAssembly = new VxAssembly();	

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	///////////NEED TO FIX !!!!!!!!!!!!!!!!!
	///// For some reason all of the Z coordinates are backwards in the original animatlab.
	///// The only thing I can think of is that this has something to do with directX.
	///// This means that all z coordinates must be negated. However, in the new version we do 
	///// not want to have this negative z for all objects, so it cannot remain hardcoded here like this.
	///// Instead, we need a file converter system that will negate the z position variable of
	///// each object when converting a 1.0 file up to a 2.0 version. Then we could remove this hardcoded rotation.
	m_oPosition.z = -m_oPosition.z; 
	///////////NEED TO FIX !!!!!!!!!!!!!!!!!

	m_osgMT = new osg::MatrixTransform;
	osg::Matrix m;	
	m.makeTranslate(m_oPosition.x, m_oPosition.y, m_oPosition.z);
	m_osgMT->setMatrix(m);

	lpVsSim->OSGRoot()->addChild(m_osgMT.get());

	Organism::Initialize();	

	//lpVsSim->Universe()->addAssembly(m_lpAssembly);


	//VxTransform tm;
 //   tm.makeRotationFromEulerAngles(0, 0, 0);
 //   tm.t().set(m_oPosition.x, m_oPosition.y, fltPosZ);
	//m_lpAssembly->transform(tm);

	m_lpSim->DisableCollisions(this, m_aryExcludeCollisionList);
}

void VsOrganism::ResetSimulation()
{
	osg::Matrix m;	
	m.makeTranslate(m_oPosition.x, m_oPosition.y, m_oPosition.z);
	m_osgMT->setMatrix(m);

	Organism::ResetSimulation();
}

	}			// Environment
}				//VortexAnimatSim
