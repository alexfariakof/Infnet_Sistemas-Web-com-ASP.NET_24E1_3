$projectTestPath = Get-Location
$projectPath =  (Resolve-Path -Path ..).Path
$projectAngular = (Resolve-Path -Path "$projectPath\AngularApp");
$sourceDirs = "$projectPath\Application;$projectPath\Domain;$projectPath\Repository;$projectPath\WebApi;"
$reportPath = Join-Path -Path $projectTestPath -ChildPath "TestResults"
$coverageXmlPath = Join-Path -Path (Join-Path -Path $projectTestPath -ChildPath "TestResults") -ChildPath "coveragereport"

# Excuta Teste Unitarios sem restore e build e gera o relatório de cobertura do Backend
dotnet test ./XunitTest.csproj --results-directory $reportPath /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura --collect:"XPlat Code Coverage;Format=opencover" --no-restore --no-build > $null 2>&1
reportgenerator -reports:$projectTestPath\coverage.cobertura.xml -targetdir:$coverageXmlPath -reporttypes:"Html;lcov;" -sourcedirs:$sourceDirs > $null 2>&1

cd $projectAngular

# Verifica se existe a pasta node_module, e sem não existir executa npm install 
if (-not (Test-Path $projectAngular\node_modules)) {
	npm install
}

# Executa Teste Unitários e gera o relatório de cobertura do Frontend 
npm run test:coverage > $null 2>&1