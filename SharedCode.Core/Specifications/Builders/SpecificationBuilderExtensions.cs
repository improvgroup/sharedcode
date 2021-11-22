// <copyright file="SpecificationBuilderExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;
using SharedCode.Specifications.Exceptions;

using System.Linq.Expressions;

/// <summary>
/// The specification builder extensions class.
/// </summary>
public static class SpecificationBuilderExtensions
{
	/// <summary>
	/// If the entity instances are modified, this will not be detected by the change tracker.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <exception cref="ArgumentNullException">specificationBuilder</exception>
	public static ISpecificationBuilder<T> AsNoTracking<T>(this ISpecificationBuilder<T> specificationBuilder) where T : class
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		specificationBuilder.Specification.AsNoTracking = true;

		return specificationBuilder;
	}

	/// <summary>
	/// The query will then keep track of returned instances (without tracking them in the normal
	/// way) and ensure no duplicates are created in the query results
	/// </summary>
	/// <remarks>for more info: https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries</remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <exception cref="ArgumentNullException">specificationBuilder</exception>
	public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(this ISpecificationBuilder<T> specificationBuilder) where T : class
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = true;

		return specificationBuilder;
	}

	/// <summary>
	/// The generated sql query will be split into multiple SQL queries
	/// </summary>
	/// <remarks>
	/// This feature was introduced in EF Core 5.0. It only works when using Include for more info: https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <exception cref="ArgumentNullException">specificationBuilder</exception>
	public static ISpecificationBuilder<T> AsSplitQuery<T>(
		this ISpecificationBuilder<T> specificationBuilder) where T : class
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		specificationBuilder.Specification.AsSplitQuery = true;

		return specificationBuilder;
	}

	/// <summary>
	/// Must be called after specifying criteria
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder">The specification builder.</param>
	/// <param name="specificationName">The specification name.</param>
	/// <param name="args">Any arguments used in defining the specification</param>
	/// <exception cref="ArgumentNullException">specificationBuilder</exception>
	/// <exception cref="ArgumentException">
	/// Required input <paramref name="specificationName" /> was null or empty.
	/// </exception>
	public static ICacheSpecificationBuilder<T> EnableCache<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		string specificationName,
		params object[] args) where T : class
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		if (string.IsNullOrEmpty(specificationName))
		{
			throw new ArgumentException($"Required input {specificationName} was null or empty.", specificationName);
		}

		specificationBuilder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";

		specificationBuilder.Specification.CacheEnabled = true;

		return new CacheSpecificationBuilder<T>(specificationBuilder.Specification);
	}

	/// <summary>
	/// Specify an include expression. This information is utilized to build Include function in the
	/// query, which ORM tools like Entity Framework use to include related entities (via navigation
	/// properties) in the query result.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="includeExpression"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
		this ISpecificationBuilder<T> specificationBuilder,
		Expression<Func<T, TProperty>> includeExpression) where T : class
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		var info = new IncludeExpressionInformation(includeExpression, typeof(T), typeof(TProperty));

		((List<IncludeExpressionInformation>)specificationBuilder.Specification.IncludeExpressions).Add(info);

		return new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification);
	}

	/// <summary>
	/// Specify a collection of navigation properties, as strings, to include in the query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="includeString"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static ISpecificationBuilder<T> Include<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		string includeString) where T : class
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		((List<string>)specificationBuilder.Specification.IncludeStrings).Add(includeString);
		return specificationBuilder;
	}

	/// <summary>
	/// Specify the query result will be ordered by <paramref name="orderExpression" /> in an
	/// ascending order
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="orderExpression"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static IOrderedSpecificationBuilder<T> OrderBy<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		Expression<Func<T, object?>> orderExpression)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		((List<(Expression<Func<T, object?>> OrderExpression, OrderType OrderType)>)specificationBuilder.Specification.OrderExpressions)
			.Add((orderExpression, OrderType.OrderBy));

		return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
	}

	/// <summary>
	/// Specify the query result will be ordered by <paramref name="orderExpression" /> in a
	/// descending order
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="orderExpression"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		Expression<Func<T, object?>> orderExpression)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		((List<(Expression<Func<T, object?>> OrderExpression, OrderType OrderType)>)specificationBuilder.Specification.OrderExpressions)
			.Add((orderExpression, OrderType.OrderByDescending));

		return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
	}

	/// <summary>
	/// Specify a transform function to apply to the result of the query and returns the same
	/// <typeparamref name="T" /> type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="predicate"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static ISpecificationBuilder<T> PostProcessingAction<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		Func<IEnumerable<T>, IEnumerable<T>> predicate)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		specificationBuilder.Specification.PostProcessingAction = predicate;

		return specificationBuilder;
	}

	/// <summary>
	/// Specify a transform function to apply to the result of the query. and returns another
	/// <typeparamref name="TResult" /> type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="predicate"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static ISpecificationBuilder<T, TResult> PostProcessingAction<T, TResult>(
		this ISpecificationBuilder<T, TResult> specificationBuilder,
		Func<IEnumerable<TResult>, IEnumerable<TResult>> predicate)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		specificationBuilder.Specification.PostProcessingAction = predicate;

		return specificationBuilder;
	}

	/// <summary>
	/// Specify a 'SQL LIKE' operations for search purposes
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="selector">the property to apply the SQL LIKE against</param>
	/// <param name="searchTerm">the value to use for the SQL LIKE</param>
	/// <param name="searchGroup">the index used to group sets of Selectors and SearchTerms together</param>
	/// <returns>The specification builder.</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public static ISpecificationBuilder<T> Search<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		Expression<Func<T, string>> selector,
		string searchTerm,
		int searchGroup = 1) where T : class
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		((List<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)>)specificationBuilder.Specification.SearchCriterias)
			.Add((selector, searchTerm, searchGroup));

		return specificationBuilder;
	}

	/// <summary>
	/// Specify a transform function to apply to the <typeparamref name="T" /> element to produce
	/// another <typeparamref name="TResult" /> element.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="selector"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static ISpecificationBuilder<T, TResult> Select<T, TResult>(
		this ISpecificationBuilder<T, TResult> specificationBuilder,
		Expression<Func<T, TResult>> selector)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		specificationBuilder.Specification.Selector = selector;

		return specificationBuilder;
	}

	/// <summary>
	/// Specify the number of elements to skip before returning the remaining elements.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="skip">number of elements to skip</param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="DuplicateSkipException"></exception>
	public static ISpecificationBuilder<T> Skip<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		int skip)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		if (specificationBuilder.Specification.Skip != null)
			throw new DuplicateSkipException();

		specificationBuilder.Specification.Skip = skip;
		specificationBuilder.Specification.IsPagingEnabled = true;
		return specificationBuilder;
	}

	/// <summary>
	/// Specify the number of elements to return.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="take"></param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="DuplicateTakeException"></exception>
	public static ISpecificationBuilder<T> Take<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		int take)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		if (specificationBuilder.Specification.Take != null)
			throw new DuplicateTakeException();

		specificationBuilder.Specification.Take = take;
		specificationBuilder.Specification.IsPagingEnabled = true;
		return specificationBuilder;
	}

	/// <summary>
	/// Specify a predicate that will be applied to the query
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="specificationBuilder"></param>
	/// <param name="criteria"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public static ISpecificationBuilder<T> Where<T>(
		this ISpecificationBuilder<T> specificationBuilder,
		Expression<Func<T, bool>> criteria)
	{
		_ = specificationBuilder ?? throw new ArgumentNullException(nameof(specificationBuilder));

		((List<Expression<Func<T, bool>>>)specificationBuilder.Specification.WhereExpressions).Add(criteria);

		return specificationBuilder;
	}
}
