/**
\file	Simulator.cpp

\brief	Implements the simulator class.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include <time.h>
#include "Gain.h"
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

namespace AnimatSim
{


Simulator *g_lpSimulator = NULL;
Simulator ANIMAT_PORT *GetSimulator() 
{return g_lpSimulator;};

/**
\brief	Default constructor.

\author	dcofer
\date	3/28/2011
**/
Simulator::Simulator()
{
	m_bSteppingSim = false;
	m_bShuttingDown = false;
	m_strID = "SIMULATOR";
	m_strName = m_strID;
	m_fltTime = 0;
	m_fltTimeStep = -1;
	m_iPhysicsStepInterval = 4;
	m_fltPhysicsTimeStep = (float) 0.01;
	m_lTimeSlice = 0;
	m_fltEndSimTime = -1;
	m_lEndSimTimeSlice = -1;
	m_lStartSimTick = 0;
	m_bStopSimulation = false;
	m_bForceSimulationStop = false;
    m_bBlockSimulation = false;
	m_bSimBlockConfirm = false;
	m_lPhysicsSliceCount = 0;
	m_lVideoSliceCount = 0;
	m_iPhysicsStepCount = 0;
	m_iVideoLoops = 0;
	m_lpAnimatClassFactory = NULL;
	m_bSimulateHydrodynamics = false;

	m_fltGravity = (float) -9.81;
	m_fltDistanceUnits = (float) 0.01;  //use centimeters
	m_fltInverseDistanceUnits = 1/m_fltDistanceUnits;
	m_fltDenominatorDistanceUnits = 1;
	m_fltMassUnits = (float) 0.001;    //use grams
	m_fltInverseMassUnits = 1/m_fltMassUnits;
	m_fltDisplayMassUnits = 0.01f;
	m_fltMouseSpringStiffness = 25;
	m_ftlMouseSpringDamping = 2.8f;
	m_fltStabilityScale = 1.0;
	m_fltLinearCompliance = 0.1e-9f;
	m_fltAngularCompliance = 0.1e-9f;
	m_fltLinearDamping = 50e9f;
	m_fltAngularDamping = 5e12f;
	m_fltLinearKineticLoss = 0.1e-9f;
	m_fltAngularKineticLoss = 1e-12f;

	m_bForceFastMoving = true;
	m_iSelectionMode = COLLISION_SELECTION_MODE;
	m_bAddBodiesMode = false;
	m_fltRecFieldSelRadius = 0.05f;
	m_iPhysicsBodyCount = 0;

	m_bPaused = true;
	m_bInitialized = false;
	m_bSimRunning = false;
	m_fltAlphaThreshold = 0.2f;

	m_lpSelOrganism = NULL;
	m_lpSelStructure = NULL;
	m_bManualStepSimulation = false;
	m_lpVideoRecorder = NULL;
	m_lpVideoPlayback = NULL;
	m_lpSimRecorder = NULL;
	m_lpSimStopPoint = NULL;
	m_bEnableSimRecording = false;
	m_lSnapshotByteSize = 0;

	m_bAutoGenerateRandomSeed = true;
	m_iManualRandomSeed = 12345;

	m_iPlaybackControlMode = PLAYBACK_MODE_MATCH_PHYSICS_STEP;
	m_fltPresetPlaybackTimeStep = 0;
	m_lStepStartTick = 0;
	m_lStepSimEndTick = 0;
	m_fltSimulationRealTimeToStep = 0;
	m_fltTotalRealTimeForStep = 0;
	m_fltPlaybackAdditionRealTimeToStep = 0;
	m_fltPrevTotalRealTimeForStep = 0;
	m_fltTotalRealTimeForStepSmooth = 0;
	m_fltPhysicsStepTime = 0;
	m_fltPrevPhysicsStepTime = 0;
	m_fltTotalNeuralStepTime = 0;
	m_fltRealTime = 0;
	m_lLastTickTaken = 0;
	m_fltExternalStimuliStepTime = 0;
	m_fltDataChartStepTime = 0;
	m_fltSimRecorderStepTime = 0;
	m_fltRemainingStepTime = 0;

	m_iDesiredFrameRate = 30;
 	m_fltDesiredFrameStep = (1/ (float) m_iDesiredFrameRate);
	m_lVideoFrameStartTick = 0;
	m_fltActualFrameRate = 0;

	m_bRecordVideo = false;
	m_strVideoFilename = "Video.avi";
	m_fltVideoRecordFrameTime = 1e-3f;
	m_fltVideoPlaybackFrameTime = 100e-3f;
	m_fltVideoStartTime = 0;
	m_fltVideoEndTime = 1;
	m_iVideoStepSize = -1;
	m_iVideoStepSize = 0;
	m_lVideoStartSlice = -1;
	m_lVideoEndSlice = -1;
	m_lVideoFrame = 0;

	m_vBackgroundColor.Set(0.2f, 0.2f, 0.6f, 1);

	m_lpSimCallback = NULL;
	m_lpWinMgr = NULL;
	m_oDataChartMgr.SetSystemPointers(this, NULL, NULL, NULL, true);
	m_oExternalStimuliMgr.SetSystemPointers(this, NULL, NULL, NULL, true);
	m_oMaterialMgr.SetSystemPointers(this, NULL, NULL, NULL, true);
	m_oLightMgr.SetSystemPointers(this, NULL, NULL, NULL, true);

	m_dblTotalStepTime = 0;
	m_lStepTimeCount = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/28/2011
**/
Simulator::~Simulator()
{

try
{
	m_bSteppingSim = false;
	m_bShuttingDown = true;
	g_lpSimulator = NULL;
	if(m_lpAnimatClassFactory) {delete m_lpAnimatClassFactory; m_lpAnimatClassFactory = NULL;}
	m_aryOrganisms.RemoveAll();
	m_aryStructures.RemoveAll();
	m_aryAllStructures.RemoveAll();
	if(m_lpSimRecorder)
	{
		delete m_lpSimRecorder;
		m_lpSimRecorder = NULL;
	}

	if(m_lpSimStopPoint)
	{
		delete m_lpSimStopPoint;
		m_lpSimStopPoint = NULL;
	}

	if(m_lpWinMgr)
	{
		m_lpWinMgr->Close();
		delete m_lpWinMgr;
		m_lpWinMgr = NULL;
	}

	if(m_lpSimCallback)
	{
		delete m_lpSimCallback;
		m_lpSimCallback = NULL;
	}

	m_aryNeuralModuleFactories.RemoveAll();

	m_arySourcePhysicsAdapters.RemoveAll();
	m_aryTargetPhysicsAdapters.RemoveAll();
	m_iTargetAdapterCount = 0;

	m_aryOdorTypes.RemoveAll();
	m_aryFoodSources.RemoveAll();	
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Simulator\r\n", "", -1, false, true);}
}


#pragma region AccessorMutators
			
#pragma region ProjectVariables

/**
\brief	Gets the project path.

\author	dcofer
\date	3/28/2011

\return	Project path.
**/
std::string Simulator::ProjectPath() {return m_strProjectPath;}

/**
\brief	Sets the project path.

\author	dcofer
\date	3/28/2011

\param	strPath	Full pathname of the project file. 
**/
void Simulator::ProjectPath(std::string strPath) {m_strProjectPath = strPath;}

/**
\brief	Gets the directory path to the executable.

\author	dcofer
\date	3/28/2011

\return	Path to the executable.
**/
std::string Simulator::ExecutablePath() {return m_strExecutablePath;}

/**
\brief	Sets the Executable path.

\author	dcofer
\date	3/28/2011

\param	strPath	Full pathname to the executable file. 
**/
void Simulator::ExecutablePath(std::string strPath) {m_strExecutablePath = strPath;}

/**
\brief	Gets the simulation filename.

\author	dcofer
\date	3/28/2011

\return	Simulation filename.
**/
std::string Simulator::SimulationFile() {return m_strSimulationFile;}

/**
\brief	Sets the simulation filename.

\author	dcofer
\date	3/28/2011

\param	strFile	The simulation filename. 
**/
void Simulator::SimulationFile(std::string strFile) {m_strSimulationFile = strFile;}

/**
\brief	Gets whether the Simulation is paused.

\author	dcofer
\date	3/28/2011

\return	true if it succeeds, false if it fails.
**/
bool Simulator::Paused() {return m_bPaused;}

/**
\brief	Sets the Paused flag.

\details This mutator should <b>not</b> called to pause the simulation. If you want to pause the simulation you
need to call the PauseSimulation method.

\author	dcofer
\date	3/28/2011

\param	bVal	true to value. 
**/
void Simulator::Paused(bool bVal) {m_bPaused = bVal;}

/**
\brief	Tells if the simulation has been initialized or not. 

\author	dcofer
\date	3/28/2011

\return	true if Initialized, false else.
**/
bool Simulator::Initialized() {return m_bInitialized;}

/**
\brief	Sets whether the simulation has been Initialized. 

\author	dcofer
\date	3/28/2011

\param	bVal	true if initialized. 
**/
void Simulator::Initialized(bool bVal) {m_bInitialized = bVal;}

/**
\brief	Gets the list of pointers to all objects in the simulation.

\author	dcofer
\date	3/28/2011

\return	Pointer to list of objects.
**/
CStdMap<std::string, AnimatBase *> *Simulator::ObjectList() {return &m_aryObjectList;}

/**
\brief	Gets the data chart manager.

\author	dcofer
\date	3/28/2011

\return	Pointer to DataChartMgr.
**/
DataChartMgr *Simulator::GetDataChartMgr() {return &m_oDataChartMgr;}

/**
\brief	Gets the external stimuli manager.

\author	dcofer
\date	3/28/2011

\return	Pointer to ExternalStimuliMgr.
**/
ExternalStimuliMgr *Simulator::GetExternalStimuliMgr() {return &m_oExternalStimuliMgr;}

/**
\brief	Gets the simulation recorder.

\author	dcofer
\date	3/28/2011

\return	Pointer to SimulationRecorder.
**/
SimulationRecorder *Simulator::GetSimulationRecorder() {return m_lpSimRecorder;}

/**
\brief	Gets the light manager.

\author	dcofer
\date	7/9/2011

\return	Pointer to the light manager.
**/
LightManager *Simulator::GetLightMgr() {return &m_oLightMgr;}

/**
\brief	Gets the material manager.

\author	dcofer
\date	3/28/2011

\return	Pointer to Materials.
**/
Materials *Simulator::GetMaterialMgr() {return &m_oMaterialMgr;}

/**
\brief	Gets the window manager.

\author	dcofer
\date	3/28/2011

\return	Pointer to SimulationWindowMgr.
**/
SimulationWindowMgr *Simulator::GetWindowMgr() {return m_lpWinMgr;}

/**
\brief	Gets the visual selection mode.

\author	dcofer
\date	3/28/2011

\return	Visual selection mode.
**/
int Simulator::VisualSelectionMode() {return m_iSelectionMode;}

/**
\brief	Sets the visual selection mode.

\details This loops through all ojects in the simulation and calls VisualSelectionModeChanged to let
it know when the selection mode has changed in the GUI.

\author	dcofer
\date	3/28/2011

\param	iVal	The new value. 
**/
void Simulator::VisualSelectionMode(int iVal)
{
	if(iVal <0 || iVal > SIMULATION_SELECTION_MODE)
		THROW_PARAM_ERROR(Al_Err_lInvalidSelMode, Al_Err_strInvalidSelMode, "Selection Mode", iVal);

	m_iSelectionMode = iVal;
	
	//Go through and call VisualSelectionModeChanged for all objects.
	CStdMap<std::string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase && lpBase != this)
			lpBase->VisualSelectionModeChanged(m_iSelectionMode);
	}
};

/**
\brief	Gets the Adds bodies mode.

\author	dcofer
\date	3/28/2011

\return	true if GUI is in AddBody mode, false else.
**/
bool Simulator::AddBodiesMode() {return m_bAddBodiesMode;}

/**
\brief	Sets the AddBodies mode. 

\details Within the GUI the user can select the AddBodies mode. When they click on another part it will 
add a new part at that location. The simulation needs to know if it is in that mode in order to behave in 
the correct manner. This flag lets it know the state of that mode.

\author	dcofer
\date	3/28/2011

\param	bVal	new value. 
**/
void Simulator::AddBodiesMode(bool bVal) {m_bAddBodiesMode = bVal;}

/**
\brief	Calback object from the simulation.

\details This class allows the simulation to make callbacks to the GUI.

\author	dcofer
\date	3/28/2011

\return	NULL if running standalone simulation, else a pointer to the callback object.
**/
ISimGUICallback *Simulator::SimCallback() {return m_lpSimCallback;}

/**
\brief	Sets the calback object from the simulation.

\details This class allows the simulation to make callbacks to the GUI.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpCallback	Pointer to a callback. 
**/
void Simulator::SimCallBack(ISimGUICallback *lpCallback) {m_lpSimCallback = lpCallback;}

#pragma endregion

#pragma region EnvironmentVariables

/**
\brief	Gets the current simulation time in seconds.

\author	dcofer
\date	3/28/2011

\return	Simulation time.
**/
float Simulator::Time() {return m_fltTime;}

/**
\brief	Gets the current simulation time in millisecond.

\author	dcofer
\date	3/28/2011

\return	Simulation time.
**/
long Simulator::Millisecond() {return (long) (Time() * 1000);}

/**
\brief	Calculates the number of time slices from a supplied number of milliseconds.

\author	dcofer
\date	3/28/2011

\param	lMillisecond	The millisecond to convert. 

\return	Number of time slices.
**/
long Simulator::MillisecondToSlice(long lMillisecond) {return (long) (lMillisecond / (m_fltTimeStep * 1000));}

/**
\brief	Calculates the number of milliseconds from a supplied number of time slices.

\author	dcofer
\date	3/28/2011

\param	lSlice	The time slices to convert. 

\return	Number of milliseconds.
**/
long Simulator::SliceToMillisecond(long lSlice) {return (long) (lSlice * m_fltTimeStep * 1000);}

/**
\brief	The time slice tick at which the simulation start.

\author	dcofer
\date	3/28/2011

\return	tick when the simulation begins.
**/
unsigned long long Simulator::StartSimTick() {return m_lStartSimTick;}

/**
\brief	Gets the current time slice.

\author	dcofer
\date	3/28/2011

\return	Time Slice.
**/
long Simulator::TimeSlice() {return m_lTimeSlice;}

/**
\brief	Sets the current time slice.

\author	dcofer
\date	3/28/2011

\param	lVal	The new value. 
**/
void Simulator::TimeSlice(long lVal) {m_lTimeSlice = lVal;}

/**
\brief	Gets the physics time slice count.

\author	dcofer
\date	3/28/2011

\return	Physics time slice count.
**/
long Simulator::PhysicsSliceCount() {return m_lPhysicsSliceCount;}

/**
\brief	Sets the physics time slice count.

\author	dcofer
\date	3/28/2011

\param	lVal	The new value. 
**/
void Simulator::PhysicsSliceCount(long lVal) {m_lPhysicsSliceCount = lVal;}

/**
\brief	Gets whether the simulation is being stepped manually by the user.

\author	dcofer
\date	3/28/2011

\return	true if manual stepping, false else.
**/
bool Simulator::ManualStepSimulation() {return m_bManualStepSimulation;}

/**
\brief	Sets whether the simulation is being stepped manually by the user.

\author	dcofer
\date	3/28/2011

\param	bVal	true to step manually. 
**/
void Simulator::ManualStepSimulation(bool bVal) {m_bManualStepSimulation = bVal;}

/**
\brief	Gets whether the simulation is running.

\author	dcofer
\date	3/28/2011

\return	true if it is running, false else.
**/
bool Simulator::SimRunning() {return m_bSimRunning;}

/**
\brief	Tells whether the simulation is shutting down or not.

\details This is used by other objects in their destructor to determine whether to make certain calls or not. 

\author	dcofer
\date	9/25/2011

\return	true if it shutting down, false otherwise.
**/
bool Simulator::ShuttingDown() {return m_bShuttingDown;}

/**
\brief	Gets whether we have set the simulation to force fast moving calculations.

\author	dcofer
\date	3/28/2011

\return	true if it force, false else.
**/
bool Simulator::ForceFastMoving() {return m_bForceFastMoving;}

