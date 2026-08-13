namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Reflection;

/// <summary>
/// Tests for <see cref="AssemblyExtensions" />.
/// </summary>
[TestClass]
public class AssemblyExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="AssemblyExtensions.GetAttribute{T}" /> returns the attribute when it
    /// is present on the assembly.
    /// </summary>
    [TestMethod]
    public void GetAttribute_AssemblyHasAttribute_ReturnsAttribute()
    {
        // Arrange
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        // Act
        var result = assembly.GetAttribute<AssemblyTitleAttribute>();

        // Assert
        result.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that <see cref="AssemblyExtensions.GetAttribute{T}" /> returns null when the
    /// attribute is not present on the assembly.
    /// </summary>
    [TestMethod]
    public void GetAttribute_AssemblyMissingAttribute_ReturnsNull()
    {
        // Arrange
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        // Act
        var result = assembly.GetAttribute<ObsoleteAttribute>();

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that <see cref="AssemblyExtensions.GetAttribute{T}" /> throws
    /// <see cref="ArgumentNullException" /> when the assembly is null.
    /// </summary>
    [TestMethod]
    public void GetAttribute_NullAssembly_ThrowsArgumentNullException()
    {
        // Arrange
        Assembly? assembly = null;

        // Act
        var act = () => assembly!.GetAttribute<AssemblyTitleAttribute>();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
