call .\prerequisites-dotnet.bat
call .\bootstrap.bat
_powerup\build\nant\nant-40\bin\nant -buildfile:main.build %*