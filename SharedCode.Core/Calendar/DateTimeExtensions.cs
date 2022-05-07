// <copyright file="DateTimeExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Calendar;

using SharedCode;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

/// <summary>
/// The date time extensions class
/// </summary>
public static class DateTimeExtensions
{
	/// <summary>
	/// Adds the specified number of work days to the date.
	/// </summary>
	/// <param name="dto">The date time.</param>
	/// <param name="days">The number of work days to add.</param>
	/// <returns>The date time.</returns>
	public static DateTimeOffset AddWorkdays(this DateTimeOffset dto, int days)
	{
		// start from a weekday
		while (dto.DayOfWeek.IsWeekday())
		{
			dto = dto.AddDays(1.0);
		}

		for (var i = 0; i < days; ++i)
		{
			dto = dto.AddDays(1.0);
			while (dto.DayOfWeek.IsWeekday())
			{
				dto = dto.AddDays(1.0);
			}
		}

		return dto;
	}

	/// <summary>
	/// Returns age based on the specified date of birth.
	/// </summary>
	/// <param name="dateOfBirth">The date of birth.</param>
	/// <returns>The age.</returns>
	public static int Age(this DateTimeOffset dateOfBirth)
	{
		return (DateTime.Today.Month < dateOfBirth.Month || DateTime.Today.Month == dateOfBirth.Month) && DateTime.Today.Day < dateOfBirth.Day
			? DateTime.Today.Year - dateOfBirth.Year - 1
			: DateTime.Today.Year - dateOfBirth.Year;
	}

	/// <summary>
	/// DateDiff in SQL style. Datepart implemented: "year" (abbr. "yy", "yyyy"), "quarter" (abbr.
	/// "qq", "q"), "month" (abbr. "mm", "m"), "day" (abbr. "dd", "d"), "week" (abbr. "wk", "ww"),
	/// "hour" (abbr. "hh"), "minute" (abbr. "mi", "n"), "second" (abbr. "ss", "s"), "millisecond"
	/// (abbr. "ms").
	/// </summary>
	/// <param name="startDate">The start date.</param>
	/// <param name="datePart">The date part.</param>
	/// <param name="endDate">The end date.</param>
	/// <exception cref="Exception">The date part is unknown.</exception>
	/// <returns>The date difference.</returns>
	/// <exception cref="ArgumentNullException">datePart</exception>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
	public static long DateDiff(this DateTime startDate, string datePart, DateTime endDate)
	{
		_ = datePart ?? throw new ArgumentNullException(nameof(datePart));

		const int MonthsInYear = 12;
		const int QuartersInYear = 4;
		const int MonthsInQuarter = 3;
		const int DaysInWeek = 7;
		var cal = Thread.CurrentThread.CurrentCulture.Calendar;
		var yearDiff = cal.GetYear(endDate) - cal.GetYear(startDate);
		var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
		return datePart.ToLower(CultureInfo.CurrentCulture).Trim() switch
		{
			"year" or "yy" or "yyyy" => yearDiff,
			"quarter" or "qq" or "q" => (yearDiff * QuartersInYear) + ((cal.GetMonth(endDate) - 1) / MonthsInQuarter) - ((cal.GetMonth(startDate) - 1) / MonthsInQuarter),
			"month" or "mm" or "m" => (yearDiff * MonthsInYear) + cal.GetMonth(endDate) - cal.GetMonth(startDate),
			"day" or "d" or "dd" => (long)ts.TotalDays,
			"week" or "wk" or "ww" => (long)(ts.TotalDays / DaysInWeek),
			"hour" or "hh" => (long)ts.TotalHours,
			"minute" or "mi" or "n" => (long)ts.TotalMinutes,
			"second" or "ss" or "s" => (long)ts.TotalSeconds,
			"millisecond" or "ms" => (long)ts.TotalMilliseconds,
			_ => throw new Exception($"DatePart \"{datePart}\" is unknown"),
		};
	}

	/// <summary>
	/// Gets the first day of the month.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>The first day of the month.</returns>
	public static DateTime FirstDayOfMonth(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, 1);

