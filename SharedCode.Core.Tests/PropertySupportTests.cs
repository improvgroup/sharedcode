namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="PropertySupport" />.
/// </summary>
public class PropertySupportTests
{
    /// <summary>
    /// Tests that <see cref="PropertySupport.ExtractPropertyName{T}" /> returns the correct
    /// property name from a valid property expression.
    /// </summary>
    [Test]
    public async Task ExtractPropertyName_ValidPropertyExpression_ReturnsPropertyName()
    {
        // Arrange
        var target = new SampleClass();

        // Act
        var result = PropertySupport.ExtractPropertyName(() => target.Name);

        // Assert
        await Assert.That(result).IsEqualTo(nameof(SampleClass.Name));
    }

    /// <summary>
    /// Tests that <see cref="PropertySupport.ExtractPropertyName{T}" /> throws
    /// <see cref="ArgumentNullException" /> when the expression is null.
    /// </summary>
    [Test]
    public async Task ExtractPropertyName_NullExpression_ThrowsArgumentNullException()
    {
        // Act / Assert
        await Assert.That(() => PropertySupport.ExtractPropertyName<string>(null!)).ThrowsExactly<ArgumentNullException>();
    }

    /// <summary>
    /// A simple class used as a target for property expression tests.
    /// </summary>
    private sealed class SampleClass
    {
        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; } = string.Empty;
    }
}
