
namespace SharedCode.Specifications.Evaluators;

using SharedCode.Specifications;
using SharedCode.Specifications.Exceptions;

/// <summary>
/// The order evaluator class. Implements the <see cref="IEvaluator" />. Implements the <see
/// cref="IInMemoryEvaluator" />.
/// </summary>
/// <seealso cref="IEvaluator" />
/// <seealso cref="IInMemoryEvaluator" />
public sealed class OrderEvaluator : IEvaluator, IInMemoryEvaluator
{
	/// <summary>
	/// Prevents a default instance of the <see cref="OrderEvaluator" /> class from being created.
	/// </summary>
	private OrderEvaluator()
	{
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static OrderEvaluator Instance { get; } = new OrderEvaluator();

	/// <inheritdoc />
	public bool IsCriteriaEvaluator { get; }

	/// <summary>
	/// Evaluates the specified query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="query">The query.</param>
	/// <param name="specification">The specification.</param>
	/// <returns>System.Collections.Generic.IEnumerable&lt;T&gt;.</returns>
	/// <exception cref="DuplicateOrderChainException"></exception>
	public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
	{
		if (specification?.OrderExpressions is not null)
		{
			if (specification.OrderExpressions.Count(x => x.OrderType is OrderType.OrderBy or OrderType.OrderByDescending) > 1)
			{
				throw new DuplicateOrderChainException();
			}

			IOrderedEnumerable<T>? orderedQuery = null;
			foreach (var orderExpression in specification.OrderExpressions)
			{
				if (orderExpression.OrderType == OrderType.OrderBy)
				{
					orderedQuery = query?.OrderBy(orderExpression.KeySelector.Compile());
				}
				else if (orderExpression.OrderType == OrderType.OrderByDescending)
				{
					orderedQuery = query?.OrderByDescending(orderExpression.KeySelector.Compile());
				}
				else if (orderExpression.OrderType == OrderType.ThenBy)
				{
					orderedQuery = orderedQuery?.ThenBy(orderExpression.KeySelector.Compile());
				}
				else if (orderExpression.OrderType == OrderType.ThenByDescending)
				{
					orderedQuery = orderedQuery?.ThenByDescending(orderExpression.KeySelector.Compile());
				}
			}

			if (orderedQuery is not null)
			{
				query = orderedQuery;
			}
		}

		return query ?? Enumerable.Empty<T>();
	}

	/// <summary>
	/// Gets the query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="query">The query.</param>
	/// <param name="specification">The specification.</param>
	/// <returns>System.Linq.IQueryable&lt;T&gt;.</returns>
	/// <exception cref="DuplicateOrderChainException"></exception>
	public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		ArgumentNullException.ThrowIfNull(query);

		if (specification?.OrderExpressions is not null)
		{
			if (specification.OrderExpressions.Count(x => x.OrderType is OrderType.OrderBy or OrderType.OrderByDescending) > 1)
			{
				throw new DuplicateOrderChainException();
			}

			IOrderedQueryable<T>? orderedQuery = null;
			foreach (var orderExpression in specification.OrderExpressions)
			{
				if (orderExpression.OrderType == OrderType.OrderBy)
				{
					orderedQuery = query?.OrderBy(orderExpression.KeySelector);
				}
				else if (orderExpression.OrderType == OrderType.OrderByDescending)
				{
					orderedQuery = query?.OrderByDescending(orderExpression.KeySelector);
				}
				else if (orderExpression.OrderType == OrderType.ThenBy)
				{
					orderedQuery = orderedQuery?.ThenBy(orderExpression.KeySelector);
				}
				else if (orderExpression.OrderType == OrderType.ThenByDescending)
				{
					orderedQuery = orderedQuery?.ThenByDescending(orderExpression.KeySelector);
				}
			}

			if (orderedQuery is not null)
			{
				query = orderedQuery;
			}
		}

		return query!;
	}
}
