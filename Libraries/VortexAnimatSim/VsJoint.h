// VsJoint.h: interface for the VsJoint class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSJOINT_H__93EDBBFE_2FA0_467C_970F_1775454FE94E__INCLUDED_)
#define AFX_VSJOINT_H__93EDBBFE_2FA0_467C_970F_1775454FE94E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		class VsRigidBody;

		/*! \brief 
			A common class for all joint data specific to vortex.

			\remarks
			This is a common class for the joint objects that 
			specifically holds vortex data and methods. The 
			reasoning behind this class is the same as that for
			the VsRigidBody. Please see that class for an explanation
			of why this class is needed.

			\sa
			RigidBody, CVsAlJoint
		*/

		class VORTEX_PORT VsJoint : public VsBody
		{
		protected:
			Vx::VxConstraint *m_vxJoint;
			Vx::VxConstraint::CoordinateID m_iCoordID;

			//This is the offset from the child to the parent. We must add the osg graphics to the parent osg node
			//because if we do not then the joint will move when the child moves, and that is not correct. However, 
			//the joint is really attached relative to the child. So I am adding this extra matrix transform that is
			//the same as the child body to make up for the fact that we have to attach to the parent osg node.
			osg::Matrix m_osgChildOffsetMatrix;
			osg::ref_ptr< osg::MatrixTransform> m_osgChildOffsetMT;

#pragma region DefaultBallGraphicsItems

			//Graphics objects for the default joint drawing code
			/// The osg default ball geometry.
			osg::ref_ptr<osg::Geometry> m_osgDefaultBall;

			/// The osg default ball matrix transform.
			osg::ref_ptr<osg::MatrixTransform> m_osgDefaultBallMT;

			/// The osg default ball material.
			osg::ref_ptr<osg::Material> m_osgDefaultBallMat;

			/// The osg default ball state set.
			osg::ref_ptr<osg::StateSet> m_osgDefaultBallSS;

			/// The osg joint matrix transform.
			osg::ref_ptr<osg::MatrixTransform> m_osgJointMT;

#pragma endregion

			Joint *m_lpThisJoint;
			VsRigidBody *m_lpVsParent;
			VsRigidBody *m_lpVsChild;

			virtual void SetThisPointers();
			void UpdatePosition();

            virtual void ResetDraggerOnResize();
            virtual void DeleteJointGraphics();
            virtual void CreateJointGraphics();
			virtual void SetupGraphics();
            virtual void DeleteGraphics();
			virtual void SetupPhysics();
			virtual VxVector3 NormalizeAxis(CStdFPoint vLocalRot);
			virtual void UpdatePositionAndRotationFromMatrix();

			virtual void LocalMatrix(osg::Matrix osgLocalMT);
			virtual void ChildOffsetMatrix(osg::Matrix osgMT);
			virtual float GetCurrentVxJointPos();

		public:
			VsJoint();
			virtual ~VsJoint();

			virtual osg::Group *ParentOSG();
			virtual osg::Group *ChildOSG();

			virtual void SetAlpha();

			virtual void Initialize();
			virtual void SetBody();
			virtual void Physics_ResetSimulation();
			virtual void Physics_CollectData();
			virtual bool Physics_SetData(const std::string &strDataType, const std::string &strValue);
			virtual void Physics_QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual void Physics_Resize();

			virtual Vx::VxConstraint* Constraint() {return m_vxJoint;};
			virtual Vx::VxConstraint::CoordinateID CoordinateID() {return m_iCoordID;};

			//Methods not used by joints.
			virtual void Physics_SetParent(MovableItem *lpParent);
			virtual void Physics_SetChild(MovableItem *lpChild);
			virtual void Physics_UpdateMatrix();
			virtual void Physics_PositionChanged();
			virtual void Physics_RotationChanged();
			virtual void BuildLocalMatrix();
			virtual void BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, std::string strName);
			virtual void Physics_EnableCollision(RigidBody *lpBody) {};
			virtual void Physics_DisableCollision(RigidBody *lpBody) {};
			virtual void Physics_AddBodyForceAtLocalPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) {};
			virtual void Physics_AddBodyForceAtWorldPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) {};
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits) {};
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) {CStdFPoint v; return v;};
			virtual float Physics_GetMass() {return 0;};
            virtual float Physics_GetDensity() {return 0;};
			virtual bool Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos);

		};

	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSJOINT_H__93EDBBFE_2FA0_467C_970F_1775454FE94E__INCLUDED_)
