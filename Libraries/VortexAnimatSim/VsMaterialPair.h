// VsMaterialPair.h: interface for the VsMaterialPair class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsMaterialPair : public AnimatSim::Environment::MaterialPair
		{
		protected:
			VxMaterialTable *m_vxMaterialTable;

		public:
			VsMaterialPair();
			virtual ~VsMaterialPair();

			virtual int GetMaterialID(string strName);
			virtual void RegisterMaterialTypes(CStdArray<string> aryMaterialTypes);
			virtual void Initialize();
		};

	}			// Visualization
}				//VortexAnimatSim
