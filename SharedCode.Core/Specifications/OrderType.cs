// <copyright file="OrderType.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications
{
	/// <summary>
	/// The order type enumeration.
	/// </summary>
	public enum OrderType
	{
		/// <summary>
		/// None
		/// </summary>
		None = 0,

		/// <summary>
		/// Order by
		/// </summary>
		OrderBy = 1,

		/// <summary>
		/// Order by descending
		/// </summary>
		OrderByDescending = 2,

		/// <summary>
		/// Then by
		/// </summary>
		ThenBy = 3,

		/// <summary>
		/// Then by descending
		/// </summary>
		ThenByDescending = 4,
	}
}
