// <copyright file="AsNoTrackingEvaluator.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework.Specifications.Evaluators;

using Microsoft.EntityFrameworkCore;

using SharedCode.Data.EntityFramework.Specifications.Extensions;
using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;

/// <summary>
/// Class IncludeEvaluator.
/// Implements the <see cref="IEvaluator" />
/// </summary>
/// <seealso cref="IEvaluator" />
public sealed class IncludeEvaluator : IEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="IncludeEvaluator"/> class from being created.
	/// </summary>
	private IncludeEvaluator() { }
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static IncludeEvaluator Instance { get; } = new IncludeEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; }

	/// <inheritdoc />
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		foreach (var includeString in specification.IncludeStrings)
		{
			query = query.Include(includeString);
		}

		foreach (var includeInfo in specification.IncludeExpressions)
		{
			if (includeInfo.Type == IncludeType.Include)
			{
				query = query.Include(includeInfo);
			}
			else if (includeInfo.Type == IncludeType.ThenInclude)
			{
				query = query.ThenInclude(includeInfo);
			}
		}

		return query;
	}
}
