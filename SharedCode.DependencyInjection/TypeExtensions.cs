// <copyright file="ReflectionExtensions.cs" company="William Forney">
//     Copyright © 2021 William Forney. All Rights Reserved.

namespace SharedCode.DependencyInjection;

using System.Reflection;

/// <summary>
/// The type extension methods class.
/// </summary>
internal static class TypeExtensions
{
	/// <summary>
	/// Determines whether this type is assignable to the other specified type.
	/// </summary>
	/// <param name="type">This type.</param>
	/// <param name="otherType">The other type.</param>
	/// <returns>
	/// <c>true</c> if this type is assignable to the other specified type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsAssignableTo(this Type type, Type otherType)
	{
		var typeInfo = type.GetTypeInfo();
		var otherTypeInfo = otherType.GetTypeInfo();

		return otherTypeInfo.IsGenericTypeDefinition
			? typeInfo.IsAssignableToGenericTypeDefinition(otherTypeInfo)
			: otherTypeInfo.IsAssignableFrom(typeInfo);
	}
}
