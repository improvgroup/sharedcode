// <copyright file="StringBuilderExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Text;

using System.Globalization;
using System.Text;

/// <summary>
/// The string builder extensions class
/// </summary>
public static class StringBuilderExtensions
{
	/// <summary>
	/// Appends the specified <paramref name="value" /> to this <see cref="StringBuilder" /> if the
	/// <paramref name="condition" /> is true.
	/// </summary>
	/// <param name="this">The string builder.</param>
	/// <param name="value">The string value.</param>
	/// <param name="condition">if set to <c>true</c> then append the string value to the builder.</param>
	/// <returns>The <see cref="StringBuilder" />.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static StringBuilder AppendIf(this StringBuilder @this, string? value, bool condition)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		if (condition)
		{
			_ = @this.Append(value);
		}

		return @this;
	}

	/// <summary>
	/// Appends the formatted string plus a line terminator to this string builder.
	/// </summary>
	/// <param name="this">The string builder.</param>
	/// <param name="format">The format string.</param>
	/// <param name="arguments">The format arguments.</param>
	/// <returns>The string builder.</returns>
	/// <exception cref="ArgumentNullException">format or arguments</exception>
	public static StringBuilder? AppendLineFormat(this StringBuilder @this, string format, params object[] arguments)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = format ?? throw new ArgumentNullException(nameof(format));
		_ = arguments ?? throw new ArgumentNullException(nameof(arguments));

		return @this.AppendFormat(CultureInfo.CurrentCulture, format, arguments).AppendLine();
	}
}
