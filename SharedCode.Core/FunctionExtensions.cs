// <copyright file="FunctionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

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
	/// <param name="function">The function to memoize.</param>
	/// <returns>The memoized function.</returns>
	/// <exception cref="ArgumentNullException">function</exception>
	public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> function) where T : notnull
	{
		_ = function ?? throw new ArgumentNullException(nameof(function));
		Contract.Ensures(Contract.Result<Func<T, TResult>>() != null);

		var dictionary = new Dictionary<T, TResult>();
		return n =>
		{
			if (dictionary.ContainsKey(n))
			{
				return dictionary[n];
			}

			var handler = function;
			if (handler is null)
			{
				throw new ArgumentNullException(nameof(function));
			}

			var result = handler(n);
			dictionary.Add(n, result);
			return result;
		};
	}
}
