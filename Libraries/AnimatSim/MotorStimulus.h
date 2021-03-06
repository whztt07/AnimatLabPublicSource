/**
\file	MotorStimulus.h

\brief	Declares the vs motor velocity stimulus class.
**/

#pragma once

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT MotorStimulus  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			std::string m_strStructureID;
			std::string m_strJointID;
			MotorizedJoint *m_lpJoint;
			int m_iTargetID;

			float *m_lpPosition;
			float *m_lpVelocity;

			std::string m_strEquation;
			CStdPostFixEval *m_lpEval;

			bool m_bDisableMotorWhenDone;

			float m_fltValue;
			float m_fltValueReport;

		public:
			MotorStimulus();
			virtual ~MotorStimulus();

			static MotorStimulus *CastToDerived(AnimatBase *lpBase) {return static_cast<MotorStimulus*>(lpBase);}

			std::string StructureID() {return m_strStructureID;};
			void StructureID(std::string strVal) {m_strStructureID = strVal;};

			std::string JointID() {return m_strJointID;};
			void JointID(std::string strVal) {m_strJointID = strVal;};

			std::string Equation() {return m_strEquation;};
			void Equation(std::string strVal);

			int TargetID() {return m_iTargetID;}
			void TargetID(int iID);
			void TargetID(std::string strID);

			bool DisableMotorWhenDone() {return m_bDisableMotorWhenDone;};
			void DisableMotorWhenDone(bool bVal) {m_bDisableMotorWhenDone = bVal;};

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual std::string Type() {return "MotorVelocity";};
			virtual void Initialize();
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		};

	}			//ExternalStimuli
}				//AnimatSim
