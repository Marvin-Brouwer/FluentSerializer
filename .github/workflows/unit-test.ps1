###
# Run unit tests
# The reason we use PowerShell for this is because the GitGub pipeline had a hard time setting up this
# command with these variables in the yaml.
# Once we got out the yaml errors, it didn't test anything.
###

. dotnet test `
--no-build --no-restore --nologo --verbosity:normal `
--configuration=`"Release`" `
--logger:`"console`;verbosity=detailed`" `
--logger:`"GitHubActions`" `
--logger `"trx`;LogFileName=test-results.trx`" `
--collect `"DotnetCodeCoverage`" `
--collect `"XPlat Code coverage`" `
--results-directory:`"$pwd/test-results`" `
--filter:`"Category=UnitTest`" `
/p:CollectCoverage=`"true`" `
/p:CoverletOutputFormat=`"opencover`" `
/p:CoverletOutput=`"$pwd/test-results/coverage/`" `
/p:MergeWith=`"$pwd/test-results/coverage/`" `
/p:Exclude=`"[*Tests]*%2c[*TestUtils]*%2c[*UseCase*]*`" `
`"$pwd/FluentSerializer.sln`";

reportgenerator `
-reports:`"$pwd/test-results/*/coverage.cobertura.xml`" `
-targetdir:`"$pwd/test-results/coverage`" `
-reporttypes:HtmlInline_AzurePipelines`;Cobertura;