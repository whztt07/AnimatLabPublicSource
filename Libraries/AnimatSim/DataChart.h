
#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Charting

	\brief	Namespace for objects related to collecting the data for charts. 
	**/
	namespace Charting
	{

		class ANIMAT_PORT DataChart : public ActivatedItem  
		{
		protected:
			string m_strProjectPath;
			string m_strConfigFilename;
			BOOL m_bSetStartEndTime;
			short m_iCollectInterval;
			long m_lCollectTimeWindow;
			float m_fltCollectTimeWindow;

			CStdMap<string, DataColumn *> m_aryColumnsMap;
			CStdPtrArray<DataColumn> m_aryDataColumns;
			float *m_aryTimeBuffer;
			float *m_aryDataBuffer;

			long m_lColumnCount;
			long m_lRowCount;
			int m_lCurrentCol;
			int m_lCurrentRow;

			virtual long CalculateChartColumnCount();
			DataColumn *LoadDataColumn(CStdXml &oXml);
			virtual DataColumn *FindColumn(string strID, int &iIndex, BOOL bThrowError);

		public:
			DataChart();
			virtual ~DataChart();

			virtual void StartTime(float fltVal);
			virtual void EndTime(float fltVal);

			BOOL SetStartEndTime() {return m_bSetStartEndTime;};
			void SetStartEndTime(BOOL bVal);

			long BufferSize() {return m_lRowCount*m_lColumnCount;};
			long UsedBufferSize() {return m_lCurrentCol*m_lCurrentRow;};

			long BufferByteSize() {return BufferSize()*sizeof(float);};
			long UsedBufferByteSize() {return UsedBufferSize()*sizeof(float);};

			float *TimeBuffer() {return m_aryTimeBuffer;};
			float *DataBuffer() {return m_aryDataBuffer;};

			int CollectInterval() {return m_iCollectInterval;};
			void CollectInterval(int iVal);
			void CollectInterval(float fltVal);

			long CollectTimeWindow() {return m_lCollectTimeWindow;};
			void CollectTimeWindow(long lVal);
			void CollectTimeWindow(float fltVal);

			string ProjectPath() {return m_strProjectPath;};
			void ProjectPath(string strVal) {m_strProjectPath = strVal;};

			long ColumnCount() {return m_lColumnCount;};

			long CurrentRow() {return m_lCurrentRow;};
			virtual void CurrentRow(long iVal) {m_lCurrentRow = iVal;};

			virtual void AddData(int iColumn, int iRow, float fltVal);

			virtual void Load(string strProjectPath, string strConfigFile);
			virtual void Load(CStdXml &oXml);
			virtual void LoadChart(CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "DataChart";};

			virtual void AddColumn(DataColumn *lpColumn);
			virtual void DataChart::AddColumn(string strXml);
			virtual void RemoveColumn(string strID, BOOL bThrowError = TRUE);
			virtual DataColumn *FindColumn(string strID, BOOL bThrowError = TRUE);

#pragma region DataAccesMethods
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

			virtual BOOL operator<(ActivatedItem *lpItem);
			virtual void Initialize();
			virtual void ReInitialize();
			virtual void ResetSimulation();
			virtual void StepSimulation();

			virtual BOOL Lock() {return TRUE;};
			virtual void Unlock() {};

		};

	}			//Charting
}				//AnimatSim
