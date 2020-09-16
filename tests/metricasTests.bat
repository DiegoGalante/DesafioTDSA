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
 
:RunOpenCoverUnitTestMetrics
"C:\Users\bibos\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"C:\Users\bibos\.nuget\packages\xunit.runner.console\2.4.1\tools\net472\xunit.console.x86.exe"  ^
-targetargs:"%~dp0TDSA.Test\bin\Debug\netcoreapp3.1\TDSA.Test.dll -noshadow" ^
-filter:"+[TDSA*]* -[TDSA.Test*]*" ^
-mergebyhash ^
-skipautoprops ^
-output:"%~dp0GeneratedReports\TDSA.Tests.xml"
exit /b %errorlevel%
 
:RunReportGeneratorOutput
"C:\Users\bibos\.nuget\packages\reportgenerator\4.5.6\tools\netcoreapp3.0\ReportGenerator.exe" ^
-reports:"%~dp0GeneratedReports\TDSA.Tests.xml" ^
-targetdir:"%~dp0GeneratedReports\ReportGenerator Output"
exit /b %errorlevel%
 
:RunLaunchReport
start "report" "%~dp0GeneratedReports\ReportGenerator Output\index.htm"
exit /b %errorlevel%