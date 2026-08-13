namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests for <see cref="Extensions" />.
/// </summary>
[TestClass]
public class ExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="Extensions.IsBetween{T}" /> returns <see langword="true" /> when the
    /// value is within bounds.
    /// </summary>
    [DataTestMethod]
    [DataRow(5, 1, 10)]
    [DataRow(1, 1, 10)]
    [DataRow(10, 1, 10)]
    public void IsBetween_ValueInRange_ReturnsTrue(int value, int low, int high)
    {
        // Act
        var result = value.IsBetween(low, high);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsBetween{T}" /> returns <see langword="false" /> when
    /// the value is outside bounds.
    /// </summary>
    [DataTestMethod]
    [DataRow(0, 1, 10)]
    [DataRow(11, 1, 10)]
    public void IsBetween_ValueOutOfRange_ReturnsFalse(int value, int low, int high)
    {
        // Act
        var result = value.IsBetween(low, high);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.In{T}" /> returns <see langword="true" /> when the value
    /// is in the list.
    /// </summary>
    [TestMethod]
    public void In_ValueIsInList_ReturnsTrue()
    {
        // Arrange
        const int value = 3;

        // Act
        var result = value.In(1, 2, 3, 4);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.In{T}" /> returns <see langword="false" /> when the value
    /// is not in the list.
    /// </summary>
    [TestMethod]
    public void In_ValueIsNotInList_ReturnsFalse()
    {
        // Arrange
        const int value = 5;

        // Act
        var result = value.In(1, 2, 3, 4);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IfNotNull{T,TResult}" /> invokes the function when the
    /// target is not null.
    /// </summary>
    [TestMethod]
    public void IfNotNull_TargetNotNull_InvokesFunction()
    {
        // Arrange
        const string target = "hello";

        // Act
        var result = target.IfNotNull(s => s.Length);

        // Assert
        result.Should().Be(5);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IfNotNull{T,TResult}" /> returns default when the target
    /// is null.
    /// </summary>
    [TestMethod]
    public void IfNotNull_TargetIsNull_ReturnsDefault()
    {
        // Arrange
        string? target = null;

        // Act
        var result = target.IfNotNull(s => s.Length);

        // Assert
        result.Should().Be(default);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsNull(object)" /> returns <see langword="true" /> for a
    /// null object.
    /// </summary>
    [TestMethod]
    public void IsNull_NullObject_ReturnsTrue()
    {
        // Arrange
        object? obj = null;

        // Act
        var result = obj!.IsNull();

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsNotNull" /> returns <see langword="true" /> for a
    /// non-null object.
    /// </summary>
    [TestMethod]
    public void IsNotNull_NonNullObject_ReturnsTrue()
    {
        // Arrange
        object obj = new();

        // Act
        var result = obj.IsNotNull();

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that <see cref="Extensions.ChangeType{T}(object,T)" /> returns the fallback value
    /// when conversion fails.
    /// </summary>
    [TestMethod]
    public void ChangeType_ConversionFails_ReturnsFallback()
    {
        // Arrange
        object source = "not-a-number";

        // Act
        var result = source.ChangeType(defaultValue: -1);

        // Assert
        result.Should().Be(-1);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.ChangeType{T}(object)" /> converts an integer to string.
    /// </summary>
    [TestMethod]
    public void ChangeType_ValidConversion_ReturnsConvertedValue()
    {
        // Arrange
        object source = 42;

        // Act
        var result = source.ChangeType<string>();

        // Assert
        result.Should().Be("42");
    }

    /// <summary>
    /// Tests that <see cref="Extensions.GetPropertyValue{T}" /> returns the correct property
    /// value via reflection.
    /// </summary>
    [TestMethod]
    public void GetPropertyValue_ValidProperty_ReturnsValue()
    {
        // Arrange
        var obj = new SampleRecord("World");

        // Act
        var result = obj.GetPropertyValue<string>("Greeting");

        // Assert
        result.Should().Be("World");
    }

    /// <summary>
    /// Tests that <see cref="Extensions.GetPropertyValue{T}" /> returns null when the property
    /// does not exist.
    /// </summary>
    [TestMethod]
    public void GetPropertyValue_MissingProperty_ReturnsNull()
    {
        // Arrange
        var obj = new SampleRecord("World");

        // Act
        var result = obj.GetPropertyValue<string>("NonExistent");

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// A simple record used as a reflection target.
    /// </summary>
    private sealed record SampleRecord(string Greeting);
}
