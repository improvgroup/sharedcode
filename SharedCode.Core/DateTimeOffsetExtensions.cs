// <copyright file="DateTimeOffsetExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2013-2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading;

/// <summary>
/// DateTimeOffset Extensions
/// </summary>
public static class DateTimeOffsetExtensions
{
	private const int DaysInAWeek = 7;
	private const int DaysInAYear = 365;
	private const int FiftyNine = 59;
	private const int HoursInADay = 24;
	private const int MinutesInAnHour = 60;
	private const int NineHundredNinetyNine = 999;
	private const int NineThousandNineHundredNinetyNine = 9999;
	private const int ThirtyOne = 31;
	private const int ThreeHundredSeventyTwo = 372;
	private const int TwentyThree = 23;
	private const int Zero = 0;

	/// <summary>
	/// Adds the given number of business days to the <see cref="DateTimeOffset" />.
	/// </summary>
	/// <param name="this">The date to be changed.</param>
	/// <param name="days">Number of business days to be added.</param>
	/// <returns>A <see cref="DateTimeOffset" /> increased by a given number of business days.</returns>
	public static DateTimeOffset AddBusinessDays(this DateTimeOffset @this, int days)
	{
		var sign = Math.Sign(days);
		var unsignedDays = Math.Abs(days);
		for (var i = Zero; i < unsignedDays; i++)
		{
			do
			{
				@this = @this.AddDays(sign);
			}
			while (@this.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday);
		}

		return @this;
	}

