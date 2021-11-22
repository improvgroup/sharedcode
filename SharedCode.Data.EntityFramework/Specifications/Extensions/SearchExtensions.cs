namespace SharedCode.Data.EntityFramework.Specifications.Extensions;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// Class SearchExtensions.
/// </summary>
public static class SearchExtensions
{
	/// <summary>
	/// Filters <paramref name="source" /> by applying an 'SQL LIKE' operation to it.
	/// </summary>
	/// <typeparam name="T">The type being queried against.</typeparam>
	/// <param name="source">The sequence of <typeparamref name="T" /></param>
	/// <param name="criterias">
	/// <list type="bullet">
	/// <item>Selector, the property to apply the SQL LIKE against.</item>
	/// <item>SearchTerm, the value to use for the SQL LIKE.</item>
	/// </list>
	/// </param>
	/// <returns>The filtered queryable.</returns>
	/// <exception cref="ArgumentNullException">criterias</exception>
	public static IQueryable<T> Search<T>(this IQueryable<T> source, IEnumerable<(Expression<Func<T, string>> selector, string searchTerm)> criterias)
	{
		_ = criterias ?? throw new ArgumentNullException(nameof(criterias));

		Expression? expr = null;
		var parameter = Expression.Parameter(typeof(T), "x");

		foreach (var (selector, searchTerm) in criterias)
		{
			if (selector is null || string.IsNullOrEmpty(searchTerm))
			{
				continue;
			}

			MethodCallExpression? likeExpression = null;
			var property = typeof(EF).GetProperty(nameof(EF.Functions));
			if (property is not null)
			{
				var functions = Expression.Property(null, property);
				var like = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { functions.Type, typeof(string), typeof(string) });
				if (like is not null)
				{
					var propertySelector = ParameterReplacerVisitor.Replace(selector, selector.Parameters[0], parameter);
					var arg1 = (propertySelector as LambdaExpression)?.Body;
					if (arg1 is not null)
					{
						likeExpression = Expression.Call(
						null,
						like,
						functions,
						arg1,
						Expression.Constant(searchTerm));
					}
				}
			}

			if (expr is null)
			{
				expr = likeExpression;
			}
			else if (likeExpression is not null)
			{
				expr = Expression.OrElse(expr, likeExpression);
			}
		}

		return expr is null
			? source
			: source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
	}
}
