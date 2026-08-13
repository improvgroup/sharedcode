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
    [TestMethod]
    [DataRow(5, 1, 10)]
    [DataRow(1, 1, 10)]
    [DataRow(10, 1, 10)]
    public void IsBetween_ValueInRange_ReturnsTrue(int value, int low, int high)
    {
        // Act
        var result = value.IsBetween(low, high);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsBetween{T}" /> returns <see langword="false" /> when
    /// the value is outside bounds.
    /// </summary>
    [TestMethod]
    [DataRow(0, 1, 10)]
    [DataRow(11, 1, 10)]
    public void IsBetween_ValueOutOfRange_ReturnsFalse(int value, int low, int high)
    {
        // Act
        var result = value.IsBetween(low, high);

        // Assert
        Assert.IsFalse(result);
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
        Assert.IsTrue(result);
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
        Assert.IsFalse(result);
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
        Assert.AreEqual(5, result);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IfNotNull{T,TResult}" /> returns default when the target
    /// is null.
    /// </summary>
    [TestMethod]
    public void IfNotNull_TargetIsNull_ReturnsDefault()
    {
        // Arrange
        string target = null!;

        // Act
        var result = target.IfNotNull(s => s.Length);

        // Assert
        Assert.AreEqual(default, result);
    }

    /// <summary>
    /// Tests that <see cref="Extensions.IsNull(object)" /> returns <see langword="true" /> for a
    /// null object.
    /// </summary>
    [TestMethod]
    public void IsNull_NullObject_ReturnsTrue()
    {
        // Arrange — use a nullable wrapper to avoid CS8602 on calling extension on null directly
        object? obj = null;

#pragma warning disable CS8604 // Possible null reference argument — intentional null test
        // Act
        var result = obj.IsNull();
#pragma warning restore CS8604

        // Assert
        Assert.IsTrue(result);
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
        Assert.IsTrue(result);
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
        var result = source.ChangeType(-1);

        // Assert
        Assert.AreEqual(-1, result);
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
        Assert.AreEqual("42", result);
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
        Assert.AreEqual("World", result);
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
        Assert.IsNull(result);
    }

    /// <summary>
    /// A simple record used as a reflection target.
    /// </summary>
    private sealed record SampleRecord(string Greeting);
}
