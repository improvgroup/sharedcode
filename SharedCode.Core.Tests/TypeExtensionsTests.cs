namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests for <see cref="TypeExtensions" />.
/// </summary>
[TestClass]
public class TypeExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="TypeExtensions.GetDisplayName" /> inserts spaces before capital
    /// letters in a PascalCase type name.
    /// </summary>
    [TestMethod]
    public void GetDisplayName_PascalCaseTypeName_InsertsSpacesBeforeCapitals()
    {
        // Arrange
        var type = typeof(TypeExtensionsTests);

        // Act
        var result = type.GetDisplayName();

        // Assert
        Assert.AreEqual("Type Extensions Tests", result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsNullable" /> returns <see langword="true" /> for a
    /// <see cref="Nullable{T}" /> type.
    /// </summary>
    [TestMethod]
    public void IsNullable_NullableType_ReturnsTrue()
    {
        // Arrange
        var type = typeof(int?);

        // Act
        var result = type.IsNullable();

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsNullable" /> returns <see langword="false" /> for a
    /// non-nullable value type.
    /// </summary>
    [TestMethod]
    public void IsNullable_NonNullableValueType_ReturnsFalse()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var result = type.IsNullable();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsNullable" /> returns <see langword="false" /> when
    /// called on a null type reference.
    /// </summary>
    [TestMethod]
    public void IsNullable_NullType_ReturnsFalse()
    {
        // Arrange
        Type? type = null;

        // Act
        var result = type.IsNullable();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsBoolean" /> returns <see langword="true" /> for
    /// <see cref="bool" />.
    /// </summary>
    [TestMethod]
    public void IsBoolean_BoolType_ReturnsTrue()
    {
        // Arrange
        var type = typeof(bool);

        // Act
        var result = type.IsBoolean();

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsBoolean" /> returns <see langword="false" /> for a
    /// non-boolean type.
    /// </summary>
    [TestMethod]
    public void IsBoolean_NonBoolType_ReturnsFalse()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var result = type.IsBoolean();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsString" /> returns <see langword="true" /> for
    /// <see cref="string" />.
    /// </summary>
    [TestMethod]
    public void IsString_StringType_ReturnsTrue()
    {
        // Arrange
        var type = typeof(string);

        // Act
        var result = type.IsString();

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsString" /> returns <see langword="false" /> for a
    /// non-string type.
    /// </summary>
    [TestMethod]
    public void IsString_NonStringType_ReturnsFalse()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var result = type.IsString();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.BaseType" /> returns the correct base type.
    /// </summary>
    [TestMethod]
    public void BaseType_DerivedClass_ReturnsBaseClass()
    {
        // Arrange
        var type = typeof(ArgumentNullException);

        // Act
        var result = type.BaseType();

        // Assert
        Assert.AreEqual(typeof(ArgumentException), result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsSubclassOfTypeByName" /> returns
    /// <see langword="true" /> when the ancestor type name matches.
    /// </summary>
    [TestMethod]
    public void IsSubclassOfTypeByName_MatchingAncestorName_ReturnsTrue()
    {
        // Arrange
        var type = typeof(ArgumentNullException);

        // Act
        var result = type.IsSubclassOfTypeByName(nameof(ArgumentException));

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsSubclassOfTypeByName" /> returns
    /// <see langword="false" /> when the ancestor type name does not match.
    /// </summary>
    [TestMethod]
    public void IsSubclassOfTypeByName_NoMatchingAncestorName_ReturnsFalse()
    {
        // Arrange
        var type = typeof(ArgumentNullException);

        // Act
        var result = type.IsSubclassOfTypeByName("NonExistentBase");

        // Assert
        Assert.IsFalse(result);
    }
}
