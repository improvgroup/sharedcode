
namespace SharedCode.Data.EntityFramework.Specifications.Evaluators;

using SharedCode.Data.EntityFramework.Specifications.Extensions;
using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;

/// <summary>
/// Class SearchEvaluator.
/// Implements the <see cref="IEvaluator" />
/// </summary>
/// <seealso cref="IEvaluator" />
public sealed class SearchEvaluator : IEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="SearchEvaluator"/> class from being created.
	/// </summary>
	private SearchEvaluator() { }
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static SearchEvaluator Instance { get; } = new SearchEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; } = true;

	/// <inheritdoc />
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		foreach (var searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
		{
			var criterias = searchCriteria.Select(x => (x.Selector, x.SearchTerm));
			query = query.Search(criterias);
		}

		return query;
	}
}
