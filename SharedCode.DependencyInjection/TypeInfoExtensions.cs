// <copyright file="ReflectionExtensions.cs" company="William Forney">
//     Copyright © 2021 William Forney. All Rights Reserved.

namespace SharedCode.DependencyInjection;

using System.Reflection;

/// <summary>
/// The type information extension methods class.
/// </summary>
internal static class TypeInfoExtensions
{
	/// <summary>
	/// Determines whether this type information is assignable to the specified generic type information.
	/// </summary>
	/// <param name="typeInfo">This type information.</param>
	/// <param name="genericTypeInfo">The generic type information.</param>
	/// <returns>
	/// <c>true</c> if this type is assignable to the specified generic type information; otherwise, <c>false</c>.
	/// </returns>
	internal static bool IsAssignableToGenericTypeDefinition(this TypeInfo typeInfo, TypeInfo genericTypeInfo)
	{
		foreach (var interfaceType in typeInfo.ImplementedInterfaces.Select(t => t.GetTypeInfo()))
		{
			if (interfaceType.IsGenericType)
			{
				var typeDefinitionTypeInfo = interfaceType
					.GetGenericTypeDefinition()
					.GetTypeInfo();

				if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
					return true;
			}
		}

		if (typeInfo.IsGenericType)
		{
			var typeDefinitionTypeInfo = typeInfo
				.GetGenericTypeDefinition()
				.GetTypeInfo();

			if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
				return true;
		}

		var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();

		return baseTypeInfo?.IsAssignableToGenericTypeDefinition(genericTypeInfo) == true;
	}
}
