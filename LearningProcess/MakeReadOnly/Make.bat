set "LocalInstaller=%~dp0%��⮢� ��⠭��騪�"
set "CIStart=%~dp0%��类ࠧ���\ci.cmd"

::����� �� ��᪥:
::���� �� ����� ����஢���� ��⠭��騪�
::�뢧�� ᮡ��⥫� ��⠭��騪��, �������� �஥��, ⨯ ��⠭��騪�, ������� ���� ��⠭��騪�, �������⥫�� ���� ��⠭��騪�
::...
call "%CIStart%" LP FULL "%LocalInstaller%" "%LocalInstaller%"

>> "%~1" rem/
xcopy /S /Q /Y /F "%LocalInstaller%\������ �����(LPFULL).exe" "%~1"

"%~2"

pause