	/// <summary>
	/// Gets the date range between this date time and the specified date time.
	/// </summary>
	/// <param name="fromDate">The from date.</param>
	/// <param name="toDate">The to date.</param>
	/// <returns>The date range <paramref name="fromDate" /> to <paramref name="toDate" />.</returns>
	public static IEnumerable<DateTime> GetDateRangeTo(this DateTime fromDate, DateTime toDate)
	{
		Contract.Ensures(Contract.Result<IEnumerable<DateTime>>() != null);

		return Enumerable.Range(0, new TimeSpan(toDate.Ticks - fromDate.Ticks).Days)
						 .Select(p => fromDate.Date.AddDays(p));
	}

	/// <summary>
	/// Returns true if two date ranges intersect.
	/// </summary>
	/// <param name="startDate">The start date.</param>
	/// <param name="endDate">The end date.</param>
	/// <param name="intersectingStartDate">The intersecting start date.</param>
	/// <param name="intersectingEndDate">The intersecting end date.</param>
	/// <returns><c>true</c> if two date ranges intersect, <c>false</c> otherwise.</returns>
	public static bool Intersects(this DateTime startDate, DateTime endDate, DateTime intersectingStartDate, DateTime intersectingEndDate) =>
		intersectingEndDate >= startDate && intersectingStartDate <= endDate;

	/// <summary>
	/// Determines whether the specified date time is between the start and end dates.
	/// </summary>
	/// <param name="dt">The date time to check.</param>
	/// <param name="startDate">The start date.</param>
	/// <param name="endDate">The end date.</param>
	/// <param name="compareTime">if set to <c>true</c> include the time in the comparison.</param>
	/// <returns>
	/// <c>true</c> if the specified date time is between the start and end dates; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsBetween(this DateTime dt, DateTime startDate, DateTime endDate, bool compareTime = false)
	{
		return compareTime
			? dt >= startDate && dt <= endDate
			: dt.Date >= startDate.Date && dt.Date <= endDate.Date;
	}

	/// <summary>
	/// Determines whether the specified value is weekend.
	/// </summary>
	/// <param name="value">The date value.</param>
	/// <returns><c>true</c> if the specified value is weekend; otherwise, <c>false</c>.</returns>
	public static bool IsWeekend(this DateTime value) => value.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday;

	/// <summary>
	/// Gets the last day of the month.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>The last day of the month.</returns>
	public static DateTime LastDayOfMonth(this DateTime dateTime) => dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);

	/// <summary>
	/// Converts the enumeration to the format string.
	/// </summary>
	/// <param name="source">The source date time.</param>
	/// <param name="dateTimeFormat">The date time format.</param>
	/// <returns>The date time format string.</returns>
	/// <exception cref="ArgumentNullException">dateTimeFormat</exception>
	public static string ToStringFormat(this DateTime source, Expression<Func<DateTimeFormat>> dateTimeFormat)
	{
		_ = dateTimeFormat ?? throw new ArgumentNullException(nameof(dateTimeFormat));
		Contract.Ensures(Contract.Result<string>() != null);

		var dateTimeFormatCompiled = dateTimeFormat.Compile().Invoke();

		var dateTimeStringFormat = Enum<string>.GetStringValue(dateTimeFormatCompiled);

		var currentCulture = Thread.CurrentThread.CurrentCulture;

		return source.ToString(dateTimeStringFormat, currentCulture);
	}

	/// <summary>
	/// Converts a System.DateTime object to Unix timestamp
	/// </summary>
	/// <param name="date">The date time.</param>
	/// <returns>The Unix timestamp.</returns>
	public static long ToUnixTimestamp(this DateTime date)
	{
		const int UnixEpochYear = 1970;
		var unixEpoch = new DateTime(year: UnixEpochYear, month: 1, day: 1, hour: 0, minute: 0, second: 0);
		var unixTimeSpan = date - unixEpoch;

		return (long)unixTimeSpan.TotalSeconds;
	}
}
