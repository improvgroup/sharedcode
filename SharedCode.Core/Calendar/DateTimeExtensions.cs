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
	/// <param name="this">The date time.</param>
	/// <param name="days">The number of work days to add.</param>
	/// <returns>The date time.</returns>
	public static DateTime AddWorkdays(this DateTime @this, int days)
	{
		var result = @this;
		var negate = days < 0;

		// If the date is on a weekend, advance to Monday.
		while (result.DayOfWeek.IsWeekend())
		{
			result = result.AddDays(negate ? -1 : 1);
		}

		// Add weekdays
		for (var i = 0; i < days; i++)
		{
			result = result.AddDays(negate ? -1 : 1);
			while (result.DayOfWeek.IsWeekend())
			{
				result = result.AddDays(negate ? -1 : 1);
			}
		}

		return result;
	}

	/// <summary>
	/// Returns age based on the specified date of birth.
	/// </summary>
	/// <param name="this">The date of birth.</param>
	/// <returns>The age.</returns>
	public static int Age(this DateTime @this)
	{
		return (DateTime.Today.Month < @this.Month || DateTime.Today.Month == @this.Month) && DateTime.Today.Day < @this.Day
			? DateTime.Today.Year - @this.Year - 1
			: DateTime.Today.Year - @this.Year;
	}

	/// <summary>
	/// DateDiff in SQL style. Datepart implemented: "year" (abbr. "yy", "yyyy"), "quarter" (abbr.
	/// "qq", "q"), "month" (abbr. "mm", "m"), "day" (abbr. "dd", "d"), "week" (abbr. "wk", "ww"),
	/// "hour" (abbr. "hh"), "minute" (abbr. "mi", "n"), "second" (abbr. "ss", "s"), "millisecond"
	/// (abbr. "ms").
	/// </summary>
	/// <param name="this">The start date.</param>
	/// <param name="datePart">The date part.</param>
	/// <param name="endDate">The end date.</param>
	/// <exception cref="Exception">The date part is unknown.</exception>
	/// <returns>The date difference.</returns>
	/// <exception cref="ArgumentNullException">datePart</exception>
	[SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
	public static long DateDiff(this DateTime @this, string datePart, DateTime endDate)
	{
		_ = datePart ?? throw new ArgumentNullException(nameof(datePart));

		const int MonthsInYear = 12;
		const int QuartersInYear = 4;
		const int MonthsInQuarter = 3;
		const int DaysInWeek = 7;
		var cal = Thread.CurrentThread.CurrentCulture.Calendar;
		var yearDiff = cal.GetYear(endDate) - cal.GetYear(@this);
		var ts = new TimeSpan(endDate.Ticks - @this.Ticks);
		return datePart.ToLower(CultureInfo.CurrentCulture).Trim() switch
		{
			"year" or "yy" or "yyyy" => yearDiff,
			"quarter" or "qq" or "q" => (yearDiff * QuartersInYear) + ((cal.GetMonth(endDate) - 1) / MonthsInQuarter) - ((cal.GetMonth(@this) - 1) / MonthsInQuarter),
			"month" or "mm" or "m" => (yearDiff * MonthsInYear) + cal.GetMonth(endDate) - cal.GetMonth(@this),
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
	/// <param name="this">The date time.</param>
	/// <returns>The first day of the month.</returns>
	public static DateTime FirstDayOfMonth(this DateTime @this) => new(@this.Year, @this.Month, 1);

	/// <summary>
	/// Gets the date range between this date time and the specified date time.
	/// </summary>
	/// <param name="this">The from date.</param>
	/// <param name="toDate">The to date.</param>
	/// <returns>The date range <paramref name="this" /> to <paramref name="toDate" />.</returns>
	public static IEnumerable<DateTime> GetDateRangeTo(this DateTime @this, DateTime toDate)
	{
		Contract.Ensures(Contract.Result<IEnumerable<DateTime>>() != null);

		return Enumerable.Range(0, new TimeSpan(toDate.Ticks - @this.Ticks).Days)
						 .Select(p => @this.Date.AddDays(p));
	}

	/// <summary>
	/// Returns true if two date ranges intersect.
	/// </summary>
	/// <param name="this">The start date.</param>
	/// <param name="endDate">The end date.</param>
	/// <param name="intersectingStartDate">The intersecting start date.</param>
	/// <param name="intersectingEndDate">The intersecting end date.</param>
	/// <returns><c>true</c> if two date ranges intersect, <c>false</c> otherwise.</returns>
	public static bool Intersects(this DateTime @this, DateTime endDate, DateTime intersectingStartDate, DateTime intersectingEndDate) =>
		intersectingEndDate >= @this && intersectingStartDate <= endDate;

	/// <summary>
	/// Determines whether the specified date time is between the start and end dates.
	/// </summary>
	/// <param name="this">The date time to check.</param>
	/// <param name="startDate">The start date.</param>
	/// <param name="endDate">The end date.</param>
	/// <param name="compareTime">if set to <c>true</c> include the time in the comparison.</param>
	/// <returns>
	/// <c>true</c> if the specified date time is between the start and end dates; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsBetween(this DateTime @this, DateTime startDate, DateTime endDate, bool compareTime = false)
	{
		return compareTime
			? @this >= startDate && @this <= endDate
			: @this.Date >= startDate.Date && @this.Date <= endDate.Date;
	}

	/// <summary>
	/// Determines whether the specified value is weekend.
	/// </summary>
	/// <param name="this">The date value.</param>
	/// <returns><c>true</c> if the specified value is weekend; otherwise, <c>false</c>.</returns>
	public static bool IsWeekend(this DateTime @this) => @this.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday;

	/// <summary>
	/// Gets the last day of the month.
	/// </summary>
	/// <param name="this">The date time.</param>
	/// <returns>The last day of the month.</returns>
	public static DateTime LastDayOfMonth(this DateTime @this) => @this.FirstDayOfMonth().AddMonths(1).AddDays(-1);

	/// <summary>
	/// Converts the enumeration to the format string.
	/// </summary>
	/// <param name="this">The source date time.</param>
	/// <param name="dateTimeFormat">The date time format.</param>
	/// <returns>The date time format string.</returns>
	/// <exception cref="ArgumentNullException">dateTimeFormat</exception>
	public static string ToStringFormat(this DateTime @this, Expression<Func<DateTimeFormat>> dateTimeFormat)
	{
		_ = dateTimeFormat ?? throw new ArgumentNullException(nameof(dateTimeFormat));
		Contract.Ensures(Contract.Result<string>() != null);

		var dateTimeFormatCompiled = dateTimeFormat.Compile().Invoke();

		var dateTimeStringFormat = Enum<string>.GetStringValue(dateTimeFormatCompiled);

		var currentCulture = Thread.CurrentThread.CurrentCulture;

		return @this.ToString(dateTimeStringFormat, currentCulture);
	}

	/// <summary>
	/// Converts a System.DateTime object to Unix timestamp
	/// </summary>
	/// <param name="this">The date time.</param>
	/// <returns>The Unix timestamp.</returns>
	public static long ToUnixTimestamp(this DateTime @this)
	{
		const int UnixEpochYear = 1970;
		var unixEpoch = new DateTime(year: UnixEpochYear, month: 1, day: 1, hour: 0, minute: 0, second: 0);
		var unixTimeSpan = @this - unixEpoch;

		return (long)unixTimeSpan.TotalSeconds;
	}
}
