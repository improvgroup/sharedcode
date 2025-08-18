
using SharedCode.Specifications.Exceptions;

namespace SharedCode.Specifications.Evaluators;
/// <summary>
/// The in-memory specification evaluator. Implements the <see
/// cref="IInMemorySpecificationEvaluator" />.
/// </summary>
/// <seealso cref="IInMemorySpecificationEvaluator" />
public class InMemorySpecificationEvaluator : IInMemorySpecificationEvaluator
{
	/// <summary>
	/// The evaluators
	/// </summary>
	private readonly List<IInMemoryEvaluator> evaluators = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="InMemorySpecificationEvaluator" /> class.
	/// </summary>
	public InMemorySpecificationEvaluator()
	{
		this.evaluators.AddRange(
			  new IInMemoryEvaluator[]
			  {
				  WhereEvaluator.Instance,
				  OrderEvaluator.Instance,
				  PaginationEvaluator.Instance
			  });
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InMemorySpecificationEvaluator" /> class.
	/// </summary>
	/// <param name="evaluators">The evaluators.</param>
	public InMemorySpecificationEvaluator(IEnumerable<IInMemoryEvaluator> evaluators) => this.evaluators.AddRange(evaluators);

	/// <summary>
	/// Gets the default.
	/// </summary>
	/// <value>The default.</value>
	/// <remarks>
	/// Will use singleton for default configuration. Yet, it can be instantiated if necessary, with
	/// default or provided evaluators.
	/// </remarks>
	public static InMemorySpecificationEvaluator Default { get; } = new InMemorySpecificationEvaluator();

	/// <inheritdoc />
	public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification)
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));
		_ = specification.Selector ?? throw new SelectorNotFoundException();

		var baseQuery = this.Evaluate(source, (ISpecification<T>)specification);

		var resultQuery = baseQuery.Select(specification.Selector.Compile());

		return specification.PostProcessingAction is null
			? resultQuery
			: specification.PostProcessingAction(resultQuery);
	}

	/// <inheritdoc />
	public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification)
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));
		if (specification.SearchCriterias.Any())
		{
			throw new NotSupportedException("The specification contains Search expressions and can't be evaluated with in-memory evaluator.");
		}

		foreach (var evaluator in this.evaluators)
		{
			source = evaluator.Evaluate(source, specification);
		}

		return specification.PostProcessingAction is null
			? source
			: specification.PostProcessingAction(source);
	}
}
