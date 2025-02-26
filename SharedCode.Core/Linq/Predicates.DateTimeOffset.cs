﻿
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
	[SuppressMessage("Readability", "RCS1238:Avoid nested ?: operators.", Justification = "<Pending>")]
	public static Expression<Func<DateTimeOffset, bool>> Between(
		DateTimeOffset start,
		DateTimeOffset end,
		bool inclusive = false)
	{
		return start == end
			? (dateTime => dateTime == start && inclusive)
			: start < end
				? inclusive
					? (dateTime => dateTime >= start && dateTime <= end)
					: (dateTime => dateTime >= start && dateTime < end)
				: Between(end, start, inclusive);
	}
}
