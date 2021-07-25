// <copyright file="EnumerableExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Collections.Generic
{
	using SharedCode.Properties;

	using System;
	using System.Collections.Generic;
	using System.Globalization;

	/// <summary>
	/// The enumeration utilities class.
	/// </summary>
	public static class EnumerationUtilities
	{
		/// <summary>
		/// Returns a list of the values in the specified enumeration.
		/// </summary>
		/// <typeparam name="T">The type of the enumeration.</typeparam>
		/// <returns>The list of the values in the specified enumeration.</returns>
		/// <exception cref="ArgumentException">
		/// The type of the specified enumeration must be derived from the base <see cref="Enum" /> structure.
		/// </exception>
		public static List<T> ToList<T>() where T : struct
		{
			var enumType = typeof(T);

			// Cannot use type constraints on value types, so have to do check like this.
			if (enumType.BaseType != typeof(Enum))
			{
				throw new ArgumentException(Resources.TheTypeOfTMustBeOfTypeSystemEnum);
			}

			var enumValuesArray = Enum.GetValues(enumType);

			var enumValuesList = new List<T>(enumValuesArray.Length);

			foreach (int val in enumValuesArray)
			{
				enumValuesList.Add((T)Enum.Parse(enumType, val.ToString(CultureInfo.InvariantCulture)));
			}

			return enumValuesList;
		}
	}
}
