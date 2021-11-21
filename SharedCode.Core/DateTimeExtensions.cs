// <copyright file="DateTimeExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2013-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode;

using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading;

/// <summary>
/// DateTime Extensions
/// </summary>
public static class DateTimeExtensions
{
	/// <summary>
	/// Adds the given number of business days to the <see cref="DateTime" />.
	/// </summary>
	/// <param name="current">The date to be changed.</param>
	/// <param name="days">Number of business days to be added.</param>
	/// <returns>A <see cref="DateTime" /> increased by a given number of business days.</returns>
	public static DateTime AddBusinessDays(this DateTime current, int days)
	{
		var sign = Math.Sign(days);
		var unsignedDays = Math.Abs(days);
		for (var i = 0; i < unsignedDays; i++)
		{
			do
			{
				current = current.AddDays(sign);
			}
			while (current.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday);
		}

		return current;
	}

	/// <summary>
	/// Adds the specified number of weekdays to the date/time.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="days">The number of days.</param>
	/// <returns>The result.</returns>
	public static DateTime AddWeekdays(this DateTime date, int days)
	{
		var sign = days < 0 ? -1 : 1;
		var unsignedDays = Math.Abs(days);
		var weekdaysAdded = 0;
		while (weekdaysAdded < unsignedDays)
		{
			date = date.AddDays(sign);
			if (date.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
			{
				weekdaysAdded++;
			}
		}

		return date;
	}

	/// <summary>
	/// Returns the given <see cref="DateTime" /> with hour and minutes set At given values.
	/// </summary>
	/// <param name="current">The current <see cref="DateTime" /> to be changed.</param>
	/// <param name="hour">The hour to set time to.</param>
	/// <param name="minute">The minute to set time to.</param>
	/// <returns><see cref="DateTime" /> with hour and minute set to given values.</returns>
	/// ReSharper disable MethodNamesNotMeaningful
	public static DateTime At(this DateTime current, int hour, int minute)
	{
		// ReSharper restore MethodNamesNotMeaningful
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);

		return current.SetTime(hour, minute);
	}

	/// <summary>
	/// Returns the given <see cref="DateTime" /> with hour and minutes and seconds set At given values.
	/// </summary>
	/// <param name="current">The current <see cref="DateTime" /> to be changed.</param>
	/// <param name="hour">The hour to set time to.</param>
	/// <param name="minute">The minute to set time to.</param>
	/// <param name="second">The second to set time to.</param>
	/// <returns><see cref="DateTime" /> with hour and minutes and seconds set to given values.</returns>
	/// ReSharper disable MethodNamesNotMeaningful
	public static DateTime At(this DateTime current, int hour, int minute, int second)
	{
		// ReSharper restore MethodNamesNotMeaningful
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);
		Contract.Requires<ArgumentOutOfRangeException>(second <= 60);

