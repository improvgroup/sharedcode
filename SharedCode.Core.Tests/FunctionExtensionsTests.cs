namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests for <see cref="FunctionExtensions" />.
/// </summary>
[TestClass]
public class FunctionExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> returns the correct result
    /// on the first call (cache miss).
    /// </summary>
    [TestMethod]
    public void Memoize_CacheMiss_ReturnsCorrectResult()
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
        result.Should().Be("5");
        callCount.Should().Be(1);
    }

    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> returns the cached result
    /// without invoking the original function a second time (cache hit).
    /// </summary>
    [TestMethod]
    public void Memoize_CacheHit_DoesNotInvokeFunctionAgain()
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
        result.Should().Be("7");
        callCount.Should().Be(1);
    }

    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> caches different keys
    /// independently.
    /// </summary>
    [TestMethod]
    public void Memoize_DifferentKeys_CachedSeparately()
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
        result1.Should().Be("1");
        result2.Should().Be("2");
        callCount.Should().Be(2);
    }

    /// <summary>
    /// Tests that <see cref="FunctionExtensions.Memoize{T,TResult}" /> throws
    /// <see cref="ArgumentNullException" /> when the function is null.
    /// </summary>
    [TestMethod]
    public void Memoize_NullFunction_ThrowsArgumentNullException()
    {
        // Arrange
        Func<int, string>? func = null;

        // Act
        var act = () => func!.Memoize();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
