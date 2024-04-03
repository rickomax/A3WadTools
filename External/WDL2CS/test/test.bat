@echo off
echo %TIME% Running Parser...
echo.
for %%f in (*.wdl) do ..\code\bin\debug\wdl2cs.exe %%f || echo. %%f
echo.
echo %TIME% Finished.