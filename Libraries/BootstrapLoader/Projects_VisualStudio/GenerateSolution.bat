echo "Generating BootstrapLoader Visual Studio Solution"

..\..\..\bin\premake4 --os=windows --file=Solution.lua vs2010

@pause