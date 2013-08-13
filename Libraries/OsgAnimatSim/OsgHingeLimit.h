
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		class ANIMAT_OSG_PORT OsgHingeLimit
		{
		protected:
			osg::ref_ptr<osg::Geometry> m_osgFlap;
			osg::ref_ptr<osg::MatrixTransform> m_osgFlapTranslateMT;
			osg::ref_ptr<osg::MatrixTransform> m_osgFlapRotateMT;

			osg::ref_ptr<osg::Material> m_osgFlapMat;
			osg::ref_ptr<osg::StateSet> m_osgFlapSS;

            virtual CStdColor GetLimitColor() = 0;

		public:
			OsgHingeLimit();
			virtual ~OsgHingeLimit();

			virtual void LimitAlpha(float fltA);
            virtual void SetLimitPos(float fltHeight, float fltLimitPos);
				
			osg::Geometry *FlapGeometry() {return m_osgFlap.get();};
			osg::MatrixTransform *FlapTranslateMT() {return m_osgFlapTranslateMT.get();};
			osg::MatrixTransform *FlapRotateMT() {return m_osgFlapRotateMT.get();};
			osg::Material *FlapMat() {return m_osgFlapMat.get();};
			osg::StateSet *FlapSS() {return m_osgFlapSS.get();};

            virtual void SetupLimitGraphics(float fltFlapWidth, float fltCylinderHeight, float fltLimitPos);
            virtual void DeleteLimitGraphics();
        };

	}			// Environment
}				//OsgAnimatSim
