namespace SharedCode.Tests.Collections;

using SharedCode.Collections;

using System;
using System.Linq;
using System.Threading.Tasks;

using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="ArrayExtensions" />.
/// </summary>
public class ArrayExtensionsTests
{
    [Test]
    public async Task ConvertTo_WithConvertibleValues_ReturnsConvertedArray()
    {
        // Arrange
        Array input = new[] { "1", "2", "3" };

        // Act
        var result = input.ConvertTo<int>();

        // Assert
        await Assert.That(result.SequenceEqual([1, 2, 3])).IsTrue();
    }

    [Test]
    public async Task ConvertTo_WithUnsupportedConversion_ThrowsNotSupportedException()
    {
        // Arrange
        Array input = new[] { DateTime.UtcNow };

        // Act / Assert
        await Assert.That(() => input.ConvertTo<Guid>()).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertTo_WithNullArray_ThrowsArgumentNullException()
    {
        // Arrange
        Array? input = null;

        // Act / Assert
        await Assert.That(() => input!.ConvertTo<int>()).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task ToList_WithNullArrayOrMapFunction_ReturnsEmptyList()
    {
        // Arrange / Act
        var nullArrayResult = ArrayExtensions.ToList<int>(null!, value => (int)value);
        var nullMapResult = new object[] { 1 }.ToList<int>(null!);

        // Assert
        await Assert.That(nullArrayResult.Count).IsEqualTo(0);
        await Assert.That(nullMapResult.Count).IsEqualTo(0);
    }

    [Test]
    public async Task ToList_WithNullMappedValues_SkipsThem()
    {
        // Arrange
        Array input = new object[] { "1", null!, "2" };

        // Act
        var result = input.ToList<string?>(value => value?.ToString());

        // Assert
        await Assert.That(result.Count).IsEqualTo(2);
        await Assert.That(result[0]).IsEqualTo("1");
        await Assert.That(result[1]).IsEqualTo("2");
    }
}
