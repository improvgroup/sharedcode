// <copyright file="IncludeExpressionInformation.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications;

using System.Linq.Expressions;

/// <summary>
/// The include expression information class.
/// </summary>
public class IncludeExpressionInformation
{
	/// <summary>
	/// Initializes a new instance of the <see cref="IncludeExpressionInformation" /> class.
	/// </summary>
	/// <param name="expression">The expression.</param>
	/// <param name="entityType">Type of the entity.</param>
	/// <param name="propertyType">Type of the property.</param>
	public IncludeExpressionInformation(
		LambdaExpression expression,
		Type entityType,
		Type propertyType)
		: this(expression, entityType, propertyType, null, IncludeType.Include)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="IncludeExpressionInformation" /> class.
	/// </summary>
	/// <param name="expression">The expression.</param>
	/// <param name="entityType">Type of the entity.</param>
	/// <param name="propertyType">Type of the property.</param>
	/// <param name="previousPropertyType">Type of the previous property.</param>
	public IncludeExpressionInformation(
		LambdaExpression expression,
		Type entityType,
		Type propertyType,
		Type previousPropertyType)
		: this(expression, entityType, propertyType, previousPropertyType, IncludeType.ThenInclude)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="IncludeExpressionInformation" /> class.
	/// </summary>
	/// <param name="expression">The expression.</param>
	/// <param name="entityType">Type of the entity.</param>
	/// <param name="propertyType">Type of the property.</param>
	/// <param name="previousPropertyType">Type of the previous property.</param>
	/// <param name="includeType">Type of the include.</param>
	/// <exception cref="ArgumentNullException">expression</exception>
	/// <exception cref="ArgumentNullException">entityType</exception>
	/// <exception cref="ArgumentNullException">propertyType</exception>
	/// <exception cref="ArgumentNullException">previousPropertyType</exception>
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
		{
			_ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));
		}

		this.LambdaExpression = expression;
		this.EntityType = entityType;
		this.PropertyType = propertyType;
		this.PreviousPropertyType = previousPropertyType;
		this.Type = includeType;
	}

	/// <summary>
	/// Gets the type of the entity.
	/// </summary>
	/// <value>The type of the entity.</value>
	public Type EntityType { get; }

	/// <summary>
	/// Gets the lambda expression.
	/// </summary>
	/// <value>The lambda expression.</value>
	public LambdaExpression LambdaExpression { get; }

	/// <summary>
	/// Gets the type of the previous property.
	/// </summary>
	/// <value>The type of the previous property.</value>
	public Type? PreviousPropertyType { get; }

	/// <summary>
	/// Gets the type of the property.
	/// </summary>
	/// <value>The type of the property.</value>
	public Type PropertyType { get; }

	/// <summary>
	/// Gets the type.
	/// </summary>
	/// <value>The type.</value>
	public IncludeType Type { get; }
}
