$ErrorActionPreference = 'Stop'

$testAssemblies = @(
    './SharedCode.Core.Tests/bin/Release/net9.0/SharedCode.Core.Tests.dll',
    './SharedCode.Core.Tests/bin/Release/net10.0/SharedCode.Core.Tests.dll',
    './SharedCode.Data.Tests/bin/Release/net9.0/SharedCode.Data.Tests.dll',
    './SharedCode.Data.Tests/bin/Release/net10.0/SharedCode.Data.Tests.dll'
)

foreach ($testAssembly in $testAssemblies)
{
    dotnet $testAssembly --no-ansi --progress off

    if ($LASTEXITCODE -ne 0)
    {
        exit $LASTEXITCODE
    }
}