/**
\brief	Set simulation to Force fast moving caculations.

\author	dcofer
\date	3/28/2011

\param	bVal	true to force. 
**/
void Simulator::ForceFastMoving(bool bVal) {m_bForceFastMoving = bVal;}

/**
\brief	Gets whether to automatically generate a random seed.

\author	dcofer
\date	3/28/2011

\return	true if auto generating seed, false to if using currently set seed.
**/
bool Simulator::AutoGenerateRandomSeed() {return m_bAutoGenerateRandomSeed;}

/**
\brief	Sets whether to automatically generate random seed.

\author	dcofer
\date	3/28/2011

\param	bVal	true to generate automatic seed. 
**/
void Simulator::AutoGenerateRandomSeed(bool bVal) {m_bAutoGenerateRandomSeed = bVal;}

/**
\brief	Gets the manual random seed value.

\author	dcofer
\date	3/28/2011

\return	seed value.
**/
int Simulator::ManualRandomSeed() {return m_iManualRandomSeed;}

/**
\brief	Sets the manual random seed value.

\author	dcofer
\date	3/28/2011

\param	iSeed	The seed. 
**/
void Simulator::ManualRandomSeed(int iSeed) {m_iManualRandomSeed = iSeed;}

/**
\brief	Sets the linear scaling factor that controls the simulation stability parameters

\author	dcofer
\date	1/2/2012

\return	scale factor.
**/
float Simulator::StabilityScale() {return m_fltStabilityScale;}

/**
\brief	Sets the linear scaling factor that controls the simulation stability parameters.

\author	dcofer
\date	1/2/2012

\param	fltVal	Scale factor.
**/
void Simulator::StabilityScale(float fltVal) {m_fltStabilityScale = fltVal;} 


/**
\brief	Gets the linear compliance of the simulation.

\author	dcofer
\date	3/28/2011

\return	Linear compliance of the simulation.
**/
float Simulator::LinearCompliance() {return m_fltLinearCompliance;}

/**
\brief	Sets the linear compliance of the simulation.

\author	dcofer
\date	3/28/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Simulator::LinearCompliance(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "LinearCompliance");
	
	if(bUseScaling)
		fltVal *= m_fltMassUnits;

	m_fltLinearCompliance = fltVal;
}

/**
\brief	Gets the angular compliance of the simulation.

\author	dcofer
\date	3/28/2011

\return	Angular compliance of the simulation.
**/
float Simulator::AngularCompliance() {return m_fltAngularCompliance;}

/**
\brief	Sets the angular compliance of the simulation.

\author	dcofer
\date	3/28/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Simulator::AngularCompliance(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "AngularCompliance");
	
	if(bUseScaling)
		fltVal *= m_fltMassUnits*m_fltDistanceUnits*m_fltDistanceUnits;

	m_fltAngularCompliance = fltVal;
}

/**
\brief	Gets the linear damping of the simulation.

\author	dcofer
\date	3/28/2011

\return	Linear damping of the simulation.
**/
float Simulator::LinearDamping() {return m_fltLinearDamping;}

/**
\brief	Sets the linear damping of the simulation.

\author	dcofer
\date	3/28/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Simulator::LinearDamping(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "LinearDamping");
	
	if(bUseScaling)
		fltVal = fltVal/this->DisplayMassUnits();

	m_fltLinearDamping = fltVal;
}

/**
\brief	Gets the angular damping of the simulation.

\author	dcofer
\date	3/28/2011

\return	Angular damping of the simulation.
**/
float Simulator::AngularDamping() {return m_fltAngularDamping;}

/**
\brief	Sets the angular damping of the simulation.

\author	dcofer
\date	3/28/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Simulator::AngularDamping(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "AngularDamping");
	
	if(bUseScaling)
		fltVal = fltVal/this->DisplayMassUnits();

	m_fltAngularDamping = fltVal;
}

/**
\brief	Gets the linear kinetic loss of the simulation.

\author	dcofer
\date	3/28/2011

\return	Linear kinetic loss.
**/
float Simulator::LinearKineticLoss() {return m_fltLinearKineticLoss;}

/**
\brief	Sets the linear kinetic loss of the simulation.

\author	dcofer
\date	3/28/2011

\param	fltVal	The new value. 
**/
void Simulator::LinearKineticLoss(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "LinearKineticLoss");
	
	if(bUseScaling)
		fltVal = fltVal * this->DisplayMassUnits();

	m_fltLinearKineticLoss = fltVal;
}

/**
\brief	Gets the angular kinetic loss of teh simulation.

\author	dcofer
\date	3/28/2011

\return	angular kinetic loss.
**/
float Simulator::AngularKineticLoss() {return m_fltAngularKineticLoss;}

/**
\brief	Sets the angular kinetic loss of the simulation.

\author	dcofer
\date	3/28/2011

\param	fltVal	The new value. 
**/
void Simulator::AngularKineticLoss(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "AngularKineticLoss");
	
	if(bUseScaling)
		fltVal = fltVal * this->DisplayMassUnits();

	m_fltAngularKineticLoss = fltVal;
}


/**
\brief	Gets the smallest integration time step used within the simulation.

\details This is the base time step that is used for the individual time slice.

\author	dcofer
\date	3/28/2011

\return	smallest time step for the simulation.
**/
float Simulator::TimeStep() 
{return m_fltTimeStep;}

/**
\brief	Gets whether to use the set simulation end time.

\author	dcofer
\date	3/28/2011

\return	true to use the sim end time, false else.
**/
bool Simulator::SetEndSimTime() {return m_bSetEndSim;}

/**
\brief	Sets whether to use the simulation end time.

\author	dcofer
\date	3/28/2011

\param	bVal	true to use simulation end time. 
**/
void Simulator::SetEndSimTime(bool bVal) {m_bSetEndSim = bVal;}

/**
\brief	Gets the time at which to automatically end the simulation.

\author	dcofer
\date	3/28/2011

\return	Simulation end time.
**/
float Simulator::EndSimTime() {return m_fltEndSimTime;}

/**
\brief	Sets the time at which to automatically end the simulation.

\author	dcofer
\date	3/28/2011

\param	fltVal	The new value. 
**/
void Simulator::EndSimTime(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "EndSimTime");

	m_fltEndSimTime = fltVal;
	m_lEndSimTimeSlice = fltVal/m_fltTimeStep; 
}

/**
\brief	Gets the time slice at which to automatically end the simulation.

\author	dcofer
\date	3/28/2011

\return	Time slice to end simulation.
**/
long Simulator::EndSimTimeSlice() {return m_lEndSimTimeSlice;}

/**
\brief	Sets the time slice at which to automatically end the simulation.

\author	dcofer
\date	3/28/2011

\param	lVal	The new time slice value. 
**/
void Simulator::EndSimTimeSlice(long lVal) 
{
	Std_IsAboveMin((long) 0, lVal, true, "EndSimTimeSlice");

	m_lEndSimTimeSlice = lVal;
	m_fltEndSimTime = m_lEndSimTimeSlice*m_fltTimeStep;
}

/**
\brief	Tells if the simulation has been stopped.

\author	dcofer
\date	3/28/2011

\return	true if it is stopped, false else.
**/
bool Simulator::Stopped() {return (m_bStopSimulation | m_bForceSimulationStop);}

/**
\brief	Gets the frame rate used for the simulation windows in cylces per second.

\author	dcofer
\date	3/28/2011

\return	Frame rate in cylces per second.
**/
int Simulator::DesiredFrameRate() {return m_iDesiredFrameRate;}

/**
\brief	Gets the frame step time.

\details This is the time between simulation frames.

\author	dcofer
\date	3/28/2011

\return	Frame duration.
**/
float Simulator::DesiredFrameStep() {return m_fltDesiredFrameStep;}

/**
\brief	Sets the frame rate in cycles per second.

\author	dcofer
\date	3/28/2011

\param	iVal	The new value. 
**/
void Simulator::DesiredFrameRate(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "FrameRate");

	m_iDesiredFrameRate = iVal;
 	m_fltDesiredFrameStep = ((1/ (float) m_iDesiredFrameRate)*1);
}


/**
\brief	Returns the real time since the simulation was started.

\author	dcofer
\date	3/28/2011

**/
float Simulator::RealTime()
{return m_fltRealTime;}

/**
\brief	Gets the physics step interval.

\details This is the physics time step divided by the m_fltTimeStep. m_fltTimeStep is the 
smallest integration time step in the system. So this is how many total time slices occur 
between each iteration of the physics engine.

\author	dcofer
\date	3/28/2011

\return	Physics step interval.
**/
short Simulator::PhysicsStepInterval() {return m_iPhysicsStepInterval;}

/**
\brief	Sets the physics step interval.

\details This is the physics time step divided by the m_fltTimeStep. m_fltTimeStep is the 
smallest integration time step in the system. So this is how many total time slices occur 
between each iteration of the physics engine.

\author	dcofer
\date	3/28/2011

\param	iVal	The new value. 
**/
void Simulator::PhysicsStepInterval(short iVal) 
{
	if(iVal == 0) iVal = 1;
	Std_IsAboveMin((int) 0, (int) iVal, true, "PhysicsStepInterval");
	m_iPhysicsStepInterval = iVal;
}

/**
\brief	Sets the integration time step for the physics engine.

\author	dcofer
\date	3/28/2011

\param	fltVal	The new value. 
**/
void Simulator::PhysicsTimeStep(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "PhysicsTimeStep");

	//Set it so that it will be taken into consideration when finding min value.
	m_fltPhysicsTimeStep = fltVal;

	//Find min time step.
	float fltMin = MinTimeStep();
	
	//Division
	int iDiv = (int) ((fltVal / fltMin) + 0.5f);
	
	//Find the number of timeslices that need to occur before the physics system is updated
	PhysicsStepInterval(iDiv);

	//Now recaculate the physics time step using the minimum time step as the base.
	m_fltPhysicsTimeStep = m_fltTimeStep * m_iPhysicsStepInterval;

	//Now reset the m_fltTimeStep of the sim.
	if(m_iPhysicsStepInterval == 1) fltMin = MinTimeStep();
}

/**
\brief	Gets the integration time step for the physics engine.

\author	dcofer
\date	3/28/2011

\return	Physics time step.
**/
float Simulator::PhysicsTimeStep() {return m_fltPhysicsTimeStep;}

/**
\brief	Gets the physics step count.

\details This is a counter that keeps track of how many time slices have occured since the StepSimulation method
started. We use this to know how many more slices to go until the physics engine steps.

\author	dcofer
\date	3/28/2011

\return	Physisc step count variable.
**/
long Simulator::PhysicsStepCount() {return m_iPhysicsStepCount;}

/**
\brief	Gets the gravity value.

\author	dcofer
\date	3/28/2011

\return	Gravity.
**/
float Simulator::Gravity() {return m_fltGravity;}

/**
\brief	Sets the Gravity value.

\author	dcofer
\date	3/28/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Simulator::Gravity(float fltVal, bool bUseScaling)
{
	//We must convert the gravity to use the correct scale.
	if(bUseScaling)
		fltVal /= m_fltDistanceUnits;

	m_fltGravity = fltVal;
}

/**
\brief	Gets the mouse spring stiffness.

\details The user can grab onto objects using a mouse spring. This sets the stiffness of that spring.

\author	dcofer
\date	3/28/2011

\return	Stiffness of the mouse spring.
**/
float Simulator::MouseSpringStiffness() {return m_fltMouseSpringStiffness;}

/**
\brief	Sets the mouse spring stiffness.

\author	dcofer
\date	3/28/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Simulator::MouseSpringStiffness(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "MouseSpringStiffness", true);

	if(bUseScaling)
		fltVal *= this->InverseMassUnits();

	m_fltMouseSpringStiffness = fltVal;
}

/**
\brief	Gets the mouse spring damping.

\details The user can grab onto objects using a mouse spring. This sets the damping of that spring.

\author	dcofer
\date	3/28/2011

\return	damping.
**/
float Simulator::MouseSpringDamping() {return m_ftlMouseSpringDamping;}

/**
\brief	Sets the mouse spring damping.

\author	dcofer
\date	3/28/2011

\param	fltVal	   	The new value. 
\param	bUseScaling	true to use unit scaling. 
**/
void Simulator::MouseSpringDamping(float fltVal, bool bUseScaling) 
{
	Std_IsAboveMin((float) 0, fltVal, true, "MouseSpringDamping", true);

	if(bUseScaling)
		fltVal = fltVal/this->DisplayMassUnits();
	m_ftlMouseSpringDamping = fltVal;
}

/**
\brief	Gets whether the simulation uses hydrodynamics.

\details Tells whether or not we will be doing hydrodynamic simulations.
If you are not doing stuff underwater then be sure this is set to
false. The hydrodynamics adds extra overhead that can slow the
simulation down slightly.

\author	dcofer
\date	3/28/2011

\return	true if it uses hydrodynamics, false else.
**/
bool Simulator::SimulateHydrodynamics() {return m_bSimulateHydrodynamics;}

/**
\brief	Sets whether the simulation uses hydrodynamics.

\details Tells whether or not we will be doing hydrodynamic simulations.
If you are not doing stuff underwater then be sure this is set to
false. The hydrodynamics adds extra overhead that can slow the
simulation down slightly.

\author	dcofer
\date	3/28/2011

\param	bVal	true use hydrodynamics. 
**/
void Simulator::SimulateHydrodynamics(bool bVal)
{
	m_bSimulateHydrodynamics = bVal;
}

/**
\brief	Gets a material identifier from the physics engine for the specified unique ID.

\details This method must be implemented within the derived class for the physics engine.

\author	dcofer
\date	3/28/2011

\param	strID	Identifier for the material. 

\return	The material identifier.
**/
int Simulator::GetMaterialID(std::string strID) {return -1;} 

/**
\brief	Query if this object is physics being updated on this time slice.

\author	dcofer
\date	3/28/2011

\return	true if physics being updated, false if not.
**/
bool Simulator::IsPhysicsBeingUpdated()
{
	if(m_iPhysicsStepCount == m_iPhysicsStepInterval)
		return true;
	else
		return false;
}

/**
\brief	Gets the background color.

\author	dcofer
\date	3/28/2011

\return	Pointer to the background color.
**/
CStdColor *Simulator::BackgroundColor() {return  &m_vBackgroundColor;}

/**
\brief	Sets the background color. 

\author	dcofer
\date	3/2/2011

\param [in,out]	aryColor	The color data. 
**/
void Simulator::BackgroundColor(CStdColor &aryColor)
{
	m_vBackgroundColor = aryColor;
	if(m_lpWinMgr) m_lpWinMgr->UpdateBackgroundColor();
}

/**
\brief	Sets the background color.

\author	dcofer
\date	3/28/2011

\param [in,out]	vColor	Pointer to the color. 
**/
void Simulator::BackgroundColor(float *aryColor) 
{
	CStdColor vColor(aryColor[0], aryColor[1], aryColor[2], aryColor[3], 1);
	BackgroundColor(vColor);
}

/**
\brief	Loads the background color from an XML data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml	The color data in an xml data packet
**/
void Simulator::BackgroundColor(std::string strXml)
{
	CStdColor vColor(1);
	vColor.Load(strXml, "Color");
	BackgroundColor(vColor);
}

float Simulator::AlphaThreshold() {return m_fltAlphaThreshold;}

void Simulator::AlphaThreshold(float fltValue)
{
	Std_InValidRange((float) 0, (float) 1, fltValue, true, "AlphaThreshold");
	m_fltAlphaThreshold = fltValue;
}

float Simulator::RecFieldSelRadius() {return m_fltRecFieldSelRadius;}

void Simulator::RecFieldSelRadius(float fltValue, bool bUseScaling, bool bUpdateAllBodies)
{
	if(bUseScaling)
		m_fltRecFieldSelRadius = fltValue * this->InverseDistanceUnits();
	else
		m_fltRecFieldSelRadius = fltValue;

	if(bUpdateAllBodies)
	{
		CStdMap<std::string, AnimatBase *>::iterator oPos;
		AnimatBase *lpBase = NULL;
		for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
		{
			lpBase = oPos->second;
			IPhysicsBody *lpBody = dynamic_cast<IPhysicsBody *>(lpBase);
			if(lpBody && lpBase != this)
				lpBody->Physics_ResizeSelectedReceptiveFieldVertex();
		}
	}
}


