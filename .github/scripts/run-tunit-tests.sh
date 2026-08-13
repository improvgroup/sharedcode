#!/usr/bin/env bash
set -euo pipefail

dotnet ./SharedCode.Core.Tests/bin/Release/net9.0/SharedCode.Core.Tests.dll --no-ansi --progress off
dotnet ./SharedCode.Core.Tests/bin/Release/net10.0/SharedCode.Core.Tests.dll --no-ansi --progress off
dotnet ./SharedCode.Data.Tests/bin/Release/net9.0/SharedCode.Data.Tests.dll --no-ansi --progress off
dotnet ./SharedCode.Data.Tests/bin/Release/net10.0/SharedCode.Data.Tests.dll --no-ansi --progress off
