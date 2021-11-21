namespace SharedCode.Specifications;

using System.Linq.Expressions;

public class IncludeExpressionInformation
{
	public IncludeExpressionInformation(
		LambdaExpression expression,
		Type entityType,
		Type propertyType)
		: this(expression, entityType, propertyType, null, IncludeType.Include)
	{
	}

	public IncludeExpressionInformation(
		LambdaExpression expression,
		Type entityType,
		Type propertyType,
		Type previousPropertyType)
		: this(expression, entityType, propertyType, previousPropertyType, IncludeType.ThenInclude)
	{
	}

	private IncludeExpressionInformation(
		LambdaExpression expression,
		Type entityType,
		Type propertyType,
		Type? previousPropertyType,
		IncludeType includeType)

	{
		_ = expression ?? throw new ArgumentNullException(nameof(expression));
		_ = entityType ?? throw new ArgumentNullException(nameof(entityType));
		_ = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

		if (includeType == IncludeType.ThenInclude)
			_ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));

		this.LambdaExpression = expression;
		this.EntityType = entityType;
		this.PropertyType = propertyType;
		this.PreviousPropertyType = previousPropertyType;
		this.Type = includeType;
	}

	public Type EntityType { get; }
	public LambdaExpression LambdaExpression { get; }
	public Type? PreviousPropertyType { get; }
	public Type PropertyType { get; }
	public IncludeType Type { get; }
}
