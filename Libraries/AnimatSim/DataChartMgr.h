/**
\file	DataChartMgr.h

\brief	Declares the data chart manager class.
**/

#pragma once

namespace AnimatSim
{
	namespace Charting
	{
		/**
		\brief	Manager for data charts. 

		\details This class is derived from ActivatedItemMgr. It is responisble for maintaining a set of data charts.
		Each chart can have its own start/end times. 
		
		\author	dcofer
		\date	3/18/2011
		**/
		class ANIMAT_PORT DataChartMgr : public ActivatedItemMgr  
		{
		protected:

			DataChart *LoadDataChart(CStdXml &oXml);

		public:
			DataChartMgr();
			virtual ~DataChartMgr();

			virtual bool AddDataChart(string strXml);
			virtual bool RemoveDataChart(string strID);

			virtual void AddDataColumn(string strChartKey, DataColumn *lpColumn);
			virtual void RemoveDataColumn(string strChartKey, string strColumnName, bool bThrowError = true);
			virtual DataColumn *FindDataColumn(string strChartKey, string strColumnName, bool bThrowError = true);

			virtual void Load(CStdXml &oXml);
		};

	}			//Charting
}				//AnimatSim
