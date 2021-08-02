namespace SharedCode.Linq
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;

	/// <summary>
	/// The expression extension methods class.
	/// </summary>
	public static class ExpressionExtensions
	{
		/// <summary>
		/// Compiles the specified expression and calls the resulting function returning the result.
		/// </summary>
		/// <typeparam name="T">The type of the result.</typeparam>
		/// <param name="lambda">The expression.</param>
		/// <returns>The result.</returns>
		public static T GetPropertyValue<T>(this Expression<Func<T>> lambda) => lambda.Compile().Invoke();

		/// <summary>
		/// Sets the underlying property's value to the given value from the specified expression that contains the property.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="lambda">The expression.</param>
		/// <param name="value">The value to set the property to.</param>
		public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
		{
			var expression = (MemberExpression)lambda.Body;
			var propertyInfo = (PropertyInfo)expression.Member;
			var target = Expression.Lambda(expression.Expression!).Compile().DynamicInvoke();
			propertyInfo.SetValue(target, value);
		}
	}
}
