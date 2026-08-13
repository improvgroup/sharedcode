---
name: code-review
description: >
  Context-aware code review guidance for the SharedCode .NET solution. Use this skill
  when performing pull request reviews to apply project-specific conventions,
  patterns, and quality checks.
---

## SharedCode Code Review Checklist

When reviewing pull requests in this repository, apply the following checks in addition
to general best practices.

### C# / .NET Conventions

- All public types and members have complete XML documentation (`<summary>`, `<param>`,
  `<returns>`, `<exception>`, `<typeparam>`, `<remarks>` where applicable).
- Nullable reference types are respected — no suppression of nullable warnings without
  a comment explaining why it is safe.
- `ArgumentNullException.ThrowIfNull` (or `ArgumentException.ThrowIfNullOrEmpty` for
  strings) is used instead of manual null checks, on .NET 6+.
- `this.` prefix is used for all instance member accesses.
- Extension methods use `@this` as the first parameter name.
- Collection expressions (`[..]`) are preferred over `new List<T> { }` or `new T[] {}`.
- Primary constructors are preferred for types that only store injected dependencies.
- Code analysis suppressions always include a meaningful `Justification`.

### Project Structure

- New extension methods are placed in the same namespace as the extended type
  and in a file named `<TypeName>Extensions.cs`.
- New projects are documented in `.github/copilot-instructions.md`.
- Package version changes are made only in `Directory.Packages.props`, never in
  individual `.csproj` files.

### Testing

- Test framework is TUnit (`[Test]`, `[Arguments]`, `Assert.That(...)`).
- Tests follow the Arrange / Act / Assert pattern with blank lines separating each block.
- "Does not throw" tests are synchronous (no `async`/`await`) and do not include
  a placeholder `await Assert.That(true).IsTrue()` assertion.
- Test files mirror the source structure
  (e.g., `Calendar/DateTimeExtensionsTests.cs` for `Calendar/DateTimeExtensions.cs`).

### Build & Packaging

- All project files include `analyzers` in the `IncludeAssets` for `GCop.All.Common`
  (consistent with `runtime; build; native; contentfiles; analyzers; buildtransitive`).
- No build warnings are introduced (warnings are treated as errors in this solution).
