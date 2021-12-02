. dotnet test `
--no-build --no-restore --nologo --verbosity:normal `
--configuration=Release `
--logger:"console;verbosity=detailed" `
--logger:"GitHubActions" `
--logger:"trx;LogFileName=test-results.trx" `
--results-directory:"./test-results" `
--collect:"DotnetCodeCoverage" `
--collect:"XPlat Code coverage" `
--filter:"Category=UnitTest"  `
"./FluentSerializer.sln";