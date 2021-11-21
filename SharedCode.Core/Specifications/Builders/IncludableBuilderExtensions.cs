namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;

using System.Linq.Expressions;

public static class IncludableBuilderExtensions
{
	public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
		this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
		Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
		where TEntity : class
	{
		var info = new IncludeExpressionInformation(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

		((List<IncludeExpressionInformation>)previousBuilder.Specification.IncludeExpressions).Add(info);

		var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

		return includeBuilder;
	}

	public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
		this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
		Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
		where TEntity : class
	{
		var info = new IncludeExpressionInformation(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

		((List<IncludeExpressionInformation>)previousBuilder.Specification.IncludeExpressions).Add(info);

		var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

		return includeBuilder;
	}
}
