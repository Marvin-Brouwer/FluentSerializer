###
# Pulls the "## @next" section out of the "Changelog.md" file to use as release notes.
###

Function Read-ReleaseNotes {
	Param (
		[Parameter(Mandatory=$true)] [String]$Path
	)

	# Read the changelog
	$releaseNotesPath = "$Path/Changelog.md";
	$releaseNotesFile = [System.IO.File]::ReadLines($releaseNotesPath);

	# Loop untill you find the next chapter
	foreach ($line in $releaseNotesFile) {
		if ($line -eq "## @next") { break };
	}

	# Append lines to the collection untill you hit the next chapter
	$releaseNotes = @();
	foreach ($line in $releaseNotesFile) {
		if ($line.StartsWith("##")) { break };
		$releaseNotes += $line;
	}

	return $releaseNotes;
};