// <copyright file="DateTimeOffsetExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2013-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.Threading;

	/// <summary>
	/// DateTimeOffset Extensions
	/// </summary>
	public static class DateTimeOffsetExtensions
	{
		/// <summary>
		/// Adds the given number of business days to the <see cref="DateTimeOffset" />.
		/// </summary>
		/// <param name="current">The date to be changed.</param>
		/// <param name="days">Number of business days to be added.</param>
		/// <returns>
		/// A <see cref="DateTimeOffset" /> increased by a given number of business days.
		/// </returns>
		public static DateTimeOffset AddBusinessDays(this DateTimeOffset current, int days)
		{
			var sign = Math.Sign(days);
			var unsignedDays = Math.Abs(days);
			for (var i = 0; i < unsignedDays; i++)
			{
				do
				{
					current = current.AddDays(sign);
				}
				while (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday);
			}

			return current;
		}

		/// <summary>
		/// Adds the specified number of weekdays to the date/time.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <param name="days">The number of days.</param>
		/// <returns>The result.</returns>
		public static DateTimeOffset AddWeekdays(this DateTimeOffset date, int days)
		{
			var sign = days < 0 ? -1 : 1;
			var unsignedDays = Math.Abs(days);
			var weekdaysAdded = 0;
			while (weekdaysAdded < unsignedDays)
			{
				date = date.AddDays(sign);
				if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
				{
					weekdaysAdded++;
				}
			}

			return date;
		}

		/// <summary>
		/// Returns the given <see cref="DateTimeOffset" /> with hour and minutes set At given values.
		/// </summary>
		/// <param name="current">The current <see cref="DateTimeOffset" /> to be changed.</param>
		/// <param name="hour">The hour to set time to.</param>
		/// <param name="minute">The minute to set time to.</param>
		/// <returns><see cref="DateTimeOffset" /> with hour and minute set to given values.</returns>
		/// ReSharper disable MethodNamesNotMeaningful
		public static DateTimeOffset At(this DateTimeOffset current, int hour, int minute)
		{
			// ReSharper restore MethodNamesNotMeaningful
			Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);
			Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);

			return current.SetTime(hour, minute);
		}

		/// <summary>
		/// Returns the given <see cref="DateTimeOffset" /> with hour and minutes and seconds set At
		/// given values.
		/// </summary>
		/// <param name="current">The current <see cref="DateTimeOffset" /> to be changed.</param>
		/// <param name="hour">The hour to set time to.</param>
		/// <param name="minute">The minute to set time to.</param>
		/// <param name="second">The second to set time to.</param>
		/// <returns>
		/// <see cref="DateTimeOffset" /> with hour and minutes and seconds set to given values.
		/// </returns>
		/// ReSharper disable MethodNamesNotMeaningful
		public static DateTimeOffset At(this DateTimeOffset current, int hour, int minute, int second)
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
		/// Returns the Start of the given day (the first millisecond of the given <see
		/// cref="DateTimeOffset" />).
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>A date/time at the beginning of the day (midnight).</returns>
		public static DateTimeOffset BeginningOfDay(this DateTimeOffset date) =>
			new(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Offset);

		/// <summary>
		/// Returns true if the date and time fall between the start and end dates, inclusive or not
		/// of the end date and time.
		/// </summary>
		/// <param name="DateTimeOffset">The date time.</param>
		/// <param name="startDateTimeOffset">The start date time.</param>
		/// <param name="endDateTimeOffset">The end date time.</param>
		/// <param name="endInclusive">if set to <c>true</c> [end inclusive].</param>
		/// <returns>
		/// <c>true</c> if the date and time fall between the start and end dates, inclusive or not
		/// of the end date and time, <c>false</c> otherwise.
		/// </returns>
		public static bool Between(
			this DateTimeOffset DateTimeOffset,
			DateTimeOffset startDateTimeOffset,
			DateTimeOffset endDateTimeOffset,
			bool endInclusive = false)
		{
			if (startDateTimeOffset == endDateTimeOffset)
			{
				return DateTimeOffset == startDateTimeOffset && endInclusive;
			}

			if (startDateTimeOffset < endDateTimeOffset)
			{
				if (endInclusive)
				{
					return DateTimeOffset >= startDateTimeOffset && DateTimeOffset <= endDateTimeOffset;
				}

				return DateTimeOffset >= startDateTimeOffset && DateTimeOffset < endDateTimeOffset;
			}

			return DateTimeOffset.Between(endDateTimeOffset, startDateTimeOffset, endInclusive);
		}

		/// <summary>
		/// Rounds the specified DateTimeOffset up to the next TimeSpan
		/// </summary>
		/// <param name="date">The DateTimeOffset to round.</param>
		/// <param name="span">The TimeSpan to round by.</param>
		/// <returns>The rounded DateTimeOffset.</returns>
		/// <example>
		/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
		/// <code>
		///MyDate.Ceiling(new TimeSpan(0,1,0));
		/// </code>
		/// </example>
		public static DateTimeOffset Ceiling(this DateTimeOffset date, TimeSpan span) =>
			new(new DateTime((date.Ticks + span.Ticks - 1) / span.Ticks * span.Ticks));

		/// <summary>
		/// Returns the number of days between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <returns>The number of days between the specified dates.</returns>
		public static int DaysBetween(this DateTimeOffset first, DateTimeOffset second) => (second.Date - first.Date).Duration().Days;

		/// <summary>
		/// Returns the number of days between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <param name="includeLastDay">
		/// A value indicating whether to include the last day in the calculation.
		/// </param>
		/// <returns>The number of days between the specified dates.</returns>
		public static int DaysBetween(this DateTimeOffset first, DateTimeOffset second, bool includeLastDay)
		{
			int days = first.DaysBetween(second);
			return includeLastDay ? days + 1 : days;
		}

		/// <summary>
		/// Decreases the <see cref="DateTimeOffset" /> object with given <see cref="TimeSpan" /> value.
		/// </summary>
		/// <param name="startDate">The start Date.</param>
		/// <param name="toSubtract">The to Subtract.</param>
		/// <returns>The result.</returns>
		public static DateTimeOffset DecreaseTime(this DateTimeOffset startDate, TimeSpan toSubtract) => startDate - toSubtract;

		/// <summary>
		/// Returns the very end of the given day (the last millisecond of the last hour for the
		/// given <see cref="DateTimeOffset" /> ).
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>A date/time with time at the end of the day. (23:59:59.999)</returns>
		public static DateTimeOffset EndOfDay(this DateTimeOffset date) =>
			new(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Offset);

		/// <summary>
		/// Sets the day of the <see cref="DateTimeOffset" /> to the first day in that month.
		/// </summary>
		/// <param name="current">The current <see cref="DateTimeOffset" /> to be changed.</param>
		/// <returns>
		/// given <see cref="DateTimeOffset" /> with the day part set to the first day in that month.
		/// </returns>
		public static DateTimeOffset FirstDayOfMonth(this DateTimeOffset current) => current.SetDay(1);

		/// <summary>
		/// Rounds the specified DateTimeOffset down to the next TimeSpan
		/// </summary>
		/// <param name="date">The DateTimeOffset to round.</param>
		/// <param name="span">The TimeSpan to round by.</param>
		/// <returns>The rounded DateTimeOffset.</returns>
		/// <example>
		/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
		/// <code>
		///MyDate.Floor(new TimeSpan(0,1,0));
		/// </code>
		/// </example>
		public static DateTimeOffset Floor(this DateTimeOffset date, TimeSpan span) =>
			new(new DateTime(date.Ticks / span.Ticks * span.Ticks));

		/// <summary>
		/// Increases the <see cref="DateTimeOffset" /> object with given <see cref="TimeSpan" /> value.
		/// </summary>
		/// <param name="startDate">The start Date.</param>
		/// <param name="toAdd">The to Add.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset IncreaseTime(this DateTimeOffset startDate, TimeSpan toAdd) => startDate + toAdd;

		/// <summary>
		/// Determines whether the specified <see cref="DateTimeOffset" /> value is After then
		/// current value.
		/// </summary>
		/// <param name="current">The current value.</param>
		/// <param name="toCompareWith">Value to compare with.</param>
		/// <returns><c>true</c> if the specified current is after; otherwise, <c>false</c>.</returns>
		public static bool IsAfter(this DateTimeOffset current, DateTimeOffset toCompareWith) => current > toCompareWith;

		/// <summary>
		/// Determines whether the specified <see cref="DateTimeOffset" /> is before then current value.
		/// </summary>
		/// <param name="current">The current value.</param>
		/// <param name="toCompareWith">Value to compare with.</param>
		/// <returns><c>true</c> if the specified current is before; otherwise, <c>false</c>.</returns>
		public static bool IsBefore(this DateTimeOffset current, DateTimeOffset toCompareWith) => current < toCompareWith;

		/// <summary>
		/// Determine if a <see cref="DateTimeOffset" /> is in the future.
		/// </summary>
		/// <param name="dateTimeOffset">The date to be checked.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="dateTimeOffset" /> is in the future; otherwise <c>false</c>.
		/// </returns>
		public static bool IsInFuture(this DateTimeOffset dateTimeOffset) => dateTimeOffset > DateTimeOffset.Now;

		/// <summary>
		/// Determine if a <see cref="DateTimeOffset" /> is in the past.
		/// </summary>
		/// <param name="dateTimeOffset">The date to be checked.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="dateTimeOffset" /> is in the past; otherwise <c>false</c>.
		/// </returns>
		public static bool IsInPast(this DateTimeOffset dateTimeOffset) => dateTimeOffset < DateTimeOffset.Now;

		/// <summary>
		/// Sets the day of the <see cref="DateTimeOffset" /> to the last day in that month.
		/// </summary>
		/// <param name="current">The current DateTimeOffset to be changed.</param>
		/// <returns>
		/// given <see cref="DateTimeOffset" /> with the day part set to the last day in that month.
		/// </returns>
		public static DateTimeOffset LastDayOfMonth(this DateTimeOffset current) =>
			current.SetDay(DateTime.DaysInMonth(current.Year, current.Month));

		/// <summary>
		/// Returns original <see cref="DateTimeOffset" /> value with time part set to midnight
		/// (alias for <see cref="BeginningOfDay" /> method).
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset Midnight(this DateTimeOffset value) => value.BeginningOfDay();

		/// <summary>
		/// Returns the number of months between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <returns>The number of months between the specified dates.</returns>
		public static int MonthsBetween(this DateTimeOffset first, DateTimeOffset second) => Math.Abs((second.DateValue() - first.DateValue()) / 31);

		/// <summary>
		/// Returns the number of months between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <param name="includeLastDay">
		/// A value indicating whether to include the last day in the calculation.
		/// </param>
		/// <returns>The number of months between the specified dates.</returns>
		public static int MonthsBetween(this DateTimeOffset first, DateTimeOffset second, bool includeLastDay)
		{
			if (!includeLastDay)
			{
				return first.MonthsBetween(second);
			}

			int days = (second >= first) ? second.AddDays(1).DateValue() - first.DateValue() : first.AddDays(1).DateValue() - second.DateValue();

			return days / 31;
		}

		/// <summary>
		/// Returns first next occurrence of specified <see cref="DayOfWeek" />.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="day">The day.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset Next(this DateTimeOffset start, DayOfWeek day)
		{
			do
			{
				start = start.NextDay();
			}
			while (start.DayOfWeek != day);

			return start;
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> increased by 24 hours i.e. Next Day.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset NextDay(this DateTimeOffset start) => start + 1.Days();

		/// <summary>
		/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar
		/// year. If that day does not exist in next year in same month, number of missing days is
		/// added to the last day in same month next year.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset NextYear(this DateTimeOffset start)
		{
			var nextYear = start.Year + 1;
			var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, start.Month);

			if (numberOfDaysInSameMonthNextYear < start.Day)
			{
				var differenceInDays = start.Day - numberOfDaysInSameMonthNextYear;
				var DateTimeOffset = new DateTimeOffset(
					nextYear,
					start.Month,
					numberOfDaysInSameMonthNextYear,
					start.Hour,
					start.Minute,
					start.Second,
					start.Millisecond,
					start.Offset);
				return DateTimeOffset + differenceInDays.Days();
			}

			return new DateTimeOffset(
				nextYear,
				start.Month,
				start.Day,
				start.Hour,
				start.Minute,
				start.Second,
				start.Millisecond,
				start.Offset);
		}

		/// <summary>
		/// Returns original <see cref="DateTimeOffset" /> value with time part set to Noon (12:00:00h).
		/// </summary>
		/// <param name="value">The <see cref="DateTimeOffset" /> find Noon for.</param>
		/// <returns>A <see cref="DateTimeOffset" /> value with time part set to Noon (12:00:00h).</returns>
		public static DateTimeOffset Noon(this DateTimeOffset value) => value.SetTime(12, 0, 0, 0);

		/// <summary>
		/// Returns first next occurrence of specified <see cref="DayOfWeek" />.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="day">The day.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset Previous(this DateTimeOffset start, DayOfWeek day)
		{
			do
			{
				start = start.PreviousDay();
			}
			while (start.DayOfWeek != day);

			return start;
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> decreased by 24h period i.e. Previous Day.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset PreviousDay(this DateTimeOffset start) => start - 1.Days();

		/// <summary>
		/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous
		/// calendar year. If that day does not exist in previous year in same month, number of
		/// missing days is added to the last day in same month previous year.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset PreviousYear(this DateTimeOffset start)
		{
			var previousYear = start.Year - 1;
			var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, start.Month);

			if (numberOfDaysInSameMonthPreviousYear < start.Day)
			{
				var differenceInDays = start.Day - numberOfDaysInSameMonthPreviousYear;
				var DateTimeOffset = new DateTimeOffset(
					previousYear,
					start.Month,
					numberOfDaysInSameMonthPreviousYear,
					start.Hour,
					start.Minute,
					start.Second,
					start.Millisecond,
					start.Offset);
				return DateTimeOffset + differenceInDays.Days();
			}

			return new DateTimeOffset(
				previousYear,
				start.Month,
				start.Day,
				start.Hour,
				start.Minute,
				start.Second,
				start.Millisecond,
				start.Offset);
		}

		/// <summary>
		/// Rounds the specified DateTimeOffset to the nearest TimeSpan
		/// </summary>
		/// <param name="date">The DateTimeOffset to round.</param>
		/// <param name="span">The TimeSpan to round by.</param>
		/// <returns>The rounded DateTimeOffset.</returns>
		/// <example>
		/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
		/// <code>
		///MyDate.Floor(new TimeSpan(0,1,0));
		/// </code>
		/// </example>
		public static DateTimeOffset Round(this DateTimeOffset date, TimeSpan span) =>
			new(new DateTime(((date.Ticks / span.Ticks) + (span.Ticks / 2) + 1) * span.Ticks));

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Year part.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="year">The year.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetDate(this DateTimeOffset value, int year)
		{
			Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

			return new DateTimeOffset(
				year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Year and Month part.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="year">The year.</param>
		/// <param name="month">The month.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetDate(this DateTimeOffset value, int year, int month)
		{
			Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(month <= 12);
			Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

			return new DateTimeOffset(
				year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Year, Month and Day part.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="year">The year.</param>
		/// <param name="month">The month.</param>
		/// <param name="day">The day.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetDate(this DateTimeOffset value, int year, int month, int day)
		{
			Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(month <= 12);
			Contract.Requires<ArgumentOutOfRangeException>(day >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

			return new DateTimeOffset(
				year, month, day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Day part.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="day">The day.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetDay(this DateTimeOffset value, int day)
		{
			Contract.Requires<ArgumentOutOfRangeException>(day >= 1);

			return new DateTimeOffset(
				value.Year, value.Month, day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Hour part.
		/// </summary>
		/// <param name="originalDate">The original Date.</param>
		/// <param name="hour">The hour.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetHour(this DateTimeOffset originalDate, int hour)
		{
			Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);

			return new DateTimeOffset(
				originalDate.Year,
				originalDate.Month,
				originalDate.Day,
				hour,
				originalDate.Minute,
				originalDate.Second,
				originalDate.Millisecond,
				originalDate.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Millisecond part.
		/// </summary>
		/// <param name="originalDate">The original Date.</param>
		/// <param name="millisecond">The millisecond.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetMillisecond(this DateTimeOffset originalDate, int millisecond)
		{
			Contract.Requires<ArgumentOutOfRangeException>(millisecond >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

			return new DateTimeOffset(
				originalDate.Year,
				originalDate.Month,
				originalDate.Day,
				originalDate.Hour,
				originalDate.Minute,
				originalDate.Second,
				millisecond,
				originalDate.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Minute part.
		/// </summary>
		/// <param name="originalDate">The original Date.</param>
		/// <param name="minute">The minute.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetMinute(this DateTimeOffset originalDate, int minute)
		{
			Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);

			return new DateTimeOffset(
				originalDate.Year,
				originalDate.Month,
				originalDate.Day,
				originalDate.Hour,
				minute,
				originalDate.Second,
				originalDate.Millisecond,
				originalDate.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Month part.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="month">The month.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetMonth(this DateTimeOffset value, int month)
		{
			Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(month <= 12);

			return new DateTimeOffset(
				value.Year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Second part.
		/// </summary>
		/// <param name="originalDate">The original Date.</param>
		/// <param name="second">The second.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetSecond(this DateTimeOffset originalDate, int second)
		{
			Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(second <= 60);

			return new DateTimeOffset(
				originalDate.Year,
				originalDate.Month,
				originalDate.Day,
				originalDate.Hour,
				originalDate.Minute,
				second,
				originalDate.Millisecond,
				originalDate.Offset);
		}

		/// <summary>
		/// Sets the time.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <param name="hour">The number of hours.</param>
		/// <returns>The date/time with the time set as specified.</returns>
		public static DateTimeOffset SetTime(this DateTimeOffset date, int hour)
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
		public static DateTimeOffset SetTime(this DateTimeOffset date, int hour, int minute)
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
		public static DateTimeOffset SetTime(this DateTimeOffset date, int hour, int minute, int second)
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
		public static DateTimeOffset SetTime(
			this DateTimeOffset date, int hour, int minute, int second, int millisecond)
		{
			Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(millisecond >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(hour <= 24);
			Contract.Requires<ArgumentOutOfRangeException>(minute <= 60);
			Contract.Requires<ArgumentOutOfRangeException>(second <= 60);
			Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

			return new DateTimeOffset(date.Year, date.Month, date.Day, hour, minute, second, millisecond, date.Offset);
		}

		/// <summary>
		/// Returns <see cref="DateTimeOffset" /> with changed Year part.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="year">The year.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset SetYear(this DateTimeOffset value, int year)
		{
			Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
			Contract.Requires<ArgumentOutOfRangeException>(year <= 9999);

			return new DateTimeOffset(
				year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Offset);
		}

		/// <summary>
		/// Returns a DateTimeOffset adjusted to the beginning of the week.
		/// </summary>
		/// <param name="DateTimeOffset">The DateTimeOffset to adjust</param>
		/// <returns>A DateTimeOffset instance adjusted to the beginning of the current week</returns>
		/// <remarks>the beginning of the week is controlled by the current Culture</remarks>
		public static DateTimeOffset StartOfWeek(this DateTimeOffset DateTimeOffset)
			=> DateTimeOffset.AddDays(
				-(DateTime.Today.DayOfWeek - Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));

		/// <summary>
		/// Subtracts the given number of business days to the <see cref="DateTimeOffset" />.
		/// </summary>
		/// <param name="current">The date to be changed.</param>
		/// <param name="days">Number of business days to be subtracted.</param>
		/// <returns>
		/// A <see cref="DateTimeOffset" /> increased by a given number of business days.
		/// </returns>
		public static DateTimeOffset SubtractBusinessDays(this DateTimeOffset current, int days) => AddBusinessDays(current, -days);

		/// <summary>
		/// Returns the relative date string.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <returns>The relative date string.</returns>
		public static string ToRelativeDateString(this DateTimeOffset date) => GetRelativeDateValue(date, DateTimeOffset.Now);

		/// <summary>
		/// Returns the relative date string for UTC time.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <returns>The relative date string for UTC time.</returns>
		public static string ToRelativeDateStringUtc(this DateTimeOffset date) => GetRelativeDateValue(date, DateTimeOffset.UtcNow);

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <returns>A <see cref="string" /> that represents this instance.</returns>
		public static string ToString(this DateTimeOffset? date) => date.ToString(default, DateTimeFormatInfo.CurrentInfo);

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <param name="format">The format string.</param>
		/// <returns>A <see cref="string" /> that represents this instance.</returns>
		public static string ToString(this DateTimeOffset? date, string? format) => date.ToString(format, DateTimeFormatInfo.CurrentInfo);

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <param name="provider">The format provider.</param>
		/// <returns>A <see cref="string" /> that represents this instance.</returns>
		public static string ToString(this DateTimeOffset? date, IFormatProvider? provider) => date.ToString(null, provider);

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <param name="date">The date/time.</param>
		/// <param name="format">The format string.</param>
		/// <param name="provider">The format provider.</param>
		/// <returns>A <see cref="string" /> that represents this instance.</returns>
		public static string ToString(this DateTimeOffset? date, string? format, IFormatProvider? provider) =>
			date?.ToString(format, provider) ?? string.Empty;

		/// <summary>
		/// Increases supplied <see cref="DateTimeOffset" /> for 7 days i.e. returns the Next Week.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset WeekAfter(this DateTimeOffset start) => start + 1.Weeks();

		/// <summary>
		/// Decreases supplied <see cref="DateTimeOffset" /> for 7 days i.e. returns the Previous Week.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <returns>The <see cref="DateTimeOffset" />.</returns>
		public static DateTimeOffset WeekEarlier(this DateTimeOffset start) => start - 1.Weeks();

		/// <summary>
		/// Returns the number of weeks between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <returns>The number of weeks between the specified dates.</returns>
		public static int WeeksBetween(this DateTimeOffset first, DateTimeOffset second) => first.DaysBetween(second) / 7;

		/// <summary>
		/// Returns the number of weeks between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <param name="includeLastDay">
		/// A value indicating whether to include the last day in the calculation.
		/// </param>
		/// <returns>The number of weeks between the specified dates.</returns>
		public static int WeeksBetween(this DateTimeOffset first, DateTimeOffset second, bool includeLastDay) => first.DaysBetween(second, includeLastDay) / 7;

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
		public static int WeeksBetween(this DateTimeOffset first, DateTimeOffset second, bool includeLastDay, out int excessDays)
		{
			int days = first.DaysBetween(second, includeLastDay);
			excessDays = days % 7;
			return days / 7;
		}

		/// <summary>
		/// Returns the number of years between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <returns>The number of years between the specified dates.</returns>
		public static int YearsBetween(this DateTimeOffset first, DateTimeOffset second) => first.MonthsBetween(second) / 12;

		/// <summary>
		/// Returns the number of years between the specified dates.
		/// </summary>
		/// <param name="first">The first date.</param>
		/// <param name="second">The second date.</param>
		/// <param name="includeLastDay">
		/// A value indicating whether to include the last day in the calculation.
		/// </param>
		/// <returns>The number of years between the specified dates.</returns>
		public static int YearsBetween(this DateTimeOffset first, DateTimeOffset second, bool includeLastDay) => first.MonthsBetween(second, includeLastDay) / 12;

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
		public static int YearsBetween(this DateTimeOffset first, DateTimeOffset second, bool includeLastDay, out int excessMonths)
		{
			int months = first.MonthsBetween(second, includeLastDay);
			excessMonths = months % 12;
			return months / 12;
		}

		/// <summary>
		/// Returns the date value as an Integer.
		/// </summary>
		/// <param name="input">The input date time.</param>
		/// <returns>The date value as an Integer.</returns>
		private static int DateValue(this DateTimeOffset input) => (input.Year * 372) + ((input.Month - 1) * 31) + input.Day - 1;

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
}
