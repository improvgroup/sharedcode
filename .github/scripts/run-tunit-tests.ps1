$ErrorActionPreference = 'Stop'

$testExecutables = @(
    './SharedCode.Core.Tests/bin/Release/net9.0/SharedCode.Core.Tests.exe',
    './SharedCode.Core.Tests/bin/Release/net10.0/SharedCode.Core.Tests.exe',
    './SharedCode.Data.Tests/bin/Release/net9.0/SharedCode.Data.Tests.exe',
    './SharedCode.Data.Tests/bin/Release/net10.0/SharedCode.Data.Tests.exe'
)

foreach ($testExecutable in $testExecutables)
{
    & $testExecutable --no-ansi --progress off

    if ($LASTEXITCODE -ne 0)
    {
        exit $LASTEXITCODE
    }
}
