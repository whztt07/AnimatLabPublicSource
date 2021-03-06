/**
\file	RigidBody.h

\brief	Declares the rigid body class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		class Odor;

		/**
		\class	RigidBody
		
		\brief	The base class for all of the basic rigid body type of objects.
		
		\details
		This class provides the base functionality for a rigid body.
		Each structure/Organism is made up of a heirarchcal tree of
		rigid bodies that are connected by Joint objects. The base
		structure has a root rigid body. That root body has a list of
		child rigid bodies that are connected to the root through a
		joint. Each of those child bodies can then have other
		children connected to them and so on.<br><br>
		Each rigid body within a structure/organism has a unique
		string identifier that is specifed for it.
		Whenever you are attempting to find a given part through a
		function like Simulator::FindRigidBody it is this
		identifier that is used as the key in the search. This class
		also contains a number of parameters that are common for all
		rigid bodies. These include things like the colors
		of the  object and the uniform density of the
		body. All rigid bodies have these properties.<br><br>
		This is a virtual base class and can not be directly created.
		You must override this class to provide specific
		functionality in order to create a real rigid body. For
		example, the Box and Cylinder are two examples of
		subclasses of a rigid body that add more parameters specific
		for their body type. Box has the x,y, and z widths for the
		box, whereas cylinder has the radius and height of the
		cylinder to create. The purpose of RigidBody and its
		subclasses like Box are to try and implement as
		much of the functionality as can be accomplished in this
		library, yet leave it flexible enough that a new physics
		engine could be easily swapped in for later use. This means that a
		lot of the mundane things like loading the data for the
		different rigid body and joint types is done for you
		automatically by the Body and Joint classes. But
		these classes can still not really implement all of the
		functionality necessary for the rigid body. The reason for
		this is that the overridable functions to create the actual
		implementation for those parts in your chosen physics engine
		must still be implemented. The two main functions related to
		that are CreateParts and CreateJoints. Since AnimatSim is
		generalized so that it is not tightly coupled to any one
		physics engine then that coupling must be done in the layer
		above this library where the chosen physics engine like
		Vortex is actually used.

		\author	dcofer
		\date	3/2/2011
		**/
		class ANIMAT_PORT RigidBody : public BodyPart    
		{
		protected:
			///The center of mass of this body relative to the center of the object. If this
			///is (0, 0, 0) then the default COM is used.
			CStdFPoint m_vCenterOfMass;

			///Specifies if the part should frozen in place to the world. If a rigid body 
			///is frozen then it is as if it is nailed in place and can not move. Gravity and 
			///and other forces will not act on it.
			bool m_bFreeze;

			///Uniform density for the rigid body.
			float m_fltDensity;

            /// The density value reported to the GUI
            float m_fltReportDensity;

			///The mass of the object
			float m_fltMass;

			///The mass of the object to report to GUI
			float m_fltReportMass;

            /// The volume for the rigid body
            float m_fltVolume;

			///The volume of the object to report to GUI.
			float m_fltReportVolume;

			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RigidBody> m_aryChildParts;

			///A pointer to the joint that connects this rigid body to its parent.
			///If this is the root object then this pointer is null. However, All child parts
			///must be connected to their parent.
			Joint *m_lpJointToParent;

			///Some body parts like contact sensors and muscle attachments do not have joints. If not then we should not 
			///attempt to load them.
			bool m_bUsesJoint;

			///This determines whether or not this is a contact sensor. If this is true then
			///this object does not take part in collisions and such, it is a contact sensor only.
			bool m_bIsContactSensor;

			///This determines whether the object is a collision geometry object
			bool m_bIsCollisionObject;

			///This keeps track of the current number of surface contacts that are occuring for this
			///contact sensor. This is only used for sensors.
			float m_fltSurfaceContactCount;

			///If a part is a contact sensor then we can also make it be a sticky part.
			///If this is enabled and the m_fltStickyOn is 1 then it will stick to any parts
			///that it comes into contact with. When m_fltStickyOn goes to 0 it will release those parts.
			bool m_bIsStickyPart;

			///If this is a suction part then this controls when stickness is on
			float m_fltStickyOn;

			///If we are doing a sticky lock then this is the child part that was locked on.
			RigidBody *m_lpStickyChild;

			/// The pointer to a receptive field ContactSensor object. This is responsible for 
			/// processing the receptive field contacts
			ContactSensor *m_lpContactSensor;

			/// The linear velocity damping for this body part.
			float m_fltLinearVelocityDamping;

			/// The angular velocity damping for this part.
			float m_fltAngularVelocityDamping;

			/// The array odor sources attached to this part.
			CStdPtrMap<std::string, Odor> m_aryOdorSources;
			
			///Tells if this body is considered a food source.
			bool m_bFoodSource;  
			
			/// The quantity of food that this part contains
			float m_fltFoodQuantity;

			/// The initial food quantity to use when simulation is reset.
			float m_fltFoodQuantityInit;

			///Tells how much food is being eaten.
			float m_fltFoodEaten;

			/// The maximum food quantity that this part can contain
			float m_fltMaxFoodQuantity;

			/// The rate at which food is replenished
			float m_fltFoodReplenishRate;

			/// The energy content of the food in calories.
			float m_fltFoodEnergyContent;

			/// Keeps track of how many time slices this part can eat.
			long m_lEatTime;

			/// Identifier for the material type this part will use.
			std::string m_strMaterialID;

			//Hyrdrodynamic properties
			///This is the relative position to the center of the buoyancy in the body.
			CStdFPoint m_vBuoyancyCenter;

			///This is a scale used to calculate the buoyancy value. It is a scale factor
			///applied to the buoyancy force which accounts for the fact that a given volume might actually have holes
			///or concavity in it which would affect the buoyancy force on the object.
			float m_fltBuoyancyScale;

			///This is the drag coefficients for the three axises for the body.
			CStdFPoint m_vLinearDrag;

			///This is the drag coefficients for the three axises for the body.
			CStdFPoint m_vAngularDrag;

            /// The maximum hyrdodynamic force that can be applied
            float m_fltMaxHydroForce;

            /// The maximum hyrdodynamic torque that can be applied
            float m_fltMaxHydroTorque;

			///The Magnus coefficient for the body. This is defaulted to zero because it almost always negligble for most body parts.
			float m_fltMagnus;
			
			/// true to enable fluid interactions.
			bool m_bEnableFluids;

            /// Determines whether a debug graphics of the rigid body is shown or not.
            /// This will show the exact placement of the rigid body in the physics engine.
            bool m_bDisplayDebugCollisionGraphic;

            ///This is the list of other  parts that this part is excluded from colliding with.
            std::unordered_set<RigidBody *> m_aryExcludeCollisionSet;

			virtual RigidBody *LoadRigidBody(CStdXml &oXml);
			virtual Joint *LoadJoint(CStdXml &oXml);

			virtual void LoadPosition(CStdXml &oXml);

			virtual RigidBody *AddRigidBody(std::string strXml);
			virtual void RemoveRigidBody(std::string strID, bool bThrowError = true);
			virtual int FindChildListPos(std::string strID, bool bThrowError = true);

			virtual void AddContactSensor(std::string strXml);
			virtual void RemoveContactSensor(std::string strID, bool bThrowError = true);
			virtual void LoadContactSensor(CStdXml &oXml);

			virtual Odor *LoadOdor(CStdXml &oXml);
			virtual void AddOdor(Odor *lpOdor);
			virtual void AddOdor(std::string strXml, bool bDoNotInit);
			virtual void RemoveOdor(std::string strID, bool bThrowError = true);

			virtual void Mass(float fltVal, bool bUseScaling, bool bPhysicsCallback);
			virtual void CenterOfMass(CStdFPoint &vPoint, bool bUseScaling, bool bPhysicsCallback);
			virtual void Freeze(bool bVal, bool bPhysicsCallback);

            virtual void RemoveCollisionExclusions();

		public:
			RigidBody();
			virtual ~RigidBody();
						
			static RigidBody *CastToDerived(AnimatBase *lpBase) {return static_cast<RigidBody*>(lpBase);}

#pragma region AccessorMutators

			virtual CStdFPoint Position();
			virtual void Position(CStdFPoint &oPoint, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
			
			virtual int VisualSelectionType();

			virtual CStdFPoint CenterOfMass();
			virtual CStdFPoint CenterOfMassWithStaticChildren();
			virtual void CenterOfMass(CStdFPoint &vPoint, bool bUseScaling = true);
			virtual void CenterOfMass(float fltX, float fltY, float fltZ, bool bUseScaling = true);
			virtual void CenterOfMass(std::string strXml, bool bUseScaling = true);

			virtual CStdPtrArray<RigidBody>* ChildParts();

			virtual Joint *JointToParent();
			virtual void JointToParent(Joint *lpValue);

			virtual ContactSensor *GetContactSensor();

			virtual float Density();
			virtual void Density(float fltVal, bool bUseScaling = true);

			virtual float Mass();
			virtual void Mass(float fltVal, bool bUseScaling = true);
            virtual float MassWithChildren();

			virtual float Volume();
			virtual void Volume(float fltVal, bool bUseScaling = true);

			virtual bool Freeze();
			virtual void Freeze(bool bVal);

			virtual bool IsContactSensor();
			virtual void IsContactSensor(bool bVal);

			virtual bool IsCollisionObject();
			virtual void IsCollisionObject(bool bVal);
			
			virtual bool IsStickyPart();
			virtual void IsStickyPart(bool bVal);

			virtual float StickyOn();
			virtual void StickyOn(float fltVal);

			virtual RigidBody *StickyChild();
			virtual void StickyChild(RigidBody *lpChild);

			virtual bool IsRoot();
			virtual bool HasStaticJoint();
            virtual bool HasStaticChildren();
            virtual float StaticChildrenMass();

			virtual bool IsFoodSource();
			virtual void IsFoodSource(bool bVal);

			virtual float FoodQuantity();
			virtual void FoodQuantity(float fltVal);

			virtual float FoodEaten();
			virtual void FoodEaten(float fltVal);
			
			virtual float FoodReplenishRate();
			virtual void FoodReplenishRate(float fltVal);

			virtual float FoodEnergyContent();
			virtual void FoodEnergyContent(float fltVal);

			virtual float MaxFoodQuantity();
			virtual void MaxFoodQuantity(float fltVal);

			virtual float LinearVelocityDamping();
			virtual void LinearVelocityDamping(float fltVal, bool bUseScaling = true);

			virtual float AngularVelocityDamping();
			virtual void AngularVelocityDamping(float fltVal, bool bUseScaling = true);

			virtual std::string MaterialID();
			virtual void MaterialID(std::string strID);

			virtual CStdFPoint BuoyancyCenter();
			virtual void BuoyancyCenter(CStdFPoint &oPoint, bool bUseScaling = true);
			virtual void BuoyancyCenter(float fltX, float fltY, float fltZ, bool bUseScaling = true);
			virtual void BuoyancyCenter(std::string strXml, bool bUseScaling = true);

			virtual float BuoyancyScale();
			virtual void BuoyancyScale(float fltVal);

			virtual CStdFPoint LinearDrag();
			virtual void LinearDrag(CStdFPoint &oPoint);
			virtual void LinearDrag(float fltX, float fltY, float fltZ);
			virtual void LinearDrag(std::string strXml);

			virtual CStdFPoint AngularDrag();
			virtual void AngularDrag(CStdFPoint &oPoint);
			virtual void AngularDrag(float fltX, float fltY, float fltZ);
			virtual void AngularDrag(std::string strXml);

			virtual float MaxHydroForce();
			virtual void MaxHydroForce(float fltVal, bool bUseScaling = true);

			virtual float MaxHydroTorque();
			virtual void MaxHydroTorque(float fltVal, bool bUseScaling = true);

			virtual float Magnus();
			virtual void Magnus(float fltVal);

			virtual bool EnableFluids();
			virtual void EnableFluids(bool bVal);

			virtual bool HasCollisionGeometry();

            virtual bool DisplayDebugCollisionGraphic() {return m_bDisplayDebugCollisionGraphic;}
            virtual void DisplayDebugCollisionGraphic(bool bVal) {m_bDisplayDebugCollisionGraphic = bVal;}

            virtual std::unordered_set<RigidBody *> *GetExclusionCollisionSet() {return &m_aryExcludeCollisionSet;};
            virtual bool FindCollisionExclusionBody(RigidBody *lpBody, bool bThrowError = true);

#pragma endregion

			virtual float SurfaceContactCount();

			virtual void Eat(float fltBiteSize, long lTimeSlice);
			virtual void AddSurfaceContact(RigidBody *lpContactedSurface);
			virtual void RemoveSurfaceContact(RigidBody *lpContactedSurface);
            virtual void SetSurfaceContactCount(int iCount);
			virtual void AddForceAtLocalPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits);
			virtual void AddForceAtWorldPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits);
			virtual void AddTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits);
			virtual CStdFPoint GetVelocityAtPoint(float x, float y, float z);
			virtual float GetMassValueWithStaticChildren();
			virtual float GetDensity();
			virtual float GetMass();
			virtual float GetVolume();
			virtual void UpdatePhysicsPosFromGraphics();
            virtual void UpdateChildPhysicsPosFromGraphics();

			virtual RigidBody *ParentWithCollisionGeometry();

			virtual void EnableCollision(RigidBody *lpBody);
			virtual void DisableCollision(RigidBody *lpBody);

			virtual void CreateParts();
            virtual void CreateChildParts();
			virtual void CreateJoints();
            virtual void CreateChildJoints();

#pragma region DataAccesMethods

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

#pragma endregion

			virtual void SimPausing();
			virtual int GetTargetDataTypeIndex(const std::string &strDataType);
			virtual void AddExternalNodeInput(int iTargetDataType, float fltInput);
			virtual void StepSimulation();
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void Kill(bool bState = true);
			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
