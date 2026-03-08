---
mode: edit
description: Add a new extension method to the SharedCode library following project conventions.
---

# Add an Extension Method

Add a new extension method to the SharedCode library.

## Steps

1. **Identify the right project and namespace**

   | Type being extended | Project | Namespace |
   |---|---|---|
   | `DateTime` / `DateTimeOffset` | `SharedCode.Core/Calendar` | `SharedCode.Calendar` |
   | `string` | `SharedCode.Core/Text` | `SharedCode.Text` |
   | `int`, `long`, other numerics | `SharedCode.Core` | `SharedCode` |
   | `IEnumerable<T>` / `IQueryable<T>` | `SharedCode.Core/Linq` | `SharedCode.Linq` |
   | `Exception` | `SharedCode.Core` | `SharedCode` |
   | Generic / `object` | `SharedCode.Core` | `SharedCode` |
   | `IServiceCollection` | matching module | matching namespace |

2. **Find or create `<TypeName>Extensions.cs`**

   - If the file exists, add the method to the existing static class.
   - If it does not exist, create it. Use `public static class <TypeName>Extensions`.
   - If the class spans multiple files use `partial`.

3. **Write the method** following these rules:
   - Name the extended parameter `@this`
   - Use `ArgumentNullException.ThrowIfNull(@this)` as the first statement for reference types
   - Use `this.` for all instance member accesses inside the body
   - Nullable annotations must be correct (`T?` vs `T`)
   - Expression-body (`=>`) is preferred for single-expression methods

4. **Add complete XML documentation** (required — `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is set globally)

5. **Verify zero warnings**: `dotnet build SharedCode.sln`

## Template

```csharp
/// <summary>
/// [One-line summary of what this method does.]
/// </summary>
/// <typeparam name="T">[Description of the type parameter, if generic.]</typeparam>
/// <param name="this">The [type name] to operate on.</param>
/// <param name="paramName">[Description of the parameter.]</param>
/// <returns>[Description of the return value.]</returns>
/// <exception cref="ArgumentNullException">
/// <paramref name="this"/> is <see langword="null"/>.
/// </exception>
public static ReturnType MethodName<T>(this TargetType @this, ParamType paramName)
{
    ArgumentNullException.ThrowIfNull(@this);

    // implementation
}
```

## Single-expression example

```csharp
/// <summary>
/// Determines whether the <paramref name="value"/> falls between
/// <paramref name="low"/> and <paramref name="high"/> (inclusive).
/// </summary>
/// <typeparam name="T">The type of the values being compared.</typeparam>
/// <param name="value">The value to test.</param>
/// <param name="low">The lower bound (inclusive).</param>
/// <param name="high">The upper bound (inclusive).</param>
/// <returns>
/// <see langword="true"/> if <paramref name="value"/> is between
/// <paramref name="low"/> and <paramref name="high"/>; otherwise <see langword="false"/>.
/// </returns>
public static bool IsBetween<T>(this T value, T low, T high)
    where T : IComparable<T> =>
    value.CompareTo(low) >= 0 && value.CompareTo(high) <= 0;
```
