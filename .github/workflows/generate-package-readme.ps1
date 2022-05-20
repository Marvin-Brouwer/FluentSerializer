Param (
	[Parameter(Mandatory=$true)]  [String]$Path
)

Write-Host "Updating version in $file to $version" -ForegroundColor DarkYellow;

$markdownFile = [System.IO.File]::ReadLines("$Path/Release notes.md");

New-Item -Path $Path/bin/README.md -ItemType File -Force
Clear-Content  -Path $Path/bin/README.md
foreach ($line in $markdownFile) {
	if ($line -eq "## @next") { break };
}
$releaseNotes = @();
foreach ($line in $markdownFile) {
	if ($line.StartsWith("##")) { break };
	$releaseNotes += $line;
}

$folderName = Split-Path $Path -Leaf
Add-Content -Path $Path/bin/README.md -Value "# $folderName" -Force
Add-Content -Path $Path/bin/README.md -Value "" -Force
Add-Content -Path $Path/bin/README.md -Value "`FluentSerializer` is a library to help you with serializing to-and-from C# POCOs using profiles." -Force
Add-Content -Path $Path/bin/README.md -Value "" -Force
Add-Content -Path $Path/bin/README.md -Value "- [$folderName Readme](https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/$folderName#readme)" -Force
Add-Content -Path $Path/bin/README.md -Value "- [FluentSerializer Main Readme](https://github.com/Marvin-Brouwer/FluentSerializer#readme)" -Force
Add-Content -Path $Path/bin/README.md -Value "- [FluentSerializer License](https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/License.md#readme)" -Force
Add-Content -Path $Path/bin/README.md -Value "" -Force
Add-Content -Path $Path/bin/README.md -Value "## Release notes" -Force

$pos = 1;
foreach ($line in $releaseNotes) {

	if ($pos -eq $releaseNotes.length) {
		Add-Content -Path $Path/bin/README.md -Value $line -Force -NoNewline
	}
	else {
		Add-Content -Path $Path/bin/README.md -Value $line -Force
	}
	$pos ++;
}