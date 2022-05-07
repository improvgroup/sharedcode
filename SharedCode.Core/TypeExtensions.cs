// <copyright file="TypeExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

/// <summary>
/// The type extensions class
/// </summary>
public static class TypeExtensions
{
	/// <summary>
	/// Loads the custom attributes from the type
	/// </summary>
	/// <typeparam name="T">The type of the custom attribute to find.</typeparam>
	/// <param name="typeWithAttributes">The calling assembly to search.</param>
	/// <returns>The custom attribute of type T, if found.</returns>
	public static T? GetAttribute<T>(this Type typeWithAttributes) where T : Attribute => typeWithAttributes.GetAttributes<T>().FirstOrDefault();

	/// <summary>
	/// Loads the custom attributes from the type
	/// </summary>
	/// <typeparam name="T">The type of the custom attribute to find.</typeparam>
	/// <param name="typeWithAttributes">The calling assembly to search.</param>
	/// <returns>An enumeration of attributes of type T that were found.</returns>
	/// <exception cref="ArgumentNullException">typeWithAttributes</exception>
	public static IEnumerable<T> GetAttributes<T>(this Type typeWithAttributes) where T : Attribute
	{
		_ = typeWithAttributes ?? throw new ArgumentNullException(nameof(typeWithAttributes));
		Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

		// Try to find the configuration attribute for the default logger if it exists
		var configAttributes = Attribute.GetCustomAttributes(typeWithAttributes, typeof(T), inherit: false);
		if (configAttributes is null)
			yield break;

		foreach (var configAttribute in configAttributes)
		{
			yield return (T)configAttribute;
		}
	}

	/// <summary>
	/// Gets the display name of the specified type.
	/// </summary>
	/// <param name="input">The input type.</param>
	/// <returns>The display name.</returns>
	/// <exception cref="ArgumentNullException">input</exception>
	public static string? GetDisplayName(this Type input)
	{
		_ = input ?? throw new ArgumentNullException(nameof(input));
		Contract.Ensures(Contract.Result<string>() != null);

		var displayName = input.Name;

		for (var i = (displayName?.Length ?? 0) - 1; i >= 0; i--)
		{
			var c = displayName?[i] ?? ' ';
			if (c == char.ToUpperInvariant(c) && i > 0)
			{
				displayName = displayName?.Insert(i, " ");
			}
		}

		return displayName;
	}

	/// <summary>
	/// Determines whether the specified type is boolean.
	/// </summary>
	/// <param name="type">The type in question.</param>
	/// <returns><c>true</c> if the specified type is boolean; otherwise, <c>false</c>.</returns>
	public static bool IsBoolean(this Type type) => type == typeof(bool);

	/// <summary>
	/// Return true if the type is a System.Nullable wrapper of a value type
	/// </summary>
	/// <param name="type">The type to check</param>
	/// <returns>True if the type is a System.Nullable wrapper</returns>
	/// <exception cref="InvalidOperationException">
	/// The current type is not a generic type. That is, <see cref="Type.IsGenericType"></see>
	/// returns false.
	/// </exception>
	/// <exception cref="NotSupportedException">
	/// The invoked method is not supported in the base class. Derived classes must provide an implementation.
	/// </exception>
	/// <exception cref="ArgumentNullException">type</exception>
	public static bool IsNullable(this Type? type) => (type?.IsGenericType ?? false) && type.GetGenericTypeDefinition() == typeof(Nullable<>);

	/// <summary>
	/// Determines whether the specified type is string.
	/// </summary>
	/// <param name="type">The type to check.</param>
	/// <returns><c>true</c> if the specified type is string; otherwise, <c>false</c>.</returns>
	public static bool IsString(this Type type) => type == typeof(string);

	/// <summary>
	/// Alternative version of <see cref="Type.IsSubclassOf" /> that supports raw generic types
	/// (generic types without any type parameters).
	/// </summary>
	/// <param name="toCheck">
	/// To type to determine for whether it derives from <paramref name="baseType" />.
	/// </param>
	/// <param name="baseType">The base type class for which the check is made.</param>
	public static bool IsSubclassOfRawGeneric(this Type? toCheck, Type? baseType)
	{
		if (toCheck is null || baseType is null)
		{
			return false;
		}

		while (toCheck != typeof(object))
		{
			var cur = toCheck?.IsGenericType ?? false ? toCheck.GetGenericTypeDefinition() : toCheck;
			if (baseType == cur)
			{
				return true;
			}

			toCheck = toCheck?.BaseType;
		}

		return false;
	}
}
