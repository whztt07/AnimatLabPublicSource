echo "Generating StdUtils codeblocks Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua codeblocks
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\bin</LibPath>" "<LibPath>../../../../bin</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\StdUtils</Include>" "<Include>../../../StdUtils</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "</OutputPath>" "</OutputPath><ExtraCompilerArguments>-std=c++0x -Wl,-rpath,'$ORIGIN'</ExtraCompilerArguments>"

@pause