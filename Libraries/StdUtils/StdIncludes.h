#if !defined STD_UTILS_INCLUDES
#define STD_UTILS_INCLUDES

#ifdef WIN32
	#define STD_UTILS_PORT __declspec( dllexport )

	#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

	#ifndef _WIN32_WCE
		#include <conio.h>
		#include <io.h>
		#include <tchar.h> 
	#endif
#else
	#define STD_UTILS_PORT 

	#include <linux/types.h>
	#include <stdbool.h>
	#include <dlfcn.h>
    #include <limits.h>
    #include <unistd.h>
    #include <utime.h>
    #include <sys/stat.h>

	#define DWORD unsigned long
	#define LPCSTR const char *
	#define LONG long
	#define ULONG unsigned long
	#define LPLONG long *
	#define __int64 int64_t
	#define LPCTSTR const char*
#endif

#pragma warning(disable: 4018 4244 4290 4786 4251 4275 4267 4311 4312 4800 4996 )

#include <sys/types.h>
#include <sys/timeb.h>


#include <exception>
#include <string>
#include <iosfwd>
#include <sstream>
#include <fstream>
#include <iterator>
#include <iostream>
#include <vector>
#include <deque>
#include <stack>
#include <map>
#include <list>
#include <unordered_set>
#include <stdio.h>
#include <stdlib.h>
#include <cctype>
#include <math.h>
#include <memory.h>
#include <algorithm>
#include <string.h>

//Std atomic not included until VS2012, but I need it in linux
//because I need to use boost 1.48 and boost::atomic is not included
//until later.
#ifdef WIN32
    #include <boost/atomic.hpp>

	//For some reason this is not in math.h on windows.
	inline double round( double d )
	{
		return floor( d + 0.5 );
	}

#else
    #include <atomic>
#endif

#include <boost/thread.hpp>
#include <boost/algorithm/string.hpp>
#include <boost/filesystem.hpp>
#include "boost/format.hpp"

#define STD_TRACING_ON
//#define STD_TRACE_DEBUG
//#define STD_TRACE_INFO
#define STD_TRACE_DETAIL

#define STD_TRACE_TO_DEBUGGER
#define STD_TRACE_TO_FILE true

namespace StdUtils
{
	class CStdFont;
	class CStdVariable;
	class CStdPostFixEval;
	class CMarkupSTL;
	class CStdXml;
	class CStdCriticalSection;
	class CStdErrorInfo;
	
	void STD_UTILS_PORT Std_RelayError(CStdErrorInfo oInfo, std::string strSourceFile, long lSourceLine);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strValueName, unsigned char iVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strValueName, unsigned short iVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strValueName, int iVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strValueName, long lVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strValueName, float fltVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strValueName, double dblVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strValueName, std::string strVal);
	void STD_UTILS_PORT Std_ThrowError(long lError, std::string strError, std::string strSourceFile, long lSourceLine, 
																		 std::string strText);
}

#include "tree.hh"
#include "tree_util.hh"

#include "StdConstants.h"
#include "StdADT.h"
#include "StdADTCopy.h"
#include "StdErrorInfo.h"
#include "StdUtilFunctions.h"
#include "MarkupSTL.h"
#include "StdXml.h"
#include "StdSerialize.h"
#include "StdFont.h"
#include "StdVariable.h"
#include "StdPostFixEval.h"
#include "StdVariant.h"
#include "StdClassFactory.h"
#include "StdLookupTable.h"
#include "StdFixed.h"
#include "StdColor.h"
#include "StdCriticalSection.h"

#ifdef WIN32
	#ifndef _WIN32_WCE
		#include "XYTrace.h"
	#else
		#include "StdLogFile.h"
	#endif
#endif

#endif //STD_UTILS_INCLUDES
