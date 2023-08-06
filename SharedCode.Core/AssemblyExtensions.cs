// <copyright file="AssemblyExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode;

using SharedCode.Collections;

using System;
using System.Reflection;

/// <summary>
/// The assembly extensions class
/// </summary>
public static class AssemblyExtensions
{
	/// <summary>
	/// Loads the configuration from assembly attributes
	/// </summary>
	/// <typeparam name="T">The type of the custom attribute to find.</typeparam>
	/// <param name="this">The calling assembly to search.</param>
	/// <returns>The custom attribute of type T, if found.</returns>
	/// <exception cref="ArgumentNullException">callingAssembly</exception>
	public static T? GetAttribute<T>(this Assembly @this) where T : Attribute
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		T? result = null;

		// Try to find the configuration attribute for the default logger if it exists
		var configAttributes = Attribute.GetCustomAttributes(@this, typeof(T), inherit: false);

		// get just the first one
		if (!configAttributes.IsNullOrEmpty())
		{
			result = (T)configAttributes[0];
		}

		return result;
	}
}
