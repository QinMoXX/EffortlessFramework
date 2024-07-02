set WORKSPACE=.\
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set UNITY=..\Unity

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-bin ^
    -d bin  ^
    --conf %WORKSPACE%\DataTables\luban.conf ^
    -x outputDataDir=%UNITY%\Assets\Res\Table ^
    -x outputCodeDir=%UNITY%\Assets\Src\HotUpdate\Table

pause