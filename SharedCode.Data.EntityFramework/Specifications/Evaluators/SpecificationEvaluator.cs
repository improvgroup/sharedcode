// <copyright file="AsNoTrackingEvaluator.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework.Specifications.Evaluators;
using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;

/// <inheritdoc/>
public class SpecificationEvaluator : ISpecificationEvaluator
{
	// Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
	/// <summary>
	/// Gets the default.
	/// </summary>
	/// <value>The default.</value>
	public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

	/// <summary>
	/// The evaluators
	/// </summary>
	private readonly List<IEvaluator> evaluators = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="SpecificationEvaluator"/> class.
	/// </summary>
	public SpecificationEvaluator()
	{
		this.evaluators.AddRange(new IEvaluator[]
		{
				WhereEvaluator.Instance,
				SearchEvaluator.Instance,
				IncludeEvaluator.Instance,
				OrderEvaluator.Instance,
				PaginationEvaluator.Instance,
				AsNoTrackingEvaluator.Instance,
				AsSplitQueryEvaluator.Instance,
				AsNoTrackingWithIdentityResolutionEvaluator.Instance
		});
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="SpecificationEvaluator"/> class.
	/// </summary>
	/// <param name="evaluators">The evaluators.</param>
	public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators) =>
		this.evaluators.AddRange(evaluators);

	/// <inheritdoc/>
	public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification) where T : class
	{
		_ = specification?.Selector ?? throw new ArgumentNullException(nameof(specification));

		inputQuery = this.GetQuery(inputQuery, (ISpecification<T>)specification);

		return inputQuery.Select(specification.Selector);
	}

	/// <inheritdoc/>
	public virtual IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification));

		foreach (var evaluator in evaluateCriteriaOnly ? this.evaluators.Where(x => x.IsCriteriaEvaluator) : this.evaluators)
		{
			inputQuery = evaluator.GetQuery(inputQuery, specification);
		}

		return inputQuery;
	}
}
