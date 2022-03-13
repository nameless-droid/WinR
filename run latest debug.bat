cd "bin\Debug"

for /f %%i in ('dir /A:D /B') do set LAST=%%i
echo The most recently created folder is %LAST%

cd %LAST%%

"WinR.exe"

::exit
pause

