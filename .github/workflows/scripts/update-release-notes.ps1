Param (
	[Parameter(Mandatory=$true)]  [String]$File
)

. "$PSScriptRoot\read-release-notes.ps1"

Write-Host "Updating release notes in $file" -ForegroundColor DarkYellow;

$releaseNotesMarkDown = . Read-ReleaseNotes -Path ($File | Split-Path)
$releaseNotes = "";

$pos = 1;
foreach ($line in $releaseNotesMarkDown) {

	if ($pos -eq 0) {
		continue;
	}
	elseif ($pos -eq $releaseNotesMarkDown.length) {
		$releaseNotes += $line;
	}
	else {
		$releaseNotes += $line;
		$releaseNotes += "`n"
	}
	$pos ++;
}

[xml]$xmlDoc = Get-Content $File;

try {
	$xmlDoc.Project.PropertyGroup[0].PackageReleaseNotes = [String]$releaseNotes;
} catch {
	$xmlDoc.Project.PropertyGroup.PackageReleaseNotes = [String]$releaseNotes;
}

$xmlDoc.Save($File);