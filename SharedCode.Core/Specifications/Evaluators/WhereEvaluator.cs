
namespace SharedCode.Specifications.Evaluators;

using SharedCode.Specifications.Builders;

/// <summary>
/// Class WhereEvaluator. Implements the <see cref="IEvaluator" />. Implements the <see
/// cref="IInMemoryEvaluator" />.
/// </summary>
/// <seealso cref="IEvaluator" />
/// <seealso cref="IInMemoryEvaluator" />
public sealed class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="WhereEvaluator" /> class from being created.
	/// </summary>
	private WhereEvaluator()
	{
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static WhereEvaluator Instance { get; } = new WhereEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; } = true;

	/// <inheritdoc />
	public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		foreach (var criteria in specification.WhereExpressions)
		{
			query = query.Where(criteria.Compile());
		}

		return query;
	}

	/// <inheritdoc />
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		foreach (var criteria in specification.WhereExpressions)
		{
			query = query.Where(criteria);
		}

		return query;
	}
}
