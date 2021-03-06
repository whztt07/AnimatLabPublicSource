/**
\file	ContactSensor.h

\brief	Declares the contact sensor class.
**/

#pragma once


namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Contact sensor for detecting receptive field contacts.

		\details Receptive fields can be defined over the surface of the body, and each one can be associated with
		one or more neurons. ContactSensor is used by the RigidBody part to manage the receptive fields and contacts.
		It contains the list of all receptive fields for that body part, and the other knowledge needed to convert 
		a contact into a current within a neuron. 
		
		\author	dcofer
		\date	3/22/2011
		**/
		class ANIMAT_PORT ContactSensor : public AnimatBase  
		{
		protected:
			/// The array of ReceptiveField objects.
			CStdPtrArray<ReceptiveField> m_aryFields;

			/// The Gain function that modulates the force of contact based on the distance from the center of the ReceptiveField.
			Gain *m_lpFieldGain;

			/// The Gain that calculates the amount of current to apply to any associated neurons based on the modulated force value.
			Gain *m_lpCurrentGain;

			/// The maximum force force that we can use when calculating the current.
			float m_fltMaxForce;

			void LoadReceptiveField(CStdXml &oXml);

			bool FindReceptiveField(CStdPtrArray<ReceptiveField> &aryFields, float fltX, float fltY, float fltZ, int &iIndex);

			void DumpVertices(CStdPtrArray<ReceptiveField> &aryFields);

			virtual void AddReceptiveField(std::string strXml);
			virtual void RemoveReceptiveField(std::string strID, bool bThrowError = true);
			virtual int FindReceptiveFieldListPos(std::string strID, bool bThrowError = true);

		public:
			ContactSensor();
			virtual ~ContactSensor();
			
			static ContactSensor *CastToDerived(AnimatBase *lpBase) {return static_cast<ContactSensor*>(lpBase);}

			Gain *FieldGain();
			Gain *CurrentGain();

			ReceptiveField *GetReceptiveField(int iIndex);
			bool FindReceptiveField(float fltX, float fltY, float fltZ, int &iIndex);
			int FindClosestReceptiveField(float fltX, float fltY, float fltZ);
			void FinishedAddingVertices();

#pragma region DataAccesMethods

			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

#pragma endregion

			void ClearCurrents();
			void ProcessContact(StdVector3 vPos, float fltForceMagnitude);
			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
