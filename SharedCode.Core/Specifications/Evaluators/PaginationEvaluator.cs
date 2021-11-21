// <copyright file="PaginationEvaluator.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Evaluators;

/// <summary>
/// Class PaginationEvaluator. Implements the <see cref="IEvaluator" />. Implements the <see
/// cref="IInMemoryEvaluator" />.
/// </summary>
/// <seealso cref="IEvaluator" />
/// <seealso cref="IInMemoryEvaluator" />
public class PaginationEvaluator : IEvaluator, IInMemoryEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="PaginationEvaluator" /> class from being created.
	/// </summary>
	private PaginationEvaluator()
	{
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static PaginationEvaluator Instance { get; } = new PaginationEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; }

	/// <inheritdoc />
	public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		if (specification.Skip is not null and not 0)
			query = query.Skip(specification.Skip.Value);

		if (specification.Take is not null)
			query = query.Take(specification.Take.Value);

		return query;
	}

	/// <inheritdoc />
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		// If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
		if (specification.Skip is not null and not 0)
			query = query.Skip(specification.Skip.Value);

		if (specification.Take is not null)
			query = query.Take(specification.Take.Value);

		return query;
	}
}
