// <copyright file="AsNoTrackingEvaluator.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework.Specifications.Evaluators;

using Microsoft.EntityFrameworkCore;

using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;

/// <summary>
/// Class AsNoTrackingWithIdentityResolutionEvaluator.
/// Implements the <see cref="IEvaluator" />
/// </summary>
/// <seealso cref="IEvaluator" />
public sealed class AsNoTrackingWithIdentityResolutionEvaluator : IEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="AsNoTrackingWithIdentityResolutionEvaluator"/> class from being created.
	/// </summary>
	private AsNoTrackingWithIdentityResolutionEvaluator() { }
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static AsNoTrackingWithIdentityResolutionEvaluator Instance { get; } = new AsNoTrackingWithIdentityResolutionEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; } = true;

	/// <inheritdoc />
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		if (specification.AsNoTrackingWithIdentityResolution)
		{
			query = query.AsNoTrackingWithIdentityResolution();
		}

		return query;
	}
}