/**
\brief	Gets the playback control mode.

\description The follow playback control modes are supported.
1. Fastest possible: In this mode no time delay is added to the playback at all.
The physics and neural engines are stepped as fast as they possibly can. However, 
this can lead to simulations that go too fast to see easily, and as parts of the simulation
because more complex with a larger number of collisions the playback rate will slow down
and then speed up again. 

2. Match Physics time step: In this mode the physics and neural engines will be stepped
to the next physics time step, and then any remaining real time left over will be used to
delay the simulation to match that value. This mode attempts to playback the simulation
in real time. If there is no time left over then it does not add any time to the playback.

3. Use Preset Value: This mode is the same as the physics time step, except it attempts to
match a preset playback time step.

\author	dcofer
\date	3/2/2011

**/
int Simulator::PlaybackControlMode()
{return m_iPlaybackControlMode;}

/**
\brief	Sets the playback control mode.

\description The follow playback control modes are supported.
1. Fastest possible: In this mode no time delay is added to the playback at all.
The physics and neural engines are stepped as fast as they possibly can. However, 
this can lead to simulations that go too fast to see easily, and as parts of the simulation
because more complex with a larger number of collisions the playback rate will slow down
and then speed up again. 

2. Match Physics time step: In this mode the physics and neural engines will be stepped
to the next physics time step, and then any remaining real time left over will be used to
delay the simulation to match that value. This mode attempts to playback the simulation
in real time. If there is no time left over then it does not add any time to the playback.

3. Use Preset Value: This mode is the same as the physics time step, except it attempts to
match a preset playback time step.

\author	dcofer
\date	3/2/2011

\param	iMode The playback mode to use.
**/
void Simulator::PlaybackControlMode(int iMode)
{
	if(iMode != PLAYBACK_MODE_FASTEST_POSSIBLE && iMode !=  PLAYBACK_MODE_MATCH_PHYSICS_STEP && iMode != PLAYBACK_MODE_PRESET_VALUE)
		THROW_PARAM_ERROR(Al_Err_lInvalidPlaybackMode, Al_Err_strInvalidPlaybackMode, "Mode", STR(iMode));

	m_iPlaybackControlMode = iMode;
}

/**
\brief	Gets the preset playback time step.

\description If the playback control mode is set to use a preset value
for the playback time step then this is the time step used.

\author	dcofer
\date	3/2/2011

**/
float Simulator::PresetPlaybackTimeStep()
{return m_fltPresetPlaybackTimeStep;}

/**
\brief	Sets the preset playback time step in milliseconds.

\description If the playback control mode is set to use a preset value
for the playback time step then this is the time step used.

\author	dcofer
\date	3/2/2011

\param	lTimeStep	The time step to use. 
**/
void Simulator::PresetPlaybackTimeStep(float fltTimeStep)
{
	if(fltTimeStep < 0)
		THROW_PARAM_ERROR(Al_Err_lInvalidPresetPlaybackTimeStep, Al_Err_strInvalidPresetPlaybackTimeStep, "Time Step", STR((float) fltTimeStep));

	m_fltPresetPlaybackTimeStep = fltTimeStep;
}

unsigned long long Simulator::StepStartTick()
{return m_lStepStartTick;}

unsigned long long Simulator::StepSimEndTick()
{return m_lStepSimEndTick;}

double Simulator::CurrentRealTimeForStep_n()
{return TimerDiff_n(m_lStepStartTick, GetTimerTick());}

double Simulator::CurrentRealTimeForStep_s()
{return TimerDiff_s(m_lStepStartTick, GetTimerTick());}

#pragma endregion

#pragma region UnitScalingVariables

/**
\brief	Sets the distance units.

\details The physcis engine in AnimatLab uses an arbitrary unit for mass and distance. It is up to the user
to decide what those units are. So for example, one distance unit can be a standard meter, or it could be a 
centimeter. However, internally all distance measurements must remain consistent and be scaled by the same amount.
This method calculates the conversion factor used throughout the simulation to insure that all distance units are
scaled by the same values. 

\author	dcofer
\date	3/28/2011

\param	strUnits	The string identifer of the distance units to use. 
**/
void Simulator::DistanceUnits(std::string strUnits)
{
	m_fltDistanceUnits = ConvertDistanceUnits(strUnits);
	m_fltInverseDistanceUnits = 1/m_fltDistanceUnits;
	m_fltDenominatorDistanceUnits = ConvertDenominatorDistanceUnits(strUnits);
}

/**
\brief	Gets the distance units used in the simulation.

\author	dcofer
\date	3/28/2011

\return	Distance units used.
**/
float Simulator::DistanceUnits() {return m_fltDistanceUnits;}

/**
\brief	Gets the inverse distance units.

\author	dcofer
\date	3/28/2011

\return	Inverse distance units.
**/
float Simulator::InverseDistanceUnits() {return m_fltInverseDistanceUnits;}

/**
\brief	Gets the denominator distance units.

\details For items that use distance unit measures in the denominator we may want to use a differnt
scale that that used for the whole app. For example, if we are using a distance scale of decimeters
we will want to use centimeters for the density instead. This allows us to do that.

\author	dcofer
\date	3/28/2011

\return	Denominator distance units.
**/
float Simulator::DenominatorDistanceUnits() {return m_fltDenominatorDistanceUnits;}

/**
\brief	Sets the mass units.

\details The physcis engine in AnimatLab uses an arbitrary unit for mass and distance. It is up to the user
to decide what those units are. So for example, one mass unit can be a standard kilogram, or it could be a 
gram. However, internally all mass measurements must remain consistent and be scaled by the same amount.
This method calculates the conversion factor used throughout the simulation to insure that all distance units are
scaled by the same values. 

\author	dcofer
\date	3/28/2011

\param	strUnits	The std::string identifer of the mass units to use. 
**/
void Simulator::MassUnits(std::string strUnits)
{
	m_fltMassUnits = ConvertMassUnits(strUnits);
	m_fltInverseMassUnits = 1/m_fltMassUnits;
	m_fltDisplayMassUnits = ConvertDisplayMassUnits(strUnits);
}

/**
\brief	Gets the mass units.

\author	dcofer
\date	3/28/2011

\return	Mass units.
**/
float Simulator::MassUnits() {return m_fltMassUnits;}

/**
\brief	Gets the inverse mass units.

\author	dcofer
\date	3/28/2011

\return	Inverse mass units.
**/
float Simulator::InverseMassUnits() {return m_fltInverseMassUnits;}

/**
\brief	Gets the density mass units.

\details The editor will save out 1 Kg as 1000. So we need to convert 1000 to 1. We use this
density mass unit value to do this.

\author	dcofer
\date	3/28/2011

\return	Density mass units.
**/
float Simulator::DisplayMassUnits() {return m_fltDisplayMassUnits;}

#pragma endregion
			
#pragma region RecordingVariables

/**
\brief	Gets the video slice count.

\author	dcofer
\date	3/28/2011

\return	video slice count.
**/
long Simulator::VideoSliceCount() {return m_lVideoSliceCount;}

/**
\brief	Sets the video slice count.

\author	dcofer
\date	3/28/2011

\param	lVal	The new value. 
**/
void Simulator::VideoSliceCount(long lVal) {m_lVideoSliceCount = lVal;}

/**
\brief	Gets the video loops.

\author	dcofer
\date	3/28/2011

\return	video loops.
**/
int Simulator::VideoLoops() {return m_iVideoLoops;}

/**
\brief	Sets the video loops.

\author	dcofer
\date	3/28/2011

\param	iVal	The new value. 
**/
void Simulator::VideoLoops(int iVal) {m_iVideoLoops = iVal;}


KeyFrame *Simulator::VideoRecorder() {return m_lpVideoRecorder;}
void Simulator::VideoRecorder(KeyFrame *lpFrame) {m_lpVideoRecorder = lpFrame;}

KeyFrame *Simulator::VideoPlayback() {return m_lpVideoPlayback;}
void Simulator::VideoPlayback(KeyFrame *lpFrame) {m_lpVideoPlayback = lpFrame;}

/**
\brief	Tells whehter simulation recording is enabled.

\author	dcofer
\date	3/28/2011

\return	true if recording is enabled, false else.
**/
bool Simulator::EnableSimRecording() {return m_bEnableSimRecording;}

/**
\brief	Sets whether simulation recording is Enabled.

\author	dcofer
\date	3/28/2011

\param	bVal	true to enable. 
**/
void Simulator::EnableSimRecording(bool bVal) {m_bEnableSimRecording = bVal;}

/**
\brief	Gets the snapshot byte size.

\author	dcofer
\date	3/28/2011

\return	snapshot byte size.
**/
long Simulator::SnapshotByteSize() {return m_lSnapshotByteSize;}

#pragma endregion

#pragma endregion

#pragma region Methods

#pragma region SimulationMethods

/**
\brief	Initializes the random number generation system.

\author	dcofer
\date	1/27/2012
**/
void Simulator::InitializeRandomNumbers()
{
	//Set the random number generator seed
	if(m_bAutoGenerateRandomSeed)
		GenerateAutoSeed();
	else
	{
		Std_SRand(m_iManualRandomSeed);
		srand(m_iManualRandomSeed);
	}
}

/**
\brief	Initializes all of the structures of this simulation.

\details This method runs through all of the "static" structures and organisms
and calls their Initialize method.

\author	dcofer
\date	3/28/2011
**/
void Simulator::InitializeStructures()
{
	InitializeRandomNumbers();

	m_oMaterialMgr.Initialize();

	//We need to rerun the code to set the physics time step here in initialize. The reason is that we set this when 
	//loading the simulator and neural modules, but if one of the neural modules has the miniumum time step then
	//we need to recalculate the time slice per step for all modules in initialize after everything has loaded.
	// Once everything is loaded and initialized, then if a given time step is changed then that one is changed in
	// the sim, and events will change it for the rest of them afterwards, so the values should be correct. 
	PhysicsTimeStep(m_fltPhysicsTimeStep);

	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->Create();
	}

	if(m_bEnableSimRecording)
		m_lSnapshotByteSize = CalculateSnapshotByteSize();

	if(m_bRecordVideo)
	{
		m_iVideoStepSize = (int) (m_fltVideoRecordFrameTime/m_fltPhysicsTimeStep);
		m_lVideoStartSlice = (long) (m_fltVideoStartTime/m_fltTimeStep);
		m_lVideoEndSlice = (long) (m_fltVideoEndTime/m_fltTimeStep);
		m_iVideoStep = 0;

		//First lets check if there is already an existing video file with that same
		//name. If there is then get rid of it.
		std::string strVideoFile = m_strProjectPath + m_strVideoFilename;
		struct stat f__stat;
		bool bFileExists = (stat(strVideoFile.c_str(), &f__stat) != 0);
		if(bFileExists)
			remove(strVideoFile.c_str( ));
	}

}

/**
\brief	Notifies the objects that time step has been modified.

\author	dcofer
\date	1/28/2012
**/
void Simulator::NotifyTimeStepModified()
{
	//Go through and call TimeStepModified for all objects.
	CStdMap<std::string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase && lpBase != this)
			lpBase->TimeStepModified();
	}
}

/**
\brief	Blocks the simulation from stepping.

\details The simulation and GUI are running in multi-threaded environment. When we make changes to the simulation
from the GUI we need to block processing of the simulation thread while we make those changes to prevent memory corruption.
This method initiates a block. We then call WaitForSimulationBlock to wait until the block takes effect before we make our
changes.

\author	dcofer
\date	3/28/2011
**/
void Simulator::BlockSimulation() {m_bBlockSimulation = true;}

/**
\brief	Unblock simulation.

\details The simulation and GUI are running in multi-threaded environment. When we make changes to the simulation
from the GUI we need to block processing of the simulation thread while we make those changes to prevent memory corruption.
This method unblocks the simulation to allow processing to resume.

\author	dcofer
\date	3/28/2011
**/
void Simulator::UnblockSimulation() {m_bBlockSimulation = false; m_bSimBlockConfirm = false;}

/**
\brief	Confirms that the simulation has been blocked.

\details The simulation and GUI are running in multi-threaded environment. When we make changes to the simulation
from the GUI we need to block processing of the simulation thread while we make those changes to prevent memory corruption.
This method tells whether the simulation has been blocked.

\author	dcofer
\date	3/28/2011

\return	true if it is blocked, false else.
**/
bool Simulator::SimulationBlockConfirm() {return m_bSimBlockConfirm;}

/**
\brief	Wait for simulation block.

\details The simulation and GUI are running in multi-threaded environment. When we make changes to the simulation
from the GUI we need to block processing of the simulation thread while we make those changes to prevent memory corruption.
This method sets the block simulation variable to true, and then waits until the simulation block is confirmed or a timeout
occurs.

\author	dcofer
\date	3/28/2011

\param	lTimeout	The timeout period in time slices. 

\return	true if it succeeds in getting the simulation block, false if it fails.
**/
bool Simulator::WaitForSimulationBlock(long lTimeout)
{
	if(!m_bSteppingSim)
		return true;

	m_bBlockSimulation = true;
	long lTime = 0;
	bool bDone = false;
	while(!bDone)
	{
		if(!m_bSimBlockConfirm)
		{
            Std_Sleep(10);
			
			lTime+=10;
			if(lTimeout > 0 && lTime >= lTimeout)
			{
				bDone = true;
				m_bBlockSimulation = false;
			}
		}
		else
			bDone = true;
	}

	return m_bSimBlockConfirm;
}

/**
\brief	Checks whether or not a simulation block has been requested.

\details This is called within the StepSimulation code. It checks whether someone has
requested a simulation block. If they have then it sets the m_bSimBlockConfirm flag to true.
This blocks the simulation and lets the WaitForSimulationBlock code to proceed.

\author	dcofer
\date	3/28/2011

\return	true if it blocks, false if it not.
**/
bool Simulator::CheckSimulationBlock()
{
	if(m_bBlockSimulation)
	{
		m_bSimBlockConfirm = true;
        Std_Sleep(1);
	}
	else
		m_bSimBlockConfirm = false;

	return m_bSimBlockConfirm;
}

