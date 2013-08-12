-- A solution contains projects, and defines the available configurations
solution "AnimatSimCode"
	configurations { "Debug", "Release" }
	dofile "..\\Libraries\\BootstrapLoader\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\StdUtils\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\AnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\FiringRateSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\IntegrateFireSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\OsgAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\VortexAnimatSim\\Projects_VisualStudio\\Projects.lua"
