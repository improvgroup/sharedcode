
namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;

using System.Linq.Expressions;

/// <summary>
/// The ordered builder extensions class.
/// </summary>
public static class OrderedBuilderExtensions
{
	/// <summary>
	/// Then by.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="orderedBuilder">The ordered builder.</param>
	/// <param name="orderExpression">The order expression.</param>
	/// <returns>IOrderedSpecificationBuilder&lt;T&gt;.</returns>
	/// <exception cref="ArgumentNullException">orderBuilder</exception>
	public static IOrderedSpecificationBuilder<T> ThenBy<T>(
		this IOrderedSpecificationBuilder<T> orderedBuilder,
		Expression<Func<T, object?>> orderExpression)
	{
		_ = orderedBuilder ?? throw new ArgumentNullException(nameof(orderedBuilder));

		((List<(Expression<Func<T, object?>> OrderExpression, OrderType OrderType)>)orderedBuilder.Specification.OrderExpressions)
			.Add((orderExpression, OrderType.ThenBy));

		return orderedBuilder;
	}

	/// <summary>
	/// Then by descending.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="orderedBuilder">The ordered builder.</param>
	/// <param name="orderExpression">The order expression.</param>
	/// <returns>IOrderedSpecificationBuilder&lt;T&gt;.</returns>
	/// <exception cref="ArgumentNullException">orderBuilder</exception>
	public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
		this IOrderedSpecificationBuilder<T> orderedBuilder,
		Expression<Func<T, object?>> orderExpression)
	{
		_ = orderedBuilder ?? throw new ArgumentNullException(nameof(orderedBuilder));

		((List<(Expression<Func<T, object?>> OrderExpression, OrderType OrderType)>)orderedBuilder.Specification.OrderExpressions)
			.Add((orderExpression, OrderType.ThenByDescending));

		return orderedBuilder;
	}
}
