-- A solution contains projects, and defines the available configurations
solution "OsgAnimatSimSim_x64"
	configurations { "Debug_x64", "Release_x64", "Debug_Double_x64", "Release_Double_x64" }
	platforms {"x64"}
	dofile "Projects_x64.lua"
