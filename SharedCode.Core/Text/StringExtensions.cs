// <copyright file="StringExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Text
{
	using System;
	using System.Globalization;

	/// <summary>
	/// The string extensions class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Fills the specified format string using the specified argument.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg">The argument.</param>
		/// <returns>The formatted string.</returns>
		public static string Fill(this string format, object? arg) => string.Format(CultureInfo.CurrentCulture, format, arg);

		/// <summary>
		/// Fills the specified format string using the specified arguments.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public static string Fill(this string format, params object?[] args) => string.Format(CultureInfo.CurrentCulture, format, args);

		/// <summary>
		/// Fills the specified format string with the provider using the specified argument.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="arg">The argument.</param>
		/// <returns>The formatted string.</returns>
		public static string Fill(this string format, IFormatProvider provider, object? arg) => string.Format(provider, format, arg);

		/// <summary>
		/// Fills the specified format string with the provider using the specified arguments.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public static string Fill(this string format, IFormatProvider provider, params object?[] args) => string.Format(provider, format, args);

		/// <summary>
		/// Fills the specified format string with the invariant culture provider using the
		/// specified argument.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg">The argument.</param>
		/// <returns>The formatted string.</returns>
		public static string FillInvariant(this string format, object? arg) => string.Format(CultureInfo.InvariantCulture, format, arg);

		/// <summary>
		/// Fills the specified format string with the invariant culture provider using the
		/// specified arguments.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public static string FillInvariant(this string format, params object?[] args) => string.Format(CultureInfo.InvariantCulture, format, args);
	}
}
