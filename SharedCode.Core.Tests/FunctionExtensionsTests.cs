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
        Assert.AreEqual("5", result);
        Assert.AreEqual(1, callCount);
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
        Assert.AreEqual("7", result);
        Assert.AreEqual(1, callCount);
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
        Assert.AreEqual("1", result1);
        Assert.AreEqual("2", result2);
        Assert.AreEqual(2, callCount);
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

        // Act / Assert
        _ = Assert.ThrowsExactly<ArgumentNullException>(() => func!.Memoize());
    }
}
