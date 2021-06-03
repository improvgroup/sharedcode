// <copyright file="StringExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Text
{
	using System;
	using System.Globalization;
	using System.Text.RegularExpressions;

	/// <summary>
	/// The string extensions class.
	/// </summary>
	public static class StringExtensions
	{
		private static readonly Regex EmailAddressRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

		/// <summary>
		/// Fills the specified format string using the specified argument.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg">The argument.</param>
		/// <returns>The formatted string.</returns>
		public static string? Fill(this string format, object? arg) => string.Format(CultureInfo.CurrentCulture, format, arg);

		/// <summary>
		/// Fills the specified format string using the specified arguments.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public static string? Fill(this string format, params object?[] args) => string.Format(CultureInfo.CurrentCulture, format, args);

		/// <summary>
		/// Fills the specified format string with the provider using the specified argument.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="arg">The argument.</param>
		/// <returns>The formatted string.</returns>
		public static string? Fill(this string format, IFormatProvider provider, object? arg) => string.Format(provider, format, arg);

		/// <summary>
		/// Fills the specified format string with the provider using the specified arguments.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public static string? Fill(this string format, IFormatProvider provider, params object?[] args) => string.Format(provider, format, args);

		/// <summary>
		/// Fills the specified format string with the invariant culture provider using the
		/// specified argument.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg">The argument.</param>
		/// <returns>The formatted string.</returns>
		public static string? FillInvariant(this string format, object? arg) => string.Format(CultureInfo.InvariantCulture, format, arg);

		/// <summary>
		/// Fills the specified format string with the invariant culture provider using the
		/// specified arguments.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public static string? FillInvariant(this string format, params object?[] args) => string.Format(CultureInfo.InvariantCulture, format, args);

		/// <summary>
		/// Replaces the format item in a specified <see cref="string" /> with the text equivalent
		/// of the value of a specified <see cref="object" /> instance.
		/// </summary>
		/// <param name="value">A composite format string</param>
		/// <param name="arg0">An <see cref="object" /> to format</param>
		/// <returns>
		/// A copy of format in which the first format item has been replaced by the <see
		/// cref="string" /> equivalent of <paramref name="arg0" />.
		/// </returns>
		public static string? Format(this string? value, object arg0) =>
			value is null ? null : string.Format(CultureInfo.CurrentCulture, value, arg0);

		/// <summary>
		/// Replaces the format item in a specified <see cref="string" /> with the text equivalent
		/// of the value of a specified <see cref="object" /> instance.
		/// </summary>
		/// <param name="value">A composite format string.</param>
		/// <param name="args">
		/// An <see cref="object" /> array containing zero or more objects to format.
		/// </param>
		/// <returns>
		/// A copy of format in which the format items have been replaced by the <see cref="string"
		/// /> equivalent of the corresponding instances of System.Object in args.
		/// </returns>
		public static string? Format(this string? value, params object[] args) =>
			value is null ? null : string.Format(CultureInfo.CurrentCulture, value, args);

		/// <summary>
		/// Returns a value indicating whether the specified values are found in this string.
		/// </summary>
		/// <param name="value">The string in which we are looking for values.</param>
		/// <param name="stringValues">Array of string values to compare</param>
		/// <returns>Return true if any string value matches</returns>
		public static bool In(this string? value, params string[] stringValues)
		{
			foreach (var otherValue in stringValues)
			{
				if (string.Equals(value, otherValue, StringComparison.Ordinal))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the specified value is numeric.
		/// </summary>
		/// <param name="theValue">The value.</param>
		/// <returns>A value indicating whether the specified value is numeric.</returns>
		public static bool IsNumeric(this string theValue) =>
			long.TryParse(theValue, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var _);

		/// <summary>
		/// Determines whether the specified string is a valid email address.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns>
		/// <c>true</c> if the specified string is a valid email address; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValidEmailAddress(this string s) => EmailAddressRegex.IsMatch(s);

		/// <summary>
		/// Returns characters from left of the value of specified length
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <param name="length">The maximum number of charaters to return.</param>
		/// <returns>The string from left.</returns>
		public static string? Left(this string? value, int length = 0) =>
			value is not null &&
				value.Length > length
					? value.Substring(0, length)
					: value;

		/// <summary>
		/// Returns characters from right of the value of the specified length
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <param name="length">The maximum number of charaters to return.</param>
		/// <returns>The string from right.</returns>
		public static string? Right(this string? value, int length = 0) =>
			value is not null &&
				value.Length > length
					? value[^length..]
					: value;

		/// <summary>
		/// Converts the string value to the specified enumeration type.
		/// </summary>
		/// <typeparam name="T">The type of enumeration.</typeparam>
		/// <param name="value">The string value to convert.</param>
		/// <returns>Returns enumeration value.</returns>
		public static T ToEnum<T>(this string value) where T : struct => value is null ? default : (T)Enum.Parse(typeof(T), value, true);

		/// <summary>
		/// Converts the specified string to title case.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The string.</returns>
		public static string ToTitleCase(this string value) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);

		/// <summary>
		/// Converts the string to title case using the specified culture information.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="cultureInfo">The culture information.</param>
		/// <returns>string.</returns>
		public static string ToTitleCase(this string value, CultureInfo cultureInfo) => cultureInfo.TextInfo.ToTitleCase(value);
	}
}
