namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="FunctionExtensions" />.
/// </summary>
public class FunctionExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> returns the correct result
    /// on the first call (cache miss).
    /// </summary>
    [Test]
    public async Task Memoize_CacheMiss_ReturnsCorrectResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, string> func = n =>
        {
            callCount++;
            return n.ToString(System.Globalization.CultureInfo.InvariantCulture);
        };
        var memoized = func.Memoize();

        // Act
        var result = memoized(5);

        // Assert
        await Assert.That(result).IsEqualTo("5");
        await Assert.That(callCount).IsEqualTo(1);
    }

    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> returns the cached result
    /// without invoking the original function a second time (cache hit).
    /// </summary>
    [Test]
    public async Task Memoize_CacheHit_DoesNotInvokeFunctionAgain()
    {
        // Arrange
        var callCount = 0;
        Func<int, string> func = n =>
        {
            callCount++;
            return n.ToString(System.Globalization.CultureInfo.InvariantCulture);
        };
        var memoized = func.Memoize();

        // Act
        _ = memoized(7);
        var result = memoized(7);

        // Assert
        await Assert.That(result).IsEqualTo("7");
        await Assert.That(callCount).IsEqualTo(1);
    }

    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> caches different keys
    /// independently.
    /// </summary>
    [Test]
    public async Task Memoize_DifferentKeys_CachedSeparately()
    {
        // Arrange
        var callCount = 0;
        Func<int, string> func = n =>
        {
            callCount++;
            return n.ToString(System.Globalization.CultureInfo.InvariantCulture);
        };
        var memoized = func.Memoize();

        // Act
        var result1 = memoized(1);
        var result2 = memoized(2);

        // Assert
        await Assert.That(result1).IsEqualTo("1");
        await Assert.That(result2).IsEqualTo("2");
        await Assert.That(callCount).IsEqualTo(2);
    }

    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> throws
    /// <see cref="ArgumentNullException" /> when the function is null.
    /// </summary>
    [Test]
    public async Task Memoize_NullFunction_ThrowsArgumentNullException()
    {
        // Arrange
        Func<int, string>? func = null;

        // Act / Assert
        await Assert.That(() => func!.Memoize()).ThrowsExactly<ArgumentNullException>();
    }
}
