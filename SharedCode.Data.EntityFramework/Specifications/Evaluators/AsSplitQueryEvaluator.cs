
namespace SharedCode.Data.EntityFramework.Specifications.Evaluators;

using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;

/// <summary>
/// Class AsSplitQueryEvaluator. Implements the <see cref="IEvaluator" />
/// </summary>
/// <seealso cref="IEvaluator" />
public sealed class AsSplitQueryEvaluator : IEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="AsSplitQueryEvaluator" /> class from being created.
	/// </summary>
	private AsSplitQueryEvaluator()
	{
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static AsSplitQueryEvaluator Instance { get; } = new AsSplitQueryEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; } = true;

	/// <inheritdoc />
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		if (specification.AsSplitQuery)
		{
			//query = query.AsSplitQuery(); // TODO: Support for split query is not in the current EF Core version. See https://github.com/dotnet/efcore/issues/21234
		}

		return query;
	}
}
