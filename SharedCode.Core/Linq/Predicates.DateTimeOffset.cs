// <copyright file="Predicates.DateTimeOffset.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Linq
{
	using System;
	using System.Linq.Expressions;

	/// <summary>
	/// The predicates class.
	/// </summary>
	public partial class Predicates
	{
		/// <summary>
		/// Returns a LINQ friendly expression that returns whether the date and time is after the
		/// specified date and time.
		/// </summary>
		/// <param name="after">The date and time to be after which we return true.</param>
		/// <returns>
		/// A LINQ friendly expression that returns whether the date and time is after the specified
		/// date and time.
		/// </returns>
		public static Expression<Func<DateTimeOffset, bool>> After(DateTimeOffset after) =>
			dateTime => dateTime > after;

		/// <summary>
		/// Returns a LINQ friendly expression that returns whether the date and time is before the
		/// specified date and time.
		/// </summary>
		/// <param name="before">The date and time to be before which we return true.</param>
		/// <returns>
		/// A LINQ friendly expression that returns whether the date and time is before the
		/// specified date and time.
		/// </returns>
		public static Expression<Func<DateTimeOffset, bool>> Before(DateTimeOffset before) =>
			dateTime => dateTime < before;

		/// <summary>
		/// Returns a LINQ friendly expression that takes a date and time and returns whether it
		/// falls between the start and end date times inclusive or exclusive of the end date time.
		/// </summary>
		/// <param name="start">The start date time.</param>
		/// <param name="end">The end date time.</param>
		/// <param name="inclusive">
		/// if set to <c>true</c> include the end date and time in the comparison.
		/// </param>
		/// <returns>
		/// A LINQ friendly expression that takes a date and time and returns whether it falls
		/// between the start and end date times inclusive or exclusive of the end date time.
		/// </returns>
		public static Expression<Func<DateTimeOffset, bool>> Between(
			DateTimeOffset start,
			DateTimeOffset end,
			bool inclusive = false)
		{
			if (start == end)
			{
				return dateTime => dateTime == start && inclusive;
			}

			if (start < end)
			{
				if (inclusive)
				{
					return dateTime => dateTime >= start && dateTime <= end;
				}

				return dateTime => dateTime >= start && dateTime < end;
			}

			return Between(end, start, inclusive);
		}
	}
}
