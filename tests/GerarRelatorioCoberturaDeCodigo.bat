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
REM OpenCover.Console.exe -register:user -target:"C:/Program Files/dotnet/dotnet.exe" -targetargs:test -filter:"+[TDSA.Business*]* -[TDSA.Test*]*" -output:".\MyProject_coverage.xml" -oldstyle
exit /b %errorlevel%
 
:RunReportGeneratorOutput
"C:\Users\bibos\.nuget\packages\reportgenerator\4.5.6\tools\netcoreapp3.0\ReportGenerator.exe" ^
-reports:"%~dp0GeneratedReports\TDSA_Cobertura.xml" ^
-targetdir:"%~dp0GeneratedReports\ReportGenerator Output"
exit /b %errorlevel%
 
:RunLaunchReport
start "report" "%~dp0GeneratedReports\ReportGenerator Output\index.htm"
exit /b %errorlevel%