/**
\file	SigmoidGain.cpp

\brief	Implements the sigmoid gain class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "SigmoidGain.h"

namespace AnimatSim
{
	namespace Gains
	{

/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
SigmoidGain::SigmoidGain()
{
	m_fltA = 0;
	m_fltB = 0;
	m_fltC = 0;
	m_fltD = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
SigmoidGain::~SigmoidGain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of SigmoidGain\r\n", "", -1, false, true);}
}

/**
\brief	Gets A parameter of sigmoidal eqation: Out = D+(B/(1+e^(C*(A-In))))

\author	dcofer
\date	3/16/2011

\return	A param. 
**/
float SigmoidGain::A() {return m_fltA;}

/**
\brief	Sets A parameter of sigmoidal eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void SigmoidGain::A(float fltVal) {m_fltA = fltVal;}

/**
\brief	Gets B parameter of sigmoidal eqation: Out = D+(B/(1+e^(C*(A-In))))

\author	dcofer
\date	3/16/2011

\return	B param. 
**/
float SigmoidGain::B() {return m_fltB;}

/**
\brief	Sets B parameter of sigmoidal eqation: Out = D+(B/(1+e^(C*(A-In))))

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void SigmoidGain::B(float fltVal) {m_fltB = fltVal;}

/**
\brief	Gets C parameter of sigmoidal eqation: Out = D+(B/(1+e^(C*(A-In))))

\author	dcofer
\date	3/16/2011

\return	C param. 
**/
float SigmoidGain::C() {return m_fltC;}

/**
\brief	Sets C parameter of sigmoidal eqation: Out = D+(B/(1+e^(C*(A-In))))

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void SigmoidGain::C(float fltVal) {m_fltC = fltVal;}

/**
\brief	Sets D parameter of sigmoidal eqation: Out = D+(B/(1+e^(C*(A-In))))

\author	dcofer
\date	3/16/2011

\return	D param. 
**/
float SigmoidGain::D() {return m_fltD;}

/**
\brief	Sets D parameter of sigmoidal eqation: Out = D+(B/(1+e^(C*(A-In))))

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void SigmoidGain::D(float fltVal) {m_fltD = fltVal;}

void SigmoidGain::Copy(CStdSerialize *lpSource)
{
	Gain::Copy(lpSource);

	SigmoidGain *lpOrig = dynamic_cast<SigmoidGain *>(lpSource);

	m_fltA = lpOrig->m_fltA;
	m_fltB = lpOrig->m_fltB;
	m_fltC = lpOrig->m_fltC;
	m_fltD = lpOrig->m_fltD;
}

CStdSerialize *SigmoidGain::Clone()
{
	CStdSerialize *lpClone = new SigmoidGain();
	lpClone->Copy(this);
	return lpClone;
}

float SigmoidGain::CalculateGain(float fltInput)
{
	if(InLimits(fltInput))
		return ((m_fltB/(1+exp(m_fltC*(m_fltA-fltInput)))) + m_fltD);
	else
		return CalculateLimitOutput(fltInput);
}


bool SigmoidGain::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(Gain::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "A")
	{
		A((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "B")
	{
		B((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "C")
	{
		C((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "D")
	{
		D((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void SigmoidGain::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Gain::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("A", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("B", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("C", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("D", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

void SigmoidGain::Load(CStdXml &oXml)
{
	Gain::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	A(oXml.GetChildFloat("A"));
	B(oXml.GetChildFloat("B"));
	C(oXml.GetChildFloat("C"));
	D(oXml.GetChildFloat("D"));

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Gains
}			//AnimatSim
