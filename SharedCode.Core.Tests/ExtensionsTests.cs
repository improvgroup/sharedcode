namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="Extensions" />.
/// </summary>
public class ExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="Extensions.IsBetween{T}" /> returns <see langword="true" /> when the
    /// value is within bounds.
    /// </summary>
    [Test]
    [Arguments(5, 1, 10)]
    [Arguments(1, 1, 10)]
    [Arguments(10, 1, 10)]
    public async Task IsBetween_ValueInRange_ReturnsTrue(int value, int low, int high)
    {
        // Act
        var result = value.IsBetween(low, high);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsBetween{T}" /> returns <see langword="false" /> when
    /// the value is outside bounds.
    /// </summary>
    [Test]
    [Arguments(0, 1, 10)]
    [Arguments(11, 1, 10)]
    public async Task IsBetween_ValueOutOfRange_ReturnsFalse(int value, int low, int high)
    {
        // Act
        var result = value.IsBetween(low, high);

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.In{T}" /> returns <see langword="true" /> when the value
    /// is in the list.
    /// </summary>
    [Test]
    public async Task In_ValueIsInList_ReturnsTrue()
    {
        // Arrange
        const int value = 3;

        // Act
        var result = value.In(1, 2, 3, 4);

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.In{T}" /> returns <see langword="false" /> when the value
    /// is not in the list.
    /// </summary>
    [Test]
    public async Task In_ValueIsNotInList_ReturnsFalse()
    {
        // Arrange
        const int value = 5;

        // Act
        var result = value.In(1, 2, 3, 4);

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IfNotNull{T,TResult}" /> invokes the function when the
    /// target is not null.
    /// </summary>
    [Test]
    public async Task IfNotNull_TargetNotNull_InvokesFunction()
    {
        // Arrange
        const string target = "hello";

        // Act
        var result = target.IfNotNull(s => s.Length);

        // Assert
        await Assert.That(result).IsEqualTo(5);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IfNotNull{T,TResult}" /> returns default when the target
    /// is null.
    /// </summary>
    [Test]
    public async Task IfNotNull_TargetIsNull_ReturnsDefault()
    {
        // Arrange
        string target = null!;

        // Act
        var result = target.IfNotNull(s => s.Length);

        // Assert
        await Assert.That(result).IsEqualTo(default);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsNull(object)" /> returns <see langword="true" /> for a
    /// null object.
    /// </summary>
    [Test]
    public async Task IsNull_NullObject_ReturnsTrue()
    {
        // Arrange — use a nullable wrapper to avoid CS8602 on calling extension on null directly
        object? obj = null;

#pragma warning disable CS8604 // Possible null reference argument — intentional null test
        // Act
        var result = obj.IsNull();
#pragma warning restore CS8604

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsNotNull" /> returns <see langword="true" /> for a
    /// non-null object.
    /// </summary>
    [Test]
    public async Task IsNotNull_NonNullObject_ReturnsTrue()
    {
        // Arrange
        object obj = new();

        // Act
        var result = obj.IsNotNull();

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.ChangeType{T}(object,T)" /> returns the fallback value
    /// when conversion fails.
    /// </summary>
    [Test]
    public async Task ChangeType_ConversionFails_ReturnsFallback()
    {
        // Arrange
        object source = "not-a-number";

        // Act
        var result = source.ChangeType(-1);

        // Assert
        await Assert.That(result).IsEqualTo(-1);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.ChangeType{T}(object)" /> converts an integer to string.
    /// </summary>
    [Test]
    public async Task ChangeType_ValidConversion_ReturnsConvertedValue()
    {
        // Arrange
        object source = 42;

        // Act
        var result = source.ChangeType<string>();

        // Assert
        await Assert.That(result).IsEqualTo("42");
    }

    /// <summary>
    /// Tests that <see cref="Extensions.GetPropertyValue{T}" /> returns the correct property
    /// value via reflection.
    /// </summary>
    [Test]
    public async Task GetPropertyValue_ValidProperty_ReturnsValue()
    {
        // Arrange
        var obj = new SampleRecord("World");

        // Act
        var result = obj.GetPropertyValue<string>("Greeting");

        // Assert
        await Assert.That(result).IsEqualTo("World");
    }

    /// <summary>
    /// Tests that <see cref="Extensions.GetPropertyValue{T}" /> returns null when the property
    /// does not exist.
    /// </summary>
    [Test]
    public async Task GetPropertyValue_MissingProperty_ReturnsNull()
    {
        // Arrange
        var obj = new SampleRecord("World");

        // Act
        var result = obj.GetPropertyValue<string>("NonExistent");

        // Assert
        await Assert.That(result is null).IsTrue();
    }

    /// <summary>
    /// A simple record used as a reflection target.
    /// </summary>
    private sealed record SampleRecord(string Greeting);
}
