###
# Run use-case tests
# The reason we use PowerShell for this is because the GitGub pipeline had a hard time setting up this
# command with these variables in the yaml.
# Once we got out the yaml errors, it didn't test anything.
###

. dotnet test `
--no-build --no-restore --nologo --verbosity:normal `
--configuration=`"Release`" `
--logger:`"console`;verbosity=detailed`" `
--logger:`"GitHubActions`" `
--filter:`"Category=UseCase`" `
`"$pwd/FluentSerializer.sln`";