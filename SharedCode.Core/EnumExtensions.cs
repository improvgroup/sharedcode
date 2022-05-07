// <copyright file="EnumExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode;

using System;
using System.Globalization;

/// <summary>
/// The enumeration extension methods class.
/// </summary>
public static class EnumExtensions
{
	/// <summary>
	/// Determines whether the specified match to is set.
	/// </summary>
	/// <param name="input">The input enumeration.</param>
	/// <param name="matchTo">The match to.</param>
	/// <returns><c>true</c> if the specified match to is set; otherwise, <c>false</c>.</returns>
	public static bool IsSet(this Enum input, Enum matchTo) =>
		(Convert.ToUInt32(input, CultureInfo.InvariantCulture) &
		Convert.ToUInt32(matchTo, CultureInfo.InvariantCulture)) != 0;
}
