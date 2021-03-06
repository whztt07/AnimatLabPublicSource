
	project "AnimatCarlSimCUDA"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp",
				 "../*.cu"}
		buildoptions { "-std=c++0x" }
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../AnimatSim",
					  "../../../../3rdParty/CARLsim/src",
					  "/usr/local/cuda/include", 
					  "/home/ubuntu/NVIDIA_CUDA-6.0_Samples/common/inc" }
		libdirs { "../../../bin",
				  "../../../../3rdParty/CARLsim/bin",
				  "/usr/local/cuda/lib" }
		links { "dl", "cudart" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "ANIMATCARLSIM_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatCarlSimCUDA_debug")
			links { "StdUtils_debug", 
					"AnimatSim_debug", 
					"CarlSim_debug"}
			postbuildcommands { "cp Debug/libAnimatCarlSimCUDA_debug.so ../../../bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "ANIMATCARLSIM_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatCarlSimCUDA")
			links { "StdUtils",
					"AnimatSim", 
					"CarlSim"}
			postbuildcommands { "cp Release/libAnimatCarlSimCUDA.so ../../../bin" }
