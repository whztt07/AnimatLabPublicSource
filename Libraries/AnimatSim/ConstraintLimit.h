/**
\file	ConstraintLimit.h

\brief	Declares the constraint limit class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Constraint limit. 

		\details A constraint limit is used to specify the movement range that is allowable for a joint. It typically 
		requires two limits per joint, but it can be more or less.  
		
		\author	dcofer
		\date	3/21/2011
		**/
		class ANIMAT_PORT ConstraintLimit : public AnimatBase
		{
		protected:
			/// Pointer to parent joint
			Joint *m_lpJoint;

			/// The limit position for the constraint. This can be in radians or meters depending on the type of joint.
			float m_fltLimitPos;

			/// The damping for the constraint.
			float m_fltDamping;

			/// The restitution coefficient for the constraint.
			float m_fltRestitution;

			/// The stiffness of the constraint.
			float m_fltStiffness;

			/// If true then this is the lower limit of a pair of ConstraintLimits, else it is the upper limit.
			bool m_bIsLowerLimit;

			/// The color used to display the limit.
			CStdColor m_vColor;

			/// Tells whether this contstraint is actually just being used to show the current position of the joint,
			/// as opposed to being used to show the limit of a constraint.
			bool m_bIsShowPosition;

			/// Sets the limit values of the joint in the child object.
			virtual void SetLimitValues() = 0;

		public:
			ConstraintLimit();
			virtual ~ConstraintLimit();
			
			static ConstraintLimit *CastToDerived(AnimatBase *lpBase) {return static_cast<ConstraintLimit*>(lpBase);}

			/**
			\brief	Gets the limit position.
			
			\author	dcofer
			\date	3/22/2011
			
			\return	limit position.
			**/
			virtual float LimitPos();

			/**
			\brief	Sets the Limit position.
			
			\author	dcofer
			\date	3/22/2011
			
			\param	fltVal			  	The new value. 
			\param	bUseScaling		  	true to use unit scaling. 
			\param	bOverrideSameCheck	true to override the check of whether the currentpos = new pos. 
			**/
			virtual void LimitPos(float fltVal, bool bUseScaling = true, bool bOverrideSameCheck = false);

			/**
			\brief	Sets the limit position using the current value set within the object.
						
			\author	dcofer
			\date	4/11/2011
			**/
			virtual void SetLimitPos() = 0;

			/**
			\brief	Gets the damping value of the contraint.
			
			\author	dcofer
			\date	3/22/2011
			
			\return	damping position.
			**/
			virtual float Damping();

			/**
			\brief	Sets the Damping value of the constraint.
			
			\author	dcofer
			\date	3/22/2011
			
			\param	fltVal	   	The new value. 
			\param	bUseScaling	true to use unit scaling. 
			**/
			virtual void Damping(float fltVal, bool bUseScaling = true);

			/**
			\brief	Gets the restitution coefficient of the constraint.
			
			\author	dcofer
			\date	3/22/2011
			
			\return	restitution coefficient.
			**/
			virtual float Restitution();

			/**
			\brief	Sets the restitution coefficient.
			
			\author	dcofer
			\date	3/22/2011
			
			\param	fltVal	The new value. 
			**/
			virtual void Restitution(float fltVal);

			/**
			\brief	Gets the stiffness of the constraint.
			
			\author	dcofer
			\date	3/22/2011
			
			\return	stiffness.
			**/
			virtual float Stiffness();

			/**
			\brief	Sets the stiffness of the constraint.
			
			\author	dcofer
			\date	3/22/2011
			
			\param	fltVal	   	The new value. 
			\param	bUseScaling	true to use unit scaling. 
			**/
			virtual void Stiffness(float fltVal, bool bUseScaling = true);

			virtual void Color(float fltR, float fltG, float fltB, float fltA);
			virtual CStdColor *Color();
			virtual void Color(std::string strXml);

			/**
			\brief	Sets the alpha color value for this constraint.
			
			\author	dcofer
			\date	3/22/2011
			
			\param	fltA	The new alpha value. 
			**/
			virtual void Alpha(float fltA) = 0;

			/**
			\brief	Gets the alpha value.
			
			\author	dcofer
			\date	3/22/2011
			
			\return	Alpha value.
			**/
			virtual float Alpha();

			virtual void IsLowerLimit(bool bVal);
			virtual bool IsLowerLimit();

			virtual void IsShowPosition(bool bVal);
			virtual bool IsShowPosition();

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, float fltPosition, bool bVerify);
			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual void VerifySystemPointers();
			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual void Load(CStdXml &oXml, std::string strName);

			/**
			\brief	Sets up the graphics for the constraint.
			
			\author	dcofer
			\date	3/22/2011
			**/
			virtual void SetupGraphics() = 0;

			/**
			\brief	Deletes up the graphics for the constraint.
			
			\author	dcofer
			\date	3/22/2011
			**/
			virtual void DeleteGraphics() = 0;
		};

	}			// Environment
}				//AnimatSim