/**
\brief	Resets all objects of the simulation to their unloaded state.

\details Use this function to completely reset a simulation to its initial default settings.
This will destroy all organisms and strcutres defined within the simulation

\author	dcofer
\date	3/28/2011
**/
void Simulator::Reset()
{
	m_fltTime = 0;
	m_fltTimeStep = -1;
	m_iPhysicsStepInterval = 4;
	m_fltPhysicsTimeStep = (float) 0.01;
	m_iPhysicsStepCount = 0;
	m_lTimeSlice = 0;
	m_fltEndSimTime = -1;
	m_lPhysicsSliceCount = 0;
	m_lVideoSliceCount = 0;
	m_iVideoLoops = 0;
	m_bSimulateHydrodynamics = false;
	m_fltGravity = (float) -9.8;
	m_fltDistanceUnits = (float) 0.01;  //use centimeters
	m_fltInverseDistanceUnits = 1/m_fltDistanceUnits;
	m_fltDenominatorDistanceUnits = 1;
	m_fltMassUnits = (float) 0.001;    //use grams
	m_fltInverseMassUnits = 1/m_fltMassUnits;
	m_fltDisplayMassUnits = 0.01f;
	m_fltMouseSpringStiffness = 25;
	m_ftlMouseSpringDamping = 2.8f;
	m_fltStabilityScale = 1.0;
	m_fltLinearCompliance = 0.1e-9f;
	m_fltAngularCompliance = 0.1e-9f;
	m_fltLinearDamping = 50e9f;
	m_fltAngularDamping = 5e12f;
	m_fltLinearKineticLoss = 0.1e-9f;
	m_fltAngularKineticLoss = 1e-12f;
	m_iPhysicsBodyCount = 0;

	m_bForceFastMoving = true;
	m_bSteppingSim = false;

	if(m_lpWinMgr)
		m_lpWinMgr->Close();

	if(m_lpAnimatClassFactory) {delete m_lpAnimatClassFactory; m_lpAnimatClassFactory = NULL;}
	m_aryOrganisms.RemoveAll();
	m_aryStructures.RemoveAll();
	m_aryAllStructures.RemoveAll();
	m_lpVideoRecorder = NULL; //Do not delete this object. It is in the list of Keyframes.
	m_lpVideoPlayback = NULL; //Do not delete this object. It is in the list of Keyframes.
	m_bEnableSimRecording = false;
	m_lSnapshotByteSize = 0;
	m_iPlaybackControlMode = PLAYBACK_MODE_MATCH_PHYSICS_STEP;
	m_fltPresetPlaybackTimeStep = 0;
	m_lStepStartTick = 0;
	m_lStepSimEndTick = 0;
	m_fltSimulationRealTimeToStep = 0;
	m_fltTotalRealTimeForStep = 0;
	m_fltPlaybackAdditionRealTimeToStep = 0;
	m_fltPrevTotalRealTimeForStep = 0;
	m_fltTotalRealTimeForStepSmooth = 0;
	m_fltPhysicsStepTime = 0;
	m_fltPrevPhysicsStepTime = 0;
	m_fltTotalNeuralStepTime = 0;
	m_fltRealTime = 0;
	m_lLastTickTaken = 0;
	m_fltExternalStimuliStepTime = 0;
	m_fltDataChartStepTime = 0;
	m_fltSimRecorderStepTime = 0;
	m_fltRemainingStepTime = 0;

	m_iDesiredFrameRate = 30;
 	m_fltDesiredFrameStep = (1/ (float) m_iDesiredFrameRate);
	m_lVideoFrameStartTick = 0;
	m_fltActualFrameRate = 0;

	m_bPaused = true;
	m_bInitialized = false;
	m_bSimRunning = false;

	m_bRecordVideo = false;
	m_strVideoFilename = "Video.avi";
	m_fltVideoRecordFrameTime = 1e-3f;
	m_fltVideoPlaybackFrameTime = 100e-3f;
	m_fltVideoStartTime = 0;
	m_fltVideoEndTime = 1;
	m_iVideoStepSize = -1;
	m_iVideoStepSize = 0;
	m_lVideoStartSlice = -1;
	m_lVideoEndSlice = -1;
	m_lVideoFrame = 0;

	m_oDataChartMgr.Reset();
	m_oExternalStimuliMgr.Reset();
	m_oMaterialMgr.Reset();
	m_oLightMgr.Reset();
	if(m_lpSimRecorder)
	{
		delete m_lpSimRecorder;
		m_lpSimRecorder = NULL;
	}

	if(m_lpSimStopPoint)
	{
		delete m_lpSimStopPoint;
		m_lpSimStopPoint = NULL;
	}

	if(m_lpSimCallback)
	{
		delete m_lpSimCallback;
		m_lpSimCallback = NULL;
	}

	m_aryNeuralModuleFactories.RemoveAll();

	m_arySourcePhysicsAdapters.RemoveAll();
	m_aryTargetPhysicsAdapters.RemoveAll();
	m_iTargetAdapterCount = 0;

	m_aryOdorTypes.RemoveAll();
	m_aryFoodSources.RemoveAll();

	//Reference pointers only
	m_lpSelOrganism = NULL;
	m_lpSelStructure = NULL;
}

/**
\brief	Resets the the simulation to its orginal settings at time 0

\author	dcofer
\date	3/28/2011
**/
void Simulator::ResetSimulation()
{
	m_fltTime = 0;
	m_lTimeSlice = 0;
	m_lPhysicsSliceCount = 0;
	m_lVideoSliceCount = 0;
	m_iPhysicsStepCount = 0;
	m_bPaused = true;
	m_bSimRunning = false;

	InitializeRandomNumbers();

	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->ResetSimulation();
	}

	int iSize = m_arySourcePhysicsAdapters.GetSize();
	for(int iIndex=0; iIndex<iSize; iIndex++)
		m_arySourcePhysicsAdapters[iIndex]->ResetSimulation();

	iSize = m_aryTargetPhysicsAdapters.GetSize();
	for(int iIndex=0; iIndex<iSize; iIndex++)
		m_aryTargetPhysicsAdapters[iIndex]->ResetSimulation();

	m_oDataChartMgr.ResetSimulation();
	m_oExternalStimuliMgr.ResetSimulation();
	m_oLightMgr.ResetSimulation();
	if(m_lpWinMgr) m_lpWinMgr->ResetSimulation();
	
	if(m_lpSimRecorder)
		m_lpSimRecorder->ResetSimulation();
}

float Simulator::MinTimeStep()
{
	m_fltTimeStep = m_fltPhysicsTimeStep;
	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->MinTimeStep(m_fltTimeStep);
	}

	return m_fltTimeStep;
}

void Simulator::MicroWait(unsigned int iMicroTime)
{
	unsigned long long lStart = GetTimerTick();
	int iRemaining = iMicroTime;

	while(iRemaining > 0)
		iRemaining = iMicroTime - (unsigned int) TimerDiff_u(lStart, GetTimerTick());

	//double dblWaitTime = TimerDiff_u(lStart, GetTimerTick());
}

void Simulator::StepPlaybackControl()
{
	RecordSimulationStepTimer();

	//If paused or stepping fastest possible then just step the video frame and exit.
	if(m_bPaused || m_iPlaybackControlMode == PLAYBACK_MODE_FASTEST_POSSIBLE 
	   || (m_iPlaybackControlMode == PLAYBACK_MODE_PRESET_VALUE && m_fltPresetPlaybackTimeStep <= 0))
		StepVideoFrame();
	else
	{
		double dblRemainingTime = 0;

		do {
			dblRemainingTime = (CalculateRemainingPlaybackTime()); //scale the remaining time back by 10% to account for the time of other things.

			if(dblRemainingTime > m_fltDesiredFrameStep)
				MicroSleep(m_fltDesiredFrameStep*1000000);
			else if(dblRemainingTime > 0)
				MicroWait(dblRemainingTime*1000000);

			StepVideoFrame();				

		} while(dblRemainingTime > 0);
	}

	RecordAddedPlaybackTime();
}

void Simulator::StepVideoFrame()
{
	double dblTime = TimeBetweenVideoFrames();
	if(dblTime > m_fltDesiredFrameStep)
	{
		UpdateSimulationWindows();
		StartVideoFrameTimer();
	}
	else if(m_bPaused)
		MicroSleep((unsigned int) (RemainingVideoFrameTime()*1000000));
}


/**
\brief	Step the neural engine of each organism.

\author	dcofer
\date	3/28/2011
**/
void Simulator::StepNeuralEngine()
{
	unsigned long long lStart = GetTimerTick();

	for(m_oOrganismIterator=m_aryOrganisms.begin(); 
	    m_oOrganismIterator!=m_aryOrganisms.end(); 
			++m_oOrganismIterator)
	{
		m_lpSelOrganism = m_oOrganismIterator->second;
		m_lpSelOrganism->StepNeuralEngine();
	}

	m_fltTotalNeuralStepTime += TimerDiff_s(lStart, GetTimerTick());
}

/**
\brief	Calls StepPhysicsEngine of all structures.

\author	dcofer
\date	3/28/2011
**/
void Simulator::StepPhysicsEngine()
{
	unsigned long long lStart = GetTimerTick();

	for(m_oStructureIterator=m_aryAllStructures.begin(); 
	    m_oStructureIterator!=m_aryAllStructures.end(); 
			++m_oStructureIterator)
	{
		m_lpSelStructure = m_oStructureIterator->second;
		m_lpSelStructure->StepPhysicsEngine();
	}

	//Now lets step all Target adapters. This will be all items outputing
	//to the physics engine. Examples are motorized joints and muscles.
	for(int iIndex=0; iIndex<m_iTargetAdapterCount; iIndex++)
		m_aryTargetPhysicsAdapters[iIndex]->StepSimulation();

	if(m_bRecordVideo)
		RecordVideoFrame();

	m_lPhysicsSliceCount++;

	m_fltPhysicsStepTime += TimerDiff_s(lStart, GetTimerTick());
}

void Simulator::StepExternalStimuli()
{
	unsigned long long lStart = GetTimerTick();

	m_oExternalStimuliMgr.StepSimulation();

	m_fltExternalStimuliStepTime += TimerDiff_s(lStart, GetTimerTick());
}

void Simulator::StepDataCharts()
{
	unsigned long long lStart = GetTimerTick();

	m_oDataChartMgr.StepSimulation();

	m_fltDataChartStepTime += TimerDiff_s(lStart, GetTimerTick());
}

void Simulator::StepSimRecorder()
{
	unsigned long long lStart = GetTimerTick();

	m_lpSimRecorder->StepSimulation();

	m_fltSimRecorderStepTime += TimerDiff_s(lStart, GetTimerTick());
}

/**
\brief	Steps the simulation forward by one time slice.

\author	dcofer
\date	3/28/2011
**/
void Simulator::Step()
{
	StepExternalStimuli();

	if(m_iPhysicsStepCount == m_iPhysicsStepInterval)
		StepPhysicsEngine();

	StepNeuralEngine();

	if(m_lpSimRecorder) 
		StepSimRecorder();

	//Must be last in order to get all data changes that were previously made.
	StepDataCharts();
		
	m_lTimeSlice++;
	m_fltTime = m_lTimeSlice*m_fltTimeStep;
}

/**
\brief	Resets the variables that are used to keep track of the time that each component of the time step takes to execute.

\author	dcofer
**/
void Simulator::ResetSimulationTimingParams()
{
	m_fltPrevPhysicsStepTime = m_fltPhysicsStepTime;
	m_fltPhysicsStepTime = 0;
	m_fltTotalNeuralStepTime = 0;
	m_fltExternalStimuliStepTime = 0;
	m_fltDataChartStepTime = 0;
	m_fltSimRecorderStepTime = 0;
	m_fltRemainingStepTime = 0;
}

/**
\brief	Steps the entire simulation forward by one physics integration time step.

\details This method steps the entire simulation forward by one physics time step. 
Remember that there are typically multiple neural steps for a given physcis step, so this
will step the simulation as a whole several times for a given physics time step. It goes through
and calls StepSimulation for every structure/organism object, which in turn calls
StepSimulation for each rigid body and joint of each of those objects. So you need 
to be VERY careful to keep all code within the StepSimulation methods short, sweet, 
and very fast. They are in the main processing loop and even a small increase in the
amount of processing time that occurrs within this loop will lead to major impacts on
the ultimate performance of the system.

\author	dcofer
\date	3/28/2011
**/
void Simulator::StepSimulation()
{
	ResetSimulationTimingParams();
	for(m_iPhysicsStepCount=1; m_iPhysicsStepCount<=m_iPhysicsStepInterval; m_iPhysicsStepCount++)
		Step();
}

/**
\brief	Called at the beginning of the Simulate method.

\author	dcofer
\date	7/5/2011
**/
void Simulator::SimulateBegin()
{
	m_bSteppingSim = true;
}

/**
\brief	Called at the end of the Simulate method.

\author	dcofer
\date	7/5/2011
**/
void Simulator::SimulateEnd()
{
}

/**
\brief	Process the an entire step of the simulation. This includes the simulation portion, video and calculating timing.

\author	dcofer
\date	7/5/2011
**/
void Simulator::ProcessSimulationStep()
{
	try
	{
		//If we are blocking the simulation stepping code for multi-threaded access then don't do this code..
		if(!CheckSimulationBlock())
		{
		 	StartSimulationStepTimer();
			StepSimulation();
			StepPlaybackControl();

			CheckEndSimulationTime();
			RecordSimulationTotalStepTimer();
		}
	}
	catch(CStdErrorInfo oError)
	{
		//A critical simulation error has occurred if we catch an exception here. We need to shut the app down.
		std::string strError = "An error occurred while stepping the simulation.\nError: " + oError.m_strError;
		HandleNonCriticalError(strError);
	}
	catch(...)
	{
		//A critical simulation error has occurred if we catch an exception here. We need to shut the app down.
		HandleCriticalError(Al_Err_strCriticalSimError);
	}
}

void Simulator::Simulate()
{
	SimulateBegin();

	do 
    {
		ProcessSimulationStep();
	} 
    while (!m_bStopSimulation && !m_bForceSimulationStop);

	SimulateEnd();
}

/**
\brief	Runs the simulation.

\details This is primarily used when running the simulation in stand-alone mode. It loads the project file, 
initializes the simulation, and the calls Simulate.

\author	dcofer
\date	3/28/2011
**/
void Simulator::RunSimulation()
{
	Load(SimulationFile());
	Initialize(0, NULL);
	Simulate();
}

/**
\brief	Simulation starting event.

\details This method is called on all objects when the simulation begins starting. It is to let the
objects know about this event so they can do any pre-processing required before the simulation starts.

\author	dcofer
\date	3/28/2011
**/
void Simulator::SimStarting()
{
	CStdMap<std::string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase != this)
			lpBase->SimStarting();
	}
}

/**
\brief	Simulation pausing event.

\details This method is called on all objects when the simulation begins pausing. It is to let the
objects know about this event so they can do any pre-processing required before the simulation pauses.

\author	dcofer
\date	3/28/2011
**/
void Simulator::SimPausing()
{
	CStdMap<std::string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase != this)
			lpBase->SimPausing();
	}
}

/**
\brief	Simulation stopping event.

\details This method is called on all objects when the simulation begins stopping. It is to let the
objects know about this event so they can do any pre-processing required before the simulation stops.

\author	dcofer
\date	3/28/2011
**/
void Simulator::SimStopping()
{
	CStdMap<std::string, AnimatBase *>::iterator oPos;
	AnimatBase *lpBase = NULL;
	for(oPos=m_aryObjectList.begin(); oPos!=m_aryObjectList.end(); ++oPos)
	{
		lpBase = oPos->second;
		if(lpBase != this)
			lpBase->SimStopping();
	}
}

void Simulator::HandleCriticalError(std::string strError)
{
	this->ShutdownSimulation();
	if(m_lpSimCallback)
		m_lpSimCallback->HandleCriticalError(Al_Err_strCriticalSimError);

}

void Simulator::HandleNonCriticalError(std::string strError)
{
	this->ResetSimulation();
	if(m_lpSimCallback)
		m_lpSimCallback->HandleCriticalError(strError);
}

void Simulator::StartSimulationStepTimer()
{
	m_lStepStartTick = GetTimerTick();
	m_fltRealTime = TimerDiff_s(m_lStartSimTick, m_lStepStartTick); 
}

void Simulator::RecordSimulationStepTimer()
{
	m_lStepSimEndTick = GetTimerTick();
	m_fltSimulationRealTimeToStep = (float) TimerDiff_s(m_lStepStartTick, m_lStepSimEndTick);

	m_fltPlaybackAdditionRealTimeToStep = 0;
}

void Simulator::RecordSimulationTotalStepTimer()
{
	unsigned long long lEnd = GetTimerTick();
	m_fltPrevTotalRealTimeForStep = m_fltTotalRealTimeForStep;
	m_fltTotalRealTimeForStep = (float) TimerDiff_s(m_lStepStartTick, lEnd);

	m_fltRemainingStepTime = m_fltTotalRealTimeForStep - (m_fltPhysicsStepTime + m_fltTotalNeuralStepTime + m_fltExternalStimuliStepTime + m_fltDataChartStepTime + m_fltSimRecorderStepTime + m_fltPlaybackAdditionRealTimeToStep);

	m_fltTotalRealTimeForStepSmooth = m_fltTotalRealTimeForStepSmooth + 0.05*(m_fltPrevTotalRealTimeForStep-m_fltTotalRealTimeForStepSmooth);

	if(m_lTimeSlice > 10 && m_lTimeSlice < 5000)
	{
		m_dblTotalStepTime += m_fltTotalRealTimeForStep;
		m_lStepTimeCount++;
	}
	else if(m_lTimeSlice == 5000)
	{
		double dblAvgStepTime = m_dblTotalStepTime/m_lStepTimeCount;
		WriteToConsole("Average total step time: " + STR(dblAvgStepTime));
	}

}

