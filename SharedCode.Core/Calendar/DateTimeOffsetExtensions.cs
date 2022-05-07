// <copyright file="DateTimeExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Calendar;

using SharedCode;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

/// <summary>
/// The date time extensions class
/// </summary>
public static class DateTimeOffsetExtensions
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
		=> (DateTime.Today.Month < dateOfBirth.Month || DateTime.Today.Month == dateOfBirth.Month)
		   && DateTime.Today.Day < dateOfBirth.Day
			? DateTime.Today.Year - dateOfBirth.Year - 1
			: DateTime.Today.Year - dateOfBirth.Year;

	/// <summary>
	/// Computes the time zone variance in minutes between the specified date time offset and UTC.
	/// </summary>
	/// <param name="dateTimeOffset">The date time offset.</param>
	/// <returns>The time zone variance in minutes.</returns>
	public static int ComputeTimeZoneVariance(this DateTimeOffset dateTimeOffset)
	{
		var difference = dateTimeOffset - dateTimeOffset.ToUniversalTime();
		return Convert.ToInt32(difference.TotalMinutes);
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
	public static long DateDiff(this DateTimeOffset startDate, string datePart, DateTimeOffset endDate)
	{
		_ = datePart ?? throw new ArgumentNullException(nameof(datePart));
		var culture = Thread.CurrentThread.CurrentCulture;
		var cal = culture.Calendar;
		var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
		return datePart.ToLower(culture).Trim() switch
		{
			"year" or "yy" or "yyyy" => cal.GetYear(endDate.LocalDateTime) - cal.GetYear(startDate.LocalDateTime),
			"quarter" or "qq" or "q" =>
				((cal.GetYear(endDate.LocalDateTime) - cal.GetYear(startDate.LocalDateTime)) * 4) +
				((cal.GetMonth(endDate.LocalDateTime) - 1) / 3) -
				((cal.GetMonth(startDate.LocalDateTime) - 1) / 3),
			"month" or "mm" or "m" =>
				((cal.GetYear(endDate.LocalDateTime) - cal.GetYear(startDate.LocalDateTime)) * 12) +
				cal.GetMonth(endDate.LocalDateTime) - cal.GetMonth(startDate.LocalDateTime),
			"day" or "d" or "dd" => (long)ts.TotalDays,
			"week" or "wk" or "ww" => (long)(ts.TotalDays / 7),
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
	public static DateTimeOffset FirstDayOfMonth(this DateTimeOffset dateTime) => new(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Offset);

	/// <summary>
	/// Gets the date range between this date time and the specified date time.
	/// </summary>
	/// <param name="fromDate">The from date.</param>
	/// <param name="toDate">The to date.</param>
	/// <returns>The date range <paramref name="fromDate" /> to <paramref name="toDate" />.</returns>
	public static IEnumerable<DateTimeOffset> GetDateRangeTo(this DateTimeOffset fromDate, DateTimeOffset toDate)
	{
		Contract.Ensures(Contract.Result<IEnumerable<DateTimeOffset>>() != null);

		return Enumerable.Range(0, new TimeSpan(toDate.Ticks - fromDate.Ticks).Days)
						 .Select(p => new DateTimeOffset(fromDate.Date.AddDays(p)));
	}

	/// <summary>
	/// Returns true if two date ranges intersect.
	/// </summary>
	/// <param name="startDate">The start date.</param>
	/// <param name="endDate">The end date.</param>
	/// <param name="intersectingStartDate">The intersecting start date.</param>
	/// <param name="intersectingEndDate">The intersecting end date.</param>
	/// <returns><c>true</c> if two date ranges intersect, <c>false</c> otherwise.</returns>
	public static bool Intersects(this DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset intersectingStartDate, DateTimeOffset intersectingEndDate) =>
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
	public static bool IsBetween(this DateTimeOffset dt, DateTimeOffset startDate, DateTimeOffset endDate, bool compareTime = false) =>
		compareTime
			? dt >= startDate && dt <= endDate
			: dt.Date >= startDate.Date && dt.Date <= endDate.Date;

	/// <summary>
	/// Determines whether the specified value is weekend.
	/// </summary>
	/// <param name="value">The date value.</param>
	/// <returns><c>true</c> if the specified value is weekend; otherwise, <c>false</c>.</returns>
	public static bool IsWeekend(this DateTimeOffset value) => value.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday;

	/// <summary>
	/// Gets the last day of the month.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>The last day of the month.</returns>
	public static DateTimeOffset LastDayOfMonth(this DateTimeOffset dateTime) => dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);

	/// <summary>
	/// Converts the enumeration to the format string.
	/// </summary>
	/// <param name="source">The source date time.</param>
	/// <param name="dateTimeFormat">The date time format.</param>
	/// <returns>The date time format string.</returns>
	/// <exception cref="ArgumentNullException">dateTimeFormat</exception>
	public static string ToStringFormat(this DateTimeOffset source, Expression<Func<DateTimeFormat>> dateTimeFormat)
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
	public static long ToUnixTimestamp(this DateTimeOffset date)
	{
		var unixEpoch = new DateTimeOffset(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, offset: DateTimeOffset.UtcNow.Offset);
		var unixTimeSpan = date - unixEpoch;

		return (long)unixTimeSpan.TotalSeconds;
	}
}
