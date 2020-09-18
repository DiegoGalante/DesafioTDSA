REM Create a 'RelatoriosGerados' folder if it does not exist
REM Cria a pasta 'RelatoriosGerados' se não existir
if not exist "%~dp0RelatoriosGerados" mkdir "%~dp0RelatoriosGerados"
 
REM Remove any previously created test output directories
REM Remove qualquer teste anteriormente criado no diretório de saída
CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"
 
REM Atribuí o nome do caminho padrão do usuario
set meuCaminho=C:\Users\%USERNAME%
 
REM (INATIVADO) Roda os testes novamente do OpenCover para gerar as metricas
REM call :RunOpenCoverUnitTestMetrics
call :RodaMetricaDeTestesOpenCover
 
REM Gera a saída do relatório baseado nos resultados dos testes 
if %errorlevel% equ 0 (
 call :GeraRelatorioDeSaida
)
 
REM Exibe o relatório
if %errorlevel% equ 0 (
 call :ExibeRelatorio
)
exit /b %errorlevel%
 
:RodaMetricaDeTestesOpenCover
echo "(RodaMetricaDeTestesOpenCover -> Metodo Inativado no momento)" ^
REM OpenCover.Console.exe -register:user -target:"C:/Program Files/dotnet/dotnet.exe" -targetargs:test -filter:"+[TDSA.Business*]* -[TDSA.Test*]*" -output:".\tests\RelatoriosGerados\TDSA_Cobertura.xml" -oldstyle
exit /b %errorlevel%
 
:GeraRelatorioDeSaida
echo "Gera Relatorio de Saída ~\RelatoriosGerados\TDSA_Cobertura.xml"
"%meuCaminho%\.nuget\packages\reportgenerator\4.6.7\tools\netcoreapp3.0\ReportGenerator.exe" ^
-reports:"%~dp0RelatoriosGerados\TDSA_Cobertura.xml" ^
-targetdir:"%~dp0RelatoriosGerados\RelatorioGerado Saida"
exit /b %errorlevel%
 
:ExibeRelatorio
echo "Exibe Relatório no Browser"
start "report" "%~dp0RelatoriosGerados\RelatorioGerado Saida\index.htm"
exit /b %errorlevel%