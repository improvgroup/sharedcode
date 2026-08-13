namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Reflection;

/// <summary>
/// Tests for <see cref="AssemblyExtensions" />.
/// </summary>
public class AssemblyExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="AssemblyExtensions.GetAttribute{T}" /> returns the attribute when it
    /// is present on the assembly.
    /// </summary>
    [Test]
    public async Task GetAttribute_AssemblyHasAttribute_ReturnsAttribute()
    {
        // Arrange
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        // Act
        var result = assembly.GetAttribute<AssemblyTitleAttribute>();

        // Assert
        await Assert.That(result is not null).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="AssemblyExtensions.GetAttribute{T}" /> returns null when the
    /// attribute is not present on the assembly.
    /// </summary>
    [Test]
    public async Task GetAttribute_AssemblyMissingAttribute_ReturnsNull()
    {
        // Arrange
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        // Act
        var result = assembly.GetAttribute<ObsoleteAttribute>();

        // Assert
        await Assert.That(result is null).IsTrue();
    }

    /// <summary>
    /// Tests that <see cref="AssemblyExtensions.GetAttribute{T}" /> throws
    /// <see cref="ArgumentNullException" /> when the assembly is null.
    /// </summary>
    [Test]
    public async Task GetAttribute_NullAssembly_ThrowsArgumentNullException()
    {
        // Arrange
        Assembly? assembly = null;

        // Act / Assert
        await Assert.That(() => assembly!.GetAttribute<AssemblyTitleAttribute>()).ThrowsExactly<ArgumentNullException>();
    }
}