		return current.SetTime(hour, minute, second);
	}

	/// <summary>
	/// Returns the Start of the given day (the first millisecond of the given <see cref="DateTime" />).
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>A date/time at the beginning of the day (midnight).</returns>
	public static DateTime BeginningOfDay(this DateTime date) =>
		new(date.Year, date.Month, date.Day, 0, 0, 0, 0);

	/// <summary>
	/// Returns true if the date and time fall between the start and end dates, inclusive or not of
	/// the end date and time.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <param name="startDateTime">The start date time.</param>
	/// <param name="endDateTime">The end date time.</param>
	/// <param name="endInclusive">if set to <c>true</c> [end inclusive].</param>
	/// <returns>
	/// <c>true</c> if the date and time fall between the start and end dates, inclusive or not of
	/// the end date and time, <c>false</c> otherwise.
	/// </returns>
	public static bool Between(this DateTime dateTime, DateTime startDateTime, DateTime endDateTime, bool endInclusive = false)
	{
		if (startDateTime == endDateTime)
		{
			return dateTime == startDateTime && endInclusive;
		}

		if (startDateTime < endDateTime)
		{
			if (endInclusive)
			{
				return dateTime >= startDateTime && dateTime <= endDateTime;
			}

			return dateTime >= startDateTime && dateTime < endDateTime;
		}

		return dateTime.Between(endDateTime, startDateTime, endInclusive);
	}

	/// <summary>
	/// Rounds the specified DateTime up to the next TimeSpan
	/// </summary>
	/// <param name="date">The DateTime to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTime.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Ceiling(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTime Ceiling(this DateTime date, TimeSpan span) =>
		new((date.Ticks + span.Ticks - 1) / span.Ticks * span.Ticks);

	/// <summary>
	/// Returns the number of days between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of days between the specified dates.</returns>
	public static int DaysBetween(this DateTime first, DateTime second) => (second.Date - first.Date).Duration().Days;

	/// <summary>
	/// Returns the number of days between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of days between the specified dates.</returns>
	public static int DaysBetween(this DateTime first, DateTime second, bool includeLastDay)
	{
		var days = first.DaysBetween(second);
		return includeLastDay ? days + 1 : days;
	}

	/// <summary>
	/// Decreases the <see cref="DateTime" /> object with given <see cref="TimeSpan" /> value.
	/// </summary>
	/// <param name="startDate">The start Date.</param>
	/// <param name="toSubtract">The to Subtract.</param>
	/// <returns>The result.</returns>
	public static DateTime DecreaseTime(this DateTime startDate, TimeSpan toSubtract) => startDate - toSubtract;

	/// <summary>
	/// Returns the very end of the given day (the last millisecond of the last hour for the given
	/// <see cref="DateTime" /> ).
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>A date/time with time at the end of the day. (23:59:59.999)</returns>
	public static DateTime EndOfDay(this DateTime date) =>
		new(date.Year, date.Month, date.Day, 23, 59, 59, 999);

	/// <summary>
	/// Sets the day of the <see cref="DateTime" /> to the first day in that month.
	/// </summary>
	/// <param name="current">The current <see cref="DateTime" /> to be changed.</param>
	/// <returns>
	/// given <see cref="DateTime" /> with the day part set to the first day in that month.
	/// </returns>
	public static DateTime FirstDayOfMonth(this DateTime current) => current.SetDay(1);

	/// <summary>
	/// Rounds the specified DateTime down to the next TimeSpan
	/// </summary>
	/// <param name="date">The DateTime to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTime.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Floor(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTime Floor(this DateTime date, TimeSpan span) =>
		new(date.Ticks / span.Ticks * span.Ticks);

	/// <summary>
	/// Increases the <see cref="DateTime" /> object with given <see cref="TimeSpan" /> value.
	/// </summary>
	/// <param name="startDate">The start Date.</param>
	/// <param name="toAdd">The to Add.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime IncreaseTime(this DateTime startDate, TimeSpan toAdd) => startDate + toAdd;

	/// <summary>
	/// Determines whether the specified <see cref="DateTime" /> value is After then current value.
	/// </summary>
	/// <param name="current">The current value.</param>
	/// <param name="toCompareWith">Value to compare with.</param>
	/// <returns><c>true</c> if the specified current is after; otherwise, <c>false</c>.</returns>
	public static bool IsAfter(this DateTime current, DateTime toCompareWith) => current > toCompareWith;

	/// <summary>
	/// Determines whether the specified <see cref="DateTime" /> is before then current value.
	/// </summary>
	/// <param name="current">The current value.</param>
	/// <param name="toCompareWith">Value to compare with.</param>
	/// <returns><c>true</c> if the specified current is before; otherwise, <c>false</c>.</returns>
	public static bool IsBefore(this DateTime current, DateTime toCompareWith) => current < toCompareWith;

	/// <summary>
	/// Determine if a <see cref="DateTime" /> is in the future.
	/// </summary>
	/// <param name="dateTime">The date to be checked.</param>
	/// <returns><c>true</c> if <paramref name="dateTime" /> is in the future; otherwise <c>false</c>.</returns>
	public static bool IsInFuture(this DateTime dateTime) => dateTime > DateTime.Now;

	/// <summary>
	/// Determine if a <see cref="DateTime" /> is in the past.
	/// </summary>
	/// <param name="dateTime">The date to be checked.</param>
	/// <returns><c>true</c> if <paramref name="dateTime" /> is in the past; otherwise <c>false</c>.</returns>
	public static bool IsInPast(this DateTime dateTime) => dateTime < DateTime.Now;

	/// <summary>
	/// Sets the day of the <see cref="DateTime" /> to the last day in that month.
	/// </summary>
	/// <param name="current">The current DateTime to be changed.</param>
	/// <returns>
	/// given <see cref="DateTime" /> with the day part set to the last day in that month.
	/// </returns>
	public static DateTime LastDayOfMonth(this DateTime current) =>
		current.SetDay(DateTime.DaysInMonth(current.Year, current.Month));

	/// <summary>
	/// Returns original <see cref="DateTime" /> value with time part set to midnight (alias for
	/// <see cref="BeginningOfDay" /> method).
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime Midnight(this DateTime value) => value.BeginningOfDay();

	/// <summary>
	/// Returns the number of months between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of months between the specified dates.</returns>
	public static int MonthsBetween(this DateTime first, DateTime second) => Math.Abs((second.DateValue() - first.DateValue()) / 31);

	/// <summary>
	/// Returns the number of months between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of months between the specified dates.</returns>
	public static int MonthsBetween(this DateTime first, DateTime second, bool includeLastDay)
	{
		if (!includeLastDay)
		{
			return first.MonthsBetween(second);
		}

		var days = (second >= first) ? second.AddDays(1).DateValue() - first.DateValue() : first.AddDays(1).DateValue() - second.DateValue();

		return days / 31;
	}

	/// <summary>
	/// Returns first next occurrence of specified <see cref="DayOfWeek" />.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime Next(this DateTime start, DayOfWeek day)
	{
		do
		{
			start = start.NextDay();
		}
		while (start.DayOfWeek != day);

		return start;
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> increased by 24 hours i.e. Next Day.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime NextDay(this DateTime start) => start + 1.Days();

	/// <summary>
	/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar year.
	/// If that day does not exist in next year in same month, number of missing days is added to
	/// the last day in same month next year.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime NextYear(this DateTime start)
	{
		var nextYear = start.Year + 1;
		var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, start.Month);

		if (numberOfDaysInSameMonthNextYear < start.Day)
		{
			var differenceInDays = start.Day - numberOfDaysInSameMonthNextYear;
			var dateTime = new DateTime(
				nextYear,
				start.Month,
				numberOfDaysInSameMonthNextYear,
				start.Hour,
				start.Minute,
				start.Second,
				start.Millisecond);
			return dateTime + differenceInDays.Days();
		}

		return new DateTime(
			nextYear,
			start.Month,
			start.Day,
			start.Hour,
			start.Minute,
			start.Second,
			start.Millisecond);
	}

	/// <summary>
	/// Returns original <see cref="DateTime" /> value with time part set to Noon (12:00:00h).
	/// </summary>
	/// <param name="value">The <see cref="DateTime" /> find Noon for.</param>
	/// <returns>A <see cref="DateTime" /> value with time part set to Noon (12:00:00h).</returns>
	public static DateTime Noon(this DateTime value) => value.SetTime(12, 0, 0, 0);

	/// <summary>
	/// Returns first next occurrence of specified <see cref="DayOfWeek" />.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime Previous(this DateTime start, DayOfWeek day)
	{
		do
		{
			start = start.PreviousDay();
		}
		while (start.DayOfWeek != day);

		return start;
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> decreased by 24h period i.e. Previous Day.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime PreviousDay(this DateTime start) => start - 1.Days();

	/// <summary>
	/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous calendar
	/// year. If that day does not exist in previous year in same month, number of missing days is
	/// added to the last day in same month previous year.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime PreviousYear(this DateTime start)
	{
		var previousYear = start.Year - 1;
		var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, start.Month);

		if (numberOfDaysInSameMonthPreviousYear < start.Day)
		{
			var differenceInDays = start.Day - numberOfDaysInSameMonthPreviousYear;
			var dateTime = new DateTime(
				previousYear,
				start.Month,
				numberOfDaysInSameMonthPreviousYear,
				start.Hour,
				start.Minute,
				start.Second,
				start.Millisecond);
			return dateTime + differenceInDays.Days();
		}

		return new DateTime(
			previousYear,
			start.Month,
			start.Day,
			start.Hour,
			start.Minute,
			start.Second,
			start.Millisecond);
	}

	/// <summary>
	/// Rounds the specified DateTime to the nearest TimeSpan
	/// </summary>
	/// <param name="date">The DateTime to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTime.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Floor(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTime Round(this DateTime date, TimeSpan span) =>
		new(((date.Ticks / span.Ticks) + (span.Ticks / 2) + 1) * span.Ticks);

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year part.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="year">The year.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDate(this DateTime value, int year)
	{
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

		return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year and Month part.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="year">The year.</param>
	/// <param name="month">The month.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDate(this DateTime value, int year, int month)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= 12);
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

		return new DateTime(year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year, Month and Day part.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="year">The year.</param>
	/// <param name="month">The month.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDate(this DateTime value, int year, int month, int day)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= 12);
		Contract.Requires<ArgumentOutOfRangeException>(day >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

		return new DateTime(year, month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Day part.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDay(this DateTime value, int day)
	{
		Contract.Requires<ArgumentOutOfRangeException>(day >= 1);

		return new DateTime(value.Year, value.Month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Hour part.
	/// </summary>
	/// <param name="originalDate">The original Date.</param>
	/// <param name="hour">The hour.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetHour(this DateTime originalDate, int hour)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);

		return new DateTime(
			originalDate.Year,
			originalDate.Month,
			originalDate.Day,
			hour,
			originalDate.Minute,
			originalDate.Second,
			originalDate.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Millisecond part.
	/// </summary>
	/// <param name="originalDate">The original Date.</param>
	/// <param name="millisecond">The millisecond.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetMillisecond(this DateTime originalDate, int millisecond)
	{
		Contract.Requires<ArgumentOutOfRangeException>(millisecond >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

		return new DateTime(
			originalDate.Year,
			originalDate.Month,
			originalDate.Day,
			originalDate.Hour,
			originalDate.Minute,
			originalDate.Second,
			millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Minute part.
	/// </summary>
	/// <param name="originalDate">The original Date.</param>
	/// <param name="minute">The minute.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetMinute(this DateTime originalDate, int minute)
	{
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);

		return new DateTime(
			originalDate.Year,
			originalDate.Month,
			originalDate.Day,
			originalDate.Hour,
			minute,
			originalDate.Second,
			originalDate.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Month part.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="month">The month.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetMonth(this DateTime value, int month)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= 12);

		return new DateTime(value.Year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Second part.
	/// </summary>
	/// <param name="originalDate">The original Date.</param>
	/// <param name="second">The second.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetSecond(this DateTime originalDate, int second)
	{
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second <= 60);

		return new DateTime(
			originalDate.Year,
			originalDate.Month,
			originalDate.Day,
			originalDate.Hour,
			originalDate.Minute,
			second,
			originalDate.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTime SetTime(this DateTime date, int hour)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);

		return date.SetTime(hour, date.Minute, date.Second, date.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <param name="minute">The number of minutes.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTime SetTime(this DateTime date, int hour, int minute)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);

		return date.SetTime(hour, minute, date.Second, date.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <param name="minute">The number of minutes.</param>
	/// <param name="second">The number of seconds.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTime SetTime(this DateTime date, int hour, int minute, int second)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);
		Contract.Requires<ArgumentOutOfRangeException>(second <= 60);

		return date.SetTime(hour, minute, second, date.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <param name="minute">The number of minutes.</param>
	/// <param name="second">The number of seconds.</param>
	/// <param name="millisecond">The number of milliseconds.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTime SetTime(this DateTime date, int hour, int minute, int second, int millisecond)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);
		Contract.Requires<ArgumentOutOfRangeException>(second <= 60);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

		return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year part.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="year">The year.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetYear(this DateTime value, int year)
	{
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

		return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
	}

	/// <summary>
	/// Returns a DateTime adjusted to the beginning of the week.
	/// </summary>
	/// <param name="dateTime">The DateTime to adjust</param>
	/// <returns>A DateTime instance adjusted to the beginning of the current week</returns>
	/// <remarks>the beginning of the week is controlled by the current Culture</remarks>
	public static DateTime StartOfWeek(this DateTime dateTime)
		=> dateTime.AddDays(-(DateTime.Today.DayOfWeek - Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));

	/// <summary>
	/// Subtracts the given number of business days to the <see cref="DateTime" />.
	/// </summary>
	/// <param name="current">The date to be changed.</param>
	/// <param name="days">Number of business days to be subtracted.</param>
	/// <returns>A <see cref="DateTime" /> increased by a given number of business days.</returns>
	public static DateTime SubtractBusinessDays(this DateTime current, int days) => AddBusinessDays(current, -days);

	/// <summary>
	/// Returns the relative date string.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <returns>The relative date string.</returns>
	public static string ToRelativeDateString(this DateTime date) => GetRelativeDateValue(date, DateTime.Now);

	/// <summary>
	/// Returns the relative date string for UTC time.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <returns>The relative date string for UTC time.</returns>
	public static string ToRelativeDateStringUtc(this DateTime date) => GetRelativeDateValue(date, DateTime.UtcNow);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTime? date) => date.ToString(default, DateTimeFormatInfo.CurrentInfo);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="format">The format string.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTime? date, string? format) =>
		date.ToString(format, DateTimeFormatInfo.CurrentInfo);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="provider">The format provider.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTime? date, IFormatProvider? provider) => date.ToString(null, provider);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="format">The format string.</param>
	/// <param name="provider">The format provider.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(
		this DateTime? date,
		string? format,
		IFormatProvider? provider) => date?.ToString(format, provider) ?? string.Empty;

	/// <summary>
	/// Increases supplied <see cref="DateTime" /> for 7 days i.e. returns the Next Week.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime WeekAfter(this DateTime start) => start + 1.Weeks();

	/// <summary>
	/// Decreases supplied <see cref="DateTime" /> for 7 days i.e. returns the Previous Week.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime WeekEarlier(this DateTime start) => start - 1.Weeks();

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of weeks between the specified dates.</returns>
	public static int WeeksBetween(this DateTime first, DateTime second) => first.DaysBetween(second) / 7;

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of weeks between the specified dates.</returns>
	public static int WeeksBetween(this DateTime first, DateTime second, bool includeLastDay) => first.DaysBetween(second, includeLastDay) / 7;

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <param name="excessDays">The remainder of excess days.</param>
	/// <returns>The number of weeks between the specified dates.</returns>
	public static int WeeksBetween(this DateTime first, DateTime second, bool includeLastDay, out int excessDays)
	{
		var days = first.DaysBetween(second, includeLastDay);
		excessDays = days % 7;
		return days / 7;
	}

	/// <summary>
	/// Returns the number of years between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of years between the specified dates.</returns>
	public static int YearsBetween(this DateTime first, DateTime second) => first.MonthsBetween(second) / 12;

	/// <summary>
	/// Returns the number of years between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of years between the specified dates.</returns>
	public static int YearsBetween(this DateTime first, DateTime second, bool includeLastDay) => first.MonthsBetween(second, includeLastDay) / 12;

	/// <summary>
	/// Returns the number of years between the specified dates.
	/// </summary>
	/// <param name="first">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <param name="excessMonths">The remainder of excess months.</param>
	/// <returns>The number of years between the specified dates.</returns>
	public static int YearsBetween(this DateTime first, DateTime second, bool includeLastDay, out int excessMonths)
	{
		var months = first.MonthsBetween(second, includeLastDay);
		excessMonths = months % 12;
		return months / 12;
	}

	/// <summary>
	/// Returns the date value as an Integer.
	/// </summary>
	/// <param name="input">The input date time.</param>
	/// <returns>The date value as an Integer.</returns>
	private static int DateValue(this DateTime input) => (input.Year * 372) + ((input.Month - 1) * 31) + input.Day - 1;

	/// <summary>
	/// Gets the relative date value.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="comparedTo">The date/time to compare to.</param>
	/// <returns>The relative date value.</returns>
	private static string GetRelativeDateValue(DateTime date, DateTime comparedTo)
	{
		// ReSharper disable StringLiteralTypo
		var diff = comparedTo.Subtract(date);
		if (diff.TotalDays >= 365)
		{
			return string.Concat("on ", date.ToString("MMMM d, yyyy", CultureInfo.CurrentCulture));
		}

		if (diff.TotalDays >= 7)
		{
			return string.Concat("on ", date.ToString("MMMM d", CultureInfo.CurrentCulture));
		}

		// ReSharper restore StringLiteralTypo
		if (diff.TotalDays > 1)
		{
			return $"{diff.TotalDays:N0} days ago";
		}

		if (Math.Abs(diff.TotalDays - 1D) < double.Epsilon)
		{
			return "yesterday";
		}

		if (diff.TotalHours >= 2)
		{
			return $"{diff.TotalHours:N0} hours ago";
		}

		if (diff.TotalMinutes >= 60)
		{
			return "more than an hour ago";
		}

		if (diff.TotalMinutes >= 5)
		{
			return $"{diff.TotalMinutes:N0} minutes ago";
		}

		if (diff.TotalMinutes >= 1)
		{
			return "a few minutes ago";
		}

		return "less than a minute ago";
	}
}