double Simulator::CalculateRemainingPlaybackTime()
{
	if(m_iPlaybackControlMode == PLAYBACK_MODE_FASTEST_POSSIBLE)
		return 0;
	else if(m_iPlaybackControlMode ==  PLAYBACK_MODE_MATCH_PHYSICS_STEP)
		return m_fltPhysicsTimeStep - CurrentRealTimeForStep_s();
	else 
		return m_fltPresetPlaybackTimeStep - CurrentRealTimeForStep_s();
}

void Simulator::RecordAddedPlaybackTime()
{
	unsigned long long lEnd = GetTimerTick();
	m_fltPlaybackAdditionRealTimeToStep = (float) TimerDiff_s(m_lStepSimEndTick, lEnd);
}

void Simulator::StartVideoFrameTimer()
{
	double dblTime = TimeBetweenVideoFrames();
	m_fltActualFrameRate = (float) 1.0/(dblTime);
	m_lVideoFrameStartTick = GetTimerTick();
}

double Simulator::TimeBetweenVideoFrames()
{
	return TimerDiff_s(m_lVideoFrameStartTick, GetTimerTick());
}

double Simulator::RemainingVideoFrameTime()
{
	double dblRemaining =  (m_fltDesiredFrameStep - TimeBetweenVideoFrames());
	if(dblRemaining < 0) dblRemaining = 0;
	return dblRemaining;
}

/**
\brief	Generates an automatic seed value based on the current time.

\author	dcofer
\date	3/28/2011
**/
void Simulator::GenerateAutoSeed()
{
    //NEED TO TEST
    time_t rawtime;
    struct tm * timeinfo;

    time ( &rawtime );
    timeinfo = localtime ( &rawtime );

	m_iManualRandomSeed = (unsigned) (timeinfo->tm_sec + timeinfo->tm_hour + Std_IRand(0, 1000));
	Std_SRand(m_iManualRandomSeed);
	srand(m_iManualRandomSeed);
}

/**
\brief	Checks the simulation end time.

\details This checks whether the user has set the simulation to automatically end or not.
If it has and the simulation time is greater than or equal to the end time then it stops the simulation.
If we are running using the GUI then the m_lpSimCallback should be set. It pauses the simulation and then
calls the NeedToStopSimulation event callback. This signals to the GUI that it needs to stop the simulation.
If we are running standalone then the simulation is shutdown direclty.

\author	dcofer
\date	3/26/2011
**/
void Simulator::CheckEndSimulationTime()
{
	//If we are running for a set time then lets stop once we reach that point.
	if(m_bSetEndSim && !m_bPaused)
	{
		if(m_fltEndSimTime >0 && this->Time() >= m_fltEndSimTime)
		{
			if(m_lpSimCallback)
			{
				PauseSimulation();
				m_lpSimCallback->NeedToStopSimulation();
			}
			else
				ShutdownSimulation();
		}
	}
}

#pragma endregion
			
#pragma region LoadMethods


/**
\brief	Loads the simulation from an xml configuration file.

\author	dcofer
\date	3/28/2011

\param	strFileName	The std::string to load. 
**/
void Simulator::Load(std::string strFileName)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading simulator config file.\r\nFileName: " + strFileName);

	if(Std_IsBlank(strFileName))
	{
	 if(Std_IsBlank(m_strSimulationFile))
		 THROW_ERROR(Al_Err_lSimFileBlank, Al_Err_strSimFileBlank);
	}	
	else
		m_strSimulationFile = strFileName;

	oXml.Load(AnimatSim::GetFilePath(m_strProjectPath, m_strSimulationFile));

	oXml.FindElement("Simulation");
	oXml.FindChildElement("");

	Load(oXml);

	TRACE_DEBUG("Finished loading simulator config file.");
}

/**
\brief	Saves the simulation file.

\author	dcofer
\date	3/28/2011

\param	strFilename	The std::string to save. 
**/
void Simulator::Save(std::string strFilename) {};


void Simulator::Load(CStdXml &oXml)
{
	TRACE_DEBUG("Loading simulator config from Xml.");

	Reset();

	AddToObjectList(this);

	if(Std_IsBlank(m_strProjectPath))
		m_strProjectPath = oXml.GetChildString("ProjectPath", "");

	//m_lUpdateDataInterval = oXml.GetChildLong("UpdateDataInterval", m_lUpdateDataInterval);
	m_bPaused = oXml.GetChildBool("StartPaused", m_bPaused);
	m_bEnableSimRecording = oXml.GetChildBool("EnableSimRecording", m_bEnableSimRecording);
	
	SetEndSimTime(oXml.GetChildBool("SetSimEnd", false));
	EndSimTime(oXml.GetChildFloat("SimEndTime", m_fltEndSimTime));
 
	PlaybackControlMode(oXml.GetChildInt("PlaybackControlMode", m_iPlaybackControlMode));
	PresetPlaybackTimeStep(oXml.GetChildFloat("PresetPlaybackTimeStep", m_fltPresetPlaybackTimeStep));

	if(m_bEnableSimRecording)
		m_lpSimRecorder = CreateSimulationRecorder();

	//Other stuff Later
	LoadEnvironment(oXml);
	m_oDataChartMgr.Load(oXml);

	if(m_lpWinMgr) 
		m_lpWinMgr->Load(oXml);

	if(oXml.FindChildElement("ExternalStimuli", false))
		m_oExternalStimuliMgr.Load(oXml);

	if(m_lpSimRecorder && oXml.FindChildElement("RecorderKeyFrames", false))
		m_lpSimRecorder->Load(oXml);

	TRACE_DEBUG("Finished loading simulator config from Xml.");
}

/**
\brief	Loads the class factory specified in the DLL module name.

\author	dcofer
\date	3/28/2011

\param	strModuleName	Name of the dll module. 

\return	Pointer to the class factory.
\exception Throws an exception if there is an error creating the class factory.
**/
IStdClassFactory *Simulator::LoadClassFactory(std::string strModuleName)
{
	IStdClassFactory *lpFactory=NULL;

try
{
	int iFindDebug = Std_ToLower(strModuleName).find("d.dll");

#ifdef _DEBUG
	if(iFindDebug == -1 )
		THROW_PARAM_ERROR(Al_Err_lLoadingReleaseLib, Al_Err_strLoadingReleaseLib, "Module Name", strModuleName);
#else
	if(iFindDebug != -1)
		THROW_PARAM_ERROR(Al_Err_lLoadingDebugLib, Al_Err_strLoadingDebugLib, "Module Name", strModuleName);
#endif

	lpFactory = IStdClassFactory::LoadModule(strModuleName);
	return lpFactory;
}
catch(CStdErrorInfo oError)
{
	if(lpFactory) delete lpFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpFactory) delete lpFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


/**
\brief	Loads all structures from from the configuration file for this simulation.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml	The xml. 
**/
void Simulator::LoadEnvironment(CStdXml &oXml)
{
	TRACE_DEBUG("Loading structures from Xml.");

	if(!m_lpAnimatClassFactory)
		THROW_ERROR(Al_Err_lClassFactoryNotDefined, Al_Err_strClassFactoryNotDefined);

	m_aryOrganisms.RemoveAll();
	m_aryStructures.RemoveAll();
	m_aryOdorTypes.RemoveAll();

	oXml.IntoChildElement("Environment"); //Into Environment Element

	DistanceUnits(oXml.GetChildString("DistanceUnits", "centimeter"));
	MassUnits(oXml.GetChildString("MassUnits", "gram"));
	Gravity(oXml.GetChildFloat("Gravity", m_fltGravity));

	//We do NOT call the TimeStep mutator here because we need to call it only after all modules are loaded so we can calculate the min time step correctly.
	m_fltPhysicsTimeStep = oXml.GetChildFloat("PhysicsTimeStep", m_fltPhysicsTimeStep);

	SimulateHydrodynamics(oXml.GetChildBool("SimulateHydrodynamics", m_bSimulateHydrodynamics));

	AutoGenerateRandomSeed(oXml.GetChildBool("AutoGenerateRandomSeed", m_bAutoGenerateRandomSeed));
	ManualRandomSeed(oXml.GetChildInt("ManualRandomSeed", m_iManualRandomSeed));

	DesiredFrameRate(oXml.GetChildInt("FrameRate", m_iDesiredFrameRate));
	ForceFastMoving(oXml.GetChildBool("FastMoving", m_bForceFastMoving));

	MouseSpringStiffness(oXml.GetChildFloat("MouseSpringStiffness", m_fltMouseSpringStiffness));
	MouseSpringDamping(oXml.GetChildFloat("MouseSpringDamping", m_ftlMouseSpringDamping));

	StabilityScale(oXml.GetChildFloat("StabilityScale", m_fltStabilityScale));
	//LinearCompliance(oXml.GetChildFloat("LinearCompliance", m_fltLinearCompliance));
	//AngularCompliance(oXml.GetChildFloat("AngularCompliance", m_fltAngularCompliance));

	//if(!m_bCalcCriticalSimParams)
	//{
	//	LinearDamping(oXml.GetChildFloat("LinearDamping", m_fltLinearDamping));
	//	AngularDamping(oXml.GetChildFloat("AngularDamping", m_fltAngularDamping));
	//	LinearKineticLoss(oXml.GetChildFloat("LinearKineticLoss", m_fltLinearKineticLoss));
	//	AngularKineticLoss(oXml.GetChildFloat("AngularKineticLoss", m_fltAngularKineticLoss));
	//}

	RecFieldSelRadius(oXml.GetChildFloat("RecFieldSelRadius", m_fltRecFieldSelRadius));
	
	m_vBackgroundColor.Load(oXml, "BackgroundColor", false);

	m_fltAlphaThreshold = oXml.GetChildFloat("AlphaThreshold", m_fltAlphaThreshold);

	m_oMaterialMgr.Load(oXml);

	if(oXml.FindChildElement("OdorTypes", false))
	{
		oXml.IntoElem();  //Into Odors Element
		int iChildCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadOdorType(oXml);
		}
		oXml.OutOfElem(); //OutOf Odors Element
	}	

	if(oXml.FindChildElement("Organisms", false))
	{
		oXml.IntoElem(); //Into Organisms Element
		int iCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadOrganism(oXml);		
		}
		oXml.OutOfElem(); //OutOf Organisms Element
	}

	if(oXml.FindChildElement("Structures", false))
	{
		oXml.IntoElem(); //Into Structures Element
		int iCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadStructure(oXml);
		}
		oXml.OutOfElem(); //OutOf Structures Element

	}

	m_oLightMgr.Load(oXml);

	oXml.OutOfElem(); //OutOf Environment Element

	TRACE_DEBUG("Finished loading structures from Xml.");
}

