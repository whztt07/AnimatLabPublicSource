/**
\file	ConstraintRelaxation.h

\brief	Declares the material pair class.
**/

#pragma once

namespace AnimatSim
{

	namespace Environment
	{
		/**
		\brief	A material type name.
				
		\author	dcofer
		\date	3/22/2011
		**/
		class ANIMAT_PORT ConstraintRelaxation : public AnimatBase
		{
		protected:
            ///The constraint coordinate ID 
            int m_iCoordinateID;

			/// The compliance of the collision between those two materials.
			float m_fltStiffness;

			/// The damping of the collision between those two materials.
			float m_fltDamping;

			virtual void SetRelaxationProperties() = 0;

		public:
			ConstraintRelaxation();
			virtual ~ConstraintRelaxation();
			
			static ConstraintRelaxation *CastToDerived(AnimatBase *lpBase) {return static_cast<ConstraintRelaxation*>(lpBase);}

            virtual int CoordinateID();
            virtual void CoordinateID(int iVal);

            virtual bool Enabled();
            virtual void Enabled(bool bVal);

    		virtual float Stiffness();
			virtual void Stiffness(float fltVal, bool bUseScaling = true);

			virtual float Damping();
			virtual void Damping(float fltVal, bool bUseScaling = true);

			virtual void CreateDefaultUnits();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void Load(CStdXml &oXml);
        };

	}			// Visualization
}				//VortexAnimatSim
