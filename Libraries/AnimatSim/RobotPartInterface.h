#pragma once

namespace AnimatSim
{
	namespace Robotics
	{

		class ANIMAT_PORT RobotPartInterface : public AnimatBase
		{
		protected:
			RobotInterface *m_lpParentInterface;
			RobotIOControl *m_lpParentIOControl;

			BodyPart *m_lpPart;
			std::string m_strPartID;

			float *m_lpProperty;
			std::string m_strPropertyName;

			/// Pointer to the Gain that will be used to convert the source value into the target value.
			Gain *m_lpGain;

			///This is a generic ID value to be used for this part. This will be the servo ID for a servo,
			///or the pin number for a digital ouput. Its meaning depends on the part type.
			int m_iIOComponentID;

			///This is a generic variable that keeps track of the data that will be sent/recieved.
			///If it is a digital value then you will need to cast it appropriately.
			float m_fltIOValue;

			///This stores the temporary IO value to be used later in IO calculations. 
			int m_iIOValue;

			///This is a generic variable that keeps track of the data that was sent/received AFTER it has been scled
			///using the gain function.
			float m_fltIOScaledValue;

			///True when the value changes.
			bool m_bChanged;

			//The time it takes to perform a step of the IO for this part.
			float m_fltStepIODuration;

			virtual void AddGain(std::string strXml);

		public:
			RobotPartInterface(void);
			virtual ~RobotPartInterface(void);
						
			static RobotPartInterface *CastToDerived(AnimatBase *lpBase) {return static_cast<RobotPartInterface*>(lpBase);}

			virtual void ParentIOControl(RobotIOControl *lpParent);
			virtual RobotIOControl *ParentIOControl();

			virtual int IOComponentID();
			virtual void IOComponentID(int iID);
			
			virtual float IOValue();
			virtual void IOValue(float fltVal);
			
			virtual int IOValueInt();
			virtual void IOValueInt(int iVal);
			
			virtual float IOScaledValue();
			virtual void IOScaledValue(float fltVal);
			
			virtual bool Changed();
			virtual void Changed(bool bVal);

			virtual void LinkedPartID(std::string strID);
			virtual std::string LinkedPartID();
			
			virtual void PropertyName(std::string strName);
			virtual std::string PropertyName();

			virtual bool IsMotorControl();
			virtual bool IncludeInPartsCycle() {return false;}

			virtual float QuantizeServoPosition(float fltPos);
			virtual float QuantizeServoVelocity(float fltVel);

			virtual Gain *GetGain();
			virtual void SetGain(Gain *lpGain);

			virtual float StepIODuration();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

			virtual void SetupIO();
			virtual void StepIO(int iPartIdx);
			virtual void ShutdownIO();

			virtual void Initialize();
			virtual void ResetSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}