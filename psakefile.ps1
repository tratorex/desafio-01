Properties {
	$rootDir = Split-Path $psake.build_script_file
    $solutionName = (ls $rootDir\*.sln)[0].BaseName
    $solutionFile = "$rootDir\$solutionName.sln"
	$srcDir = "$rootDir\src"
    $webDir = "$srcDir\$solutionName.WebApp"
    $testDir = [System.IO.Path]::GetFullPath("$PSScriptRoot\test")
    $unitTestDir = "$testDir\$solutionName.UnitTest"
    $integrationTestDir = "$testDir\$solutionName.IntegrationTest"
    $acceptanceTestDir = "$testDir\$solutionName.AcceptanceTest"
    $stopOnError = $true
    $dontStopTests = $true
    $testDirs = $unitTestDir, $integrationTestDir, $acceptanceTestDir
    $projDirs = $testDirs, $webDir
	$vsPath = exec { . "${Env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"  -latest -property installationPath}
	$msbuild = "$vsPath\MSBuild\15.0\Bin\MSBuild.exe"
    $pathFluentMigrate = "$Env:HomeDrive$Env:HomePath\.nuget\packages\fluentmigrator\1.6.2\tools\"
	$migrateProj = "$PSScriptRoot\src\$solutionName.Migrations"
	$pathMigrate = "$migrateProj\bin\Release\netcoreapp2.2\$solutionName.Migrations.dll"
	$pathMigrateDebug = "$migrateProj\bin\Debug\netcoreapp2.2\$solutionName.Migrations.dll"
	$home = "$Env:Home"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Include ".\Migrate.ps1"