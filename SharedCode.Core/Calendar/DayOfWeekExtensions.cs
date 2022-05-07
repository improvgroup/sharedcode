// <copyright file="DayOfWeekExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Calendar
{

	using System;

	/// <summary>
	///     The day of week extensions class
	/// </summary>
	public static class DayOfWeekExtensions
	{
		/// <summary>
		///     Determines whether the specified <paramref name="dayOfWeek"/> is weekday.
		/// </summary>
		/// <param name="dayOfWeek">The day of the week.</param>
		/// <returns><c>true</c> if the specified <paramref name="dayOfWeek"/> is weekday; otherwise, <c>false</c>.</returns>
		public static bool IsWeekday(this DayOfWeek dayOfWeek)
		{
			switch (dayOfWeek)
			{
				case DayOfWeek.Sunday:
				case DayOfWeek.Saturday:
					return false;

				default:
					return true;
			}
		}

		/// <summary>
		///     Determines whether the specified <paramref name="dayOfWeek"/> is weekend.
		/// </summary>
		/// <param name="dayOfWeek">The day of the week.</param>
		/// <returns><c>true</c> if the specified <paramref name="dayOfWeek"/> is weekend; otherwise, <c>false</c>.</returns>
		public static bool IsWeekend(this DayOfWeek dayOfWeek) => !dayOfWeek.IsWeekday();
	}
}