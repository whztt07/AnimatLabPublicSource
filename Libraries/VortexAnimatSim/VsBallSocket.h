/**
\file	VsBallSocket.h

\brief	Declares the vortex ball socket class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
			/**
			\brief	Vortex ball-and-socket joint class.

			\details This class implements a spherical, or ball and socket joint.
			This type of joint prevents linear motion for all three dimensions, but
			allows angular motion around all three axises. This type of joint does
			not have any contraint limits available for it. To prevent motion for
			this joint you must use rigid bodies within the simulation to constrain
			the movement. This joit type also is not motorized. It can only be passively
			moved, or it can be controlled through muscle movements.
			
			\author	dcofer
			\date	4/15/2011
			**/
			class VORTEX_PORT VsBallSocket : public VsJoint, public AnimatSim::Environment::Joints::BallSocket     
			{
			protected:
				/// The vortex socket class.
				Vx::VxBallAndSocket *m_vxSocket;

				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				VsBallSocket();
				virtual ~VsBallSocket();

#pragma region DataAccesMethods

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
