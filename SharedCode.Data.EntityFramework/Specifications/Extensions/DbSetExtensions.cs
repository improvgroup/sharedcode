// <copyright file="DbSetExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>
namespace SharedCode.Data.EntityFramework.Specifications.Extensions;

using Microsoft.EntityFrameworkCore;

using SharedCode.Data.EntityFramework.Specifications.Evaluators;
using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The <see cref="DbSet{TEntity}"/> extension methods class.
/// </summary>
public static class DbSetExtensions
{
	/// <summary>
	/// To enumerable as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TSource">The type of the entities in the source.</typeparam>
	/// <param name="source">The source.</param>
	/// <param name="specification">The specification.</param>
	/// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
	public static async Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(this DbSet<TSource> source, ISpecification<TSource> specification, CancellationToken cancellationToken = default) where TSource : class
	{
		var result = await SpecificationEvaluator.Default.GetQuery(source, specification).ToListAsync(cancellationToken).ConfigureAwait(false);

		return specification?.PostProcessingAction is null
			? result
			: specification.PostProcessingAction(result);
	}

	/// <summary>
	/// To list as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TSource">The type of the entities in the source.</typeparam>
	/// <param name="source">The source.</param>
	/// <param name="specification">The specification.</param>
	/// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
	public static async Task<List<TSource>> ToListAsync<TSource>(this DbSet<TSource> source, ISpecification<TSource> specification, CancellationToken cancellationToken = default) where TSource : class
	{
		var result = await SpecificationEvaluator.Default.GetQuery(source, specification).ToListAsync(cancellationToken).ConfigureAwait(false);

		return specification?.PostProcessingAction is null
			? result
			: specification.PostProcessingAction(result).ToList();
	}

	/// <summary>
	/// Adds the specification filter to the queryable.
	/// </summary>
	/// <typeparam name="TSource">The type of the entities in the source.</typeparam>
	/// <param name="source">The source.</param>
	/// <param name="specification">The specification.</param>
	/// <param name="evaluator">The evaluator.</param>
	/// <returns>IQueryable&lt;TSource&gt;.</returns>
	public static IQueryable<TSource> WithSpecification<TSource>(this IQueryable<TSource> source, ISpecification<TSource> specification, ISpecificationEvaluator? evaluator = null) where TSource : class
	{
		evaluator ??= SpecificationEvaluator.Default;
		return evaluator.GetQuery(source, specification);
	}
}
