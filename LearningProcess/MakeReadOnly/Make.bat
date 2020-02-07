set "LocalInstaller=%~dp0%готовые установщики"
set "CIStart=%~dp0%всякоразное\ci.cmd"

::Далее по маске:
::путь до папки копирования установщика
::вывзов собирателя установщиков, название проекта, тип установщика, локальный путь установщика, дополнительный путь установщика
::...
call "%CIStart%" LP FULL "%LocalInstaller%" "%LocalInstaller%"

>> "%~1" rem/
xcopy /S /Q /Y /F "%LocalInstaller%\запусти меня (LPFULL).exe" "%~1"

"%~2"

pause