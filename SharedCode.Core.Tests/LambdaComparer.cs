// <copyright file="LambdaComparer.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Tests;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The lambda comparer class.
/// </summary>
/// <typeparam name="T">The type of the objects being compared.</typeparam>
/// <seealso cref="IComparer" />
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class LambdaComparer<T> : IComparer, IComparer<T>
{
	/// <summary>
	/// The compare function
	/// </summary>
	[NotNull] private readonly CompareFunc<T> CompareFunction;

	/// <summary>
	/// Initializes a new instance of the <see cref="LambdaComparer{T}" /> class.
	/// </summary>
	/// <param name="compareFunction">The compare function.</param>
	public LambdaComparer([NotNull] CompareFunc<T> compareFunction) => this.CompareFunction = compareFunction ?? throw new ArgumentNullException(nameof(compareFunction));

	/// <summary>
	/// Compares two objects and returns a value indicating whether one is less than, equal to, or
	/// greater than the other.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref
	/// name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x"
	/// /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y"
	/// />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
	/// </returns>
	[SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Would result in nested conditionals.")]
	public int Compare(object? x, object? y)
	{
		if (x is null && y is null)
			return 0;

		if (x is not T t1 || y is not T t2)
			return -1;

		return this.CompareFunction(t1, t2) ? 0 : 1;
	}

	/// <summary>
	/// Compares two objects and returns a value indicating whether one is less than, equal to, or
	/// greater than the other.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref
	/// name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x"
	/// /> is less than <paramref name="y" />.Zero <paramref name="x" /> equals <paramref name="y"
	/// />.Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
	/// </returns>
	[SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Would result in nested conditionals.")]
	public int Compare(T? x, T? y)
	{
		if (EqualityComparer<T>.Default.Equals(x, default) && EqualityComparer<T>.Default.Equals(y, default))
			return 0;

		if (x is not T t1 || y is not T t2)
			return -1;

		return this.CompareFunction(t1, t2) ? 0 : 1;
	}
}
