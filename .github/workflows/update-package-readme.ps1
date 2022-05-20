Param (
	[Parameter(Mandatory=$true)]  [String]$Path
)

$releaseNotesPath = "$Path/Release notes.md";
$nuGetReadmePath = "$Path/Readme.NuGet.md";
$releaseNotesFile = [System.IO.File]::ReadLines($releaseNotesPath);

foreach ($line in $releaseNotesFile) {
	if ($line -eq "## @next") { break };
}
$releaseNotes = @();
foreach ($line in $releaseNotesFile) {
	if ($line.StartsWith("##")) { break };
	$releaseNotes += $line;
}

Add-Content -Path $nuGetReadmePath -Value "" -Force
Add-Content -Path $nuGetReadmePath -Value "## Release notes" -Force

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