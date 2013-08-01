/**
\file	Structure.h

\brief	Declares the structure class. 
**/

#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Environment

	\brief	This namespace contains all of the objects that are related to the physical part of the 
	virtual 3-D world. All of the neural components are located in the Behavioral namespace. 
	**/
	namespace Environment
	{

		/**
		\class	CollisionPair
		
		\brief	Collision pair. 
		
		\details Keeps track of the ID's for two pairs of body parts that can, or cannot collide.
		This is primarily used in the collision exclusion list to disable collisions between the 
		two objects.

		\author	dcofer
		\date	2/25/2011
		**/
		class ANIMAT_PORT CollisionPair : public AnimatBase
		{
		public:
			/// GUID ID of the first part of the collision pair.
			string m_strPart1ID;
			/// GUID ID of the second part of the collision pair.
			string m_strPart2ID;

			/**
			\brief	Default constructor.
			
			\author	dcofer
			\date	3/22/2011
			**/
			CollisionPair() {};

			/**
			\brief	Finaliser.
			
			\author	dcofer
			\date	3/22/2011
			**/
			~CollisionPair() {};
		};


		/*! \brief 
			A "static" structure in the simulation.

			\remarks
			This object is a "static" structure in the simulation. I say "static" 
			because can move, but it is part of the background environment and does
			not have a brain controlling its movement. For example, if you wanted to
			have a house as an obstacle in the environemt then that house would probably
			have a door. The door would be connected to the wall of the house using a 
			hinge joint. So the door can open and close, but it does not have a brain 
			to control its movement. Another example is a rock. If you create a rock and
			place it at a precarious position on the top of a hill then it will move by
			rolling down the hill, but this is a passive effect of gravity acting on the
			body of the rock, and not something that the rock actively did. An organism
			on the other hand does have a neural network that actively controls the 
			movements of its limbs to change its position and orientation within the world.
					
			\sa
			Structure, Organism, Body, Joint
		*/
		class ANIMAT_PORT Structure : public AnimatBase, public MovableItem 
		{
		protected:
			///The root rigid body object of this structure.
			RigidBody *m_lpBody;

			///A list of rigid bodies contained within this structure.
			///The objects in this list are references only. They are not
			///destroyed by this list. It is used primarily for finding
			///rigid bodies and ensuring that only unique ID's are used
			///for the rigid bodies.
			CStdMap<string, RigidBody *> m_aryRigidBodies;

			///A list of joints contained within this structure.
			///The objects in this list are references only. They are not
			///destroyed by this list. It is used primarily for finding
			///joints and ensuring that only unique ID's are used
			///for the joints.
			CStdMap<string, Joint *> m_aryJoints;

			///This is the list of other body part ID's to exclude from collision tests.
			CStdPtrArray<CollisionPair> m_aryExcludeCollisionList;

			///This is a pointer to an instance of the IMovableItemCallback interface. This
			///interface is used to send back information to the GUI. If you are running in
			///simulation mode only then this will be NULL and no info will be sent back up.
			///If you are running with the GUI then it makes the calls back up to it.
			IMovableItemCallback *m_lpCallback;

			/// The radius of the graphical sphere shown for the structure position.
			float m_fltSize;

			virtual void LoadLayout(CStdXml &oXml);
			virtual void LoadCollisionPair(CStdXml &oXml);
			virtual RigidBody *LoadRoot(CStdXml &oXml);

			virtual void AddRoot(string strXml);
			virtual void RemoveRoot(string strID, bool bThrowError = true);
			
			virtual void UpdateData();

		public:
			Structure();
			virtual ~Structure();

			void Sim(Simulator *lpSim);

			virtual void Body(RigidBody *lpBody);
			virtual RigidBody *Body();

			virtual CStdFPoint Position();
			virtual void Position(CStdFPoint &oPoint, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);

			virtual float Size();
			virtual void Size(float fltVal, bool bUseScaling = true);

			virtual bool AllowTranslateDragX();
			virtual bool AllowTranslateDragY();
			virtual bool AllowTranslateDragZ();

			virtual bool AllowRotateDragX();
			virtual bool AllowRotateDragY();
			virtual bool AllowRotateDragZ();

			virtual CStdPtrArray<CollisionPair> ExclusionList();
			virtual void AddCollisionPair(string strID1, string strID2);

			virtual void MinTimeStep(float &fltMin);

            virtual void Create();
			virtual void StepPhysicsEngine();
			virtual void ResetSimulation();

			void AddJoint(Joint *lpJoint);
			void RemoveJoint(string strID);
			void AddRigidBody(RigidBody *lpBody);
			void RemoveRigidBody(string strID);

			virtual Joint *FindJoint(string strJointID, bool bThrowError = true);
			virtual RigidBody *FindRigidBody(string strBodyID, bool bThrowError = true);
			virtual Node *FindNode(string strID, bool bThrowError = true);
			//virtual AnimatBase *FindCollisionPair(string strID, bool bThrowError = true);

			virtual void EnableMotor(string strJointID, bool bVal);
			virtual void SetMotorInput(string strJointID, float fltInput);

			virtual void EnableCollision(RigidBody *lpCollisionBody);
			virtual void DisableCollision(RigidBody *lpCollisionBody);

			virtual void Selected(bool bValue, bool bSelectMultiple); 
			virtual void UpdatePhysicsPosFromGraphics();

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual float *GetDataPointer(const string &strDataType);
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
			virtual bool AddItem(const string &strItemType, const string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const string &strItemType, const string &strID, bool bThrowError = true);

#pragma endregion

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
