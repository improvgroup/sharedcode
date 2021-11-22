// <copyright file="AsNoTrackingEvaluator.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework.Specifications.Evaluators;

using Microsoft.EntityFrameworkCore;

using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;

/// <summary>
/// Class AsNoTrackingEvaluator. Implements the <see cref="IEvaluator" />
/// </summary>
/// <seealso cref="IEvaluator" />
public sealed class AsNoTrackingEvaluator : IEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="AsNoTrackingEvaluator" /> class from being created.
	/// </summary>
	private AsNoTrackingEvaluator()
	{
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static AsNoTrackingEvaluator Instance { get; } = new AsNoTrackingEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; } = true;

	/// <inheritdoc />
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		if (specification.AsNoTracking)
		{
			query = query.AsNoTracking();
		}

		return query;
	}
}
