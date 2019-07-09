@echo off
echo This tool will create mirror of Unity Project suitable for multiplayer testing.
set /p @targetName="Enter name of mirror project: "

set @root=%~dp0
set @pathRelativeToUnityProjectRootDir=client\
set @dirsToLink=Assets Packages ProjectSettings
set @mirrorProjectPath=..\..\%@targetName%\

echo %@mirrorProjectPath%
echo %@root%

if not exist "%@mirrorProjectPath%" mkdir "%@mirrorProjectPath%"

echo Created %@mirrorProjectPath%

setlocal EnableDelayedExpansion

(for %%a in (%@dirsToLink%) do ( 
	set @sourceDirPath=%@root%%@pathRelativeToUnityProjectRootDir%%%a\
	set @targetDirPath=%@root%%@mirrorProjectPath%%%a\
	
	if not exist "!@sourceDirPath!" (
		echo ERROR: source path !@sourceDirPath! does not exist. Terminating.
		pause
	) 
	
	mklink /J !@targetDirPath! !@sourceDirPath!   
))

echo Done

pause