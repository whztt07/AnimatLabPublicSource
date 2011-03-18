#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Adapters

	\brief	Namespace for the adapter objects.

	\details Adapters are generalized mechanism that allows items in one module to talk to items in another. For example, 
	It allows a neuron in a firing rate neural module to inject current into an integrate and fire neuron, or to talk to the
	physics engine.
	**/
	namespace Adapters
	{

		/**
		\class	Adapter
		
		\brief	Adapter. 
		
		\details Adapters are generalized mechanism that allows items in one module to talk to items in another. For example, 
		It allows a neuron in a firing rate neural module to inject current into an integrate and fire neuron, or to talk to the
		physics engine.

		\author	dcofer
		\date	2/28/2011
		**/
		class ANIMAT_PORT Adapter : public Node 
		{
		protected:
			/// Name of the source NeuralModule
			string m_strSourceModule;

			/// GUID ID of the source node.
			string m_strSourceID;

			/// DateType of the source variable that will be converted. This is retrieved using the GetDataPointer method.
			string m_strSourceDataType;

			/// Pointer to the source node
			Node *m_lpSourceNode;

			/// Pointer to the source data varaible.
			float *m_lpSourceData;

			/// Name of the target NeuralModule
			string m_strTargetModule;

			/// GUID ID of the target node.
			string m_strTargetID;

			/// DateType of the target variable that will be converted. This is retrieved using the GetDataPointer method.
			string m_strTargetDataType;

			/// Pointer to the target node
			Node *m_lpTargetNode;

			/// Pointer to the Gain that will be used to convert the source value into the target value.
			Gain *m_lpGain;

		public:
			Adapter();
			virtual ~Adapter();

			virtual string SourceModule();
			virtual void SourceModule(string strName);

			virtual string SourceID();
			virtual void SourceID(string strID);

			virtual string SourceDataType();
			virtual void SourceDataType(string strType);

			virtual Node *SourceNode();

			virtual string TargetModule();
			virtual void TargetModule(string strName);

			virtual string TargetID();
			virtual void TargetID(string strID);

			virtual string TargetDataType();
			virtual void TargetDataType(string strType);

			virtual Node *TargetNode();

			virtual Gain *GetGain();

			/**
			\brief	Attaches this adapter for the source to the correct NeuralModule. This allows the neural module to process the
			adapters correctly when it steps the simulation.

			\author	dcofer
			\date	3/18/2011

			\param [in,out]	lpStructure	Pointer to parent structure. 
			\param [in,out]	lpNode	   	Pointer to the source node. 
			**/
			virtual void AttachSourceAdapter(Structure *lpStructure, Node *lpNode);

			/**
			\brief	Attaches this adapter for the target to the correct NeuralModule. This allows the neural module to process the
			adapters correctly when it steps the simulation.

			\author	dcofer
			\date	3/18/2011

			\param [in,out]	lpStructure	Pointer to parent structure. 
			\param [in,out]	lpNode	   	Pointer to the source node. 
			**/
			virtual void AttachTargetAdapter(Structure *lpStructure, Node *lpNode);

			virtual void Initialize();
			virtual void AddExternalNodeInput(float fltInput);
			virtual float *GetDataPointer(string strDataType);
			virtual void ResetSimulation() {};
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim
