cd %JENKINS_DATA%\workspace\%JOB_NAME%\
"%UNITYPATH%\Unity.exe" -batchmode -quit -logFile stdout.log -projectPath %cd%  -buildWindows64Player "%cd%\builds\mygame.exe"
