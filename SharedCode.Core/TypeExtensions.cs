namespace SharedCode;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

/// <summary>
/// The type extensions class
/// </summary>
public static class TypeExtensions
{
	/// <summary>
	/// Gets the base type of this.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>Type.</returns>
	public static Type? BaseType(this Type @this) => @this.GetTypeInfo().BaseType;

	/// <summary>
	/// Returns the generic type arguments.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>Type[].</returns>
	public static Type[] GenericArguments(this Type @this) => @this.GetTypeInfo().GenericTypeArguments;

	/// <summary>
	/// Gets all fields.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>FieldInfo[].</returns>
	public static FieldInfo[] GetAllFields(this Type @this) => @this.GetTypeInfo().DeclaredFields.Where(x => !x.IsStatic).ToArray();

	/// <summary>
	/// Loads the custom attributes from the type
	/// </summary>
	/// <typeparam name="T">The type of the custom attribute to find.</typeparam>
	/// <param name="this">This type.</param>
	/// <returns>The custom attribute of type T, if found.</returns>
	public static T? GetAttribute<T>(this Type @this) where T : Attribute => @this.GetAttributes<T>().FirstOrDefault();

	/// <summary>
	/// Loads the custom attributes from the type
	/// </summary>
	/// <typeparam name="T">The type of the custom attribute to find.</typeparam>
	/// <param name="this">This type.</param>
	/// <returns>An enumeration of attributes of type T that were found.</returns>
	/// <exception cref="ArgumentNullException">typeWithAttributes</exception>
	public static IEnumerable<T> GetAttributes<T>(this Type @this) where T : Attribute
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

		// Try to find the configuration attribute for the default logger if it exists
		var configAttributes = Attribute.GetCustomAttributes(@this, typeof(T), inherit: false);
		if (configAttributes is null)
			yield break;

