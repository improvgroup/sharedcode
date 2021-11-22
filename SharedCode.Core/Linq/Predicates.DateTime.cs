// <copyright file="Predicates.DateTime.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Linq;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

/// <summary>
/// The predicates class.
/// </summary>
public static partial class Predicates
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
	public static Expression<Func<DateTime, bool>> After(DateTime after) => dateTime => dateTime > after;

	/// <summary>
	/// Returns a LINQ friendly expression that returns whether the date and time is before the
	/// specified date and time.
	/// </summary>
	/// <param name="before">The date and time to be before which we return true.</param>
	/// <returns>
	/// A LINQ friendly expression that returns whether the date and time is before the specified
	/// date and time.
	/// </returns>
	public static Expression<Func<DateTime, bool>> Before(DateTime before) => dateTime => dateTime < before;


	/// <summary>
	/// Returns a LINQ friendly expression that takes a date and time and returns whether it falls
	/// between the start and end date times inclusive or exclusive of the end date time.
	/// </summary>
	/// <param name="startDateTime">The start date time.</param>
	/// <param name="endDateTime">The end date time.</param>
	/// <param name="endInclusive">if set to <c>true</c> include the end date and time in the comparison.</param>
	/// <returns>
	/// A LINQ friendly expression that takes a date and time and returns whether it falls between
	/// the start and end date times inclusive or exclusive of the end date time.
	/// </returns>
	[SuppressMessage("Readability", "RCS1238:Avoid nested ?: operators.", Justification = "<Pending>")]
	public static Expression<Func<DateTime, bool>> Between(
		DateTime startDateTime,
		DateTime endDateTime,
		bool endInclusive = false)
	{
		if (startDateTime == endDateTime)
		{
			return dateTime => dateTime == startDateTime && endInclusive;
		}

		return startDateTime < endDateTime
			? endInclusive
				? (dateTime => dateTime >= startDateTime && dateTime <= endDateTime)
				: (dateTime => dateTime >= startDateTime && dateTime < endDateTime)
			: Between(endDateTime, startDateTime, endInclusive);
	}
}
