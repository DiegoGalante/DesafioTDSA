REM Create a 'GeneratedReports' folder if it does not exist
if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"

REM Remove any previously created test output directories
CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"

REM Run the tests against the targeted output
call :RunOpenCoverUnitTestMetrics

REM Generate the report output based on the test results
if %errorlevel% equ 0 ( 
	call :RunReportGeneratorOutput	
)

REM Launch the report
if %errorlevel% equ 0 ( 
	call :RunLaunchReport	
)
exit /b %errorlevel%

REM :RunOpenCoverUnitTestMetrics
REM "%~dp0packages\OpenCover.4.7.922\tools\OpenCover.Console.exe" ^
REM -register:user ^
REM -target:"%~dp0packages\xunit.runner.console.2.4.1\tools\net452\xunit.console.x86.exe"  ^
REM -targetargs:"%~dp0TDSA.Test\bin\Debug\netcoreapp3.1\TDSA.Test.dll -noshadow" ^
REM -filter:"+[TDSA.Test*]* -[TDSA.Business]* " ^
REM -output:"%~dp0GeneratedReports\TDSA.Tests.xml"
REM PING -n 10 127.0.0.1>nul
REM exit /b %errorlevel%

:RunOpenCoverUnitTestMetrics
"%~dp0packages\opencover\4.7.922\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"C:/Program Files/dotnet/dotnet.exe" 
-targetargs:"%~dp0TDSA.Test\bin\Debug\netcoreapp3.1\TDSA.Test.dll -noshadow" ^
-targetargs:test -filter:"+[TDSA.Business*]* -[TDSA.Test*]*" 
-output:"%~dp0GeneratedReports\TDSA_Cobertura.xml" -oldstyle
PING -n 5 127.0.0.1>nul
exit /b %errorlevel%


:RunReportGeneratorOutput
PING -n 10 127.0.0.1>nul
"%~dp0packages\reportgenerator\4.5.6\tools\netcoreapp3.0\ReportGenerator.exe" 
-reports:"%~dp0GeneratedReports\TDSA_Cobertura.xml" ^
-targetdir:"%~dp0GeneratedReports" ^
exit /b %errorlevel%

:RunLaunchReport
PING -n 5 127.0.0.1>nul
start "report" "%~dp0GeneratedReports\index.htm" 
PING -n 5 127.0.0.1>nul
exit /b %errorlevel%

pause					