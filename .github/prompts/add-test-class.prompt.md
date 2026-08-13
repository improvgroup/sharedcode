---
mode: edit
description: Add a new MSTest test class for a SharedCode source file following project conventions.
---

# Add a Test Class

Add a new MSTest test class that exercises a source file in the SharedCode library.

## Steps

1. **Identify the source file** you want to test and locate it in the solution.

2. **Choose the right test project**

   | Source project | Test project |
   |---|---|
   | `SharedCode.Core` | `SharedCode.Core.Tests` |
   | `SharedCode.Data` | `SharedCode.Data.Tests` |

3. **Mirror the source folder structure**

   Place the new file in the same relative subfolder as the source:

   | Source file | Test file |
   |---|---|
   | `SharedCode.Core/Calendar/DateTimeExtensions.cs` | `SharedCode.Core.Tests/Calendar/DateTimeExtensionsTests.cs` |
   | `SharedCode.Data/Paging/PagingDescriptor.cs` | `SharedCode.Data.Tests/PagingDescriptorTests.cs` |

4. **Write the test class** following these rules:
   - Annotate with `[TestClass]`
   - Suppress `CA1515` — MSTest requires `public` test classes
   - Use `[TestMethod]` for single-scenario tests
   - Use `[DataTestMethod]` + `[DataRow(...)]` for parameterized tests
   - Follow the **Arrange / Act / Assert** pattern with blank lines separating each block
   - Use **MSTest assertions** (`Assert.AreEqual`, `Assert.IsTrue`, `Assert.IsNotNull`, `Assert.ThrowsExactly`)
   - Name test methods as `<MemberUnderTest>_<Scenario>_<ExpectedOutcome>`

5. **Verify zero warnings**: `dotnet build SharedCode.sln`

## Template — single-scenario test

```csharp
namespace SharedCode.Tests.<Folder>;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="<TypeUnderTest>" />.
/// </summary>
[TestClass]
[SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "MSTest requires public test classes.")]
public class <TypeUnderTest>Tests
{
    /// <summary>
    /// Tests that <Member> does [expected behavior].
    /// </summary>
    [TestMethod]
    public void <Member>_<Scenario>_<ExpectedOutcome>()
    {
        // Arrange
        var sut = <create instance or value>;

        // Act
        var result = sut.<Member>(...);

        // Assert
        Assert.AreEqual(<expected>, result);
    }
}
```

## Template — parameterized test

```csharp
/// <summary>
/// Tests that <Member> returns the expected result for various inputs.
/// </summary>
[TestMethod]
[DataRow(<input1>, <expected1>)]
[DataRow(<input2>, <expected2>)]
public void <Member>_<Scenario>_<ExpectedOutcome>(<InputType> input, <ExpectedType> expected)
{
    // Arrange
    var sut = <create instance or value>;

    // Act
    var result = sut.<Member>(input);

    // Assert
    result.Should().Be(expected);
}
```

## Template — exception test

```csharp
/// <summary>
/// Tests that <Member> throws <ExceptionType> when [condition].
/// </summary>
[TestMethod]
public void <Member>_<Condition>_Throws<ExceptionType>()
{
    // Act / Assert
    _ = Assert.ThrowsExactly<<ExceptionType>>(
        () => <sut>.<Member>(<args>));
}
```
