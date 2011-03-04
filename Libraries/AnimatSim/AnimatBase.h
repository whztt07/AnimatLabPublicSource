/**
\file	AnimatBase.h

\brief	Base class file for all Animat simulation objects.
**/

#pragma once

/**
\namespace	AnimatSim

\brief	Root namespace for the base simulation library for AnimatLab.

\detils This is the root namespace for the simulation library that contains
all of the base classes used in AnimatLab. The classes and methods in this 
library contain the core functionality used throughout the entire simulation system.
If you want to create new functionality for AnimatLab simulations you will be using
the base classes from this library.
**/
namespace AnimatSim
{
	/**
	\class	AnimatBase
	
	\brief	Animat base class. 
	
	\details This class contains the base variables and methods that are used
	by all of the other animat simulation objects. For example, it contains the
	unique ID string, type, name, etc. that is used for object creation and searching.
	It also has the base methods used to set data items and add/remove new items.

	\author	dcofer
	\date	2/21/2011
	**/

	class ANIMAT_PORT AnimatBase : public CStdSerialize 
	{
	protected:
		 /// The pointer to a Simulation
		Simulator *m_lpSim;

		/// The pointer to this items parent Structure. If this is not relevant for this object then this is NULL
		AnimatSim::Environment::Structure *m_lpStructure;

		/// The pointer to this items parentNeuralModule. If this is not relevant for this object then this is NULL
		AnimatSim::Behavior::NeuralModule *m_lpModule; 

		/// The pointer to this items parent Node. If this is not relevant for this object then this is NULL
		Node *m_lpNode; 

		///The unique Id for this object. 
		string m_strID;  

		///The type for this object. Examples are Box, Plane, Neuron, etc.. 
		string m_strType;  

		///The name for this object. 
		string m_strName;  

		///Tells whether the object is selected or not
		BOOL m_bSelected;

	public:
		AnimatBase();
		virtual ~AnimatBase();

		virtual Simulator *GetSimulator();
		virtual Structure *GetStructure();
		virtual NeuralModule *GetNeuralModule();
		virtual Node *GetNode();

		virtual string ID() ;
		virtual void ID(string strValue);

		virtual string Name();
		virtual void Name(string strValue);

		virtual string Type();
		virtual void Type(string strValue);

		virtual BOOL Selected();
		virtual void Selected(BOOL bValue, BOOL bSelectMultiple);

#pragma region DataAccesMethods

		/**
		\brief	Sets the system pointers.

		\details There are a number of system pointers that are needed for use in the objects. The
		primariy one being a pointer to the simulation object itself so that you can get global
		parameters like the scale units and so on. However, each object may need other types of pointers
		as well, for example neurons need to have a pointer to their parent structure/organism, and to
		the NeuralModule they reside within. So different types of objects will need different sets of
		system pointers. We call this method to set the pointers just after creation and before Load is
		called. We then call VerifySystemPointers here, during Load and during Initialize in order to
		ensure that the correct pointers have been set for each type of objects. These pointers can then
		be safely used throughout the rest of the system. 

		\author	dcofer
		\date	3/2/2011

		\param [in,out]	lpSim		The pointer to a simulation. 
		\param [in,out]	lpStructure	The pointer to the parent structure. 
		\param [in,out]	lpModule	The pointer to the parent module module. 
		\param [in,out]	lpNode		The pointer to the parent node. 
		**/
		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode);
		virtual void VerifySystemPointers();
		virtual float *GetDataPointer(string strDataType);
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
		virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

#pragma region SimulationMethods

		virtual void Reset();
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void AfterResetSimulation();
		virtual void ReInitialize();
		virtual void Kill(BOOL bState = TRUE);
		virtual void StepSimulation();

		virtual void SimStarting();
		virtual void SimPausing();
		virtual void SimStopping();

#pragma endregion

#pragma region SnapshotMethods

		virtual long CalculateSnapshotByteSize();
		virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
		virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

#pragma endregion

		virtual void VisualSelectionModeChanged(int iNewMode);
		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
