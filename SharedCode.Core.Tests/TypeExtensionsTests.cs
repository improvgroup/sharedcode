namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="TypeExtensions" />.
/// </summary>
public class TypeExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="TypeExtensions.GetDisplayName" /> inserts spaces before capital
    /// letters in a PascalCase type name.
    /// </summary>
    [Test]
    public async Task GetDisplayName_PascalCaseTypeName_InsertsSpacesBeforeCapitals()
    {
        // Arrange
        var type = typeof(TypeExtensionsTests);

        // Act
        var result = type.GetDisplayName();

        // Assert
        await Assert.That(result).IsEqualTo("Type Extensions Tests");
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsNullable" /> returns <see langword="true" /> for a
    /// <see cref="Nullable{T}" /> type.
    /// </summary>
    [Test]
    public async Task IsNullable_NullableType_ReturnsTrue()
    {
        // Arrange
        var type = typeof(int?);

        // Act
        var result = type.IsNullable();

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsNullable" /> returns <see langword="false" /> for a
    /// non-nullable value type.
    /// </summary>
    [Test]
    public async Task IsNullable_NonNullableValueType_ReturnsFalse()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var result = type.IsNullable();

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsNullable" /> returns <see langword="false" /> when
    /// called on a null type reference.
    /// </summary>
    [Test]
    public async Task IsNullable_NullType_ReturnsFalse()
    {
        // Arrange
        Type? type = null;

        // Act
        var result = type.IsNullable();

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsBoolean" /> returns <see langword="true" /> for
    /// <see cref="bool" />.
    /// </summary>
    [Test]
    public async Task IsBoolean_BoolType_ReturnsTrue()
    {
        // Arrange
        var type = typeof(bool);

        // Act
        var result = type.IsBoolean();

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsBoolean" /> returns <see langword="false" /> for a
    /// non-boolean type.
    /// </summary>
    [Test]
    public async Task IsBoolean_NonBoolType_ReturnsFalse()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var result = type.IsBoolean();

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsString" /> returns <see langword="true" /> for
    /// <see cref="string" />.
    /// </summary>
    [Test]
    public async Task IsString_StringType_ReturnsTrue()
    {
        // Arrange
        var type = typeof(string);

        // Act
        var result = type.IsString();

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsString" /> returns <see langword="false" /> for a
    /// non-string type.
    /// </summary>
    [Test]
    public async Task IsString_NonStringType_ReturnsFalse()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var result = type.IsString();

        // Assert
        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.BaseType" /> returns the correct base type.
    /// </summary>
    [Test]
    public async Task BaseType_DerivedClass_ReturnsBaseClass()
    {
        // Arrange
        var type = typeof(ArgumentNullException);

        // Act
        var result = type.BaseType();

        // Assert
        await Assert.That(result).IsEqualTo(typeof(ArgumentException));
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsSubclassOfTypeByName" /> returns
    /// <see langword="true" /> when the ancestor type name matches.
    /// </summary>
    [Test]
    public async Task IsSubclassOfTypeByName_MatchingAncestorName_ReturnsTrue()
    {
        // Arrange
        var type = typeof(ArgumentNullException);

        // Act
        var result = type.IsSubclassOfTypeByName(nameof(ArgumentException));

        // Assert
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="TypeExtensions.IsSubclassOfTypeByName" /> returns
    /// <see langword="false" /> when the ancestor type name does not match.
    /// </summary>
    [Test]
    public async Task IsSubclassOfTypeByName_NoMatchingAncestorName_ReturnsFalse()
    {
        // Arrange
        var type = typeof(ArgumentNullException);

        // Act
        var result = type.IsSubclassOfTypeByName("NonExistentBase");

        // Assert
        await Assert.That(result).IsFalse();
    }
}
