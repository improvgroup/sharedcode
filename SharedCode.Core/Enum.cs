// <copyright file="Enum.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode;

using SharedCode.Attributes;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;

/// <summary>
/// The enumeration class.
/// </summary>
/// <typeparam name="T">The type of the enumeration.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Ignoring this on purpose.")]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This would defeat the purpose of this class.")]
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This would defeat the purpose of this class.")]
public static class Enum<T>
{
	/// <summary>
	/// The string values
	/// </summary>
	private static readonly Hashtable StringValues = new();

	/// <summary>
	/// Gets the underlying enum type for this instance.
	/// </summary>
	/// <value>The type of the enum.</value>
	public static Type EnumType => typeof(T);

	/// <summary>
	/// Gets the values as a 'bindable' list datasource.
	/// </summary>
	/// <returns>IList for data binding</returns>
	public static IList GetListValues()
	{
		Contract.Ensures(Contract.Result<IList>() != null);

		var underlyingType = Enum.GetUnderlyingType(typeof(T));
		var values = new ArrayList();

		// Look for our string value associated with fields in this enum
		foreach (var fi in typeof(T).GetFields())
		{
			// Check for our custom attribute
			var attrs = fi.GetCustomAttributes<StringValueAttribute>(inherit: false);
			if (attrs.Any())
			{
				_ = values.Add(
					new DictionaryEntry(
						Convert.ChangeType(Enum.Parse(typeof(T), fi.Name), underlyingType, CultureInfo.CurrentCulture),
						attrs.First().Value));
			}
		}

		return values;
	}

	/// <summary>
	/// Gets the string value associated with the given enum value.
	/// </summary>
	/// <param name="valueName">Name of the enum value.</param>
	/// <returns>String Value</returns>
	/// <exception cref="ArgumentNullException">valueName</exception>
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
	[SuppressMessage("Roslynator", "RCS1075:Avoid empty catch clause that catches System.Exception.", Justification = "<Pending>")]
	public static string? GetStringValue(string valueName)
	{
		_ = valueName ?? throw new ArgumentNullException(nameof(valueName));

		string? stringValue = null;
		try
		{
			var enumType = (Enum)Enum.Parse(typeof(T), valueName, ignoreCase: true);
			stringValue = GetStringValue(enumType);
		}
		catch (Exception)
		{
			// ignored
		}

		return stringValue;
	}

	/// <summary>
	/// Gets the string value.
	/// </summary>
	/// <param name="value">The enumeration value.</param>
	/// <returns>The string value.</returns>
	/// <exception cref="ArgumentNullException">value</exception>
	public static string? GetStringValue(Enum value)
	{
		var type = value?.GetType() ?? throw new ArgumentNullException(nameof(value));
		string? output = null;

		// Check first in our cached results...
		if (StringValues.ContainsKey(value))
		{
			output = (StringValues[value] as StringValueAttribute)?.Value;
		}
		else
		{
			// Look for our 'StringValueAttribute' in the field's custom attributes
			var fi = type.GetField(value.ToString());
			var attrs = fi?.GetCustomAttributes<StringValueAttribute>(inherit: false) as StringValueAttribute[];
			if (attrs?.Length > 0)
			{
				StringValues.Add(value, attrs[0]);
				output = attrs[0].Value;
			}
		}

		return output;
	}

	/// <summary>
	/// Gets the string values associated with the enum.
	/// </summary>
	/// <returns>String value array</returns>
	public static Array GetStringValues()
	{
		Contract.Ensures(Contract.Result<Array>() != null);

		var values = new ArrayList();

		// Look for our string value associated with fields in this enum
		foreach (var fi in typeof(T).GetFields())
		{
			// Check for our custom attribute
			var attrs = fi.GetCustomAttributes<StringValueAttribute>(inherit: false);
			if (attrs.Any())
				_ = values.Add(attrs.First().Value);
		}

		return values.ToArray();
	}

	/// <summary>
	/// Return the existence of the given string value within the enum.
	/// </summary>
	/// <param name="stringValue">String value.</param>
	/// <returns>Existence of the string value</returns>
	public static bool IsStringDefined(string stringValue) => Enum.Parse(typeof(T), stringValue) is not null;

	/// <summary>
	/// Return the existence of the given string value within the enum.
	/// </summary>
	/// <param name="stringValue">String value.</param>
	/// <param name="ignoreCase">
	/// Denotes whether to conduct a case-insensitive match on the supplied string value
	/// </param>
	/// <returns>Existence of the string value</returns>
	public static bool IsStringDefined(string stringValue, bool ignoreCase) => Enum.Parse(typeof(T), stringValue, ignoreCase) is not null;

