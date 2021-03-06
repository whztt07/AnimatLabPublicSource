/**
\file	ReceptiveField.h

\brief	Declares the receptive field class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Receptive field that generates current based on the amount of contact force, and how close it is to the center of the field.

		\details 
		
		\author	dcofer
		\date	3/24/2011
		**/
		class ANIMAT_PORT ReceptiveField : public AnimatBase 
		{
		public:
			/// The vertex of the center of the receptive field in global coordinates.
			StdVector3 m_vVertex;

			/// The accumulated current for this receptive field.
			float m_fltCurrent;

			ReceptiveField();
			ReceptiveField(float fltX, float fltY, float fltZ, float fltStim);
			virtual ~ReceptiveField();

			static ReceptiveField *CastToDerived(AnimatBase *lpBase) {return static_cast<ReceptiveField*>(lpBase);}

			void SetVertex(CStdFPoint vPoint);

			bool operator<(ReceptiveField *lpItem);
			bool operator>(ReceptiveField *lpItem);
			bool operator==(ReceptiveField *lpItem);
			bool LessThanThan(float fltX, float fltY, float fltZ);
			bool GreaterThanThan(float fltX, float fltY, float fltZ);
			bool Equals(float fltX, float fltY, float fltZ);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