	/// <summary>
	/// Adds the specified number of weekdays to the date/time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="days">The number of days.</param>
	/// <returns>The result.</returns>
	public static DateTimeOffset AddWeekdays(this DateTimeOffset @this, int days)
	{
		var sign = days < Zero ? -1 : 1;
		var unsignedDays = Math.Abs(days);
		var weekdaysAdded = Zero;
		while (weekdaysAdded < unsignedDays)
		{
			@this = @this.AddDays(sign);
			if (@this.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
			{
				weekdaysAdded++;
			}
		}

		return @this;
	}

	/// <summary>
	/// Returns the given <see cref="DateTimeOffset" /> with hour and minutes set At given values.
	/// </summary>
	/// <param name="this">The current <see cref="DateTimeOffset" /> to be changed.</param>
	/// <param name="hour">The hour to set time to.</param>
	/// <param name="minute">The minute to set time to.</param>
	/// <returns><see cref="DateTimeOffset" /> with hour and minute set to given values.</returns>
	/// ReSharper disable MethodNamesNotMeaningful
	public static DateTimeOffset At(this DateTimeOffset @this, int hour, int minute)
	{
		// ReSharper restore MethodNamesNotMeaningful
		Contract.Requires<ArgumentOutOfRangeException>(hour >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);

		return @this.SetTime(hour, minute);
	}

	/// <summary>
	/// Returns the given <see cref="DateTimeOffset" /> with hour and minutes and seconds set At
	/// given values.
	/// </summary>
	/// <param name="this">The current <see cref="DateTimeOffset" /> to be changed.</param>
	/// <param name="hour">The hour to set time to.</param>
	/// <param name="minute">The minute to set time to.</param>
	/// <param name="second">The second to set time to.</param>
	/// <returns>
	/// <see cref="DateTimeOffset" /> with hour and minutes and seconds set to given values.
	/// </returns>
	/// ReSharper disable MethodNamesNotMeaningful
	public static DateTimeOffset At(this DateTimeOffset @this, int hour, int minute, int second)
	{
		// ReSharper restore MethodNamesNotMeaningful
		Contract.Requires<ArgumentOutOfRangeException>(hour >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(second >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);
		Contract.Requires<ArgumentOutOfRangeException>(second <= MinutesInAnHour);

		return @this.SetTime(hour, minute, second);
	}

	/// <summary>
	/// Returns the Start of the given day (the first millisecond of the given <see
	/// cref="DateTimeOffset" />).
	/// </summary>
	/// <param name="this">The date.</param>
	/// <returns>A date/time at the beginning of the day (midnight).</returns>
	public static DateTimeOffset BeginningOfDay(this DateTimeOffset @this) =>
		new(@this.Year, @this.Month, @this.Day, Zero, Zero, Zero, Zero, @this.Offset);

	/// <summary>
	/// Returns true if the date and time fall between the start and end dates, inclusive or not of
	/// the end date and time.
	/// </summary>
	/// <param name="this">The date time.</param>
	/// <param name="startDateTimeOffset">The start date time.</param>
	/// <param name="endDateTimeOffset">The end date time.</param>
	/// <param name="endInclusive">if set to <c>true</c> [end inclusive].</param>
	/// <returns>
	/// <c>true</c> if the date and time fall between the start and end dates, inclusive or not of
	/// the end date and time, <c>false</c> otherwise.
	/// </returns>
	[SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Avoiding nested conditionals.")]
	public static bool Between(
		this DateTimeOffset @this,
		DateTimeOffset startDateTimeOffset,
		DateTimeOffset endDateTimeOffset,
		bool endInclusive = false)
	{
		if (startDateTimeOffset == endDateTimeOffset)
		{
			return @this == startDateTimeOffset && endInclusive;
		}

		if (startDateTimeOffset < endDateTimeOffset)
		{
			return endInclusive
				? @this >= startDateTimeOffset && @this <= endDateTimeOffset
				: @this >= startDateTimeOffset && @this < endDateTimeOffset;
		}

		return @this.Between(endDateTimeOffset, startDateTimeOffset, endInclusive);
	}

	/// <summary>
	/// Rounds the specified DateTimeOffset up to the next TimeSpan
	/// </summary>
	/// <param name="this">The DateTimeOffset to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTimeOffset.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Ceiling(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTimeOffset Ceiling(this DateTimeOffset @this, TimeSpan span) =>
		new(new DateTime((@this.Ticks + span.Ticks - 1) / span.Ticks * span.Ticks));

	/// <summary>
	/// Returns the number of days between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of days between the specified dates.</returns>
	public static int DaysBetween(this DateTimeOffset @this, DateTimeOffset second) => (second.Date - @this.Date).Duration().Days;

	/// <summary>
	/// Returns the number of days between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of days between the specified dates.</returns>
	public static int DaysBetween(this DateTimeOffset @this, DateTimeOffset second, bool includeLastDay)
	{
		var days = @this.DaysBetween(second);
		return includeLastDay ? days + 1 : days;
	}

	/// <summary>
	/// Decreases the <see cref="DateTimeOffset" /> object with given <see cref="TimeSpan" /> value.
	/// </summary>
	/// <param name="this">The start Date.</param>
	/// <param name="toSubtract">The to Subtract.</param>
	/// <returns>The result.</returns>
	public static DateTimeOffset DecreaseTime(this DateTimeOffset @this, TimeSpan toSubtract) => @this - toSubtract;

	/// <summary>
	/// Returns the very end of the given day (the last millisecond of the last hour for the given
	/// <see cref="DateTimeOffset" /> ).
	/// </summary>
	/// <param name="this">The date.</param>
	/// <returns>A date/time with time at the end of the day. (23:59:59.999)</returns>
	[SuppressMessage("Style", "IDE0022:Use expression body for method", Justification = "Rule is in conflict.")]
	public static DateTimeOffset EndOfDay(this DateTimeOffset @this)
	{
		return new(@this.Year, @this.Month, @this.Day, TwentyThree, FiftyNine, FiftyNine, NineHundredNinetyNine, @this.Offset);
	}

	/// <summary>
	/// Sets the day of the <see cref="DateTimeOffset" /> to the first day in that month.
	/// </summary>
	/// <param name="this">The current <see cref="DateTimeOffset" /> to be changed.</param>
	/// <returns>
	/// given <see cref="DateTimeOffset" /> with the day part set to the first day in that month.
	/// </returns>
	public static DateTimeOffset FirstDayOfMonth(this DateTimeOffset @this) => @this.SetDay(1);

	/// <summary>
	/// Rounds the specified DateTimeOffset down to the next TimeSpan
	/// </summary>
	/// <param name="this">The DateTimeOffset to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTimeOffset.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Floor(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTimeOffset Floor(this DateTimeOffset @this, TimeSpan span) =>
		new(new DateTime(@this.Ticks / span.Ticks * span.Ticks));

	/// <summary>
	/// Increases the <see cref="DateTimeOffset" /> object with given <see cref="TimeSpan" /> value.
	/// </summary>
	/// <param name="this">The start Date.</param>
	/// <param name="toAdd">The to Add.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset IncreaseTime(this DateTimeOffset @this, TimeSpan toAdd) => @this + toAdd;

	/// <summary>
	/// Determines whether the specified <see cref="DateTimeOffset" /> value is After then current value.
	/// </summary>
	/// <param name="this">The current value.</param>
	/// <param name="toCompareWith">Value to compare with.</param>
	/// <returns><c>true</c> if the specified current is after; otherwise, <c>false</c>.</returns>
	public static bool IsAfter(this DateTimeOffset @this, DateTimeOffset toCompareWith) => @this > toCompareWith;

	/// <summary>
	/// Determines whether the specified <see cref="DateTimeOffset" /> is before then current value.
	/// </summary>
	/// <param name="this">The current value.</param>
	/// <param name="toCompareWith">Value to compare with.</param>
	/// <returns><c>true</c> if the specified current is before; otherwise, <c>false</c>.</returns>
	public static bool IsBefore(this DateTimeOffset @this, DateTimeOffset toCompareWith) => @this < toCompareWith;

	/// <summary>
	/// Determine if a <see cref="DateTimeOffset" /> is in the future.
	/// </summary>
	/// <param name="this">The date to be checked.</param>
	/// <returns>
	/// <c>true</c> if <paramref name="this" /> is in the future; otherwise <c>false</c>.
	/// </returns>
	public static bool IsInFuture(this DateTimeOffset @this) => @this > DateTimeOffset.Now;

	/// <summary>
	/// Determine if a <see cref="DateTimeOffset" /> is in the past.
	/// </summary>
	/// <param name="this">The date to be checked.</param>
	/// <returns>
	/// <c>true</c> if <paramref name="this" /> is in the past; otherwise <c>false</c>.
	/// </returns>
	public static bool IsInPast(this DateTimeOffset @this) => @this < DateTimeOffset.Now;

	/// <summary>
	/// Sets the day of the <see cref="DateTimeOffset" /> to the last day in that month.
	/// </summary>
	/// <param name="this">The current DateTimeOffset to be changed.</param>
	/// <returns>
	/// given <see cref="DateTimeOffset" /> with the day part set to the last day in that month.
	/// </returns>
	public static DateTimeOffset LastDayOfMonth(this DateTimeOffset @this) =>
		@this.SetDay(DateTime.DaysInMonth(@this.Year, @this.Month));

	/// <summary>
	/// Returns original <see cref="DateTimeOffset" /> value with time part set to midnight (alias
	/// for <see cref="BeginningOfDay" /> method).
	/// </summary>
	/// <param name="this">The value.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset Midnight(this DateTimeOffset @this) => @this.BeginningOfDay();

	/// <summary>
	/// Returns the number of months between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of months between the specified dates.</returns>
	public static int MonthsBetween(this DateTimeOffset @this, DateTimeOffset second) => Math.Abs((second.DateValue() - @this.DateValue()) / ThirtyOne);

	/// <summary>
	/// Returns the number of months between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of months between the specified dates.</returns>
	public static int MonthsBetween(this DateTimeOffset @this, DateTimeOffset second, bool includeLastDay)
	{
		if (!includeLastDay)
		{
			return @this.MonthsBetween(second);
		}

		var days = (second >= @this) ? second.AddDays(1).DateValue() - @this.DateValue() : @this.AddDays(1).DateValue() - second.DateValue();

		return days / ThirtyOne;
	}

	/// <summary>
	/// Returns first next occurrence of specified <see cref="DayOfWeek" />.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <param name="day">The day of the week.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset Next(this DateTimeOffset @this, DayOfWeek day)
	{
		do
		{
			@this = @this.NextDay();
		}
		while (@this.DayOfWeek != day);

		return @this;
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> increased by 24 hours i.e. Next Day.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset NextDay(this DateTimeOffset @this) => @this + 1.Days();

	/// <summary>
	/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar year.
	/// If that day does not exist in next year in same month, number of missing days is added to
	/// the last day in same month next year.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset NextYear(this DateTimeOffset @this)
	{
		var nextYear = @this.Year + 1;
		var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, @this.Month);

		if (numberOfDaysInSameMonthNextYear < @this.Day)
		{
			var differenceInDays = @this.Day - numberOfDaysInSameMonthNextYear;
			var DateTimeOffset = new DateTimeOffset(
				nextYear,
				@this.Month,
				numberOfDaysInSameMonthNextYear,
				@this.Hour,
				@this.Minute,
				@this.Second,
				@this.Millisecond,
				@this.Offset);
			return DateTimeOffset + differenceInDays.Days();
		}

		return new DateTimeOffset(
			nextYear,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			@this.Second,
			@this.Millisecond,
			@this.Offset);
	}

	/// <summary>
	/// Returns original <see cref="DateTimeOffset" /> value with time part set to Noon (12:00:00h).
	/// </summary>
	/// <param name="this">The <see cref="DateTimeOffset" /> find Noon for.</param>
	/// <returns>A <see cref="DateTimeOffset" /> value with time part set to Noon (12:00:00h).</returns>
	public static DateTimeOffset Noon(this DateTimeOffset @this) => @this.SetTime(12, Zero, Zero, Zero);

	/// <summary>
	/// Returns first next occurrence of specified <see cref="DayOfWeek" />.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <param name="day">The day of the week.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset Previous(this DateTimeOffset @this, DayOfWeek day)
	{
		do
		{
			@this = @this.PreviousDay();
		}
		while (@this.DayOfWeek != day);

		return @this;
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> decreased by 24h period i.e. Previous Day.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset PreviousDay(this DateTimeOffset @this) => @this - 1.Days();

	/// <summary>
	/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous calendar
	/// year. If that day does not exist in previous year in same month, number of missing days is
	/// added to the last day in same month previous year.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset PreviousYear(this DateTimeOffset @this)
	{
		var previousYear = @this.Year - 1;
		var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, @this.Month);

		if (numberOfDaysInSameMonthPreviousYear < @this.Day)
		{
			var differenceInDays = @this.Day - numberOfDaysInSameMonthPreviousYear;
			var dateTimeOffset = new DateTimeOffset(
				previousYear,
				@this.Month,
				numberOfDaysInSameMonthPreviousYear,
				@this.Hour,
				@this.Minute,
				@this.Second,
				@this.Millisecond,
				@this.Offset);
			return dateTimeOffset + differenceInDays.Days();
		}

		return new DateTimeOffset(
			previousYear,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			@this.Second,
			@this.Millisecond,
			@this.Offset);
	}

	/// <summary>
	/// Rounds the specified DateTimeOffset to the nearest TimeSpan
	/// </summary>
	/// <param name="this">The DateTimeOffset to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTimeOffset.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Floor(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTimeOffset Round(this DateTimeOffset @this, TimeSpan span) =>
		new(new DateTime(((@this.Ticks / span.Ticks) + (span.Ticks / 2) + 1) * span.Ticks));

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Year part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year to set.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetDate(this DateTimeOffset @this, int year)
	{
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTimeOffset(
			year, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond, @this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Year and Month part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year.</param>
	/// <param name="month">The month.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetDate(this DateTimeOffset @this, int year, int month)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= 12);
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTimeOffset(
			year, month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond, @this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Year, Month and Day part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year.</param>
	/// <param name="month">The month.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetDate(this DateTimeOffset @this, int year, int month, int day)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= 12);
		Contract.Requires<ArgumentOutOfRangeException>(day >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTimeOffset(
			year, month, day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond, @this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Day part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetDay(this DateTimeOffset @this, int day)
	{
		Contract.Requires<ArgumentOutOfRangeException>(day >= 1);

		return new DateTimeOffset(
			@this.Year, @this.Month, day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond, @this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Hour part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="hour">The hour.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetHour(this DateTimeOffset @this, int hour)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);

		return new DateTimeOffset(
			@this.Year,
			@this.Month,
			@this.Day,
			hour,
			@this.Minute,
			@this.Second,
			@this.Millisecond,
			@this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Millisecond part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="millisecond">The millisecond.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetMillisecond(this DateTimeOffset @this, int millisecond)
	{
		Contract.Requires<ArgumentOutOfRangeException>(millisecond >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

		return new DateTimeOffset(
			@this.Year,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			@this.Second,
			millisecond,
			@this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Minute part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="minute">The minute.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetMinute(this DateTimeOffset @this, int minute)
	{
		Contract.Requires<ArgumentOutOfRangeException>(minute >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);

		return new DateTimeOffset(
			@this.Year,
			@this.Month,
			@this.Day,
			@this.Hour,
			minute,
			@this.Second,
			@this.Millisecond,
			@this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Month part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="month">The month.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetMonth(this DateTimeOffset @this, int month)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= 12);

		return new DateTimeOffset(
			@this.Year, month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond, @this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Second part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="second">The second.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetSecond(this DateTimeOffset @this, int second)
	{
		Contract.Requires<ArgumentOutOfRangeException>(second >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(second <= MinutesInAnHour);

		return new DateTimeOffset(
			@this.Year,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			second,
			@this.Millisecond,
			@this.Offset);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTimeOffset SetTime(this DateTimeOffset @this, int hour)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);

		return @this.SetTime(hour, @this.Minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <param name="minute">The number of minutes.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTimeOffset SetTime(this DateTimeOffset @this, int hour, int minute)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);

		return @this.SetTime(hour, minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <param name="minute">The number of minutes.</param>
	/// <param name="second">The number of seconds.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTimeOffset SetTime(this DateTimeOffset @this, int hour, int minute, int second)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(second >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);
		Contract.Requires<ArgumentOutOfRangeException>(second <= MinutesInAnHour);

		return @this.SetTime(hour, minute, second, @this.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <param name="minute">The number of minutes.</param>
	/// <param name="second">The number of seconds.</param>
	/// <param name="millisecond">The number of milliseconds.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTimeOffset SetTime(
		this DateTimeOffset @this, int hour, int minute, int second, int millisecond)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(second >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond >= Zero);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);
		Contract.Requires<ArgumentOutOfRangeException>(second <= MinutesInAnHour);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

		return new DateTimeOffset(@this.Year, @this.Month, @this.Day, hour, minute, second, millisecond, @this.Offset);
	}

	/// <summary>
	/// Returns <see cref="DateTimeOffset" /> with changed Year part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset SetYear(this DateTimeOffset @this, int year)
	{
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTimeOffset(
			year, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond, @this.Offset);
	}

	/// <summary>
	/// Returns a DateTimeOffset adjusted to the beginning of the week.
	/// </summary>
	/// <param name="this">The DateTimeOffset to adjust</param>
	/// <returns>A DateTimeOffset instance adjusted to the beginning of the current week</returns>
	/// <remarks>the beginning of the week is controlled by the current Culture</remarks>
	public static DateTimeOffset StartOfWeek(this DateTimeOffset @this)
	{
		return @this.AddDays(
			-(DateTime.Today.DayOfWeek - Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
	}

	/// <summary>
	/// Subtracts the given number of business days to the <see cref="DateTimeOffset" />.
	/// </summary>
	/// <param name="this">The date to be changed.</param>
	/// <param name="days">Number of business days to be subtracted.</param>
	/// <returns>A <see cref="DateTimeOffset" /> increased by a given number of business days.</returns>
	public static DateTimeOffset SubtractBusinessDays(this DateTimeOffset @this, int days) => AddBusinessDays(@this, -days);

	/// <summary>
	/// Returns the relative date string.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <returns>The relative date string.</returns>
	public static string ToRelativeDateString(this DateTimeOffset @this) => GetRelativeDateValue(@this, DateTimeOffset.Now);

	/// <summary>
	/// Returns the relative date string for UTC time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <returns>The relative date string for UTC time.</returns>
	public static string ToRelativeDateStringUtc(this DateTimeOffset @this) => GetRelativeDateValue(@this, DateTimeOffset.UtcNow);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTimeOffset? @this) => @this.ToString(default, DateTimeFormatInfo.CurrentInfo);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="format">The format string.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTimeOffset? @this, string? format) => @this.ToString(format, DateTimeFormatInfo.CurrentInfo);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="provider">The format provider.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTimeOffset? @this, IFormatProvider? provider) => @this.ToString(null, provider);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="format">The format string.</param>
	/// <param name="provider">The format provider.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTimeOffset? @this, string? format, IFormatProvider? provider) =>
		@this?.ToString(format, provider) ?? string.Empty;

	/// <summary>
	/// Increases supplied <see cref="DateTimeOffset" /> for 7 days i.e. returns the Next Week.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset WeekAfter(this DateTimeOffset @this) => @this + 1.Weeks();

	/// <summary>
	/// Decreases supplied <see cref="DateTimeOffset" /> for 7 days i.e. returns the Previous Week.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTimeOffset" />.</returns>
	public static DateTimeOffset WeekEarlier(this DateTimeOffset @this) => @this - 1.Weeks();

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of weeks between the specified dates.</returns>
	public static int WeeksBetween(this DateTimeOffset @this, DateTimeOffset second) => @this.DaysBetween(second) / DaysInAWeek;

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of weeks between the specified dates.</returns>
	public static int WeeksBetween(this DateTimeOffset @this, DateTimeOffset second, bool includeLastDay) => @this.DaysBetween(second, includeLastDay) / DaysInAWeek;

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <param name="excessDays">The remainder of excess days.</param>
	/// <returns>The number of weeks between the specified dates.</returns>
	[SuppressMessage("Style", "GCop408:Flag or switch parameters (bool) should go after all non-optional parameters. If the boolean parameter is not a flag or switch, split the method into two different methods, each doing one thing.", Justification = "<Pending>")]
	[SuppressMessage("Design", "GCop119:Don’t use {0} parameters in method definition. To return several objects, define a class or struct for your method return type.", Justification = "<Pending>")]
	public static int WeeksBetween(this DateTimeOffset @this, DateTimeOffset second, bool includeLastDay, out int excessDays)
	{
		var days = @this.DaysBetween(second, includeLastDay);
		excessDays = days % DaysInAWeek;
		return days / DaysInAWeek;
	}

	/// <summary>
	/// Returns the number of years between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of years between the specified dates.</returns>
	public static int YearsBetween(this DateTimeOffset @this, DateTimeOffset second) => @this.MonthsBetween(second) / 12;

	/// <summary>
	/// Returns the number of years between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of years between the specified dates.</returns>
	public static int YearsBetween(this DateTimeOffset @this, DateTimeOffset second, bool includeLastDay) => @this.MonthsBetween(second, includeLastDay) / 12;

	/// <summary>
	/// Returns the number of years between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <param name="excessMonths">The remainder of excess months.</param>
	/// <returns>The number of years between the specified dates.</returns>
	[SuppressMessage("Style", "GCop408:Flag or switch parameters (bool) should go after all non-optional parameters. If the boolean parameter is not a flag or switch, split the method into two different methods, each doing one thing.", Justification = "<Pending>")]
	[SuppressMessage("Design", "GCop119:Don’t use {0} parameters in method definition. To return several objects, define a class or struct for your method return type.", Justification = "<Pending>")]
	public static int YearsBetween(this DateTimeOffset @this, DateTimeOffset second, bool includeLastDay, out int excessMonths)
	{
		var months = @this.MonthsBetween(second, includeLastDay);
		excessMonths = months % 12;
		return months / 12;
	}

	/// <summary>
	/// Returns the date value as an Integer.
	/// </summary>
	/// <param name="input">The input date time.</param>
	/// <returns>The date value as an Integer.</returns>
	private static int DateValue(this DateTimeOffset input) => (input.Year * ThreeHundredSeventyTwo) + ((input.Month - 1) * ThirtyOne) + input.Day - 1;

	/// <summary>
	/// Gets the relative date value.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="comparedTo">The date/time to compare to.</param>
	/// <returns>The relative date value.</returns>
	private static string GetRelativeDateValue(DateTimeOffset date, DateTimeOffset comparedTo)
	{
		// ReSharper disable StringLiteralTypo
		var diff = comparedTo.Subtract(date);
		switch (diff.TotalDays)
		{
			case >= DaysInAYear:
				return string.Concat("on ", date.ToString("MMMM d, yyyy", CultureInfo.CurrentCulture));
			case >= DaysInAWeek:
				return string.Concat("on ", date.ToString("MMMM d", CultureInfo.CurrentCulture));
			case > 1:
				return $"{diff.TotalDays:N0} days ago";
			default:
				if (Math.Abs(diff.TotalDays - 1D) < double.Epsilon)
				{
					return "yesterday";
				}

				if (diff.TotalHours >= 2)
				{
					return $"{diff.TotalHours:N0} hours ago";
				}

				return diff.TotalMinutes switch
				{
					>= MinutesInAnHour => "more than an hour ago",
					>= 5 => $"{diff.TotalMinutes:N0} minutes ago",
					>= 1 => "a few minutes ago",
					_ => "less than a minute ago"
				};
		}
	}
}
