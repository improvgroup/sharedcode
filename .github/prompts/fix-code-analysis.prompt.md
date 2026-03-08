---
mode: edit
description: Resolve a compiler warning or code-analysis diagnostic in the SharedCode solution.
---

# Fix a Code Analysis Warning

Resolve a compiler warning or Roslyn diagnostic in the SharedCode solution.

## Process

1. **Read the full diagnostic** — note the rule ID (e.g., `CA1031`, `RCS1234`, `IDE0046`),
   the message, and the exact file and line number.

2. **Understand the rule** before touching any code.

3. **Apply the minimal fix** that aligns with the existing code style:
   - Fix the root cause whenever possible.
   - If suppression is the only viable option, use `[SuppressMessage]` with a meaningful
     `Justification` (never suppress without explanation).

4. **Verify no new warnings are introduced**: `dotnet build SharedCode.sln`

5. **Do not change unrelated code.**

## Common Fixes

| Rule ID | Description | Typical Fix |
|---|---|---|
| `CA1062` | Validate parameter is non-null | `ArgumentNullException.ThrowIfNull(param)` |
| `CA1031` | Do not catch general exception types | Catch specific exception types instead |
| `CA2201` | Do not raise reserved exception types | Throw a more derived `Exception` subclass |
| `CA1014` | Mark assemblies with CLSCompliant | Already suppressed globally via `<NoWarn>CA1014</NoWarn>` |
| `IDE0046` | Convert to conditional expression | Use ternary `? :` only when it improves readability |
| `IDE0028` | Use collection initializer | Replace `Add()` calls with `{ ... }` initializer syntax |
| `RCS1175` | Unused `this` parameter | Remove unused `this` param, or add `[SuppressMessage]` with justification |
| `nullable` | Nullable warning (CS8600–CS8629) | Add `?` to nullable types or add a null-guard |
| `CA1515` | Consider making public types internal | **Do not apply in test projects** — MSTest requires `public` test classes |

## Suppression template

Only use when the rule genuinely does not apply:

```csharp
[SuppressMessage(
    "Category",
    "RuleId:Short title",
    Justification = "One sentence explaining why this rule does not apply here.")]
```

## Null-guard patterns

```csharp
// .NET 6+ (preferred)
ArgumentNullException.ThrowIfNull(param);

// .NET 7+ for strings
ArgumentException.ThrowIfNullOrEmpty(param);
ArgumentException.ThrowIfNullOrWhiteSpace(param);

// Discard pattern for guard-and-assign
_ = param ?? throw new ArgumentNullException(nameof(param));
```
