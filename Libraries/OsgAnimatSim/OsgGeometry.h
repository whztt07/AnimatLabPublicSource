
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
        void ANIMAT_OSG_PORT SetMatrixUtil(OsgMatrixUtil *lpUtil);
		osg::Matrix ANIMAT_OSG_PORT SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot);
		osg::Matrix ANIMAT_OSG_PORT SetupMatrix(CStdFPoint &localPos, osg::Quat qRot);
        CStdFPoint ANIMAT_OSG_PORT EulerRotationFromMatrix(osg::Matrix osgMT);
        osg::Matrix ANIMAT_OSG_PORT LoadMatrix(CStdXml &oXml, std::string strElementName); 
        std::string ANIMAT_OSG_PORT SaveMatrixString(osg::Matrix osgMT);
        void ANIMAT_OSG_PORT SaveMatrix(CStdXml &oXml, std::string strElementName, osg::Matrix osgMT); 

		void ANIMAT_OSG_PORT ApplyVertexTransform(osg::Node *node, osg::Matrix omat);
		bool ANIMAT_OSG_PORT OsgMatricesEqual(osg::Matrix v1, osg::Matrix v2);
		void ANIMAT_OSG_PORT SetNodeColor(osg::Node *osgNode, CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess);
		void ANIMAT_OSG_PORT AddNodeTexture(osg::Node *osgNode, std::string strTexture, osg::StateAttribute::GLMode eTextureMode);
		osg::Geometry ANIMAT_OSG_PORT *CreateBoxGeometry(float xsize, float ysize, float zsize, float fltXSegWidth, float fltYSegWidth, float fltZSegWidth);
		osg::Geometry ANIMAT_OSG_PORT *CreateConeGeometry(float height, float topradius, float botradius, int sides, bool doSide, bool doTop, bool doBottom);
		osg::Geometry ANIMAT_OSG_PORT *CreateSphereGeometry(int latres, int longres, float radius);
		osg::Geometry ANIMAT_OSG_PORT *CreateEllipsoidGeometry(int latres, int longres, float rSemiMajorAxis, float rSemiMinorAxis);
		osg::Geometry ANIMAT_OSG_PORT *CreatePlaneGeometry(float fltCornerX, float fltCornerY, float fltXSize, float fltYSize, float fltXGrid, float fltYGrid, bool bBothSides);
		osg::MatrixTransform ANIMAT_OSG_PORT *CreateLinearAxis(float fltGripScale, CStdFPoint vRotAxis);   
		osg::Geode ANIMAT_OSG_PORT *CreateCircle( int plane, int approx, float radius, float width );
		osg::Vec3Array ANIMAT_OSG_PORT *CreateCircleVerts( int plane, int approx, float radius );
		osg::Geometry ANIMAT_OSG_PORT *CreateTorusGeometry(float innerRadius, float outerRadius, int sides, int rings);
		osg::Node ANIMAT_OSG_PORT *CreateHeightField(std::string heightFile, float fltSegWidth, float fltSegLength, float fltMaxHeight, osg::HeightField **osgMap, bool bAdjustHeight);

	}			// Environment
}				//OsgAnimatSim


