echo "Generating VortexAnimatSim Mono Solution"

premake4m --os=linux --file=Solution.lua monodevelop

..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\bin</LibPath>" "<LibPath>../../../../bin</LibPath>"
..\..\..\bin\fart *.cproj "<Include>..\..\..\StdUtils</Include>" "<Include>../../../StdUtils</Include>"
..\..\..\bin\fart *.cproj "<Include>..\..\..\AnimatSim</Include>" "<Include>../../../AnimatSim</Include>"
..\..\..\bin\fart *.cproj "</OutputPath>" "</OutputPath><ExtraCompilerArguments>-std=c++0x -Wl,-rpath,'$ORIGIN'</ExtraCompilerArguments>"
..\..\..\bin\fart *.cproj "<Include>..\..\..\Vortex_5_1\3rdparty\osg-2.8.3\include</Include>" "<Include>../../../Vortex_5_1/3rdparty/osg-2.8.3/include</Include>"
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\Vortex_5_1\3rdparty\osg-2.8.3\lib</LibPath>" "<LibPath>../../../Vortex_5_1/3rdparty/osg-2.8.3/lib</LibPath>"

@pause