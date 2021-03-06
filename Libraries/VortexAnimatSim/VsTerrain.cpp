// VsTerrain.cpp: implementation of the VsTerrain class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsMeshBase.h"
#include "VsTerrain.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsTerrain::VsTerrain()
{
	SetThisPointers();
	m_bCullBackfaces = true; //we want back face culling on by default for Terrains.
	m_osgHeightField = NULL;
	m_vxHeightField = NULL;
}

VsTerrain::~VsTerrain()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsTerrain/\r\n", "", -1, false, true);}
}

void VsTerrain::CreateGraphicsGeometry()
{
	m_osgGeometry = CreatePlaneGeometry(-1, -1, 2, 2, 2, 2, false);
}

void VsTerrain::SetTexture(std::string strTexture)
{
	if(m_osgMeshNode.valid())
	{
		if(!Std_IsBlank(strTexture))
		{
			std::string strFile = AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), strTexture);
			osg::ref_ptr<osg::Image> image = osgDB::readImageFile(strFile);
			if(!image)
				THROW_PARAM_ERROR(Vs_Err_lTextureLoad, Vs_Err_strTextureLoad, "Image File", strFile);

			osg::StateSet* state = m_osgMeshNode->getOrCreateStateSet();
			m_osgTexture = new osg::Texture2D(image.get());
		    m_osgTexture->setDataVariance(osg::Object::DYNAMIC); // protect from being optimized away as static state.

			m_osgTexture->setFilter(osg::Texture2D::MIN_FILTER,osg::Texture2D::LINEAR_MIPMAP_LINEAR);
			m_osgTexture->setFilter(osg::Texture2D::MAG_FILTER,osg::Texture2D::LINEAR);
			m_osgTexture->setWrap(osg::Texture2D::WRAP_S, osg::Texture2D::REPEAT);
			m_osgTexture->setWrap(osg::Texture2D::WRAP_T, osg::Texture2D::REPEAT);

			osg::Matrixd matrix;
			matrix.makeScale(osg::Vec3(m_iTextureLengthSegments, m_iTextureWidthSegments, 1.0)); 

			osg::ref_ptr<osg::TexMat> matTexture = new osg::TexMat;
			matTexture->setMatrix(matrix); 

			state->setTextureAttributeAndModes(0, m_osgTexture.get());
			state->setTextureAttributeAndModes(0, matTexture.get(), osg::StateAttribute::ON); 

			state->setTextureMode(0, m_eTextureMode, osg::StateAttribute::ON);
			state->setMode(GL_BLEND,osg::StateAttribute::ON);
			
			//state->setRenderingHint(osg::StateSet::TRANSPARENT_BIN);
		}
		else if(m_osgTexture.valid()) //If we have already set it and we are clearing it then reset the state
		{
			m_osgTexture.release();
			osg::StateSet* state = m_osgMeshNode->getOrCreateStateSet();
			state->setTextureAttributeAndModes(0, NULL); 
			state->setTextureMode(0, m_eTextureMode, osg::StateAttribute::OFF);
		}
	}
}

//Terrains can never have fluid interactions/dynamics.
void VsTerrain::Physics_FluidDataChanged()
{}

void VsTerrain::LoadMeshNode()
{
	std::string strPath = m_lpThisAB->GetSimulator()->ProjectPath();
	std::string strMeshFile = m_lpThisMesh->MeshFile();
	std::string strFile = AnimatSim::GetFilePath(strPath, strMeshFile);

	//Get the terrain node loaded in.
	m_osgBaseMeshNode = CreateHeightField(strFile, m_fltSegmentWidth, m_fltSegmentLength, m_fltMaxHeight, &m_osgHeightField);
	SetTexture(m_lpThisRB->Texture());

	osg::Matrix osgScaleMatrix = osg::Matrix::identity();
	m_osgMeshNode = new osg::MatrixTransform(osgScaleMatrix);

	m_osgMeshNode->addChild(m_osgBaseMeshNode.get());
	m_osgMeshNode->setName(m_lpThisAB->Name() + "_MeshNode");
}

void VsTerrain::CreatePhysicsGeometry()
{
	m_vxGeometry = NULL;
	if(m_osgHeightField)
	{
		m_vxHeightField = CreateVxHeightField(m_osgHeightField, m_fltSegmentWidth, m_fltSegmentLength, 0, 0, 0);
		m_vxGeometry = m_vxHeightField;
	}

	m_eControlType = VxEntity::kControlNode;  //This is not a dynamic part.

	if(!m_vxGeometry)
		THROW_TEXT_ERROR(Vs_Err_lCreatingGeometry, Vs_Err_strCreatingGeometry, "Body: " + m_lpThisAB->Name() + " Mesh: " + AnimatSim::GetFilePath(m_lpThisAB->GetSimulator()->ProjectPath(), m_lpThisMesh->MeshFile()));
}

void VsTerrain::CreateParts()
{
	CreateGeometry();

	VsMeshBase::CreateItem();
	Terrain::CreateParts();
	VsMeshBase::SetBody();
}

void VsTerrain::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Terrain::CreateJoints();
	VsMeshBase::Initialize();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
