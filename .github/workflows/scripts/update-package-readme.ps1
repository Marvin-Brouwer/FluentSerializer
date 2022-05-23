###
# Appends the release notes for this release to the "Readme.NuGet.md"
###

Param (
	[Parameter(Mandatory=$true)]  [String]$Path
)

$nuGetReadmePath = "$Path/Readme.NuGet.md";
$packageName = $Path | Split-path -Leaf

Write-Host "Appending release notes to $nuGetReadmePath" -ForegroundColor DarkYellow;

# Get the release notes
. "$PSScriptRoot\read-release-notes.ps1"
$releaseNotes = . Read-ReleaseNotes -Path $Path

# Append release notes section to the "Readme.NuGet.md" file
Add-Content -Path $nuGetReadmePath -Value "" -Force
Add-Content -Path $nuGetReadmePath -Value "## [Release notes](https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/$packageName/Changelog.md)" -Force

# Append the release notes from the changelog
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