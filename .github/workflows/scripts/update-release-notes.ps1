Param (
	[Parameter(Mandatory=$true)]  [String]$File
)

. "$PSScriptRoot\read-release-notes.ps1"

$linkReplacement = "";
$linkRemovalPattern = [System.Text.RegularExpressions.Regex]::new(
	"- \[#[0-9]+]\(https:\/\/github\.com\/Marvin-Brouwer\/FluentSerializer\/issues\/[0-9]+\) ",
	[System.Text.RegularExpressions.RegexOptions]::IgnoreCase
);

Write-Host "Updating release notes in $file" -ForegroundColor DarkYellow;

$releaseNotesMarkDown = . Read-ReleaseNotes -Path ($File | Split-Path)
$releaseNotes = "";

$pos = 1;
foreach ($line in $releaseNotesMarkDown) {

	if ($pos -eq 0) {
		continue;
	}
	elseif ($pos -eq $releaseNotesMarkDown.length) {
		$releaseNotes += $line -Replace $linkRemovalPattern, $linkReplacement;
	}
	else {
		$releaseNotes += $line -Replace $linkRemovalPattern, $linkReplacement;
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