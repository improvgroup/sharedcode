---
mode: edit
description: Identify untested public members and add test coverage for them.
---

# Improve Test Coverage

Identify public members in a SharedCode source file (or folder) that have no test coverage
and add tests for them.

## Process

### 1 — Identify what is missing

For each `.cs` file in the source project:

1. List every `public` method, property, and indexer.
2. Open the corresponding `*Tests.cs` file in the test project (if it exists).
3. Note every member that has **no** `[TestMethod]` exercising it.
4. If no test file exists at all, every public member needs coverage.

### 2 — Prioritize

Cover members in this order:
1. Pure logic methods (no I/O or infrastructure) — easiest to test
2. Guard-clause paths (`ArgumentNullException`, `ArgumentException`)
3. Edge cases (empty collections, null-optional parameters, boundary values)
4. Happy paths for remaining members

### 3 — Write the tests

Follow the conventions in `add-test-class.prompt.md`:
- `[TestMethod]` for single scenarios
- `[DataRow]` for parameterized scenarios
- MSTest assertions (`Assert.AreEqual`, `Assert.IsTrue`, `Assert.ThrowsExactly`)
- Arrange / Act / Assert blocks separated by blank lines

### 4 — Verify

```bash
dotnet test SharedCode.sln
```

Zero failures required before merging.

## Checklist per source file

Run through these questions for each public member:

- [ ] Is there a happy-path test?
- [ ] Is there a null-argument test (if the member accepts reference-type parameters)?
- [ ] Is there a boundary/edge-case test (empty string, zero, `int.MaxValue`, etc.)?
- [ ] Is the test parameterized with `[DataRow]` instead of repeated copy-paste?

## Common coverage gaps in this solution

| Source file | Members typically missing coverage |
|---|---|
| `AssemblyExtensions.cs` | `GetAttribute<T>` (found / not found) |
| `EventHandlerExtensions.cs` | `Raise` overloads (null handler, non-null handler) |
| `Extensions.cs` | `IsBetween`, `In`, `IfNotNull`, `IsNull<T>`, `ChangeType<T>` |
| `FunctionExtensions.cs` | `Memoize` (cache hit, cache miss) |
| `TypeExtensions.cs` | `GetDisplayName`, `IsNullable`, `IsSubclassOfRawGeneric` |
| `PropertySupport.cs` | `ExtractPropertyName` |
| `Linq/` | All `IEnumerable<T>` extension methods |
| `Security/` | Hashing / encryption helpers |
| `Text/` | All string extension methods |
| `Threading/` | All task/threading helpers |
| `Domain/` | `ValueObject` equality |
| `Specifications/` | `InMemorySpecificationEvaluator` |
