---
mode: edit
description: Implement a new Specification class following the repository's Specification pattern.
---

# Add a Specification

Create a new concrete specification in the SharedCode library.

## Background

The Specification pattern is implemented in `SharedCode.Core/Specifications`. A specification
encapsulates a query (filter, order, paging, and includes) independently of any persistence
mechanism. `InMemorySpecificationEvaluator` evaluates specs against in-memory collections;
the EF evaluator in `SharedCode.Data.EntityFramework` evaluates them against a `DbContext`.

## Steps

1. **Choose the right project**

   | Scope | Project | Folder |
   |---|---|---|
   | In-memory / framework-agnostic | `SharedCode.Core` | `Specifications/` |
   | Entity Framework | `SharedCode.Data.EntityFramework` | `Specifications/` |

2. **Choose the base class**

   - `Specification<T>` — returns a collection of `T`
   - `Specification<T, TResult>` — projects `T` to `TResult` via a `Selector` expression

3. **Create `<Name>Specification.cs`** in the appropriate folder.

4. **Build the query in the constructor** using `this.Query`:

   | Builder method | Purpose |
   |---|---|
   | `.Where(e => ...)` | Filter predicate |
   | `.OrderBy(e => ...)` | Ascending order |
   | `.OrderByDescending(e => ...)` | Descending order |
   | `.ThenBy(e => ...)` / `.ThenByDescending(...)` | Secondary sort |
   | `.Skip(n).Take(m)` | Paging |
   | `.Include(e => e.Navigation)` | Eager loading (EF only) |
   | `.EnableCache(key)` | Second-level cache hint |

5. **Seal the class** unless it is explicitly designed for inheritance.

6. **Add full XML documentation** on the class and all constructors.

7. **Verify zero warnings**: `dotnet build SharedCode.sln`

## Template — filter specification

```csharp
namespace SharedCode.Specifications;

/// <summary>
/// A specification that selects [describe what is filtered].
/// </summary>
public sealed class ExampleByIdSpecification : Specification<ExampleEntity>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleByIdSpecification" /> class.
    /// </summary>
    /// <param name="id">The identifier of the entity to match.</param>
    public ExampleByIdSpecification(int id) =>
        this.Query.Where(e => e.Id == id);
}
```

## Template — projection specification

```csharp
namespace SharedCode.Specifications;

/// <summary>
/// A specification that projects [source entity] to [result type].
/// </summary>
public sealed class ExampleSummarySpecification : Specification<ExampleEntity, ExampleSummary>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleSummarySpecification" /> class.
    /// </summary>
    /// <param name="isActive">When <see langword="true"/>, only active entities are returned.</param>
    public ExampleSummarySpecification(bool isActive)
    {
        this.Query
            .Where(e => e.IsActive == isActive)
            .OrderBy(e => e.Name)
            .Select(e => new ExampleSummary { Id = e.Id, Name = e.Name });
    }
}
```

## Template — paged specification

```csharp
namespace SharedCode.Specifications;

/// <summary>
/// A paged specification for [entity type].
/// </summary>
public sealed class PagedExampleSpecification : Specification<ExampleEntity>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedExampleSpecification" /> class.
    /// </summary>
    /// <param name="pageNumber">The one-based page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public PagedExampleSpecification(int pageNumber, int pageSize) =>
        this.Query
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
}
```
