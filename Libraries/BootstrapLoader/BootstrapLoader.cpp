// BootstrapLoader.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"

typedef int (*RunLibrary)(int argc, const char **argv); 

#ifdef WIN32

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    return TRUE;
}

/**
\brief	Finds if a given file exists.

\author	dcofer
\date	8/29/2013

\param	strFullPath	   	Full pathname of the string full file. 

**/
bool FileExists(std::string strFullPath)
{
    try 
    {
	    boost::filesystem::path path = boost::filesystem::canonical(strFullPath);
        return true;
    } 
    catch(const boost::filesystem::filesystem_error& e)
    {
       if(e.code() == boost::system::errc::permission_denied)
           std::cout << "Search permission is denied for one of the directories "
                     << "in the path prefix of " << "\n";
       else
           std::cout << "is_directory("  << ") failed with "
                     << e.code().message() << '\n';
       return false;
    }
}

int FindRunLibrary(const std::string &strSimFile, std::string &strRunLibrary)
{
    std::ifstream ifSimFile;
    char sBuffer[1000]; 

    if(!FileExists(strSimFile))
    {
        std::cout << "No simulation file '" << strSimFile << "' was found.\n";
        return -1;
    }

    ifSimFile.open(strSimFile);

    if(ifSimFile.is_open())
    {
        ifSimFile.read(sBuffer, 1000);       // read the first 1000 chars. Assume the sim lib text is in it.
        ifSimFile.close();

        std::string strText = sBuffer;

        if(strText.length() == 0)
        {
            std::cout << "Simulation file '" << strSimFile << "' is empty.\n";
            return -1;
        }

        int iModuleStart = strText.find("<AnimatModule>");
        int iModuleEnd = strText.find("</AnimatModule>");

        if(iModuleStart == -1)
        {
            std::cout << "<AnimatModule> tag was not found within the first 1000 characters of the simulation file '" << strSimFile << "'.\n";
            return -1;
        }

        if(iModuleEnd == -1)
        {
            std::cout << "</AnimatModule> tag was not found within the first 5000 characters of the simulation file '" << strSimFile << "'.\n";
            return -1;
        }

        int iLen = iModuleEnd - iModuleStart - 14; //Take off the <AnimatModule>

        strRunLibrary = strText.substr((iModuleStart+14), iLen);
    }
    else
    {
        std::cout << "Unable to open the simulation file '" << strSimFile << "'.\n";
        return -1;
    }

    return 0;
}

void ShowErrorText()
{
    std::cout << "Incorrect usage.\n";
    std::cout << "animatsimulator sim_file.\n";
}

/**
\brief	Boot strap method to run an arbitary library.

\author	dcofer
\date	4/7/2011

\exception	strError	Thrown when string error. 

\param	argc	The argc. 
\param	argv	The argv. 

\return	.
**/
int BOOTSTRAP_LOADER_PORT BootStrap_RunLibrary(int argc, const char **argv)
{
	int iParam=0;
	bool bRetrieved=false, bFound = false;

    if(argc != 2)
        ShowErrorText();

    std::string strSimFile = argv[1];
    std::string strRunLibrary = "";
    
    int iRet = FindRunLibrary(strSimFile, strRunLibrary);
    if(iRet != 0)
        return iRet;

	HMODULE hMod = LoadLibrary(strRunLibrary.c_str());
	
	if(!hMod)
	{
        cout << "Unable to load the library module named '" << strRunLibrary << "'.";
		return -1;
	}

	RunLibrary lpRunLibrary = NULL;
	lpRunLibrary = (RunLibrary) GetProcAddress(hMod, "BootstrapRunLibrary"); 

	if(!lpRunLibrary)
	{
        cout << "Unable to obtain a pointer to the run library entry function in library '" << strRunLibrary << "'.";
		return -1;
	}

	return lpRunLibrary(argc, argv);
}


#else

/**
\brief	Boot strap method to run an arbitary library.

\author	dcofer
\date	4/7/2011

\exception	strError	Thrown when string error. 

\param	argc	The argc. 
\param	argv	The argv. 

\return	.
**/
int BOOTSTRAP_LOADER_PORT BootStrap_RunLibrary(int argc, const char **argv)
{
	int iParam=0;
	bool bRetrieved=false, bFound = false;
	char strParam[200], strLibrary[200], strError[200];

	//Clear out the library first.
	strcpy(strLibrary, "");

	while(!bRetrieved && iParam<argc)
	{
		strcpy(strParam, argv[iParam]);

		if(bFound)
		{
			strcpy(strLibrary, strParam);
			bRetrieved = TRUE;
		}

		if(_stricmp(strParam, "-library") == 0 || _stricmp(strParam, "-Library") == 0)
			bFound = TRUE;

		iParam++;
	}

	if(strlen(strLibrary) == 0)
		throw "No library was specified on the command line.";
	
	void *hMod = NULL;

	hMod = dlopen(strLibrary, RTLD_LAZY);
	
	if(!hMod)
	{
		sprintf(strError, "Unable to load the library module named '%s'. Error: %s", strLibrary, dlerror());
		cout << strError << "\r\n\r\n";
		throw strError;
	}

	RunLibrary lpRunLibrary = NULL;
	lpRunLibrary = (RunLibrary) dlsym(hMod, "BootstrapRunLibrary");

	if(!lpRunLibrary || dlerror() != NULL)
	{
		sprintf(strError, "Unable to obtain a pointer to the run library entry function in library '%s'. Error: %s", strLibrary, dlerror());
		throw strError;
	}

	return lpRunLibrary(argc, argv);
}

#endif
