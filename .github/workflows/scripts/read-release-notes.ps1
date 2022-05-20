Function Read-ReleaseNotes {
	Param (
		[Parameter(Mandatory=$true)] [String]$Path
	)

	$releaseNotesPath = "$Path/Changelog.md";
	$releaseNotesFile = [System.IO.File]::ReadLines($releaseNotesPath);

	foreach ($line in $releaseNotesFile) {
		if ($line -eq "## @next") { break };
	}
	$releaseNotes = @();
	foreach ($line in $releaseNotesFile) {
		if ($line.StartsWith("##")) { break };
		$releaseNotes += $line;
	}

	return $releaseNotes;
};