
namespace SharedCode.Linq;

using System.Collections.Generic;

/// <summary>
/// The enumerator extensions class.
/// </summary>
public static class EnumeratorExtensions
{
	/// <summary>
	/// Returns an enumerable from the enumerator.
	/// </summary>
	/// <typeparam name="T">The type of the enumerated items.</typeparam>
	/// <param name="enumerator">The input enumerator.</param>
	/// <returns>The enumerable.</returns>
	/// <exception cref="ArgumentNullException">enumerator</exception>
	public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
	{
		_ = enumerator ?? throw new ArgumentNullException(nameof(enumerator));

		while (enumerator.MoveNext())
		{
			yield return enumerator.Current;
		}
	}
}
