namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests for <see cref="PropertySupport" />.
/// </summary>
[TestClass]
public class PropertySupportTests
{
    /// <summary>
    /// Tests that <see cref="PropertySupport.ExtractPropertyName{T}" /> returns the correct
    /// property name from a valid property expression.
    /// </summary>
    [TestMethod]
    public void ExtractPropertyName_ValidPropertyExpression_ReturnsPropertyName()
    {
        // Arrange
        var target = new SampleClass();

        // Act
        var result = PropertySupport.ExtractPropertyName(() => target.Name);

        // Assert
        Assert.AreEqual(nameof(SampleClass.Name), result);
    }

    /// <summary>
    /// Tests that <see cref="PropertySupport.ExtractPropertyName{T}" /> throws
    /// <see cref="ArgumentNullException" /> when the expression is null.
    /// </summary>
    [TestMethod]
    public void ExtractPropertyName_NullExpression_ThrowsArgumentNullException()
    {
        // Act / Assert
        _ = Assert.ThrowsExactly<ArgumentNullException>(
            () => PropertySupport.ExtractPropertyName<string>(null!));
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