/**
\brief	Loads a structure.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml	The xml that defines the structure to load. 

\return	Pointer to the structure.
\exception Throws an exception if there is a problem creating or loading the structure.
**/
Structure *Simulator::LoadStructure(CStdXml &oXml)
{
	Structure *lpStructure = NULL;

try
{
	lpStructure = dynamic_cast<Structure *>(m_lpAnimatClassFactory->CreateObject("Structure", "Structure", true));
	lpStructure->SetSystemPointers(this, NULL, NULL, NULL, true);
	lpStructure->Load(oXml);

	AddStructure(lpStructure);
	return lpStructure;
}
catch(CStdErrorInfo oError)
{
	if(lpStructure) delete lpStructure;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpStructure) delete lpStructure;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Loads an organism.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml	The xml that defines the organism to load. 

\return	Pointer to the organism.
\exception Throws an exception if there is a problem creating or loading the organism.
**/
Organism *Simulator::LoadOrganism(CStdXml &oXml)
{
	Organism *lpOrganism = NULL;
	std::string strModule;
	std::string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpOrganism = dynamic_cast<Organism *>(CreateObject(strModule, "Organism", strType));
	if(!lpOrganism)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	lpOrganism->SetSystemPointers(this, NULL, NULL, NULL, true);
	lpOrganism->Load(oXml);

	AddOrganism(lpOrganism);
	return lpOrganism;
}
catch(CStdErrorInfo oError)
{
	if(lpOrganism) delete lpOrganism;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpOrganism) delete lpOrganism;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Loads an odor type.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml	The xml that defines the odor type to load. 

\return	Pointer to the odor type.
\exception Throws an exception if there is a problem creating or loading the odor type.

**/
OdorType *Simulator::LoadOdorType(CStdXml &oXml)
{
	OdorType *lpOdorType = NULL;

try
{
	lpOdorType = new OdorType();

	lpOdorType->SetSystemPointers(this, NULL, NULL, NULL, true);
	lpOdorType->Load(oXml);

	AddOdorType(lpOdorType);
	return lpOdorType;
}
catch(CStdErrorInfo oError)
{
	if(lpOdorType) delete lpOdorType;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpOdorType) delete lpOdorType;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Loads an animat module name.

\author	dcofer
\date	3/28/2011

\param	strFile				   	The string file. 
\param [in,out]	strAnimatModule	The string animat module. 
**/
void Simulator::LoadAnimatModuleName(std::string strFile, std::string &strAnimatModule)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading simulator module name file.\r\nFileName: " + strFile);

	if(Std_IsBlank(strFile))
		THROW_ERROR(Al_Err_lSimFileBlank, Al_Err_strSimFileBlank);

	oXml.Load(strFile);

	LoadAnimatModuleName(oXml, strAnimatModule);

	TRACE_DEBUG("Finished loading simulator module name.");
}

/**
\brief	Loads an animat module name.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml		   	The xml. 
\param [in,out]	strAnimatModule	The string animat module. 
**/
void Simulator::LoadAnimatModuleName(CStdXml &oXml, std::string &strAnimatModule)
{
	oXml.FindElement("Simulation");
	oXml.FindChildElement("");

	strAnimatModule = oXml.GetChildString("AnimatModule");
}

#pragma endregion

#pragma region CreateMethods

/**
\brief	Creates an object using a class factory.

\details The method takes a dll module name, a class name, and a class type and creates an object. The dll is loaded
using the specified dll module name. A StdClassFactory pointer is retrieved from the dll, and then we call the CreateObject
method of the class factory to create the actual object we need. This allows us to create objects of any type in a modular manner
by simply specifying the name of the dll and the class we want.

\author	dcofer
\date	3/28/2011

\param	strModule   	The dll module name. 
\param	strClassName	Name of the class to create. 
\param	strType			The specific type of the class to create. 
\param	bThrowError 	true to throw error if there is a problem. 

\return	Pointer to the created object.
**/
CStdSerialize *Simulator::CreateObject(std::string strModule, std::string strClassName, std::string strType, bool bThrowError)
{
	strModule = Std_CheckString(strModule);
	
	if(strModule == "" || strModule == "ANIMATLAB")
	{
		if(!m_lpAnimatClassFactory)
			THROW_ERROR(Al_Err_lClassFactoryNotDefined, Al_Err_strClassFactoryNotDefined);

		return m_lpAnimatClassFactory->CreateObject(strClassName, strType, bThrowError);
	}
	else
	{
		IStdClassFactory *lpFactory = FindNeuralModuleFactory(strModule, false);

		if(lpFactory)
			return lpFactory->CreateObject(strClassName, strType, bThrowError);
		else
		{
			//Lets load the dynamic library and get a pointer to the class factory.
			lpFactory = LoadClassFactory(strModule);

			//Now create an instance of a neural module. There is only one type of 
			return lpFactory->CreateObject(strClassName, strType, bThrowError);
		}
	}

	return NULL;
}

/**
\brief	Creates a simulator from the command line.

\details This method is primarily used when running the simulator in stand-alone mode.

\author	dcofer
\date	3/28/2011

\param	argc	The argc parameter from the command line. 
\param	argv	The argv parameter from the command line. 

\return	Pointer to the new simulator.
**/
Simulator *Simulator::CreateSimulator(int argc, const char **argv)
{	
	std::string strExecutablePath, strExeFile;

    std::string strBuffer = Std_ExecutablePath();
	Std_SplitPathAndFile(strBuffer, strExecutablePath, strExeFile);

	//Set the log file prefix
	if(Std_DirectoryExists(strExecutablePath + "Logs"))
		Std_SetLogFilePrefix(strExecutablePath + "Logs\\AnimatSimulator");
	else
		Std_SetLogFilePrefix(strExecutablePath + "AnimatSimulator");

	std::string strProject = Std_RetrieveParam(argc, argv, "-PROJECT", false);
	std::string strAnimatModule = Std_RetrieveParam(argc, argv, "-LIBRARY", true);

	if(Std_IsBlank(strProject))
		THROW_ERROR(Al_Err_lNoProjectParamOnCommandLine, Al_Err_strNoProjectParamOnCommandLine);

	if(Std_IsBlank(strAnimatModule))
		THROW_ERROR(Al_Err_lNoModuleParamOnCommandLine, Al_Err_strNoModuleParamOnCommandLine);

	return CreateSimulator(strAnimatModule, strProject);
}

/**
\brief	Creates a simulator from a simulator file.

\author	dcofer
\date	3/28/2011

\param	strSimulationFile	The string simulation file. 

\return	Pointer to the new simulator.
**/
Simulator *Simulator::CreateSimulator(std::string strAnimatModule, std::string strSimulationFile)
{
	Simulator *lpSim = NULL;
	IStdClassFactory *lpAnimatFactory=NULL;

	std::string strProjectPath, strProjectFile;
	std::string strExecutablePath, strExeFile;

try
{ 
    std::string strBuffer = Std_ExecutablePath();
	Std_SplitPathAndFile(strBuffer, strExecutablePath, strExeFile);

	//_getcwd( strBuffer, 2000 );
	//Std_SetLogFilePrefix(strExecutablePath + "Logs\\AnimatSimulator");
 
    if(!Std_FileExists(strSimulationFile))
		THROW_PARAM_ERROR(Al_Err_lSimFileNotFound, Al_Err_strSimFileNotFound, "Simulation File", strSimulationFile);

	Std_SplitPathAndFile(strSimulationFile, strProjectPath, strProjectFile);

	if(!Std_IsFullPath(strSimulationFile))
	{
		strProjectPath = strExecutablePath;
		strSimulationFile = strProjectPath + strSimulationFile;
	}

	if(Std_IsBlank(strAnimatModule))
		LoadAnimatModuleName(strSimulationFile, strAnimatModule);

	lpAnimatFactory = LoadClassFactory(strAnimatModule);

	//Now we need to get the simulation application itself. This ALWAYS
	//comes from the animat engine because the animat engine uses the
	//neural engine it is higher up the food chain and it decides the
	//actual simulator that needs to be used.
	lpSim = dynamic_cast<Simulator *>(lpAnimatFactory->CreateObject("Simulator", ""));
	if(!lpSim)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Simulator");

	lpSim->ProjectPath(strProjectPath);
	lpSim->ExecutablePath(strExecutablePath);
	lpSim->SimulationFile(strProjectFile);

	if(lpAnimatFactory) 
		{delete lpAnimatFactory; lpAnimatFactory = NULL;}

	g_lpSimulator = lpSim;
	return lpSim;
}
catch(CStdErrorInfo oError)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Creates a simulator from an xml packet.

\author	dcofer
\date	3/28/2011

\param [in,out]	oXml	The xml to load. 

\return	Pointer to the new simulator.
**/
Simulator *Simulator::CreateSimulator(std::string strAnimatModule, CStdXml &oXml)
{
	Simulator *lpSim = NULL;
	IStdClassFactory *lpAnimatFactory=NULL;

	std::string strProjectPath, strProjectFile;
	std::string strExecutablePath, strExeFile;

try
{ 
    std::string strBuffer = Std_ExecutablePath();
	Std_SplitPathAndFile(strBuffer, strExecutablePath, strExeFile);

	if(Std_IsBlank(strAnimatModule))
		LoadAnimatModuleName(oXml, strAnimatModule);

	lpAnimatFactory = LoadClassFactory(strAnimatModule);

	//Now we need to get the simulation application itself. This ALWAYS
	//comes from the animat engine because the animat engine uses the
	//neural engine it is higher up the food chain and it decides the
	//actual simulator that needs to be used.
	lpSim = dynamic_cast<Simulator *>(lpAnimatFactory->CreateObject("Simulator", ""));
	if(!lpSim)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Simulator");

	lpSim->ProjectPath(strProjectPath);
	lpSim->ExecutablePath(strExecutablePath);

	if(lpAnimatFactory) 
		{delete lpAnimatFactory; lpAnimatFactory = NULL;}

	g_lpSimulator = lpSim;
	return lpSim;
}
catch(CStdErrorInfo oError)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Creates a simulator from passed in settings..

\author	dcofer
\date	3/28/2011

\param	strAnimatModule  	name of the dll module to load for the class factory.
\param	strProjectPath   	Full pathname of the string project file.
\param	strExecutablePath	Full pathname of the string executable file.

\return	Pointer to the new simulator.
**/
Simulator *Simulator::CreateSimulator(std::string strAnimatModule, std::string strProjectPath, std::string strExecutablePath)
{
	Simulator *lpSim = NULL;
	IStdClassFactory *lpAnimatFactory=NULL;


try
{ 
	lpAnimatFactory = LoadClassFactory(strAnimatModule);

	//Now we need to get the simulation application itself. This ALWAYS
	//comes from the animat engine because the animat engine uses the
	//neural engine it is higher up the food chain and it decides the
	//actual simulator that needs to be used.
	lpSim = dynamic_cast<Simulator *>(lpAnimatFactory->CreateObject("Simulator", ""));
	if(!lpSim)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Simulator");

	lpSim->ProjectPath(strProjectPath);
	lpSim->ExecutablePath(strExecutablePath);

	if(lpAnimatFactory) 
		{delete lpAnimatFactory; lpAnimatFactory = NULL;}

	g_lpSimulator = lpSim;
	return lpSim;
}
catch(CStdErrorInfo oError)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSim) delete lpSim;
	if(lpAnimatFactory) delete lpAnimatFactory;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


#pragma endregion
	
#pragma region FindMethods

/**
\brief	Searches for the first neural module factory with the matching name.

\author	dcofer
\date	3/28/2011

\param	strModuleName	Name of the string module to find. 
\param	bThrowError  	true to throw error if there is a problem. 

\return	null if it fails and bThrowError=false, else the found neural module factory.
\exception If bThrowError=True and no factory is found it throws an exception.
**/
IStdClassFactory *Simulator::FindNeuralModuleFactory(std::string strModuleName, bool bThrowError)
{
	IStdClassFactory *lpFactory = NULL;
	CStdMap<std::string, IStdClassFactory *>::iterator oPos;
	oPos = m_aryNeuralModuleFactories.find(Std_CheckString(strModuleName));

	if(oPos != m_aryNeuralModuleFactories.end())
		lpFactory =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lModuleNameNotFound, Al_Err_strModuleNameNotFound, "ModuleName", strModuleName);

	return lpFactory;
}

/**
\brief	Searches for the first organism with the specified ID.

\details This searches only the organisms. It does not include structures.

\author	dcofer
\date	3/28/2011

\param	strOrganismID	GUID ID for the organism. 
\param	bThrowError  	true to throw error if no organism is found. 

\return	null if it fails, else the found organism.
\exception If bThrowError=True and no organism is found it throws an exception.
**/
Organism *Simulator::FindOrganism(std::string strOrganismID, bool bThrowError)
{
	Organism *lpOrganism = NULL;
	CStdPtrMap<std::string, Organism>::iterator oPos;
	oPos = m_aryOrganisms.find(Std_CheckString(strOrganismID));

	if(oPos != m_aryOrganisms.end())
		lpOrganism =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lOrganismIDNotFound, Al_Err_strOrganismIDNotFound, "OrganismID", strOrganismID);

	return lpOrganism;
}



/**
\brief	Searches for the first structure with the specified ID.

\details This searches only the structures. It does not include organisms.

\author	dcofer
\date	3/28/2011

\param	strStructureID	GUID ID for the structure. 
\param	bThrowError  	true to throw error if no structure is found. 

\return	null if it fails, else the found structure.
\exception If bThrowError=True and no structure is found it throws an exception.
**/
Structure *Simulator::FindStructure(std::string strStructureID, bool bThrowError)
{
	Structure *lpStructure = NULL;
	CStdPtrMap<std::string, Structure>::iterator oPos;
	oPos = m_aryStructures.find(Std_CheckString(strStructureID));

	if(oPos != m_aryStructures.end())
		lpStructure =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lStructureIDNotFound, Al_Err_strStructureIDNotFound, "StructureID", strStructureID);

	return lpStructure;
}

/**
\brief	Searches for the first odor type with the specified ID.

\author	dcofer
\date	3/28/2011

\param	strOdorID	GUID ID for the odor type. 
\param	bThrowError  	true to throw error if no odor type is found. 

\return	null if it fails, else the found odor type.
\exception If bThrowError=True and no odor type is found it throws an exception.
**/
OdorType *Simulator::FindOdorType(std::string strOdorID, bool bThrowError)
{
	OdorType *lpOdorType = NULL;
	CStdPtrMap<std::string, OdorType>::iterator oPos;
	oPos = m_aryOdorTypes.find(Std_CheckString(strOdorID));

	if(oPos != m_aryOdorTypes.end())
		lpOdorType =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lOdorIDNotFound, Al_Err_strOdorIDNotFound, "OdorID", strOdorID);

	return lpOdorType;
}


/**
\brief	Searches for the first structure with the specified ID.

\details This searches both the structures and organisms.

\author	dcofer
\date	3/28/2011

\param	strStructureID	GUID ID for the structure. 
\param	bThrowError  	true to throw error if no structure is found. 

\return	null if it fails, else the found structure.
\exception If bThrowError=True and no structure is found it throws an exception.
**/
Structure *Simulator::FindStructureFromAll(std::string strStructureID, bool bThrowError)
{
	Structure *lpStructure = NULL;
	CStdPtrMap<std::string, Structure>::iterator oPos;
	oPos = m_aryAllStructures.find(Std_CheckString(strStructureID));

	if(oPos != m_aryAllStructures.end())
		lpStructure =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lStructureIDNotFound, Al_Err_strStructureIDNotFound, "StructureID", strStructureID);

	return lpStructure;
}

/**
\brief	Finds a joint with the specified ID in the specified structure.

\details Returns a pointer to the joint with the specified ID that is inside the specified structure if
one is found. If either the structure or joint are not found then it will either throw an
exception or return NULL depending on the value of bThrowError. This uses the
FindStructureFromAll method to search both the "static" structures and the organisms for the
specified StructureID.

\author	dcofer
\date	3/28/2011

\param	strStructureID	GUID ID for the structure. 
\param	strJointID	  	GUID ID for the joint. 
\param	bThrowError   	true to throw error if no structure is found. 

\return	null if it fails, else the found structure.

\exception	If	bThrowError=True and no structure or joint is found it throws an exception. 
**/
Joint *Simulator::FindJoint(std::string strStructureID, std::string strJointID, bool bThrowError)
{
	Structure *lpStructure = FindStructureFromAll(strStructureID, bThrowError);

	if(lpStructure)
		return lpStructure->FindJoint(strJointID, bThrowError);
	else
		return NULL;
}

/**
\brief	Finds a rigid body with the specified ID in the specified structure.

\details Returns a pointer to the body with the specified ID that is inside the specified structure if
one is found. If either the structure or body are not found then it will either throw an
exception or return NULL depending on the value of bThrowError. This uses the
FindStructureFromAll method to search both the "static" structures and the organisms for the
specified StructureID.

\author	dcofer
\date	3/28/2011

\param	strStructureID	GUID ID for the structure. 
\param	strBodyID	  	GUID ID for the bodu. 
\param	bThrowError   	true to throw error if no structure is found. 

\return	null if it fails, else the found structure.
\exception	If	bThrowError=True and no structure or body is found it throws an exception. 
**/
RigidBody *Simulator::FindRigidBody(std::string strStructureID, std::string strBodyID, bool bThrowError)
{
	Structure *lpStructure = FindStructureFromAll(strStructureID, bThrowError);

	if(lpStructure)
		return lpStructure->FindRigidBody(strBodyID, bThrowError);
	else
		return NULL;
}

/**
\brief	Searches for the object with the specified ID.

\details This loops through the list of all objects (m_aryObjectList) in the simulation
and tries to find one with a matching ID.

\author	dcofer
\date	3/28/2011

\param	strID	   	GUID ID for the object to find. 
\param	bThrowError	true to throw error nothing is found. 

\return	null if it fails, else the found by identifier.
\exception	If	bThrowError=True and no objects is found it throws an exception. 
**/
AnimatBase *Simulator::FindByID(std::string strID, bool bThrowError)
{
	AnimatBase *lpFind = NULL;
	CStdMap<std::string, AnimatBase *>::iterator oPos;
	oPos = m_aryObjectList.find(Std_CheckString(strID));

	if(oPos != m_aryObjectList.end())
		lpFind =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lIDNotFound, Al_Err_strIDNotFound, "ID", strID);

	return lpFind;
}

/**
\brief	Searches for the food source that is closest to the specified mouth position and not beyond a given radius.

\author	dcofer
\date	3/28/2011

\param [in,out]	oMouthPos	The mouth position. 
\param	fltMinRadius	 	The minimum radius. 

\return	null if it fails, else the found closest food source.
**/
void Simulator::FindClosestFoodSources(CStdFPoint &oMouthPos, float fltMinRadius, CStdArray<RigidBody *> &arySources, CStdArray<float> &aryDistances)
{
	RigidBody *lpFood = NULL, *lpMinFood = NULL;
	float fltDist=0, fltMinDist=0;
	int iCount = m_aryFoodSources.GetSize();

    arySources.RemoveAll();
    aryDistances.RemoveAll();

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpFood = m_aryFoodSources[iIndex];
		CStdFPoint oPos = lpFood->GetCurrentPosition();
		fltDist = Std_CalculateDistance(oMouthPos, oPos);

		if( (fltDist <= fltMinRadius) && ((fltDist < fltMinDist) || !lpMinFood))
		{
            arySources.Add(lpFood);
            aryDistances.Add(fltDist);
		}
	}
}

#pragma endregion

#pragma region AddRemoveMethods

/**
\brief	Adds an object to the list of all simulation objects. 

\author	dcofer
\date	3/28/2011

\param [in,out]	lpItem	Pointer to the item to add. 
**/
void Simulator::AddToObjectList(AnimatBase *lpItem)
{
	if(FindByID(lpItem->ID(), false) != NULL)
		THROW_PARAM_ERROR(Al_Err_lDuplicateAddOfObject, Al_Err_strDuplicateAddOfObject, "ID", lpItem->ID());

	m_aryObjectList.Add(lpItem->ID(), lpItem);
}

/**
\brief	Removes an object from the list of all simulation objects.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpItem	Pointer to the item to remove. 
**/
void Simulator::RemoveFromObjectList(AnimatBase *lpItem)
{
	if(FindByID(lpItem->ID(), false))
		m_aryObjectList.Remove(lpItem->ID());
}

/**
\brief	Adds a neural module factory to the list of factories for this simulation.

\details This method ensures that the module is only added to the list once.

\author	dcofer
\date	3/28/2011

\param	strModuleName   	Name of the dll module. 
\param [in,out]	lpModule	Pointer to thea module to add. 
**/
void Simulator::AddNeuralModuleFactory(std::string strModuleName, NeuralModule *lpModule)
{
	if(!lpModule->ClassFactory())
		THROW_PARAM_ERROR(Al_Err_lModuleClassFactoryNotDefined, Al_Err_strModuleClassFactoryNotDefined, "ModuleName", strModuleName);

	if(!FindNeuralModuleFactory(strModuleName, false))
		m_aryNeuralModuleFactories.Add(Std_CheckString(strModuleName), lpModule->ClassFactory());
}


int Simulator::FindAdapterListIndex(CStdArray<Adapter *> aryAdapters, std::string strID, bool bThrowError)
{
	int iCount = aryAdapters.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(aryAdapters[iIdx]->ID() == strID)
			return iIdx;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lAdapterIDNotFound, Al_Err_strAdapterIDNotFound, "ID", strID);

	return -1;
}

