echo "Generating VortexAnimatSim Mono Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua monodevelop
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\bin</LibPath>" "<LibPath>../../../../bin</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\StdUtils</Include>" "<Include>../../../StdUtils</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\AnimatSim</Include>" "<Include>../../../AnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "</OutputPath>" "</OutputPath><ExtraCompilerArguments>-std=c++0x -Wl,-rpath,'$ORIGIN'</ExtraCompilerArguments>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\Vortex_5_1\3rdparty\osg-2.8.3\include</Include>" "<Include>../../../Vortex_5_1/3rdparty/osg-2.8.3/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\Vortex_5_1\3rdparty\osg-2.8.3\lib</LibPath>" "<LibPath>../../../Vortex_5_1/3rdparty/osg-2.8.3/lib</LibPath>"

@pause