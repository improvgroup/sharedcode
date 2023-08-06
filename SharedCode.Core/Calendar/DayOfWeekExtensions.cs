// <copyright file="DayOfWeekExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Calendar
{
	using System;

	/// <summary>
	/// The day of week extensions class
	/// </summary>
	public static class DayOfWeekExtensions
	{
		/// <summary>
		/// Determines whether the specified <paramref name="dayOfWeek" /> is weekday.
		/// </summary>
		/// <param name="dayOfWeek">The day of the week.</param>
		/// <returns>
		/// <c>true</c> if the specified <paramref name="dayOfWeek" /> is weekday; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsWeekday(this DayOfWeek dayOfWeek)
		{
			return dayOfWeek switch
			{
				DayOfWeek.Saturday or DayOfWeek.Sunday => false,
				DayOfWeek.Monday => true,
				DayOfWeek.Tuesday => true,
				DayOfWeek.Wednesday => true,
				DayOfWeek.Thursday => true,
				DayOfWeek.Friday => true,
				_ => false,
			};
		}

		/// <summary>
		/// Determines whether the specified <paramref name="this" /> is weekend.
		/// </summary>
		/// <param name="this">The day of the week.</param>
		/// <returns>
		/// <c>true</c> if the specified <paramref name="this" /> is weekend; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsWeekend(this DayOfWeek @this) => !@this.IsWeekday();
	}
}