/**
\brief	Attaches a source adapter.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpStructure	Pointer to a structure. 
\param [in,out]	lpAdapter  	Pointer to an adapter. 
**/
void Simulator::AttachSourceAdapter(Structure *lpStructure, Adapter *lpAdapter)
{
	std::string strModuleName = Std_CheckString(lpAdapter->SourceModule());

	//If no neural module name is specified then this must be getting attached to the physics engine.
	//Otherwise it gets attached to the specified neural module in an organism
	if(strModuleName == "" || strModuleName == "ANIMATLAB")
	{
		if(FindAdapterListIndex(m_arySourcePhysicsAdapters, lpAdapter->ID(), false) == -1)
			m_arySourcePhysicsAdapters.Add(lpAdapter);
	}
	else
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
		if(!lpOrganism)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

		NeuralModule *lpModule = lpOrganism->GetNervousSystem()->FindNeuralModule(strModuleName);
		lpModule->AttachSourceAdapter(lpAdapter);
	}
}

/**
\brief	Removes a source adapter.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpStructure	Pointer to a structure. 
\param [in,out]	lpAdapter  	Pointer to an adapter. 
**/
void Simulator::RemoveSourceAdapter(Structure *lpStructure, Adapter *lpAdapter)
{
	std::string strModuleName = Std_CheckString(lpAdapter->SourceModule());

	//If no neural module name is specified then this must be getting attached to the physics engine.
	//Otherwise it gets attached to the specified neural module in an organism
	if(strModuleName == "" || strModuleName == "ANIMATLAB")
	{
		int iIdx = FindAdapterListIndex(m_arySourcePhysicsAdapters, lpAdapter->ID(), false);
		if(iIdx > -1)
			m_arySourcePhysicsAdapters.RemoveAt(iIdx);
	}
	else
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
		if(!lpOrganism)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

		NeuralModule *lpModule = lpOrganism->GetNervousSystem()->FindNeuralModule(strModuleName);
		lpModule->RemoveSourceAdapter(lpAdapter);
	}
}

/**
\brief	Attaches a target adapter.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpStructure	Pointer to a structure. 
\param [in,out]	lpAdapter  	Pointer to an adapter. 
**/
void Simulator::AttachTargetAdapter(Structure *lpStructure, Adapter *lpAdapter)
{
	std::string strModuleName = Std_CheckString(lpAdapter->TargetModule());

	//If no neural module name is specified then this must be getting attached to the physics engine.
	//Otherwise it gets attached to the specified neural module in an organism
	if(strModuleName == "" || strModuleName == "ANIMATLAB")
	{
		if(FindAdapterListIndex(m_aryTargetPhysicsAdapters, lpAdapter->ID(), false) == -1)
		{
			m_aryTargetPhysicsAdapters.Add(lpAdapter);
			m_iTargetAdapterCount = m_aryTargetPhysicsAdapters.GetSize();
		}
	}
	else
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
		if(!lpOrganism)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

		NeuralModule *lpModule = lpOrganism->GetNervousSystem()->FindNeuralModule(strModuleName);
		lpModule->AttachTargetAdapter(lpAdapter);
	}
}

/**
\brief	Remoaves a target adapter.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpStructure	Pointer to a structure. 
\param [in,out]	lpAdapter  	Pointer to an adapter. 
**/
void Simulator::RemoveTargetAdapter(Structure *lpStructure, Adapter *lpAdapter)
{
	std::string strModuleName = Std_CheckString(lpAdapter->TargetModule());

	//If no neural module name is specified then this must be getting attached to the physics engine.
	//Otherwise it gets attached to the specified neural module in an organism
	if(strModuleName == "" || strModuleName == "ANIMATLAB")
	{
		int iIdx = FindAdapterListIndex(m_aryTargetPhysicsAdapters, lpAdapter->ID(), false);
		if(iIdx > -1)
		{
			m_aryTargetPhysicsAdapters.RemoveAt(iIdx);
			m_iTargetAdapterCount = m_aryTargetPhysicsAdapters.GetSize();
		}
	}
	else
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
		if(!lpOrganism)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

		NeuralModule *lpModule = lpOrganism->GetNervousSystem()->FindNeuralModule(strModuleName);
		lpModule->RemoveTargetAdapter(lpAdapter);
	}
}


/**
\brief	Adds a food source to the list of all simulation food sources. 

\author	dcofer
\date	3/28/2011

\param [in,out]	lpFood	Pointer to a food. 
**/
void Simulator::AddFoodSource(RigidBody *lpFood)
{
	int iIndex = FindFoodSourceIndex(lpFood);
	if(iIndex < 0)
		m_aryFoodSources.Add(lpFood);
}

/**
\brief	Removes a food source from the list of all simulation food sources.

\author	dcofer
\date	6/3/2012

\param [in,out]	lpFood	Pointer to a food. 
**/
void Simulator::RemoveFoodSource(RigidBody *lpFood)
{
	int iIndex = FindFoodSourceIndex(lpFood);
	if(iIndex >= 0)
		m_aryFoodSources.RemoveAt(iIndex);
}

/**
\brief	Searches for the index of the food source.

\author	dcofer
\date	6/3/2012

\param [in,out]	lpFood	The pointer to a food source.

\return	The found food source index, or -1.
**/
int Simulator::FindFoodSourceIndex(RigidBody *lpFood)
{
	int iCount = m_aryFoodSources.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(m_aryFoodSources[iIdx] == lpFood)
			return iIdx;

	return -1;
}

void Simulator::AddOrganism(Organism *lpOrganism)
{
	if(!lpOrganism)
		THROW_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined);

	try
	{
			m_aryAllStructures.Add(lpOrganism->ID(), lpOrganism);
			m_aryOrganisms.Add(lpOrganism->ID(), lpOrganism);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate Organism Key: " + lpOrganism->ID(); 
		RELAY_ERROR(oError);
	}
}

/**
\brief	Adds an organism defined by an xml data packet. 

\details This is primarily used by the GUI to create a new organism within the simulation.

\author	dcofer
\date	3/28/2011

\param	strXml	The xml packet to load. 
**/
void Simulator::AddOrganism(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Organism");

	Organism *lpOrg = LoadOrganism(oXml);
	lpOrg->Create();
}

/**
\brief	Removes an organism by its ID.

\details This is primarily used by the GUI to remove an organism from the simulation.

\author	dcofer
\date	3/28/2011

\param	strID	   	GUID ID of the organism to remove. 
\param	bThrowError	true to throw error if the organism is not found. 
**/
void Simulator::RemoveOrganism(std::string strID, bool bThrowError)
{
	m_aryAllStructures.Remove(strID);
	m_aryOrganisms.Remove(strID);
}

void Simulator::AddStructure(Structure *lpStructure)
{
	if(!lpStructure)
		THROW_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined);

	try
	{
			m_aryAllStructures.Add(lpStructure->ID(), lpStructure);
			m_aryStructures.Add(lpStructure->ID(), lpStructure);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate structure Key: " + lpStructure->ID(); 
		RELAY_ERROR(oError);
	}
}

/**
\brief	Adds an structure defined by an xml data packet. 

\details This is primarily used by the GUI to create a new organism within the simulation.

\author	dcofer
\date	3/28/2011

\param	strXml	The xml packet to load. 
**/
void Simulator::AddStructure(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Structure");

	Structure *lpStruct = LoadStructure(oXml);
	lpStruct->Create();
}

/**
\brief	Removes a structure by its ID.

\details This is primarily used by the GUI to remove a structure from the simulation.


\author	dcofer
\date	3/28/2011

\param	strID	   	Identifier for the structure. 
\param	bThrowError	true to throw error if the structure is not found. 
**/
void Simulator::RemoveStructure(std::string strID, bool bThrowError)
{
	m_aryAllStructures.Remove(strID);
	m_aryStructures.Remove(strID);
}

/**
\brief	Adds an odor type. 

\author	dcofer
\date	3/28/2011

\param [in,out]	lpOdorType	Pointer to an odor type. 
**/
void Simulator::AddOdorType(OdorType *lpOdorType)
{
	if(!lpOdorType)
		THROW_ERROR(Al_Err_lOdorNotDefined, Al_Err_strOdorNotDefined);

	try
	{
			m_aryOdorTypes.Add(lpOdorType->ID(), lpOdorType);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate odor type Key: " + lpOdorType->ID(); 
		RELAY_ERROR(oError);
	}
}

void Simulator::AddOdorType(std::string strXml, bool bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("OdorType");

	OdorType *lpType = LoadOdorType(oXml);

	if(!bDoNotInit)
		lpType->Initialize();
}

void Simulator::RemoveOdorType(std::string strID, bool bThrowError)
{
	m_aryOdorTypes.Remove(strID);
}

void  Simulator::IncrementPhysicsBodyCount()
{
	m_iPhysicsBodyCount++;
}

#pragma endregion

#pragma region DataAccesMethods

float *Simulator::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	if(strType == "TIME")
		lpData = &m_fltTime;
	else if(strType == "PHYSICSTIMESTEP")
		lpData = &m_fltPhysicsTimeStep;
	else if(strType == "SIMULATIONREALTIMETOSTEP")
		lpData = &m_fltSimulationRealTimeToStep;
	else if(strType == "PLAYBACKADDITIONREALTIMETOSTEP")
		lpData = &m_fltPlaybackAdditionRealTimeToStep;
	else if(strType == "TOTALREALTIMEFORSTEP")
		lpData = &m_fltTotalRealTimeForStep;
	else if(strType == "ACTUALFRAMERATE")
		lpData = &m_fltActualFrameRate;
	else if(strType == "REALTIME")
		lpData = &m_fltRealTime;
	else if(strType == "TOTALREALTIMEFORSTEPSMOOTHED")
		lpData = &m_fltTotalRealTimeForStepSmooth;
	else if(strType == "PHYSICSREALTIMEFORSTEP")
		lpData = &m_fltPrevPhysicsStepTime;
	else if(strType == "NEURALREALTIMEFORSTEP")
		lpData = &m_fltTotalNeuralStepTime;
	else if(strType == "EXTERNALSTIMULIREALTIMEFORSTEP")
		lpData = &m_fltExternalStimuliStepTime;
	else if(strType == "DATACHARTREALTIMEFORSTEP")
		lpData = &m_fltDataChartStepTime;
	else if(strType == "SIMRECORDERREALTIMEFORSTEP")
		lpData = &m_fltSimRecorderStepTime;
	else if(strType == "REMAININGSTEPTIME")
		lpData = &m_fltRemainingStepTime;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);

	return lpData;
}

