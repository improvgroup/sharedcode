namespace SharedCode.Specifications.Evaluators;
/// <summary>
/// The evaluator interface.
/// </summary>
public interface IEvaluator
{
	/// <summary>
	/// Gets a value indicating whether this instance is criteria evaluator.
	/// </summary>
	/// <value><c>true</c> if this instance is criteria evaluator; otherwise, <c>false</c>.</value>
	bool IsCriteriaEvaluator { get; }

	/// <summary>
	/// Gets the query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="query">The query.</param>
	/// <param name="specification">The specification.</param>
	/// <returns>IQueryable&lt;T&gt;.</returns>
	IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class;
}
