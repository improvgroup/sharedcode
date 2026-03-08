# GitHub Copilot Instructions for SharedCode

## Project Overview

SharedCode is a multi-project .NET 9 solution that provides reusable utility libraries
published as individual NuGet packages. Each project corresponds to one NuGet package:

| Project | NuGet Package | Purpose |
|---|---|---|
| `SharedCode.Core` | `SharedCode.Core` | Core utilities: extensions, specifications, domain primitives, text, calendar, security, threading, reactive |
| `SharedCode.Core.Tests` | *(test only)* | Unit tests for `SharedCode.Core` |
| `SharedCode.Data` | `SharedCode.Data` | Data access abstractions and helpers (repository pattern, paging, query results) |
| `SharedCode.Data.CosmosDb` | `SharedCode.Data.CosmosDb` | Azure Cosmos DB integration |
| `SharedCode.Data.EntityFramework` | `SharedCode.Data.EntityFramework` | Entity Framework Core integration (auditable contexts, EF repository, specifications) |
| `SharedCode.DependencyInjection` | `SharedCode.DependencyInjection` | DI assembly scanning and auto-registration |
| `SharedCode.MediatR` | `SharedCode.MediatR` | MediatR pipeline integration helpers |
| `SharedCode.Web` | `SharedCode.Web` | ASP.NET Core helpers (base controller, service collection extensions) |
| `SharedCode.Windows.WPF` | `SharedCode.Windows.WPF` | WPF helpers (BindableBase, converters, attached properties, MVVM mediator) |

## Build & Test

```bash
# Build the entire solution
dotnet build SharedCode.sln

# Run all tests
dotnet test SharedCode.sln

# Run tests with verbose output and GitHub Actions logging
dotnet test --logger GitHubActions --verbosity normal SharedCode.sln
```

## Code Conventions

### Language & Framework

- **Target framework**: `net9.0`
- **Language version**: `preview` (latest C# features enabled)
- **Nullable reference types**: enabled (`<Nullable>enable</Nullable>`)
- **Implicit usings**: enabled
- **Warnings as errors**: all warnings are treated as errors
  (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`,
  `<CodeAnalysisTreatWarningsAsErrors>true</CodeAnalysisTreatWarningsAsErrors>`)

### Formatting

- 4-space indentation; no tabs
- CRLF line endings
- UTF-8 BOM for `.cs` files
- Maximum line length: 100 characters
- Opening brace on its own line for all constructs
  (`csharp_new_line_before_open_brace = all`)

### Naming

- `PascalCase` for types, public members, and constants
- `camelCase` for parameters and local variables
- Private fields: `camelCase` without an underscore prefix
- Use `this.` prefix for all instance member accesses
- Use `_` as the discard variable
  (e.g., `_ = value ?? throw new ArgumentNullException(nameof(value))`)
- Use `@this` as the parameter name for the extended type in extension methods
  (e.g., `this DateTime @this`)

### XML Documentation

Every public type and member **must** have complete XML documentation, including:

- `<summary>` — describes what the member does
- `<typeparam>` — for each generic type parameter
- `<param>` — for each method parameter
- `<returns>` — describes the return value when non-void
- `<exception>` — lists each documented exception
- `<remarks>` — additional usage notes where appropriate

Example from `DateTimeExtensions.cs`:

```csharp
/// <summary>
/// Adds the given number of business days to the <see cref="DateTime" />.
/// </summary>
/// <param name="this">The date to be changed.</param>
/// <param name="days">Number of business days to be added.</param>
/// <returns>A <see cref="DateTime" /> increased by a given number of business days.</returns>
public static DateTime AddBusinessDays(this DateTime @this, int days) { ... }
```

### Null Handling

- Prefer `ArgumentNullException.ThrowIfNull(param)` (.NET 6+)
- Prefer `ArgumentException.ThrowIfNullOrEmpty(param)` for strings (.NET 7+)
- When targeting older frameworks via polyfill, wrap with `#if NET6_0_OR_GREATER`

### Code Analysis Suppressions

- Suppress with `[SuppressMessage("Category", "RuleId:Title", Justification = "reason")]`
- Always provide a meaningful `Justification`; never suppress without explanation

## Extension Method Conventions

- Place extension methods in the same namespace as the extended type
  (e.g., `namespace SharedCode` for core types, `namespace SharedCode.Text` for string helpers)
- One static extension class per file: `<TypeName>Extensions.cs`
- Mark the class `public static` (never `internal`)
- Use `@this` as the first parameter name

## Key Design Patterns

### Specification Pattern (`SharedCode.Core/Specifications`)

Concrete specifications inherit `Specification<T>` (or `Specification<T, TResult>` for
projections) and build filter/order/paging expressions via the `this.Query` fluent builder
in the constructor:

```csharp
public sealed class ActiveUsersSpec : Specification<User>
{
    public ActiveUsersSpec() =>
        this.Query.Where(u => u.IsActive).OrderBy(u => u.LastName);
}
```

### Repository Pattern (`SharedCode.Data`)

`IQueryRepository<T>` and `ICommandRepository<T>` define data access contracts.
`QueryRepository` is the default in-memory/generic implementation.
EF-specific implementations live in `SharedCode.Data.EntityFramework`.

### Dependency Injection (`SharedCode.DependencyInjection`)

Services implement `IDependencyRegister` to self-register via assembly scanning with
`DependencyLoader` / `CatalogSelector`.

## Testing Conventions

Unit tests live in `SharedCode.Core.Tests/` and follow these rules:

- **Framework**: MSTest (`[TestClass]`, `[TestMethod]`, `[DataRow]`)
- **Assertions**: AwesomeAssertions
- **Pattern**: Arrange / Act / Assert with blank lines separating each block
- **File location**: mirror the source structure
  (e.g., `Calendar/DateTimeExtensionsTests.cs` for `Calendar/DateTimeExtensions.cs`)
- Suppress `CA1515` on test classes — MSTest requires them to be `public`

## Shared Build Configuration

- `Directory.Build.props` — solution-wide MSBuild settings (analyzer settings, package metadata,
  version scheme, SourceLink, Roslynator)
- `Directory.Packages.props` — centrally managed package versions; always update here, never in
  individual `.csproj` files

## Maintenance

These instructions must stay aligned with the codebase. Use the prompt at
`.github/prompts/maintain-copilot-instructions.prompt.md` whenever:

- A new project or module is added to the solution
- Coding conventions or patterns change
- New analyzers or packages are introduced
- Test infrastructure changes

The workflow at `.github/workflows/maintain-copilot-instructions.yml` automatically
detects undocumented projects and opens a GitHub Issue to prompt a review.
