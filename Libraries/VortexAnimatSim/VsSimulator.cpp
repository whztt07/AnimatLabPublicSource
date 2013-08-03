// VsSimulator.cpp: implementation of the VsSimulator class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "VsOsgGeometry.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsClassFactory.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


VsSimulator::VsSimulator()
{
	m_uUniverse = NULL;
	m_grpScene = NULL;	
	m_vsWinMgr = NULL;
	m_vsWinMgr = new OsgSimulationWindowMgr;
	m_lpWinMgr = m_vsWinMgr;
	m_lpWinMgr->SetSystemPointers(this, NULL, NULL, NULL, true);
	m_uUniverse = NULL;
	m_vxFrame = NULL;
	m_dblTotalStepTime = 0;
	m_lStepTimeCount = 0;
	m_dblTotalStepTime= 0;
	m_lStepTimeCount = 0;
	m_dblTotalVortexStepTime = 0;
	m_lStepVortexTimeCount = 0;
	m_lpMeshMgr = NULL;
	m_osgAlphafunc = NULL;

    //Setup the global matrix util.
    m_lpMatrixUtil = new VsMatrixUtil;
    SetMatrixUtil(m_lpMatrixUtil);
}

VsSimulator::~VsSimulator()
{

try
{
	if(m_lpMeshMgr)
	{
		delete m_lpMeshMgr;
		m_lpMeshMgr = NULL;
	}

	m_bShuttingDown = true;

	//Set this to NULL so all of the DeletePhysics calls will not try and remove
	//entities from the universe when shutting down.
	m_uUniverse = NULL;

	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Simulator\r\n", "", -1, false, true);}
}

VxUniverse *VsSimulator::Universe()
{return m_uUniverse;}

Vx::VxFrame* VsSimulator::Frame()
{return m_vxFrame;}

#pragma region MutatorOverrides

void VsSimulator::StabilityScale(float fltVal)
{
	OsgSimulator::StabilityScale(fltVal);
	SetSimulationStabilityParams();
}

void VsSimulator::LinearCompliance(float fltVal, bool bUseScaling)
{
	OsgSimulator::LinearCompliance(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::AngularCompliance(float fltVal, bool bUseScaling)
{
	OsgSimulator::AngularCompliance(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::LinearDamping(float fltVal, bool bUseScaling)
{
	OsgSimulator::LinearDamping(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::AngularDamping(float fltVal, bool bUseScaling)
{
	OsgSimulator::AngularDamping(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::LinearKineticLoss(float fltVal, bool bUseScaling)
{
	OsgSimulator::LinearKineticLoss(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::AngularKineticLoss(float fltVal, bool bUseScaling)
{
	OsgSimulator::AngularKineticLoss(fltVal, bUseScaling);
	SetSimulationStabilityParams();
}

void VsSimulator::PhysicsTimeStep(float fltVal)
{
	OsgSimulator::PhysicsTimeStep(fltVal);

	if(m_vxFrame)
	{
		m_vxFrame->setTimeStep(m_fltPhysicsTimeStep);		
		SetSimulationStabilityParams();
	}
}

void VsSimulator::Gravity(float fltVal, bool bUseScaling)
{
	OsgSimulator::Gravity(fltVal, bUseScaling);

	if(m_uUniverse)
		m_uUniverse->setGravity(0, m_fltGravity, 0);
}

int VsSimulator::GetMaterialID(string strID)
{
	if(m_vxFrame)
	{
		VxMaterialTable *lpMat = m_vxFrame->getMaterialTable();
		
		if(lpMat)
			return lpMat->getMaterialID(strID.c_str());
		return -1;
	}

	return -1;
}

#pragma endregion


SimulationRecorder *VsSimulator::CreateSimulationRecorder()
{
//NEED_TO_FIX
	return NULL; //new VsSimulationRecorder;
}

void VsSimulator::Reset()
{
	OsgSimulator::Reset();

	if(m_vxFrame)
		m_vxFrame->release(); 

	if(!m_lpAnimatClassFactory) 
		m_lpAnimatClassFactory = new VsClassFactory;
}

void VsSimulator::ResetSimulation()
{
	OsgSimulator::ResetSimulation();

	if(m_uUniverse)
	{
		m_uUniverse->resetDynamics();
		m_uUniverse->resetContacts();
	}
	m_bSimRunning = false;
}

void VsSimulator::InitializeVortexViewer(int argc, const char **argv)
{
    osg::ArgumentParser arguments(&argc, (char **) argv);

	osg::setNotifyLevel(osg::NotifySeverity::NOTICE);  //ConvertTraceLevelToOSG());

	//osg::notify(osg::NOTICE) << "Setting OSG notice level to '" << Std_GetTraceLevel() << std::endl

    // set up the usage document, in case we need to print out how to use this program.
    arguments.getApplicationUsage()->setApplicationName(arguments.getApplicationName());
    arguments.getApplicationUsage()->setDescription(arguments.getApplicationName()+" is the standard OpenSceneGraph example which loads and visualises 3d models.");
    arguments.getApplicationUsage()->setCommandLineUsage(arguments.getApplicationName()+" [options] filename ...");
    arguments.getApplicationUsage()->addCommandLineOption("-h or --help", "Display this information");

	//Ensure that our exe path is the only place it will attempt to find library files.
	osgDB::FilePathList aryList = osgDB::getLibraryFilePathList();
	string strPath = m_strExecutablePath.substr(0, m_strExecutablePath.length()-1);
	aryList.clear();
	aryList.push_front(strPath);
	osgDB::setLibraryFilePathList(aryList);

    // add resource search paths
    osgDB::getDataFilePathList().push_front("./Resources");
    osgDB::getDataFilePathList().push_front("../Resources");
    osgDB::getDataFilePathList().push_front("../../Resources");

	string strFile = osgDB::findLibraryFile("osgdb_freetype.dll");

	//This is the root of the scenegraph.  Which will corrospond
	//to the root of the simulation
	m_grpScene = new osg::Group;	
    m_grpScene->setName("World");

	//Add the mouse spring lines to the scene
	m_grpScene->addChild(m_lpMouseSpring->GetNode());

	//Create the windows, cameras, and Huds
	m_vsWinMgr->Initialize();

	//Create the command manager if needed.
	if(!m_osgCmdMgr.valid())
	    m_osgCmdMgr = new osgManipulator::CommandManager;

    osg::StateSet* rootStateSet = m_grpScene->getOrCreateStateSet();
	rootStateSet->setMode( GL_LIGHTING, osg::StateAttribute::ON );
	rootStateSet->setMode( GL_LIGHT0, osg::StateAttribute::ON );
	//rootStateSet->setMode( GL_LIGHT1, osg::StateAttribute::ON );

    // set up an alphafunc by default to speed up blending operations.
    m_osgAlphafunc = new osg::AlphaFunc;
    m_osgAlphafunc->setFunction(osg::AlphaFunc::GEQUAL, m_fltAlphaThreshold);
    rootStateSet->setAttributeAndModes(m_osgAlphafunc, osg::StateAttribute::ON);

	m_oLightMgr.Initialize();
}

void VsSimulator::SetSimulationStabilityParams()
{
	if(m_uUniverse)
	{
	/*	if(m_bCalcCriticalSimParams)
		{*/
			//VxReal halflife = 5;
			//m_uUniverse->getSolverParameters(0)->setConstraintLinearCompliance(m_fltLinearCompliance);
			//m_uUniverse->getSolverParameters(0)->setConstraintAngularCompliance(m_fltAngularCompliance);
			//m_uUniverse->setCriticalConstraintParameters(0, halflife);
		/*}
		else
		{
			m_uUniverse->getSolverParameters(0)->setConstraintLinearCompliance(m_fltLinearCompliance);
			m_uUniverse->getSolverParameters(0)->setConstraintLinearDamping(m_fltLinearDamping);
			m_uUniverse->getSolverParameters(0)->setConstraintLinearKineticLoss(m_fltLinearKineticLoss);
			m_uUniverse->getSolverParameters(0)->setConstraintAngularCompliance(m_fltAngularCompliance);
			m_uUniverse->getSolverParameters(0)->setConstraintAngularDamping(m_fltAngularDamping);
			m_uUniverse->getSolverParameters(0)->setConstraintAngularKineticLoss(m_fltAngularKineticLoss);
		}*/
		 
		//VxSolverParameters* sp = m_uUniverse->getSolverParameters(0);
		//sp->setConstraintLinearKineticLoss(0.0006);
		//sp->setConstraintLinearKineticLoss(0.0006);
		//sp->setConstraintLinearCompliance(1e-5);
		//sp->setConstraintAngularCompliance(1e-5);
		//sp->setConstraintLinearDamping(1e4);
		//sp->setConstraintAngularDamping(1e4);

		VxSolverParameters* sp = m_uUniverse->getSolverParameters(0);
		sp->setConstraintLinearKineticLoss(sp->getConstraintLinearKineticLoss()*m_fltStabilityScale);
		sp->setConstraintLinearKineticLoss(sp->getConstraintAngularKineticLoss()*m_fltStabilityScale);
		sp->setConstraintLinearCompliance(sp->getConstraintLinearCompliance()/m_fltStabilityScale);
		sp->setConstraintAngularCompliance(sp->getConstraintAngularCompliance()/m_fltStabilityScale);
		sp->setConstraintLinearDamping(sp->getConstraintLinearDamping()*m_fltStabilityScale);
		sp->setConstraintAngularDamping(sp->getConstraintAngularDamping()*m_fltStabilityScale);

		TRACE_DETAIL("Reset simulation stability params\r\n");
		TRACE_DETAIL("Angular Compliance: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintAngularCompliance()) + "\r\n");
		TRACE_DETAIL("Angular Damping: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintAngularDamping()) + "\r\n");
		TRACE_DETAIL("Angular Kinetic Loss: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintAngularKineticLoss()) + "\r\n");
		TRACE_DETAIL("Linear Compliance: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintLinearCompliance()) + "\r\n");
		TRACE_DETAIL("Linear Damping: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintLinearDamping()) + "\r\n");
		TRACE_DETAIL("Linear Kinetic Loss: " + STR(m_uUniverse->getSolverParameters(0)->getConstraintLinearKineticLoss()) + "\r\n");
	}
}

//This function initializes the Vortex related
//classes and the vortex viewer.
void VsSimulator::InitializeVortex(int argc, const char **argv)
{
	InitializeVortexViewer(argc, argv);

	int iObjectCount = 100 + m_iPhysicsBodyCount;
	int iCollisionCount = iObjectCount*40;

	//create the frame
	m_vxFrame = VxFrame::instance();
	m_uUniverse = new Vx::VxUniverse(iObjectCount, iCollisionCount);
	m_uUniverse->setGravity(0, m_fltGravity, 0);
	//m_uUniverse->setGravity(0, 0, m_fltGravity);
	m_vxFrame->addUniverse(m_uUniverse);

	//set the frame timestep
	m_vxFrame->setTimeStep(m_fltPhysicsTimeStep);		

	VxFrameRegisterAllInteractions(m_vxFrame);

	SetSimulationStabilityParams();

    // Register the simple callback to be notified at beginning and end of the interaction between parts and sensors.
    m_uUniverse->addIntersectSubscriber(VxUniverse::kResponseSensor, VxUniverse::kResponsePart, VxUniverse::kEventFirst, &m_vsIntersect, 0);
    m_uUniverse->addIntersectSubscriber(VxUniverse::kResponseSensor, VxUniverse::kResponsePart, VxUniverse::kEventDisjoint, &m_vsIntersect, 0);

}

Vx::VxTriangleMesh *VsSimulator::CreatTriangleMeshFromOsg(osg::Node *osgNode)
{
	Vx::VxTriangleMesh *vxMesh = NULL;
	vxMesh = VxTriangleMesh::createFromNode(osgNode);

	return vxMesh;
}

Vx::VxConvexMesh *VsSimulator::CreateConvexMeshFromOsg(osg::Node *osgNode)
{
	Vx::VxConvexMesh *vxMesh = NULL;
	vxMesh = VxConvexMesh::createFromNode(osgNode); 

	return vxMesh;
}

/**
\brief	Generates a collision mesh file.

\details When we want to use a collision mesh then we need to create a new .osg file using the 
mesh created by the physics engine instead of the graphics one. The UI will call this method
when the user sets a new mesh file or type so we can create the new file. It will load the graphics
file, convert it usign the physics engine, and then save it back out. When loading the convex mesh
we will use this new file instead of the original one.

\author	dcofer
\date	6/10/2011

\param	strOriginalMeshFile 	The original mesh file. 
\param	strCollisionMeshFile	The new collision mesh file. 
**/
void VsSimulator::GenerateCollisionMeshFile(string strOriginalMeshFile, string strCollisionMeshFile, float fltScaleX, float fltScaleY, float fltScaleZ)
{
	//First load the original mesh in.
	string strPath = this->ProjectPath();
	string strOrigFile = AnimatSim::GetFilePath(strPath, strOriginalMeshFile);
	string strNewFile = AnimatSim::GetFilePath(strPath, strCollisionMeshFile);

	osg::ref_ptr<osg::Node> osgNode = MeshMgr()->LoadMesh(strOrigFile); //osgDB::readNodeFile(strOrigFile.c_str());

	//Make sure the mesh loaded is valid.
	if(!osgNode.valid())
		THROW_PARAM_ERROR(Vs_Err_lErrorLoadingMesh, Vs_Err_strErrorLoadingMesh, "Original Mesh file", strOriginalMeshFile);

	//Now create a convex mesh with the physics engine using the loaded mesh.
	Vx::VxConvexMesh *vxMesh = VxConvexMesh::createFromNode(osgNode.get()); 

	//Now use that convexmesh geometry to create a new osg node.
	osg::ref_ptr<osg::Geometry> osgGeom = CreateOsgFromVxConvexMesh(vxMesh);
	osg::ref_ptr<osg::Geode> osgNewNode = new osg::Geode;
	osgNewNode->addDrawable(osgGeom.get());

	osg::Matrix osgScale = osg::Matrix::scale(fltScaleX, fltScaleY, fltScaleZ);
	osg::ref_ptr<osg::MatrixTransform> osgScaleMT = new osg::MatrixTransform(osgScale);
	osgScaleMT->addChild(osgNewNode.get());
	osgScaleMT->setDataVariance(osg::Object::STATIC);

	// Now do some OSG voodoo, which should spread the transform downward
	//  through the loaded model, and delete the transform.
	osgUtil::Optimizer optimizer;
	optimizer.optimize(osgScaleMT.get(), osgUtil::Optimizer::FLATTEN_STATIC_TRANSFORMS);

	//Now save out the new collision mesh.
	osgDB::writeNodeFile(*osgNewNode, strNewFile.c_str());

	//Make sure we stamp the new file tim on the file. For some reason osgDB is not settign that correctly.
	//If we do not do this then the mesh mgr will not recognize that it has changed, and will not load it.
	///NEED TO FIX
#ifdef WIN32
	Std_SetFileTime(strNewFile);
#endif
}

void VsSimulator::ConvertV1MeshFile(string strOriginalMeshFile, string strNewMeshFile, string strTexture)
{
	//First load the original mesh in.
	string strPath = this->ProjectPath();
	string strOrigFile = AnimatSim::GetFilePath(strPath, strOriginalMeshFile);
	string strNewFile = AnimatSim::GetFilePath(strPath, strNewMeshFile);
	string strTextFile = "";
	
	if(!Std_IsBlank(strTexture))
		strTextFile = AnimatSim::GetFilePath(strPath, strTexture);

	osg::ref_ptr<osg::Node> osgNode = MeshMgr()->LoadMesh(strOrigFile); //osgDB::readNodeFile(strOrigFile.c_str());

	//Make sure the mesh loaded is valid.
	if(!osgNode.valid())
		THROW_PARAM_ERROR(Vs_Err_lErrorLoadingMesh, Vs_Err_strErrorLoadingMesh, "Original Mesh file", strOriginalMeshFile);

	CStdFPoint vPos(0, 0, 0), vRot( -(osg::PI/2), 0, 0);
	//ApplyVertexTransform(osgNode.get(), SetupMatrix(vPos, vRot));   //NEED TO FIX

	////Now add a matrix tranform to rotate about the x axis by -90 degrees.
	//osg::ref_ptr<osg::MatrixTransform> m_osgRotateMT = new osg::MatrixTransform;
	////CStdFPoint vPos(0, 0, 0), vRot( -(osg::PI/2), 0, 0);
	//CStdFPoint vPos(0, 0, 0), vRot( 0, 0, 0);
	//m_osgRotateMT->setMatrix(SetupMatrix(vPos, vRot));

	//m_osgRotateMT->addChild(osgNode.get());

	AddNodeTexture(osgNode.get(), strTextFile, GL_TEXTURE_2D);

	//Now save out the new collision mesh.
	osgDB::writeNodeFile(*osgNode, strNewFile.c_str());
}


void VsSimulator::GetPositionAndRotationFromD3DMatrix(float (&aryTransform)[4][4], CStdFPoint &vPos, CStdFPoint &vRot)
{
	osg::Matrix osgMT(aryTransform[0][0], aryTransform[0][1], aryTransform[0][2], aryTransform[0][3], 
					  aryTransform[1][0], aryTransform[1][1], aryTransform[1][2], aryTransform[1][3], 
					  aryTransform[2][0], aryTransform[2][1], aryTransform[2][2], aryTransform[2][3], 
					  aryTransform[3][0], aryTransform[3][1], aryTransform[3][2], aryTransform[3][3]);

	osg::Matrix osgFinal;
	
	osgFinal = osgMT;

	//Lets get the current world coordinates for this body part and then recalculate the 
	//new local position for the part and then finally reset its new local position.
	osg::Vec3 vL = osgFinal.getTrans();
	vPos.Set(vL.x(), vL.y(), vL.z());
	vPos.ClearNearZero();
		
	//Now lets get the euler angle rotation
	Vx::VxReal44 vxTM;
	VxOSG::copyOsgMatrix_to_VxReal44(osgFinal, vxTM);
	Vx::VxTransform vTrans(vxTM);
	Vx::VxReal3 vEuler;
	vTrans.getRotationEulerAngles(vEuler);
	vRot.Set(vEuler[0], vEuler[1] ,vEuler[2]);
	vRot.ClearNearZero();
}

void VsSimulator::Initialize(int argc, const char **argv)
{
	InitializeVortex(argc, argv);

    OsgSimulator::Initialize(argc, argv);
}

void VsSimulator::StepSimulation()
{
	//if(m_lTimeSlice > 10 && m_lTimeSlice < 5000 && !m_timePeriod.TimerStarted())
	//	m_timePeriod.StartTimer();


	try
	{ 
		//step the frame and update the windows
		if (!m_bPaused)	
		{
			OsgSimulator::StepSimulation();

			unsigned long long lStart = GetTimerTick();
			m_vxFrame->step();
			double dblVal = TimerDiff_s(lStart, GetTimerTick());
			m_fltPhysicsStepTime += dblVal;

			if(m_lTimeSlice > 10 && m_lTimeSlice < 5000)
			{
				m_dblTotalVortexStepTime += dblVal;
				m_lStepVortexTimeCount++;
			}
			else if(m_lTimeSlice == 5000)
			{
				double dblAvgStepTime = m_dblTotalVortexStepTime/m_lStepVortexTimeCount;
				osg::notify(osg::NOTICE) << "Average step time: " << dblAvgStepTime << std::endl;
				osg::notify(osg::NOTICE) << "Total vortex step time: " << m_dblTotalVortexStepTime << std::endl;
				osg::notify(osg::NOTICE) << "Slice Time: " << m_lTimeSlice << std::endl;
				osg::notify(osg::NOTICE) << "Sim Time: " << Time() << std::endl;
			}
		}

	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while step the simulation.\nError: " + oError.m_strError;
		HandleNonCriticalError(strError);
	}


	//double dblVal2 = m_timeSimulationStep.StopTimer();
	//if(m_lTimeSlice > 10 && m_lTimeSlice < 5000)
	//{
	//	m_dblTotalStepTime += dblVal2;
	//	m_lStepTimeCount++;
	//}
	//else if(m_lTimeSlice == 5000)
	//{
	//	double dblAvgStepTime = m_dblTotalStepTime/m_lStepTimeCount;
	//	cout << "Average step time: " << dblAvgStepTime << std::endl;
	//	cout << "Total step time: " << m_dblTotalStepTime << ", " << m_lStepTimeCount << std::endl;
	//	cout << "Period time: " << m_timePeriod.StopTimer() << std::endl;
	//	cout << "Slice Time: " << m_lTimeSlice << std::endl;
	//	cout << "Sim Time: " << Time() << std::endl;
	//}

}


void VsSimulator::SimulateEnd()
{
	m_vxFrame->release();
	m_vxFrame = NULL;
}


/*
__declspec( dllexport ) void RunSimulator(void)
{
	VsSimulator oSim;
	oSim.ProjectPath("C:\\Projects\\bin\\Experiments\\Robot\\");

	oSim.Load("Robot.asim");
	oSim.Initialize(0, NULL);
	oSim.Simulate();
}
*/

}			//VortexAnimatSim
