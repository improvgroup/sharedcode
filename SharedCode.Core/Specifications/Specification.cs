// <copyright file="Specification.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications;

using SharedCode.Specifications.Builders;
using SharedCode.Specifications.Evaluators;

using System.Linq.Expressions;

/// <inheritdoc />
public abstract class Specification<T, TResult> : Specification<T>, ISpecification<T, TResult>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Specification{T, TResult}" /> class.
	/// </summary>
	protected Specification()
		: this(InMemorySpecificationEvaluator.Default)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Specification{T, TResult}" /> class.
	/// </summary>
	/// <param name="inMemorySpecificationEvaluator">The in memory specification evaluator.</param>
	protected Specification(IInMemorySpecificationEvaluator inMemorySpecificationEvaluator)
		: base(inMemorySpecificationEvaluator) =>
		this.Query = new SpecificationBuilder<T, TResult>(this);

	/// <inheritdoc />
	public new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; internal set; }

	/// <inheritdoc />
	public Expression<Func<T, TResult>>? Selector { get; internal set; }

	/// <summary>
	/// Gets the query.
	/// </summary>
	/// <value>The query.</value>
	protected virtual new ISpecificationBuilder<T, TResult> Query { get; }

	/// <inheritdoc />
	public virtual new IEnumerable<TResult> Evaluate(IEnumerable<T> entities) => this.Evaluator.Evaluate(entities, this);
}

/// <summary>
/// The specification class. Implements the <see cref="ISpecification{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="ISpecification{T}" />
public abstract class Specification<T> : ISpecification<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Specification{T}" /> class.
	/// </summary>
	protected Specification()
		: this(InMemorySpecificationEvaluator.Default)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Specification{T}" /> class.
	/// </summary>
	/// <param name="inMemorySpecificationEvaluator">The in memory specification evaluator.</param>
	protected Specification(IInMemorySpecificationEvaluator inMemorySpecificationEvaluator)
	{
		this.Evaluator = inMemorySpecificationEvaluator;
		this.Query = new SpecificationBuilder<T>(this);
	}

	/// <inheritdoc />
	public bool AsNoTracking { get; internal set; }

	/// <inheritdoc />
	public bool AsNoTrackingWithIdentityResolution { get; internal set; }

	/// <inheritdoc />
	public bool AsSplitQuery { get; internal set; }

	/// <inheritdoc />
	public bool CacheEnabled { get; internal set; }

	/// <inheritdoc />
	public string? CacheKey { get; internal set; }

	/// <inheritdoc />
	public IEnumerable<IncludeExpressionInformation> IncludeExpressions { get; } = new List<IncludeExpressionInformation>();

	/// <inheritdoc />
	public IEnumerable<string> IncludeStrings { get; } = new List<string>();

	/// <inheritdoc />
	public bool IsPagingEnabled { get; internal set; }

	public IEnumerable<(Expression<Func<T, object>> KeySelector, OrderType OrderType)> OrderExpressions { get; } =
		new List<(Expression<Func<T, object>> KeySelector, OrderType OrderType)>();

	/// <inheritdoc />
	public Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; internal set; }

	/// <inheritdoc />
	public IEnumerable<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)> SearchCriterias { get; } =
		new List<(Expression<Func<T, string>> Selector, string SearchTerm, int SearchGroup)>();

	/// <inheritdoc />
	public int? Skip { get; internal set; }

	/// <inheritdoc />
	public int? Take { get; internal set; }

	/// <inheritdoc />
	public IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; } = new List<Expression<Func<T, bool>>>();

	/// <summary>
	/// Gets the evaluator.
	/// </summary>
	/// <value>The evaluator.</value>
	protected IInMemorySpecificationEvaluator Evaluator { get; }

	/// <summary>
	/// Gets the query.
	/// </summary>
	/// <value>The query.</value>
	protected virtual ISpecificationBuilder<T> Query { get; }

	/// <inheritdoc />
	public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities) => this.Evaluator.Evaluate(entities, this);
}
