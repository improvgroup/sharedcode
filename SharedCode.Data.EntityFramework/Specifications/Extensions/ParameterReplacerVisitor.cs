// <copyright file="ParameterReplacerVisitor.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework.Specifications.Extensions;

using System.Linq.Expressions;

/// <summary>
/// Class ParameterReplacerVisitor. This class cannot be inherited. Implements the <see
/// cref="ExpressionVisitor" />.
/// </summary>
/// <seealso cref="ExpressionVisitor" />
internal sealed class ParameterReplacerVisitor : ExpressionVisitor
{
	/// <summary>
	/// The new expression
	/// </summary>
	private readonly Expression newExpression;

	/// <summary>
	/// The old parameter
	/// </summary>
	private readonly ParameterExpression oldParameter;

	/// <summary>
	/// Initializes a new instance of the <see cref="ParameterReplacerVisitor" /> class.
	/// </summary>
	/// <param name="oldParameter">The old parameter.</param>
	/// <param name="newExpression">The new expression.</param>
	private ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newExpression)
	{
		this.oldParameter = oldParameter;
		this.newExpression = newExpression;
	}

	/// <summary>
	/// Replaces the specified expression.
	/// </summary>
	/// <param name="expression">The expression.</param>
	/// <param name="oldParameter">The old parameter.</param>
	/// <param name="newExpression">The new expression.</param>
	/// <returns>Expression.</returns>
	internal static Expression Replace(Expression expression, ParameterExpression oldParameter, Expression newExpression) =>
		new ParameterReplacerVisitor(oldParameter, newExpression).Visit(expression);

	/// <inheritdoc />
	protected override Expression VisitParameter(ParameterExpression node) =>
		node == this.oldParameter ? this.newExpression : node;
}
