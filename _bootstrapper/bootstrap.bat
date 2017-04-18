@echo off
if '%1'=='' goto NOPACKAGE
goto RUN

:NOPACKAGE
	echo Package Id is required
	echo e.g. Bootstrap.bat PowerUp
	exit /B
	
:RUN
	powershell -ExecutionPolicy Bypass -inputformat none -command ".\bootstrap.ps1 -packageId %1"

	if %ERRORLEVEL% EQU 0 goto OK  
	if %ERRORLEVEL% GEQ 1 goto ERROR

:ERROR
	echo An error occurred
	exit /B
	
:OK