
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
            class ANIMAT_OSG_PORT OsgHingeLimit
		    {
		    protected:
                ConstraintLimit *m_lpThisLimit;

                osg::ref_ptr<osg::Geometry> m_osgFlap;
			    osg::ref_ptr<osg::MatrixTransform> m_osgFlapTranslateMT;
			    osg::ref_ptr<osg::MatrixTransform> m_osgFlapRotateMT;

			    osg::ref_ptr<osg::Material> m_osgFlapMat;
			    osg::ref_ptr<osg::StateSet> m_osgFlapSS;

		    public:
			    OsgHingeLimit();
			    virtual ~OsgHingeLimit();
            
                virtual void SetConstraintLimit(ConstraintLimit *lpLimit) {m_lpThisLimit = lpLimit;};

			    virtual void LimitAlpha(float fltA);
                virtual void SetLimitPos(float fltHeight);
				
			    osg::Geometry *FlapGeometry() {return m_osgFlap.get();};
			    osg::MatrixTransform *FlapTranslateMT() {return m_osgFlapTranslateMT.get();};
			    osg::MatrixTransform *FlapRotateMT() {return m_osgFlapRotateMT.get();};
			    osg::Material *FlapMat() {return m_osgFlapMat.get();};
			    osg::StateSet *FlapSS() {return m_osgFlapSS.get();};

                virtual void SetupLimitGraphics(float fltFlapWidth, float fltCylinderHeight);
                virtual void DeleteLimitGraphics();
            };

    	}			// Joints
	}			// Environment
}				//OsgAnimatSim
