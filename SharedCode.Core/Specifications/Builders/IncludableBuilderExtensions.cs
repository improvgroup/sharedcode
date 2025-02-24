
namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;

using System.Linq.Expressions;

/// <summary>
/// The includable builder extensions class.
/// </summary>
public static class IncludableBuilderExtensions
{
	/// <summary>
	/// Then include...
	/// </summary>
	/// <typeparam name="TEntity">The type of the t entity.</typeparam>
	/// <typeparam name="TPreviousProperty">The type of the t previous property.</typeparam>
	/// <typeparam name="TProperty">The type of the t property.</typeparam>
	/// <param name="previousBuilder">The previous builder.</param>
	/// <param name="thenIncludeExpression">The then include expression.</param>
	/// <returns>IIncludableSpecificationBuilder&lt;TEntity, TProperty&gt;.</returns>
	/// <exception cref="ArgumentNullException">previousBuilder</exception>
	public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
		this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
		Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
		where TEntity : class
	{
		_ = previousBuilder ?? throw new ArgumentNullException(nameof(previousBuilder));

		var info = new IncludeExpressionInformation(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

		((List<IncludeExpressionInformation>)previousBuilder.Specification.IncludeExpressions).Add(info);

		return new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);
	}

	/// <summary>
	/// Then include...
	/// </summary>
	/// <typeparam name="TEntity">The type of the t entity.</typeparam>
	/// <typeparam name="TPreviousProperty">The type of the t previous property.</typeparam>
	/// <typeparam name="TProperty">The type of the t property.</typeparam>
	/// <param name="previousBuilder">The previous builder.</param>
	/// <param name="thenIncludeExpression">The then include expression.</param>
	/// <returns>IIncludableSpecificationBuilder&lt;TEntity, TProperty&gt;.</returns>
	/// <exception cref="ArgumentNullException">previousBuilder</exception>
	public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
		this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
		Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
		where TEntity : class
	{
		_ = previousBuilder ?? throw new ArgumentNullException(nameof(previousBuilder));

		var info = new IncludeExpressionInformation(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

		((List<IncludeExpressionInformation>)previousBuilder.Specification.IncludeExpressions).Add(info);

		return new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);
	}
}
