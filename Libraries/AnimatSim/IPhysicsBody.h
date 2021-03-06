#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT IPhysicsBody
		{
		protected:

		public:
			IPhysicsBody(void);
			virtual ~IPhysicsBody(void);

            virtual bool Physics_IsDefined() = 0;
            virtual bool Physics_IsGeometryDefined() = 0;
			virtual void Physics_SetFreeze(bool bVal) = 0;
			virtual void Physics_SetDensity(float fltVal) = 0;
			virtual float Physics_GetDensity() = 0;
			virtual void Physics_SetMass(float fltVal) = 0;
			virtual float Physics_GetMass() = 0;
			virtual void Physics_SetMaterialID(std::string strID) = 0;
			virtual void Physics_SetVelocityDamping(float fltLinear, float fltAngular) = 0;
			virtual void Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz) = 0;

			virtual void Physics_UpdateNode() = 0;
			virtual void Physics_EnableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_DisableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_AddBodyForceAtLocalPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) = 0;
			virtual void Physics_AddBodyForceAtWorldPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) = 0;
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits) = 0;
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) = 0;
			virtual void Physics_ResizeSelectedReceptiveFieldVertex() = 0;
			virtual void Physics_FluidDataChanged() = 0;
			virtual bool Physics_HasCollisionGeometry() = 0;
            virtual void Physics_WakeDynamics() = 0;
            virtual void Physics_ContactSensorAdded(ContactSensor *lpSensor) = 0;
            virtual void Physics_ContactSensorRemoved() = 0;
            virtual void Physics_StepHydrodynamicSimulation() = 0;

            virtual void Physics_ChildBodyAdded(RigidBody *lpChild) = 0;
            virtual void Physics_ChildBodyRemoved(bool bHasStaticJoint) = 0;
			virtual void Physics_DeleteStickyLock() = 0;
		};

	}
}