
namespace SharedCode;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

/// <summary>
/// The function extensions class
/// </summary>
public static class FunctionExtensions
{
	/// <summary>
	/// Memoizes the function.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="this">The function to memoize.</param>
	/// <returns>The memoized function.</returns>
	/// <exception cref="ArgumentNullException">function</exception>
	public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> @this) where T : notnull
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		Contract.Ensures(Contract.Result<Func<T, TResult>>() != null);

		var dictionary = new Dictionary<T, TResult>();
		return n =>
		{
			if (dictionary.TryGetValue(n, out var value))
			{
				return value;
			}

			var handler = @this ?? throw new ArgumentNullException(nameof(@this));
			var result = handler(n);
			dictionary.Add(n, result);
			return result;
		};
	}
}
