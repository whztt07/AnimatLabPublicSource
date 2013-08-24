
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class BULLET_PORT BlPrismaticLimit : public AnimatSim::Environment::ConstraintLimit, public OsgAnimatSim::Environment::Joints::OsgPrismaticLimit
			{
			protected:
				Vx::VxPrismatic *m_vxPrismatic;

				virtual void SetLimitValues();

			public:
				BlPrismaticLimit();
				virtual ~BlPrismaticLimit();

				virtual void PrismaticRef(Vx::VxPrismatic *vxPrismatic);

				virtual void Alpha(float fltA);
				virtual void SetLimitPos();

				virtual void SetupGraphics();
                virtual void DeleteGraphics();
            };

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
