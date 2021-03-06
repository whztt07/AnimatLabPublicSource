#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		/**
		\brief	Vortex base body class.

		\details This is a base, secondary derived class for all body part objects within animatlab. It is
		derived from VsMovableItem, which contains all of the base OSG graphics manipulation code. It is 
		also derived from IPhysicsBody, which has the methods specifically required by the body part classes.
		
		\author	dcofer
		\date	5/2/2011
		**/
		class VORTEX_PORT VsBody : public VsMovableItem, public AnimatSim::Environment::IPhysicsBody
		{
		protected:
			BodyPart *m_lpThisBP;
			Vx::VxEntity::EntityControlTypeEnum m_eControlType;

			virtual void SetThisPointers();

		public:
			VsBody();
			virtual ~VsBody();

			virtual void Physics_SetFreeze(bool bVal) {};
			virtual void Physics_SetDensity(float fltVal) {};
            virtual void Physics_SetMass(float fltVal) {};
			virtual void Physics_SetMaterialID(std::string strID) {};
			virtual void Physics_SetVelocityDamping(float fltLinear, float fltAngular) {};
			virtual void Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz) {};
			virtual void Physics_ResizeSelectedReceptiveFieldVertex() {};
			virtual void Physics_FluidDataChanged() {};
            virtual bool Physics_IsDefined() {return false;};
            virtual bool Physics_IsGeometryDefined() {return false;};
            virtual void Physics_WakeDynamics() {};
            virtual void Physics_ContactSensorAdded(ContactSensor *lpSensor) {};
            virtual void Physics_ContactSensorRemoved() {};
            virtual void Physics_ChildBodyAdded(RigidBody *lpChild) {};
            virtual void Physics_ChildBodyRemoved(bool bHasStaticJoint) {};
            virtual void Physics_StepHydrodynamicSimulation() {};

            virtual void SetBody() = 0;

			virtual void Physics_UpdateNode() {};
			virtual bool Physics_HasCollisionGeometry() {return false;};
			virtual void Physics_DeleteStickyLock() {};
		};

	}			// Environment
}				//VortexAnimatSim


