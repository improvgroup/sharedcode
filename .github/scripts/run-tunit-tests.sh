#!/usr/bin/env bash
set -euo pipefail

./SharedCode.Core.Tests/bin/Release/net9.0/SharedCode.Core.Tests --no-ansi --progress off
./SharedCode.Core.Tests/bin/Release/net10.0/SharedCode.Core.Tests --no-ansi --progress off
./SharedCode.Data.Tests/bin/Release/net9.0/SharedCode.Data.Tests --no-ansi --progress off
./SharedCode.Data.Tests/bin/Release/net10.0/SharedCode.Data.Tests --no-ansi --progress off