bool Simulator::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "VISUALSELECTIONMODE")
	{
		VisualSelectionMode(atoi(strValue.c_str()));
		return true;
	}
	else if(strType == "ADDBODIESMODE")
	{
		AddBodiesMode(Std_ToBool(strValue));
		return true;
	}
	else if(strType == "DISTANCEUNITS")
	{
		DistanceUnits(strValue);
		return true;
	}
	else if(strType == "MASSUNITS")
	{
		MassUnits(strValue);
		return true;
	}
	else if(strType == "GRAVITY")
	{
		Gravity((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "PHYSICSTIMESTEP")
	{
		PhysicsTimeStep((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "SIMULATEHYDRODYNAMICS")
	{
		SimulateHydrodynamics(Std_ToBool(strValue));
		return true;
	}
	else if(strType == "AUTOGENERATERANDOMSEED")
	{
		AutoGenerateRandomSeed(Std_ToBool(strValue));
		return true;
	}
	else if(strType == "MANUALRANDOMSEED")
	{
		ManualRandomSeed((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "FRAMERATE")
	{
		DesiredFrameRate(atoi(strValue.c_str()));
		return true;
	}
	else if(strType == "FORCEFASTMOVING")
	{
		ForceFastMoving(atoi(strValue.c_str()));
		return true;
	}
	else if(strType == "MOUSESPRINGSTIFFNESS")
	{
		MouseSpringStiffness((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "MOUSESPRINGDAMPING")
	{
		MouseSpringDamping((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "LINEARCOMPLIANCE")
	{
		LinearCompliance((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "ANGULARCOMPLIANCE")
	{
		AngularCompliance((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "LINEARDAMPING")
	{
		LinearDamping((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "ANGULARDAMPING")
	{
		AngularDamping((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "LINEARKINETICLOSS")
	{
		LinearKineticLoss((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "ANGULARKINETICLOSS")
	{
		AngularKineticLoss((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "SETENDSIMTIME")
	{
		SetEndSimTime(Std_ToBool(strValue));
		return true;
	}
	else if(strType == "ENDSIMTIME")
	{
		EndSimTime((float) atof(strValue.c_str()));
		return true;
	}
	else if(strType == "RECFIELDSELRADIUS")
	{
		RecFieldSelRadius((float) atof(strValue.c_str()));
		return true;
	}
	else if(strDataType == "BACKGROUNDCOLOR")
	{
		BackgroundColor(strValue);
		return true;
	}
	else if(strDataType == "ALPHATHRESHOLD")
	{
		AlphaThreshold((float) atof(strValue.c_str()));
		return true;
	}
	else if(strDataType == "BACKGROUNDCOLOR.RED")
	{
		float aryVal[4] = {(float) atof(strValue.c_str()), m_vBackgroundColor.g(), m_vBackgroundColor.b(), m_vBackgroundColor.a()};
		BackgroundColor(aryVal);
		return true;
	}
	else if(strDataType == "BACKGROUNDCOLOR.GREEN")
	{
		float aryVal[4] = {m_vBackgroundColor.r(), (float) atof(strValue.c_str()), m_vBackgroundColor.b(), m_vBackgroundColor.a()};
		BackgroundColor(aryVal);
		return true;
	}
	else if(strDataType == "BACKGROUNDCOLOR.BLUE")
	{
		float aryVal[4] = {m_vBackgroundColor.r(), m_vBackgroundColor.g(), (float) atof(strValue.c_str()), m_vBackgroundColor.a()};
		BackgroundColor(aryVal);
		return true;
	}
	else if(strDataType == "BACKGROUNDCOLOR.ALPHA")
	{
		float aryVal[4] = {m_vBackgroundColor.r(), m_vBackgroundColor.g(), m_vBackgroundColor.b(), (float) atof(strValue.c_str())};
		BackgroundColor(aryVal);
		return true;
	}
	else if(strDataType == "TIMESTEPMODIFIED")
	{
		NotifyTimeStepModified();
		return true;
	}
	else if(strDataType == "PLAYBACKCONTROLMODE")
	{
		PlaybackControlMode(atoi(strValue.c_str()));
		return true;
	}
	else if(strDataType == "PRESETPLAYBACKTIMESTEP")
	{
		PresetPlaybackTimeStep((float) atof(strValue.c_str()));
		return true;
	}
	else if(strDataType == "STABILITYSCALE")
	{
		StabilityScale((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Simulator::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("VisualSelectionMode");
	aryTypes.Add("Integer");

	aryNames.Add("AddBodiesMode");
	aryTypes.Add("Boolean");

	aryNames.Add("DistanceUnits");
	aryTypes.Add("String");

	aryNames.Add("MassUnits");
	aryTypes.Add("String");

	aryNames.Add("Gravity");
	aryTypes.Add("Float");

	aryNames.Add("PhysicsTimeStep");
	aryTypes.Add("Float");

	aryNames.Add("SimulateHydrodynamics");
	aryTypes.Add("Boolean");

	aryNames.Add("AutoGenerateRandomSeed");
	aryTypes.Add("Boolean");

	aryNames.Add("ManualRandomSeed");
	aryTypes.Add("Integer");

	aryNames.Add("FrameRate");
	aryTypes.Add("Integer");

	aryNames.Add("ForceFastMoving");
	aryTypes.Add("Integer");

	aryNames.Add("MouseSpringStiffness");
	aryTypes.Add("Float");

	aryNames.Add("MouseSpringDamping");
	aryTypes.Add("Float");

	aryNames.Add("CalcCriticalSimParams");
	aryTypes.Add("Boolean");

	aryNames.Add("LinearCompliance");
	aryTypes.Add("Float");

	aryNames.Add("AngularCompliance");
	aryTypes.Add("Float");

	aryNames.Add("LinearDamping");
	aryTypes.Add("Float");

	aryNames.Add("AngularDamping");
	aryTypes.Add("Float");

	aryNames.Add("LinearKineticLoss");
	aryTypes.Add("Float");

	aryNames.Add("AngularKineticLoss");
	aryTypes.Add("Float");

	aryNames.Add("SetEndSimTime");
	aryTypes.Add("Boolean");

	aryNames.Add("EndSimTime");
	aryTypes.Add("Float");

	aryNames.Add("RecFieldSelRadius");
	aryTypes.Add("Float");

	aryNames.Add("BackgroundColor");
	aryTypes.Add("Xml");

	aryNames.Add("BackgroundColor.Red");
	aryTypes.Add("Float");

	aryNames.Add("BackgroundColor.Blue");
	aryTypes.Add("Float");

	aryNames.Add("BackgroundColor.Green");
	aryTypes.Add("Float");

	aryNames.Add("BackgroundColor.Alpha");
	aryTypes.Add("Float");

	aryNames.Add("PlaybackControlMode");
	aryTypes.Add("Integer");

	aryNames.Add("PresetPlaybackTimeStep");
	aryTypes.Add("Float");

	aryNames.Add("StabilityScale");
	aryTypes.Add("Float");

	aryNames.Add("AlphaThreshold");
	aryTypes.Add("Float");
}

bool Simulator::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "STIMULUS")
		return m_oExternalStimuliMgr.AddStimulus(strXml);
	else if(strType == "DATACHART")
		return m_oDataChartMgr.AddDataChart(strXml);
	else if(strType == "LIGHT")
		return m_oLightMgr.AddItem(strItemType, strXml, bThrowError);
	else if(strType == "STRUCTURE")
	{
		AddStructure(strXml);
		return true;
	}
	else if(strType == "ORGANISM")
	{
		AddOrganism(strXml);
		return true;
	}
	else if(strType == "HUDITEM")
	{
		if(m_lpWinMgr)
			return m_lpWinMgr->AddItem(strType, strXml, true);
		return false;
	}
	else if(strType == "ODORTYPE")
	{
		AddOdorType(strXml, bDoNotInit);
		return true;
	}
	else if(strType == "MATERIALTYPE" || strType == "MATERIALPAIR")
		return m_oMaterialMgr.AddItem(strItemType, strXml, bThrowError, bDoNotInit);

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool Simulator::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "STIMULUS")
		return m_oExternalStimuliMgr.RemoveStimulus(strID);
	else if(strType == "DATACHART")
		return m_oDataChartMgr.RemoveDataChart(strID);
	else if(strType == "LIGHT")
		return m_oLightMgr.RemoveItem(strItemType, strID, bThrowError);
	else if(strType == "STRUCTURE")
	{
		RemoveStructure(strID);
		return true;
	}
	else if(strType == "ORGANISM")
	{
		RemoveOrganism(strID);
		return true;
	}
	else if(strType == "ODORTYPE")
	{
		RemoveOdorType(strID);
		return true;
	}
	else if(strType == "MATERIALTYPE" || strType == "MATERIALPAIR")
		return m_oMaterialMgr.RemoveItem(strItemType, strID, bThrowError);
	else if(strType == "SCRIPTEDSIMWINDOW")
		return true; //Nothing to remove here. It is actually removed elsewhere in the simulationwindowmgr

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

#pragma endregion

#pragma region RecorderMethods

/**
\brief	Enables the video playback.

\author	dcofer
\date	3/28/2011

\param	strKeyFrameID	GUID ID for the string key frame. 
**/
void Simulator::EnableVideoPlayback(std::string strKeyFrameID)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback && m_lpVideoPlayback->ID() == strKeyFrameID)
		return;

	KeyFrame *lpFrame = dynamic_cast<KeyFrame *>(m_lpSimRecorder->Find(strKeyFrameID));
	lpFrame->EnableVideoPlayback();
}

/**
\brief	Disables the video playback.

\author	dcofer
\date	3/28/2011
**/
void Simulator::DisableVideoPlayback()
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->DisableVideoPlayback();
}

/**
\brief	Starts a video playback.

\author	dcofer
\date	3/28/2011
**/
void Simulator::StartVideoPlayback()
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->StartVideoPlayback();
}

/**
\brief	Stop video playback.

\author	dcofer
\date	3/28/2011
**/
void Simulator::StopVideoPlayback()
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->StopVideoPlayback();
}

/**
\brief	Step video playback.

\author	dcofer
\date	3/28/2011

\param	iFrameCount	Number of frames. 
**/
void Simulator::StepVideoPlayback(int iFrameCount)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->StepVideoPlayback( iFrameCount);
}

/**
\brief	Saves a video.

\author	dcofer
\date	3/28/2011

\param	strPath	Full pathname of the string file. 
**/
void Simulator::SaveVideo(std::string strPath)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(m_lpVideoPlayback)
		m_lpVideoPlayback->SaveVideo(strPath);
}

/**
\brief	Adds a key frame.

\author	dcofer
\date	3/28/2011

\param	strType	Type of the string. 
\param	lStart 	The start time slice. 
\param	lEnd   	The end time slice. 

\return	.
**/
std::string Simulator::AddKeyFrame(std::string strType, long lStart, long lEnd)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	KeyFrame *lpFrame = m_lpSimRecorder->Add(strType, lStart, lEnd);
	return lpFrame->ID();
}

/**
\brief	Removes the key frame described by strID.

\author	dcofer
\date	3/28/2011

\param	strID	GUID ID for the key frame. 
**/
void Simulator::RemoveKeyFrame(std::string strID)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	m_lpSimRecorder->Remove(strID);
}

/**
\brief	Move key frame.

\author	dcofer
\date	3/28/2011

\param	strID 	GUID ID for the key frame. 
\param	lStart 	The start time slice. 
\param	lEnd   	The end time slice. 

\return	.
**/
std::string Simulator::MoveKeyFrame(std::string strID, long lStart, long lEnd)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	KeyFrame *lpFrame = dynamic_cast<KeyFrame *>(m_lpSimRecorder->Find(strID));

	//If neither the start or end slice is changed then jump out of here.
	if(lpFrame->StartSlice() == lStart && lpFrame->EndSlice() == lEnd)
		return lpFrame->ID();

	//unsigned char iType = lpFrame->Type();
	m_lpSimRecorder->Remove(strID);
	lpFrame = m_lpSimRecorder->Add(lpFrame->Type(), lStart, lEnd);
	return lpFrame->ID();
}

/**
\brief	Move simulation to key frame.

\author	dcofer
\date	3/28/2011

\param	strKeyFrameID	GUID ID for the key frame. 
**/
void Simulator::MoveSimulationToKeyFrame(std::string strKeyFrameID)
{
	if(!m_lpSimRecorder)
		THROW_ERROR(Al_Err_lNoRecorderDefined, Al_Err_strNoRecorderDefined);

	if(!Std_IsBlank(strKeyFrameID))
	{
		KeyFrame *lpFrame = dynamic_cast<KeyFrame *>(m_lpSimRecorder->Find(strKeyFrameID));
		lpFrame->MakeCurrentFrame();
	}
	else if(m_lpSimStopPoint)
		m_lpSimStopPoint->MakeCurrentFrame();
}

/**
\brief	Calculates the snapshot byte size.

\author	dcofer
\date	3/28/2011

\return	The calculated snapshot byte size.
**/
long Simulator::CalculateSnapshotByteSize()
{
	long lByteSize = 0;
	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lByteSize += lpStructure->CalculateSnapshotByteSize();
	}

	return lByteSize;
}

/**
\brief	Saves a key frame snapshot.

\author	dcofer
\date	3/28/2011

\param [in,out]	aryBytes	Array of bytes for the snapshot. 
\param [in,out]	lIndex  	Index into the byte array. 
**/
void Simulator::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->SaveKeyFrameSnapshot(aryBytes, lIndex);
	}
}

/**
\brief	Loads a key frame snapshot.

\author	dcofer
\date	3/28/2011

\param [in,out]	aryBytes	Array of bytes for the snapshot. 
\param [in,out]	lIndex  	Index into the byte array. 
**/
void Simulator::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->LoadKeyFrameSnapshot(aryBytes, lIndex);
	}
}

/**
\brief	Record video frame.

\author	dcofer
\date	3/28/2011
**/
void Simulator::RecordVideoFrame()
{
}

#pragma endregion
			
#pragma region CollisionMethods

/**
\brief	Enables collision between the past-in pairs of objects.

\details This method enables collision responses between the rigid body pairs that are past in.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpStruct		  	Pointer to a structure. 
\param [in,out]	m_aryCollisionList	List of CollisionPair objects. 
**/
void Simulator::EnableCollisions(Structure *lpStruct, CStdPtrArray<CollisionPair> &m_aryCollisionList)
{
	//Now lets disable any collisions that have been added to the exclusion list.
	int iCount = m_aryCollisionList.GetSize();
	CollisionPair *lpPair = NULL;
	RigidBody *lpPart1=NULL, *lpPart2=NULL;

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair =  m_aryCollisionList[iIndex];
		lpPart1 = lpStruct->FindRigidBody(lpPair->m_strPart1ID);
		lpPart2 = lpStruct->FindRigidBody(lpPair->m_strPart2ID);
		
		lpPart1->EnableCollision(lpPart2);
	}	
}

void Simulator::EnableCollision(RigidBody *lpBody)
{
	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->EnableCollision(lpBody);
	}
}

/**
\brief	Disables collision between the past-in pairs of objects.

\details This method enables collision responses between the pairs of rigid body objects that were past in.

\author	dcofer
\date	3/28/2011

\param [in,out]	lpStruct		  	Pointer to a structure. 
\param [in,out]	m_aryCollisionList	List of CollisionPair objects. 
**/
void Simulator::DisableCollisions(Structure *lpStruct, CStdPtrArray<CollisionPair> &m_aryCollisionList)
{
	//Now lets disable any collisions that have been added to the exclusion list.
	int iCount = m_aryCollisionList.GetSize();
	CollisionPair *lpPair = NULL;
	RigidBody *lpPart1=NULL, *lpPart2=NULL;

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair =  m_aryCollisionList[iIndex];
		lpPart1 = lpStruct->FindRigidBody(lpPair->m_strPart1ID);
		lpPart2 = lpStruct->FindRigidBody(lpPair->m_strPart2ID);
		
		lpPart1->DisableCollision(lpPart2);
	}	
}


void Simulator::DisableCollision(RigidBody *lpBody)
{
	CStdMap<std::string, Structure *>::iterator oPos;
	Structure *lpStructure = NULL;
	for(oPos=m_aryAllStructures.begin(); oPos!=m_aryAllStructures.end(); ++oPos)
	{
		lpStructure = oPos->second;
		lpStructure->DisableCollision(lpBody);
	}
}

#pragma endregion

#pragma region UnitScaleMethods

/**
\brief	Convert the string ID of the distance units to a conversion factor.

\details This determines the scaling factor that is used within the simulation to scale the arbitrary units
to the distance units chosen by the user.

\author	dcofer
\date	3/28/2011

\param	strUnits	The string ID the distance units. 

\return	conversion factor for the units chosen.
**/
float Simulator::ConvertDistanceUnits(std::string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOMETERS" || strUnits == "KILOMETER")
		return (float) 1000;

	if(strUnits == "CENTAMETERS" || strUnits == "CENTAMETER")
		return (float) 100;

	if(strUnits == "DECAMETERS" || strUnits == "DECAMETER")
		return (float) 10;

	if(strUnits == "METERS" || strUnits == "METER")
		return (float) 1;

	if(strUnits == "DECIMETERS" || strUnits == "DECIMETER")
		return (float) 0.1;

	if(strUnits == "CENTIMETERS" || strUnits == "CENTIMETER")
		return (float) 0.01;

	if(strUnits == "MILLIMETERS" || strUnits == "MILLIMETER")
		return (float) 0.001;

	THROW_PARAM_ERROR(Al_Err_lInvalidDistanceUnits, Al_Err_strInvalidDistanceUnits, "DistanceUnits", strUnits);

	return (float) 1;
}

/**
\brief	Convert the string ID of the distance units to a conversion factor for the denominator of the distance units.

\details This determines the scaling factor that is used within the simulation to scale the arbitrary units
to the denominator distance units chosen by the user.

\author	dcofer
\date	3/28/2011

\param	strUnits	The string units. 

\return	.
**/
float Simulator::ConvertDenominatorDistanceUnits(std::string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOMETERS" || strUnits == "KILOMETER")
		return (float) 1;

	if(strUnits == "CENTAMETERS" || strUnits == "CENTAMETER")
		return (float) 1;

	if(strUnits == "DECAMETERS" || strUnits == "DECAMETER")  //1 Unit = 10 m
		return (float) 10;

	if(strUnits == "METERS" || strUnits == "METER")
		return (float) 1;

	if(strUnits == "DECIMETERS" || strUnits == "DECIMETER")  //1 Unit = 10 cm 
		return (float) 10;

	if(strUnits == "CENTIMETERS" || strUnits == "CENTIMETER")
		return (float) 1;

	if(strUnits == "MILLIMETERS" || strUnits == "MILLIMETER")
		return (float) 1;

	THROW_PARAM_ERROR(Al_Err_lInvalidDistanceUnits, Al_Err_strInvalidDistanceUnits, "DenominatorDistanceUnits", strUnits);

	return (float) 1;
}

/**
\brief	Convert the string ID of the mass units to a conversion factor.

\details This determines the scaling factor that is used within the simulation to scale the arbitrary units
to the mass units chosen by the user.

\author	dcofer
\date	3/28/2011

\param	strUnits	The string ID the mass units. 

\return	conversion factor for the units chosen.
**/
float Simulator::ConvertMassUnits(std::string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOGRAMS" || strUnits == "KILOGRAM")
		return (float) 1;

	if(strUnits == "CENTAGRAMS" || strUnits == "CENTAGRAM")
		return (float) 0.1;

	if(strUnits == "DECAGRAMS" || strUnits == "DECAGRAM")
		return (float) 0.01;

	if(strUnits == "GRAMS" || strUnits == "GRAM")
		return (float) 0.001;

	if(strUnits == "DECIGRAMS" || strUnits == "DECIGRAM")
		return (float) 1e-4;

	if(strUnits == "CENTIGRAMS" || strUnits == "CENTIGRAM")
		return (float) 1e-5;

	if(strUnits == "MILLIGRAMS" || strUnits == "MILLIGRAM")
		return (float) 1e-6;

	THROW_PARAM_ERROR(Al_Err_lInvalidMassUnits, Al_Err_strInvalidMassUnits, "MassUnits", strUnits);

	return (float) 1;
}

/**
\brief	Convert the string ID of the display units to a conversion factor.

\details Withing the GUI we sometimes have to use grams as the display unit. Then they can select Kg or mg, etc.. However, 
the standard units are usually 1 Kg, not 1 g. So we need to do a special conversion for these display units. So for example,
Lets say the Mass units chosen is Kg, and they have a mass of 1 kg, then this would be 1000 grams/1000 = 1 Kg. However, if they
set the units to be grams and had a 1 Kg mass then they would have 1000 grams/1 = 1 Kg.

\author	dcofer
\date	3/28/2011

\param	strUnits	The string ID the display units. 

\return	conversion factor for the units chosen.
**/
float Simulator::ConvertDisplayMassUnits(std::string strUnits)
{
	strUnits = Std_CheckString(strUnits);

	if(strUnits == "KILOGRAMS" || strUnits == "KILOGRAM")
		return (float) 1000;

	if(strUnits == "CENTAGRAMS" || strUnits == "CENTAGRAM")
		return (float) 100;

	if(strUnits == "DECAGRAMS" || strUnits == "DECAGRAM")
		return (float) 10;

	if(strUnits == "GRAMS" || strUnits == "GRAM")
		return (float) 1;

	if(strUnits == "DECIGRAMS" || strUnits == "DECIGRAM")
		return (float) 0.1;

	if(strUnits == "CENTIGRAMS" || strUnits == "CENTIGRAM")
		return (float) 0.01;

	if(strUnits == "MILLIGRAMS" || strUnits == "MILLIGRAM")
		return (float) 0.001;

	THROW_PARAM_ERROR(Al_Err_lInvalidMassUnits, Al_Err_strInvalidMassUnits, "MassUnits", strUnits);

	return (float) 1;
}

#pragma endregion

#pragma endregion

}			//AnimatSim