	/// <summary>
	/// Return the existence of the given string value within the enum.
	/// </summary>
	/// <param name="enumType">Type of enum</param>
	/// <param name="stringValue">String value.</param>
	/// <returns>Existence of the string value</returns>
	public static bool IsStringDefined(Type enumType, string stringValue) => Parse(enumType, stringValue) is not null;

	/// <summary>
	/// Return the existence of the given string value within the enum.
	/// </summary>
	/// <param name="enumType">Type of enum</param>
	/// <param name="stringValue">String value.</param>
	/// <param name="ignoreCase">
	/// Denotes whether to conduct a case-insensitive match on the supplied string value
	/// </param>
	/// <returns>Existence of the string value</returns>
	public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase) => Parse(enumType, stringValue, ignoreCase) is not null;

	/// <summary>
	/// Parses the supplied enum and string value to find an associated enum value (case sensitive).
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="stringValue">String value.</param>
	/// <returns>Enum value associated with the string value, or null if not found.</returns>
	public static object? Parse(Type type, string stringValue) => Parse(type, stringValue, ignoreCase: false);

	/// <summary>
	/// Parses the supplied enum and string value to find an associated enum value.
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="stringValue">String value.</param>
	/// <param name="ignoreCase">
	/// Denotes whether to conduct a case-insensitive match on the supplied string value
	/// </param>
	/// <returns>Enum value associated with the string value, or null if not found.</returns>
	/// <exception cref="ArgumentException">Supplied type must be an Enum.</exception>
	/// <exception cref="ArgumentNullException">type or stringValue</exception>
	public static object? Parse(Type type, string stringValue, bool ignoreCase)
	{
		_ = type ?? throw new ArgumentNullException(nameof(type));
		_ = stringValue ?? throw new ArgumentNullException(nameof(stringValue));

		object? output = null;
		string? enumStringValue = null;

		if (!type.IsEnum)
			throw new ArgumentException($"Supplied type must be an Enum.  Type was {type}");

		// Look for our string value associated with fields in this enum
		foreach (var fi in type.GetFields())
		{
			// Check for our custom attribute
			var attrs = fi.GetCustomAttributes<StringValueAttribute>(inherit: false);
			if (attrs.Any())
				enumStringValue = attrs.First().Value;

			// Check for equality then select actual enum value.
			if (string.Equals(enumStringValue, stringValue, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				output = Enum.Parse(type, fi.Name);
				break;
			}
		}

		return output;
	}

	/// <summary>
	/// Converts Enumeration type into a dictionary of names and values
	/// </summary>
	/// <returns>IDictionary&lt;System.String, System.Int32&gt;.</returns>
	/// <exception cref="ArgumentNullException">enumType</exception>
	/// <exception cref="InvalidCastException">object is not an Enumeration</exception>
	[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
	public static IDictionary<string, int> ToDictionary()
	{
		Contract.Ensures(Contract.Result<IDictionary<string, int>>() != null);

		if (typeof(T) is null)
		{
			throw new ArgumentNullException(typeof(T).Name);
		}

		if (!typeof(T).IsEnum)
		{
			throw new InvalidCastException("object is not an Enumeration");
		}

		var names = Enum.GetNames(typeof(T));
		var values = Enum.GetValues(typeof(T));

		return Enumerable
			.Range(0, names.Length)
			.Select(i => new KeyValuePair<string, int>(names[i], (int?)values.GetValue(i) ?? 0))
			.ToDictionary(k => k.Key, k => k.Value);
	}

	/// <summary>
	/// Converts an enumeration to a list.
	/// </summary>
	/// <returns>The list.</returns>
	/// <exception cref="ArgumentException">T must be of type System.Enum.</exception>
	public static IList<T> ToList()
	{
		Contract.Ensures(Contract.Result<List<T>>() != null);

		var enumType = typeof(T);

		// Can't use type constraints on value types, so have to do check like this
		if (enumType.BaseType != typeof(Enum))
			throw new ArgumentException("T must be of type System.Enum");

		var enumValArray = Enum.GetValues(enumType);
		var enumValList = new List<T>(enumValArray.Length);

		foreach (int val in enumValArray)
		{
			enumValList.Add((T)Enum.Parse(enumType, val.ToString(CultureInfo.CurrentCulture)));
		}

		return enumValList;
	}
}
