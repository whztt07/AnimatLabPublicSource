/**
\file	BodyPart.h

\brief	Declares the body part class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Baes class for all items that can be moved/roated within the environment.
		
		\author	dcofer
		\date	4/22/2011
		**/
		class ANIMAT_PORT MovableItem
		{
		protected:
			 /// The pointer to a Simulation
			Simulator *m_lpMovableSim;
			
			///The parent rigid body of this part. If this value is null
			///then it is assumed that this is the root part of a structure or the structure itself.
			RigidBody *m_lpParent;

			///The absolute position of the movable item in world coordinates.
			///This is calcualted during loading of the part using the position of 
			///the parent part and the relative position specified in the configuration file.
			CStdFPoint m_oAbsPosition;

			///These are rotation and position coords relative to the parent if this is a body part. 
			// If this item is a structure then this is the world coordinates.
			CStdFPoint m_oPosition;

			///This is used for reporting the position back to the GUI. It is the position scaled for distance units.
			CStdFPoint m_oReportPosition;

			///This is used for reporting the position back to the GUI. It is the world position scaled for distance units.
			CStdFPoint m_oReportWorldPosition;

			///The rotation to apply to this body part. It is defined by the three
			///euler angles in radians.
			CStdFPoint m_oRotation;

			///This is used for reporting the rotation back to the GUI. We need to keep the
			///regular rotation information so it can be used during a simulation reset.
			CStdFPoint m_oReportRotation;

			///Determines if this body is physically seen or not. If this is false then 
			///whatever geometry this is, like a box, is not seen in the graphics.
			bool m_bIsVisible;

			//Used to report back if visible.
			float m_fltReportIsVisible;

			//Transparencies
			/// The alpha transparency used in the Graphics VisualSelectionMode
			float m_fltGraphicsAlpha;

			/// The alpha transparency used in the Collisions VisualSelectionMode
			float m_fltCollisionsAlpha;

			/// The alpha transparency used in the Joints VisualSelectionMode
			float m_fltJointsAlpha;

			/// The alpha transparency used in the Receptive Fields VisualSelectionMode
			float m_fltReceptiveFieldsAlpha;

			/// The alpha transparency used in the Simulation VisualSelectionMode
			float m_fltSimulationAlpha;

			/// The current alpha transparency for this body part.
			float m_fltAlpha; //Current alpha

			///The ambient color to apply to this part. It is specified as red, green, blue, and alpha.
			CStdColor m_vAmbient;

			///The diffuse color to apply to this part. It is specified as red, green, blue, and alpha.
			CStdColor m_vDiffuse;

			///The specular color to apply to this part. It is specified as red, green, blue, and alpha.
			CStdColor m_vSpecular;

			///The shininess of the part. A value between 0 and 128. 
			float m_fltShininess;

			///An optional texture to apply to the rigid body.
			std::string m_strTexture;

			/// The relative position of the selected vertex
			CStdFPoint m_vSelectedVertex;

			/// This is an interface pointer to a callback class that allows us to notify the gui
			/// of events that occur within the simulation.
			IMovableItemCallback *m_lpCallback;

			/// This is an interface references to the Vs version of this object.
			/// It will allow us to call methods directly in the Vs (OSG) version of the object
			/// directly without having to overload a bunch of methods in each box, sphere, etc..
			IPhysicsMovableItem *m_lpPhysicsMovableItem;

			/// User defined drag handle radius. If this is -1 then the user has not set the value and the default is used.
			float m_fltUserDefinedDraggerRadius;

            ///Keeps track of whether this movable item has been selected or not.
            bool m_bIsSelected;

			virtual void LoadPosition(CStdXml &oXml);
			virtual void LoadRotation(CStdXml &oXml);

		public:
			MovableItem(void);
			virtual ~MovableItem(void);

#pragma region AccessorMutators

			RigidBody *Parent();
			void Parent(RigidBody *lpValue);

			virtual int VisualSelectionType() ;
			virtual bool AllowMouseManipulation();

			virtual CStdFPoint Position();
			virtual void Position(CStdFPoint &oPoint, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
			virtual void Position(float fltX, float fltY, float fltZ, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
			virtual void Position(std::string strXml, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);

			virtual CStdFPoint AbsolutePosition();
			virtual void AbsolutePosition(CStdFPoint &oPoint);
			virtual void AbsolutePosition(float fltX, float fltY, float fltZ);

			virtual CStdFPoint ReportPosition();
			virtual void ReportPosition(CStdFPoint &oPoint);
			virtual void ReportPosition(float fltX, float fltY, float fltZ);

			virtual CStdFPoint ReportWorldPosition();
			virtual void ReportWorldPosition(CStdFPoint &oPoint);
			virtual void ReportWorldPosition(float fltX, float fltY, float fltZ);

			virtual CStdFPoint UpdateAbsolutePosition();
			virtual CStdFPoint GetCurrentPosition();

			virtual CStdFPoint Rotation();
			virtual void Rotation(CStdFPoint &oPoint, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
			virtual void Rotation(float fltX, float fltY, float fltZ, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
			virtual void Rotation(std::string strXml, bool bFireChangeEvent = false, bool bUpdateMatrix = true);

			virtual CStdFPoint ReportRotation();
			virtual void ReportRotation(CStdFPoint &oPoint);
			virtual void ReportRotation(float fltX, float fltY, float fltZ);

            virtual std::string LocalTransformationMatrixString();

			virtual bool IsVisible();
			virtual void IsVisible(bool bVal);

			virtual float GraphicsAlpha();
			virtual void GraphicsAlpha(float fltVal);

			virtual float CollisionsAlpha();
			virtual void CollisionsAlpha(float fltVal);

			virtual float JointsAlpha();
			virtual void JointsAlpha(float fltVal);

			virtual float ReceptiveFieldsAlpha();
			virtual void ReceptiveFieldsAlpha(float fltVal);

			virtual float SimulationAlpha();
			virtual void SimulationAlpha(float fltVal);

			virtual float Alpha();
			virtual void Alpha(float fltAlpha);

			virtual CStdColor *Ambient();
			virtual void Ambient(CStdColor &aryColor);
			virtual void Ambient(float *aryColor);
			virtual void Ambient(std::string strXml);

			virtual CStdColor *Diffuse();
			virtual void Diffuse(CStdColor &aryColor);
			virtual void Diffuse(float *aryColor);
			virtual void Diffuse(std::string strXml);

			virtual CStdColor *Specular();
			virtual void Specular(CStdColor &aryColor);
			virtual void Specular(float *aryColor);
			virtual void Specular(std::string strXml);

			virtual float Shininess();
			virtual void Shininess(float fltVal);

			virtual std::string Texture();
			virtual void Texture(std::string strValue);
	
			virtual CStdFPoint SelectedVertex();
			virtual void SelectedVertex(CStdFPoint &vPoint, bool bFireChangeEvent = false, bool bUpdatePhysics = true);
			virtual void SelectedVertex(float fltX, float fltY, float fltZ, bool bFireChangeEvent = false, bool bUpdatePhysics = true);

			virtual IMovableItemCallback *Callback();
			virtual void Callback(IMovableItemCallback *lpCallback);

			virtual IPhysicsMovableItem *PhysicsMovableItem();
			virtual void PhysicsMovableItem(IPhysicsMovableItem *lpBase);

			virtual float GetBoundingRadius();
			virtual BoundingBox GetBoundingBox();
			virtual void SetBoundingBox(int iIdx, float fltVal);

			virtual bool AllowTranslateDragX();
			virtual bool AllowTranslateDragY();
			virtual bool AllowTranslateDragZ();

			virtual bool AllowRotateDragX();
			virtual bool AllowRotateDragY();
			virtual bool AllowRotateDragZ();

			virtual void UserDefinedDraggerRadius(float fltRadius);
			virtual float UserDefinedDraggerRadius();

            virtual bool IsSelected();

#pragma endregion

#pragma region DataAccesMethods

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

			virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
			virtual void Selected(bool bValue, bool bSelectMultiple); 
			virtual void VisualSelectionModeChanged(int iNewMode);
			virtual void OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm);
			virtual bool CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
