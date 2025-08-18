namespace SharedCode.Specifications.Evaluators;
/// <summary>
/// The in-memory specification evaluator interface.
/// </summary>
public interface IInMemorySpecificationEvaluator
{
	/// <summary>
	/// Evaluates the specified source.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="source">The source.</param>
	/// <param name="specification">The specification.</param>
	/// <returns>IEnumerable&lt;TResult&gt;.</returns>
	IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification);

	/// <summary>
	/// Evaluates the specified source.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The source.</param>
	/// <param name="specification">The specification.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification);
}
