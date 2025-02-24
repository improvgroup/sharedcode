
namespace SharedCode.Data.EntityFramework.Specifications.Extensions;

using Microsoft.EntityFrameworkCore;

using SharedCode.Specifications;

using System;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// Class IncludeExtensions.
/// </summary>
public static class IncludeExtensions
{
	/// <summary>
	/// Includes the specified information.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The source.</param>
	/// <param name="info">The information.</param>
	/// <returns>IQueryable&lt;T&gt;.</returns>
	/// <exception cref="ArgumentNullException">source</exception>
	/// <exception cref="ArgumentNullException">info</exception>
	public static IQueryable<T> Include<T>(this IQueryable<T> source, IncludeExpressionInformation info)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(info);

		var queryExpr = Expression.Call(
			typeof(EntityFrameworkQueryableExtensions),
			"Include",
			[
				info.EntityType,
				info.PropertyType
			],
			source.Expression,
			info.LambdaExpression
			);

		return source.Provider.CreateQuery<T>(queryExpr);
	}

	/// <summary>
	/// Thens the include.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The source.</param>
	/// <param name="info">The information.</param>
	/// <returns>IQueryable&lt;T&gt;.</returns>
	/// <exception cref="ArgumentNullException">source</exception>
	/// <exception cref="ArgumentNullException">info</exception>
	/// <exception cref="ArgumentNullException">PreviousPropertyType</exception>
	public static IQueryable<T> ThenInclude<T>(this IQueryable<T> source, IncludeExpressionInformation info)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(info);
		ArgumentNullException.ThrowIfNull(info.PreviousPropertyType);

		var queryExpr = Expression.Call(
			typeof(EntityFrameworkQueryableExtensions),
			"ThenInclude",
			[
				info.EntityType,
				info.PreviousPropertyType,
				info.PropertyType
			],
			source.Expression,
			info.LambdaExpression
			);

		return source.Provider.CreateQuery<T>(queryExpr);
	}
}
