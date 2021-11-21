// <copyright file="PredicateBuilder.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Linq;

using System;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// The predicate builder class.
/// </summary>
public static class PredicateBuilder
{
	/// <summary>
	/// Returns an expression for the &amp;&amp; operator with the specified left and right sides.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <param name="left">The left expression.</param>
	/// <param name="right">The right expression.</param>
	/// <returns>
	/// An expression for the &amp;&amp; operator with the specified left and right sides.
	/// </returns>
	/// <exception cref="ArgumentNullException">left</exception>
	public static Expression<Func<T, bool>> And<T>(
		this Expression<Func<T, bool>> left,
		Expression<Func<T, bool>> right)
	{
		_ = left ?? throw new ArgumentNullException(nameof(left));

		return Expression.Lambda<Func<T, bool>>(
			Expression.AndAlso(
				left.Body,
				Expression.Invoke(
					right,
					left.Parameters.Cast<Expression>())),
			left.Parameters);
	}

	/// <summary>
	/// Returns a predicate expression that returns false.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <returns>A predicate expression that returns false.</returns>
	public static Expression<Func<T, bool>> False<T>() => _ => false;

	/// <summary>
	/// Returns an expression for the || operator with the specified left and right sides.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <param name="left">The left expression.</param>
	/// <param name="right">The right expression.</param>
	/// <returns>An expression for the || operator with the specified left and right sides.</returns>
	/// <exception cref="ArgumentNullException">left</exception>
	public static Expression<Func<T, bool>> Or<T>(
		this Expression<Func<T, bool>> left,
		Expression<Func<T, bool>> right)
	{
		_ = left ?? throw new ArgumentNullException(nameof(left));

		return Expression.Lambda<Func<T, bool>>(
			Expression.OrElse(
				left.Body,
				Expression.Invoke(
					right,
					left.Parameters.Cast<Expression>())),
			left.Parameters);
	}

	/// <summary>
	/// Returns a predicate expression that returns true.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <returns>A predicate expression that returns true.</returns>
	public static Expression<Func<T, bool>> True<T>() => _ => true;
}
