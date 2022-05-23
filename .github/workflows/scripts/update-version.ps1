###
# Writes the version to the CSPROJ file for correct package generating
###

Param (
	[Parameter(Mandatory=$true)]  [String]$Version,
	[Parameter(Mandatory=$true)]  [String]$File
)

Write-Host "Updating version in $file to $version" -ForegroundColor DarkYellow;

[xml]$xmlDoc = Get-Content $File;

try {
	$xmlDoc.Project.PropertyGroup[0].Version = [String]$Version;
} catch {
	$xmlDoc.Project.PropertyGroup.Version = [String]$Version;
}

$xmlDoc.Save($File);