/**
\file	Node.h

\brief	Declares the node class. 
**/

#pragma once


namespace AnimatSim
{
	/**
	\class	Node
	
	\brief	Base class for body parts and neural network nodes. 
	
	\details This is the base class used for body parts and network nodes. It contains
	links the simulation, its containing structure and the neural module. It also has some
	common methods and variables used by these types of objects.

	\author	dcofer
	\date	2/24/2011
	**/

	class ANIMAT_PORT Node : public AnimatBase 
	{
	protected:
		/// The pointer to this node's organism
		Organism *m_lpOrganism;

		///Determines if this node is enabled. This will only have any effect if this node can be disabled.
		///The majority of nodes, like rigid bodies, can not be disabled.
		BOOL m_bEnabled;

		///Keeps track of the enabled state before this node is killed.
		///If the node is killed and then turned on again we can reset the
		///enabled state to what it was before it was killed.
		BOOL m_bEnabledMem;

		///This is used for reporting the enabled state in a GetDataPointer call.
		float m_fltEnabled;

		virtual void UpdateData();

	public:
		Node();
		virtual ~Node();

		virtual BOOL Enabled();
		virtual void Enabled(BOOL bValue);

		virtual void Kill(BOOL bState = TRUE);

		/**
		\brief	Adds an external node input.
		
		\details This is used by the adapter to add a new external value to this node. It is up to the
		node to interpret what that value means. For example, if it is a neuron then it can interpret it
		to be a current. This value is added to the current total so that multiple adapters can call this
		in a given time step. It is cleared out to zero at the beginning of the time step. 
		
		\author	dcofer
		\date	3/4/2011
		
		\param	fltInput	The new input. 
		**/
		virtual void AddExternalNodeInput(float fltInput) = 0;

		/**
		\brief	Attach this node to a source adapter. 
		
		\author	dcofer
		\date	3/2/2011
		
		\param [in,out]	lpStructure	The pointer to a structure. 
		\param [in,out]	lpAdapter	The pointer to an adapter. 
		**/
		virtual void AttachSourceAdapter(Structure *lpStructure, Adapter *lpAdapter);

		/**
		\brief	Attach this node to a target adapter. 
		
		\author	dcofer
		\date	3/2/2011
		
		\param [in,out]	lpStructure	The pointer to a structure. 
		\param [in,out]	lpAdapter	The pointer to an adapter. 
		**/
		virtual void AttachTargetAdapter(Structure *lpStructure, Adapter *lpAdapter);

		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode);
		virtual void VerifySystemPointers();
	};

}				//AnimatSim

