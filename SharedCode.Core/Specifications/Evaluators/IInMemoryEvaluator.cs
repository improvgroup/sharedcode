namespace SharedCode.Specifications.Evaluators;
/// <summary>
/// The in-memory evaluator interface.
/// </summary>
public interface IInMemoryEvaluator
{
	/// <summary>
	/// Evaluates the specified query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="query">The query.</param>
	/// <param name="specification">The specification.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification);
}
