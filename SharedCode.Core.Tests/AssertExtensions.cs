namespace SharedCode.Tests;

using System.Collections;
using System.Threading.Tasks;

using TUnit.Assertions;

/// <summary>
/// The assert extensions class.
/// </summary>
public static class AssertExtensions
{
    /// <summary>
    /// Asserts that the expected and actual values are equal using the specified comparer.
    /// </summary>
    /// <typeparam name="T">The type being compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="comparer">The comparer class.</param>
    public static async Task AreEqual<T>(T expected, T actual, IComparer comparer)
    {
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        await Assert.That(comparer.Compare(expected, actual)).IsEqualTo(0);
    }

    /// <summary>
    /// Asserts that the expected and actual values are equal using the specified comparer.
    /// </summary>
    /// <typeparam name="T">The type being compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="compareFunction">The compare function.</param>
    public static async Task AreEqual<T>(T expected, T actual, CompareFunc<T> compareFunction)
    {
        _ = compareFunction ?? throw new ArgumentNullException(nameof(compareFunction));

        await Assert.That(compareFunction(expected, actual)).IsTrue();
    }
}
