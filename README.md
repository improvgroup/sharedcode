# Shared Code

[![.NET](https://github.com/improvgroup/sharedcode/actions/workflows/dotnet.yml/badge.svg)](https://github.com/improvgroup/sharedcode/actions/workflows/dotnet.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![NuGet: SharedCode.Core](https://img.shields.io/nuget/v/SharedCode.Core.svg?label=SharedCode.Core)](https://www.nuget.org/packages/SharedCode.Core)
[![NuGet: SharedCode.Data](https://img.shields.io/nuget/v/SharedCode.Data.svg?label=SharedCode.Data)](https://www.nuget.org/packages/SharedCode.Data)
[![NuGet: SharedCode.Data.CosmosDb](https://img.shields.io/nuget/v/SharedCode.Data.CosmosDb.svg?label=SharedCode.Data.CosmosDb)](https://www.nuget.org/packages/SharedCode.Data.CosmosDb)
[![NuGet: SharedCode.Data.EntityFramework](https://img.shields.io/nuget/v/SharedCode.Data.EntityFramework.svg?label=SharedCode.Data.EntityFramework)](https://www.nuget.org/packages/SharedCode.Data.EntityFramework)
[![NuGet: SharedCode.DependencyInjection](https://img.shields.io/nuget/v/SharedCode.DependencyInjection.svg?label=SharedCode.DependencyInjection)](https://www.nuget.org/packages/SharedCode.DependencyInjection)
[![NuGet: SharedCode.MediatR](https://img.shields.io/nuget/v/SharedCode.MediatR.svg?label=SharedCode.MediatR)](https://www.nuget.org/packages/SharedCode.MediatR)
[![NuGet: SharedCode.Web](https://img.shields.io/nuget/v/SharedCode.Web.svg?label=SharedCode.Web)](https://www.nuget.org/packages/SharedCode.Web)
[![NuGet: SharedCode.Windows.WPF](https://img.shields.io/nuget/v/SharedCode.Windows.WPF.svg?label=SharedCode.Windows.WPF)](https://www.nuget.org/packages/SharedCode.Windows.WPF)

A collection of reusable utility libraries for .NET, published as individual NuGet packages. Each
package targets a specific concern — from core extensions and domain primitives to data access,
dependency injection, MediatR pipelines, ASP.NET Core helpers, and WPF utilities — so you can pull
in only what you need.

---

## Table of Contents

- [Packages](#packages)
- [Requirements](#requirements)
- [Installation](#installation)
- [Package Details](#package-details)
  - [SharedCode.Core](#sharedcodecore)
  - [SharedCode.Data](#sharedcodedata)
  - [SharedCode.Data.CosmosDb](#sharedcodedatacosmosdb)
  - [SharedCode.Data.EntityFramework](#sharedcodedataentityframework)
  - [SharedCode.DependencyInjection](#sharedcodedependencyinjection)
  - [SharedCode.MediatR](#sharedcodemediatr)
  - [SharedCode.Web](#sharedcodeweb)
  - [SharedCode.Windows.WPF](#sharedcodewindowswpf)
- [Building from Source](#building-from-source)
- [Contributing](#contributing)
- [License](#license)

---

## Packages

| Package | NuGet | Target Frameworks | Description |
|---|---|---|---|
| `SharedCode.Core` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.Core.svg)](https://www.nuget.org/packages/SharedCode.Core) | `netstandard2.0`, `netstandard2.1`, `net8.0`, `net9.0` | Core utilities: extensions, specifications, domain primitives, security, threading, reactive |
| `SharedCode.Data` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.Data.svg)](https://www.nuget.org/packages/SharedCode.Data) | `netstandard2.0`, `netstandard2.1`, `net8.0`, `net9.0` | Framework-agnostic data access abstractions: repository pattern, paging, query results |
| `SharedCode.Data.CosmosDb` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.Data.CosmosDb.svg)](https://www.nuget.org/packages/SharedCode.Data.CosmosDb) | `net8.0`, `net9.0` | Azure Cosmos DB integration and helpers |
| `SharedCode.Data.EntityFramework` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.Data.EntityFramework.svg)](https://www.nuget.org/packages/SharedCode.Data.EntityFramework) | `net8.0`, `net9.0` | Entity Framework Core integration: auditable contexts, EF repository, specifications |
| `SharedCode.DependencyInjection` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.DependencyInjection.svg)](https://www.nuget.org/packages/SharedCode.DependencyInjection) | `netstandard2.0`, `netstandard2.1`, `net8.0`, `net9.0` | DI assembly scanning and auto-registration via `IDependencyRegister` |
| `SharedCode.MediatR` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.MediatR.svg)](https://www.nuget.org/packages/SharedCode.MediatR) | `net8.0`, `net9.0` | MediatR command/query abstractions and pipeline helpers |
| `SharedCode.Web` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.Web.svg)](https://www.nuget.org/packages/SharedCode.Web) | `net9.0` | ASP.NET Core helpers: base controller, resilient HTTP client setup, health checks |
| `SharedCode.Windows.WPF` | [![NuGet](https://img.shields.io/nuget/v/SharedCode.Windows.WPF.svg)](https://www.nuget.org/packages/SharedCode.Windows.WPF) | `net9.0-windows10.0.22621.0` | WPF helpers: `BindableBase`, value converters, attached properties, MVVM mediator |

---

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for `net9.0` targets)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for `net8.0` targets)
- .NET Standard 2.0 / 2.1 targets are compatible with .NET Framework 4.6.1+ and .NET Core 2.0+

---

## Installation

Install any package via the NuGet Package Manager, the .NET CLI, or by editing your project file.

**.NET CLI**

```bash
dotnet add package SharedCode.Core
dotnet add package SharedCode.Data
dotnet add package SharedCode.Data.CosmosDb
dotnet add package SharedCode.Data.EntityFramework
dotnet add package SharedCode.DependencyInjection
dotnet add package SharedCode.MediatR
dotnet add package SharedCode.Web
dotnet add package SharedCode.Windows.WPF
```

**Package Manager Console**

```powershell
Install-Package SharedCode.Core
Install-Package SharedCode.Data
Install-Package SharedCode.Data.CosmosDb
Install-Package SharedCode.Data.EntityFramework
Install-Package SharedCode.DependencyInjection
Install-Package SharedCode.MediatR
Install-Package SharedCode.Web
Install-Package SharedCode.Windows.WPF
```

---

## Package Details

### SharedCode.Core

[![NuGet](https://img.shields.io/nuget/v/SharedCode.Core.svg)](https://www.nuget.org/packages/SharedCode.Core)

Core utilities and domain primitives that are shared across all other packages. Targets
`netstandard2.0`, `netstandard2.1`, `net8.0`, and `net9.0`.

**Highlights**

- **Extension methods** — `DateTime`, `DateTimeOffset`, `string`, `int`, `Enum`, collections,
  `Type`, `IEnumerable`, reflection, and more
- **Specification pattern** — strongly typed `Specification<T>` / `Specification<T, TResult>` with
  a fluent query builder (filter, ordering, paging, includes)
- **Domain primitives** — `ValueObject`, `BaseException`, result types (`IResult`,
  `IErrorResult`, `ValidationErrorResult`)
- **Security** — `Hasher` utility supporting multiple hash algorithms
- **Text** — `StringExtensions`, `StringBuilderExtensions`, masking helpers
- **Calendar** — business-day arithmetic, `DateTimeFormat`, `DayOfWeekExtensions`
- **Reactive** — built on `System.Reactive` and `System.Interactive`
- **Threading** — async-friendly utilities

**Quick Start**

```csharp
// Add business days
var nextBusinessDay = DateTime.Today.AddBusinessDays(3);

// Specification pattern
public sealed class ActiveUsersSpec : Specification<User>
{
    public ActiveUsersSpec() =>
        this.Query.Where(u => u.IsActive).OrderBy(u => u.LastName);
}
```

---

### SharedCode.Data

[![NuGet](https://img.shields.io/nuget/v/SharedCode.Data.svg)](https://www.nuget.org/packages/SharedCode.Data)

Framework-agnostic data access abstractions. Targets `netstandard2.0`, `netstandard2.1`,
`net8.0`, and `net9.0`.

**Highlights**

- `IQueryRepository<T>` / `ICommandRepository<T>` contracts
- `QueryRepository<T>` — default generic / in-memory implementation
- `IQueryResult<T>` with paging support via `PagingDescriptor` and `PageBoundry`
- `AddOrUpdate` descriptor pattern
- `DataReaderExtensions`, `DynamicDataRecord`, and OData `DataServiceQueryExtensions`

**Quick Start**

```csharp
public class UserService
{
    private readonly IQueryRepository<User> repository;

    public UserService(IQueryRepository<User> repository) =>
        this.repository = repository;

    public async Task<IQueryResult<User>> GetActiveUsersAsync(CancellationToken ct) =>
        await this.repository.ListAsync(new ActiveUsersSpec(), ct);
}
```

---

### SharedCode.Data.CosmosDb

[![NuGet](https://img.shields.io/nuget/v/SharedCode.Data.CosmosDb.svg)](https://www.nuget.org/packages/SharedCode.Data.CosmosDb)

Azure Cosmos DB integration built on top of the official
[Microsoft.Azure.Cosmos](https://www.nuget.org/packages/Microsoft.Azure.Cosmos) SDK.
Targets `net8.0` and `net9.0`.

**Highlights**

- Cosmos DB query helpers and logger extensions
- `Query<T>` helper for building Cosmos DB LINQ queries

---

### SharedCode.Data.EntityFramework

[![NuGet](https://img.shields.io/nuget/v/SharedCode.Data.EntityFramework.svg)](https://www.nuget.org/packages/SharedCode.Data.EntityFramework)

Entity Framework Core integration. Targets `net8.0` and `net9.0`.

**Highlights**

- `AuditableDbContext` — automatically stamps `CreatedAt` / `ModifiedAt` on `IAuditableEntity`
- `EfRepository<T>` — EF Core implementation of `IQueryRepository<T>` and `ICommandRepository<T>`
- `Entity` base class and `IAuditableEntity` / `IAuditedEntity` interfaces
- Specification evaluator wired directly into EF Core queries
- `IDataService` / `DataService` for unit-of-work style operations

**Quick Start**

```csharp
// Register with resilient SQL Server connection
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));
```

---

### SharedCode.DependencyInjection

[![NuGet](https://img.shields.io/nuget/v/SharedCode.DependencyInjection.svg)](https://www.nuget.org/packages/SharedCode.DependencyInjection)

Assembly scanning and automatic service registration. Targets `netstandard2.0`,
`netstandard2.1`, `net8.0`, and `net9.0`.

**Highlights**

- `IDependencyRegister` — implement on any class to self-register services
- `DependencyLoader` — scans assemblies and invokes all `IDependencyRegister` implementations
- `CatalogSelector` / `TypeSourceSelector` for fine-grained assembly and type filtering

**Quick Start**

```csharp
// In Startup.cs / Program.cs
services.LoadDependencies(typeof(Program).Assembly);

// In any library
public class MyServiceRegistrar : IDependencyRegister
{
    public void Register(IServiceCollection services) =>
        services.AddScoped<IMyService, MyService>();
}
```

---

### SharedCode.MediatR

[![NuGet](https://img.shields.io/nuget/v/SharedCode.MediatR.svg)](https://www.nuget.org/packages/SharedCode.MediatR)

[MediatR](https://github.com/jbogard/MediatR) pipeline abstractions. Targets `net8.0` and
`net9.0`.

**Highlights**

- `ICommand` / `ICommand<TResponse>` — strongly typed command markers
- `ICommandHandler` / `ICommandHandler<TCommand, TResponse>` — handler contracts
- `IQuery<TResponse>` / `IQueryHandler<TQuery, TResponse>` — query/response contracts
- Extension method for registering MediatR from an assembly

**Quick Start**

```csharp
// Define a command
public sealed record CreateUserCommand(string Name) : ICommand<Guid>;

// Implement a handler
public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
    {
        // create user ...
        return newUser.Id;
    }
}

// Register in Program.cs
services.AddSharedCodeMediatR(typeof(Program).Assembly);
```

---

### SharedCode.Web

[![NuGet](https://img.shields.io/nuget/v/SharedCode.Web.svg)](https://www.nuget.org/packages/SharedCode.Web)

ASP.NET Core helpers. Targets `net9.0`.

**Highlights**

- `BaseController` — abstract controller base that exposes an `IHttpClientFactory`
- Service collection extensions for resilient HTTP clients (Polly retry/wait)
- Health check registration helpers

**Quick Start**

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    public UsersController(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory) { }
}
```

---

### SharedCode.Windows.WPF

[![NuGet](https://img.shields.io/nuget/v/SharedCode.Windows.WPF.svg)](https://www.nuget.org/packages/SharedCode.Windows.WPF)

WPF helpers targeting `net9.0-windows10.0.22621.0`.

**Highlights**

- `BindableBase` — base class implementing `INotifyPropertyChanged`
- **Value converters** — `BooleanToVisibilityConverter`, `StringToVisibilityConverter`,
  `NumberToStringConverter`
- `PasswordHelper` — attached property for binding `PasswordBox.Password`
- `Mediator` — simple in-process message broker for loosely coupled view-models
- `ModalTemplateAttribute` — data-template helper for modal view-models

> **Note:** The WPF community has excellent implementations of commands and more in the
> [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm) package.
> That package is referenced as a dependency and its commands are the recommended approach.

---

## Building from Source

```bash
# Clone the repository
git clone https://github.com/improvgroup/sharedcode.git
cd sharedcode

# Restore and build
dotnet build SharedCode.sln

# Run all tests
dotnet test SharedCode.sln
```

The solution targets **.NET 9** for the library projects. The WPF project additionally requires
Windows 10 SDK version 10.0.19041.0 or later.

---

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository and create a feature branch.
2. Make your changes, ensuring all existing tests pass (`dotnet test SharedCode.sln`).
3. Add or update tests as appropriate.
4. Open a pull request against the `main` branch.

Please read the [Code of Conduct](CODE_OF_CONDUCT.md) and [Security Policy](SECURITY.md) before
contributing.

---

## License

This project is licensed under the [MIT License](LICENSE).  
Copyright © 2025 [improvGroup, LLC](https://github.com/improvgroup) and contributors.
