Param (
	[Parameter(Mandatory=$true)]  [String]$Path
)

. "$PSScriptRoot\read-release-notes.ps1"

$nuGetReadmePath = "$Path/Readme.NuGet.md";
$packageName = $Path | Split-path -Leaf

Write-Host "Appending release notes to $nuGetReadmePath" -ForegroundColor DarkYellow;

$releaseNotes = . Read-ReleaseNotes -Path $Path

Add-Content -Path $nuGetReadmePath -Value "" -Force
Add-Content -Path $nuGetReadmePath -Value "## [Release notes](https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/$packageName/Changelog.md)" -Force

$pos = 1;
foreach ($line in $releaseNotes) {

	if ($pos -eq $releaseNotes.length) {
		Add-Content -Path $nuGetReadmePath -Value $line -Force -NoNewline
	}
	else {
		Add-Content -Path $nuGetReadmePath -Value $line -Force
	}
	$pos ++;
}