Properties {
	$rootDir = Split-Path $psake.build_script_file
    $solutionName = (ls $rootDir\*.sln)[0].BaseName
	$migrateProj = "$PSScriptRoot\src\$solutionName.Migrations" #C:\src\Consultorio\src\Consultorio.Migrations
	$pathMigrate = "$migrateProj\bin\Debug\netcoreapp2.2\$solutionName.Migrations.dll"
    $home = "$Env:HomeDrive$Env:HomePath"
	$pathFluentMigrate = "$home\.nuget\packages\fluentmigrator.console\3.1.3\net461\any\"
}

Task get-build {
	Add-Type -Path (${env:ProgramFiles(x86)} + '\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\amd64\Microsoft.Build.dll')
	$slnFile = [Microsoft.Build.Construction.SolutionFile]::Parse("$rootDir\$solutionName.sln")
	$slnFile.ProjectsInOrder | Where-Object { $_.ProjectType -eq "KnownToBeMSBuildFormat" } | % {

	$outValue = $null
	$found = $_.ProjectConfigurations.TryGetValue("Release|Any CPU", [ref]$outValue)

	write-host -Foreground yellow $slnFile.ProjectsInOrder;

		if($found)
		{
			if($outValue.IncludeInBuild) # This bit is set by the MS code when parsing the project configurations with ...Build.0
			{
			  #Do stuff here
			  write-host -Foreground yellow "Ambiente em Homologação";
			}
		}
	 }
	
}

Task Migrate -Description "Run Script Migrate" {
	
	#Build do Projeto de Migrate
	Pop-Location;
	Push-Location $migrateProj;
	dotnet build -c RELEASE;
	Pop-Location;

	#atualiza assembly antes de executar Migrate
	dotnet build
		    
	#Obtem connectionstring e path do arquivo de script
	$connection = Get-DbConnection
	Write-Host -Foreground Green "Connection String: $connection"

	#Define local onde será salva arquivo de Migrate caso seja solicitado a criação do mesmo	
	$fileMigrate = "$home\DeskTop\Migrate.sql"

	#Muda path para pasta do Migrate
    cd "$pathFluentMigrate"
		
    dotnet-fm migrate --verbose --processor SqlServer2016 --connection $connection --tag usuario --assembly $pathMigrate
}

###### Exemplo de chamada com passagem de parametro ######
#clear; psake Migrate-Down -parameters "@{version='20181226161100'}"
Task Migrate-Down -Description "Run Script Migrate" {
	
	#Build do Projeto de Migrate
	Pop-Location;
	Push-Location $migrateProj;
	dotnet build -c RELEASE;
	Pop-Location;

	#atualiza assembly antes de executar Migrate
	dotnet build
		    
	#Obtem connectionstring e path do arquivo de script
	$connection = Get-DbConnection
	Write-Host -Foreground Green "Connection String: $connection"

	#Define local onde será salva arquivo de Migrate caso seja solicitado a criação do mesmo	
	$fileMigrate = "$home\DeskTop\Migrate.sql"

	#Muda path para pasta do Migrate
    cd "$pathFluentMigrate"
		
    dotnet-fm migrate --verbose --processor SqlServer2016 --connection $connection --tag usuario --assembly $pathMigrate down -t $version
}


Task Migrate-File -Description "Run Script Migrate" {
	
	#Build do Projeto de Migrate
	Pop-Location;
	Push-Location $migrateProj;
	dotnet build -c RELEASE;
	Pop-Location;

	#atualiza assembly antes de executar Migrate
	dotnet build
		    
	#Obtem connectionstring e path do arquivo de script
	$connection = Get-DbConnection
	Write-Host -Foreground Green "Connection String: $connection"

	#Define local onde será salva arquivo de Migrate caso seja solicitado a criação do mesmo	
	$fileMigrate = "$home\DeskTop\Migrate.sql"

	#Muda path para pasta do Migrate
    cd "$pathFluentMigrate"
	
	#executa script de migrate
    dotnet-fm migrate --verbose --processor SqlServer2016  --connection $connection --tag usuario --assembly $pathMigrate --output=$FileMigrate 
}

function Get-DbConnection {
	$config = Get-Content "$PSScriptRoot\src\$solutionName.WebApp\appsettings.json" -Raw | ConvertFrom-Json
    New-Object -TypeName System.Data.SqlClient.SqlConnectionStringBuilder -ArgumentList $config.ConnectionStrings.DefaultConnection
}
