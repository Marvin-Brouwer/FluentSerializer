Param (
	[Parameter(Mandatory=$true)]  [String]$Version,
	[Parameter(Mandatory=$true)]  [String]$File
)

Write-Host "Updating version in $file to $version" -ForegroundColor DarkYellow;

[xml]$xmlDoc = Get-Content $File;

$xmlDoc.Project.PropertyGroup[0].Version = $Version;

$xmlDoc.Save($File);