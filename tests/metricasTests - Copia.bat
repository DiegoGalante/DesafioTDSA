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
"C:\Users\bibos\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"C:\Users\bibos\.nuget\packages\xunit.runner.console\2.4.1\tools\net472\xunit.console.x86.exe"  ^
-targetargs:"%~dp0TDSA.Test\bin\Debug\netcoreapp3.1\TDSA.Test.dll -noshadow" ^
-filter:"+[TDSA.Test*]* -[TDSA.Business]* " ^
-output:"%~dp0GeneratedReports\TDSA.Tests.xml"
PING -n 10 127.0.0.1>nul
exit /b %errorlevel%


:RunReportGeneratorOutput
PING -n 10 127.0.0.1>nul
"C:\Users\bibos\.nuget\packages\reportgenerator\4.5.6\tools\netcoreapp3.0\ReportGenerator.exe" 
-reports:"%~dp0GeneratedReports\TDSA.Tests.xml" ^
-targetdir:"%~dp0GeneratedReports" ^
exit /b %errorlevel%

:RunLaunchReport
PING -n 10 127.0.0.1>nul
start "report" "%~dp0GeneratedReports\index.htm" 
PING -n 10 127.0.0.1>nul
exit /b %errorlevel%

pause					