		foreach (var configAttribute in configAttributes)
		{
			yield return (T)configAttribute;
		}
	}

	/// <summary>
	/// Gets the declared fields.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>FieldInfo[].</returns>
	public static FieldInfo[] GetDeclaredFields(this Type @this) => @this.GetTypeInfo().DeclaredFields.Where(x => !x.IsStatic).ToArray();

	/// <summary>
	/// Gets the display name of the specified type.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>The display name.</returns>
	/// <exception cref="ArgumentNullException">input</exception>
	public static string? GetDisplayName(this Type @this)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		Contract.Ensures(Contract.Result<string>() != null);

		var displayName = @this.Name;

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
	/// Gets the method.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <param name="methodName">Name of the method.</param>
	/// <returns>MethodInfo.</returns>
	public static MethodInfo? GetMethod(this Type @this, string methodName) => @this.GetTypeInfo().GetDeclaredMethod(methodName);

	/// <summary>
	/// Gets the private constructors.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>ConstructorInfo[].</returns>
	public static ConstructorInfo[] GetPrivateConstructors(this Type @this) => @this.GetTypeInfo().DeclaredConstructors.ToArray();

	/// <summary>
	/// Gets the private field.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <param name="fieldName">Name of the field.</param>
	/// <returns>FieldInfo.</returns>
	public static FieldInfo? GetPrivateField(this Type @this, string fieldName) => @this.GetTypeInfo().GetDeclaredField(fieldName);

	/// <summary>
	/// Gets the private method.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <param name="methodName">Name of the method.</param>
	/// <returns>MethodInfo.</returns>
	public static MethodInfo? GetPrivateMethod(this Type @this, string methodName) => @this.GetTypeInfo().GetDeclaredMethod(methodName);

	/// <summary>
	/// Gets the private static field.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <param name="fieldName">Name of the field.</param>
	/// <returns>FieldInfo.</returns>
	public static FieldInfo? GetPrivateStaticField(this Type @this, string fieldName) => @this.GetTypeInfo().GetDeclaredField(fieldName);

	/// <summary>
	/// Gets the private static method.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <param name="methodName">Name of the method.</param>
	/// <returns>MethodInfo.</returns>
	public static MethodInfo? GetPrivateStaticMethod(this Type @this, string methodName) => @this.GetTypeInfo().GetDeclaredMethod(methodName);

	/// <summary>
	/// Gets the public constructors.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>ConstructorInfo[].</returns>
	public static ConstructorInfo[] GetPublicConstructors(this Type @this) => @this.GetTypeInfo().DeclaredConstructors.ToArray();

	/// <summary>
	/// Gets the public properties.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns>PropertyInfo[].</returns>
	public static PropertyInfo[] GetPublicProperties(this Type @this) => @this.GetTypeInfo().DeclaredProperties.ToArray();

	/// <summary>
	/// Determines whether this is assignable from the specified type.
	/// </summary>
	/// <param name="this">From.</param>
	/// <param name="to">To.</param>
	/// <returns><c>true</c> if this is assignable from the specified type; otherwise, <c>false</c>.</returns>
	public static bool IsAssignableFrom(this Type @this, Type to) => @this.GetTypeInfo().IsAssignableFrom(to.GetTypeInfo());

	/// <summary>
	/// Determines whether the specified type is boolean.
	/// </summary>
	/// <param name="this">The type in question.</param>
	/// <returns><c>true</c> if the specified type is boolean; otherwise, <c>false</c>.</returns>
	public static bool IsBoolean(this Type @this) => @this == typeof(bool);

	/// <summary>
	/// Determines whether this is class.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns><c>true</c> if the specified this is class; otherwise, <c>false</c>.</returns>
	public static bool IsClass(this Type @this) => @this.GetTypeInfo().IsClass;

	/// <summary>
	/// Determines whether the specified this is enum.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns><c>true</c> if the specified this is enum; otherwise, <c>false</c>.</returns>
	public static bool IsEnum(this Type @this) => @this.GetTypeInfo().IsEnum;

	/// <summary>
	/// Determines whether this is an instance of the type of the specified object.
	/// </summary>
	/// <param name="this">From.</param>
	/// <param name="to">To.</param>
	/// <returns>
	/// <c>true</c> if this is an instance of the type of the specified object; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsInstanceOfType(this Type @this, object to) => @this?.IsAssignableFrom(to?.GetType()) ?? false;

	/// <summary>
	/// Return true if the type is a System.Nullable wrapper of a value type
	/// </summary>
	/// <param name="this">The type to check</param>
	/// <returns>True if the type is a System.Nullable wrapper</returns>
	/// <exception cref="InvalidOperationException">
	/// The current type is not a generic type. That is, <see cref="Type.IsGenericType"></see>
	/// returns false.
	/// </exception>
	/// <exception cref="NotSupportedException">
	/// The invoked method is not supported in the base class. Derived classes must provide an implementation.
	/// </exception>
	/// <exception cref="ArgumentNullException">type</exception>
	public static bool IsNullable(this Type? @this) => (@this?.IsGenericType ?? false) && @this.GetGenericTypeDefinition() == typeof(Nullable<>);

	/// <summary>
	/// Determines whether the specified type is string.
	/// </summary>
	/// <param name="this">The type to check.</param>
	/// <returns><c>true</c> if the specified type is string; otherwise, <c>false</c>.</returns>
	public static bool IsString(this Type @this) => @this == typeof(string);

	/// <summary>
	/// Alternative version of <see cref="Type.IsSubclassOf" /> that supports raw generic types
	/// (generic types without any type parameters).
	/// </summary>
	/// <param name="this">
	/// To type to determine for whether it derives from <paramref name="baseType" />.
	/// </param>
	/// <param name="baseType">The base type class for which the check is made.</param>
	public static bool IsSubclassOfRawGeneric(this Type? @this, Type? baseType)
	{
		if (@this is null || baseType is null)
		{
			return false;
		}

		while (@this != typeof(object))
		{
			var cur = @this?.IsGenericType ?? false ? @this.GetGenericTypeDefinition() : @this;
			if (baseType == cur)
			{
				return true;
			}

			@this = @this?.BaseType;
		}

		return false;
	}

	/// <summary>
	/// Determines whether the specified type name is a subclass of this type.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <param name="typeName">Name of the type.</param>
	/// <returns>
	/// <c>true</c> if the specified type name is a subclass of this type; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsSubclassOfTypeByName(this Type? @this, string typeName)
	{
		while (@this is not null)
		{
			if (@this.Name == typeName)
			{
				return true;
			}

			@this = @this.BaseType();
		}

		return false;
	}

	/// <summary>
	/// Determines whether this is a value type.
	/// </summary>
	/// <param name="this">This type.</param>
	/// <returns><c>true</c> if this is a value type; otherwise, <c>false</c>.</returns>
	public static bool IsValueType(this Type @this) => @this.GetTypeInfo().IsValueType;
}
