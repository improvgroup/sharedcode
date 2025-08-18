namespace SharedCode;
/// <summary>
/// DateTime Extensions
/// </summary>
public static class DateTimeExtensions
{
	private const int DaysInAWeek = 7;
	private const int DaysInAYear = 365;
	private const int FiftyNine = 59;
	private const int HoursInADay = 24;
	private const int MinutesInAnHour = 60;
	private const int MonthsInAYear = 12;
	private const int NineHundredNinetyNine = 999;
	private const int NineThousandNineHundredNinetyNine = 9999;
	private const int ThirtyOne = 31;
	private const int ThreeHundredSeventyTwo = 372;
	private const int TwentyThree = 23;

	/// <summary>
	/// Adds the given number of business days to the <see cref="DateTime" />.
	/// </summary>
	/// <param name="this">The date to be changed.</param>
	/// <param name="days">Number of business days to be added.</param>
	/// <returns>A <see cref="DateTime" /> increased by a given number of business days.</returns>
	public static DateTime AddBusinessDays(this DateTime @this, int days)
	{
		var sign = Math.Sign(days);
		var unsignedDays = Math.Abs(days);
		for (var i = 0; i < unsignedDays; i++)
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
	public static DateTime AddWeekdays(this DateTime @this, int days)
	{
		var sign = days < 0 ? -1 : 1;
		var unsignedDays = Math.Abs(days);
		var weekdaysAdded = 0;
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
	/// Returns the given <see cref="DateTime" /> with hour and minutes set At given values.
	/// </summary>
	/// <param name="this">The current <see cref="DateTime" /> to be changed.</param>
	/// <param name="hour">The hour to set time to.</param>
	/// <param name="minute">The minute to set time to.</param>
	/// <returns><see cref="DateTime" /> with hour and minute set to given values.</returns>
	/// ReSharper disable MethodNamesNotMeaningful
	public static DateTime At(this DateTime @this, int hour, int minute)
	{
		// ReSharper restore MethodNamesNotMeaningful
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);

		return @this.SetTime(hour, minute);
	}

	/// <summary>
	/// Returns the given <see cref="DateTime" /> with hour and minutes and seconds set At given values.
	/// </summary>
	/// <param name="this">The current <see cref="DateTime" /> to be changed.</param>
	/// <param name="hour">The hour to set time to.</param>
	/// <param name="minute">The minute to set time to.</param>
	/// <param name="second">The second to set time to.</param>
	/// <returns><see cref="DateTime" /> with hour and minutes and seconds set to given values.</returns>
	/// ReSharper disable MethodNamesNotMeaningful
	public static DateTime At(this DateTime @this, int hour, int minute, int second)
	{
		// ReSharper restore MethodNamesNotMeaningful
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);
		Contract.Requires<ArgumentOutOfRangeException>(second <= MinutesInAnHour);

		return @this.SetTime(hour, minute, second);
	}

	/// <summary>
	/// Returns the Start of the given day (the first millisecond of the given <see cref="DateTime" />).
	/// </summary>
	/// <param name="this">The date.</param>
	/// <returns>A date/time at the beginning of the day (midnight).</returns>
	public static DateTime BeginningOfDay(this DateTime @this) =>
		new(@this.Year, @this.Month, @this.Day, 0, 0, 0, 0);

	/// <summary>
	/// Returns true if the date and time fall between the start and end dates, inclusive or not of
	/// the end date and time.
	/// </summary>
	/// <param name="this">The date time.</param>
	/// <param name="startDateTime">The start date time.</param>
	/// <param name="endDateTime">The end date time.</param>
	/// <param name="endInclusive">if set to <c>true</c> [end inclusive].</param>
	/// <returns>
	/// <c>true</c> if the date and time fall between the start and end dates, inclusive or not of
	/// the end date and time, <c>false</c> otherwise.
	/// </returns>
	[SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Avoiding nested conditionals.")]
	public static bool Between(this DateTime @this, DateTime startDateTime, DateTime endDateTime, bool endInclusive = false)
	{
		if (startDateTime == endDateTime)
		{
			return @this == startDateTime && endInclusive;
		}

		if (startDateTime < endDateTime)
		{
			return endInclusive
				? @this >= startDateTime && @this <= endDateTime
				: @this >= startDateTime && @this < endDateTime;
		}

		return @this.Between(endDateTime, startDateTime, endInclusive);
	}

	/// <summary>
	/// Rounds the specified DateTime up to the next TimeSpan
	/// </summary>
	/// <param name="this">The DateTime to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTime.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Ceiling(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTime Ceiling(this DateTime @this, TimeSpan span) =>
		new((@this.Ticks + span.Ticks - 1) / span.Ticks * span.Ticks);

	/// <summary>
	/// Returns the number of days between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of days between the specified dates.</returns>
	public static int DaysBetween(this DateTime @this, DateTime second) => (second.Date - @this.Date).Duration().Days;

	/// <summary>
	/// Returns the number of days between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of days between the specified dates.</returns>
	public static int DaysBetween(this DateTime @this, DateTime second, bool includeLastDay)
	{
		var days = @this.DaysBetween(second);
		return includeLastDay ? days + 1 : days;
	}

	/// <summary>
	/// Decreases the <see cref="DateTime" /> object with given <see cref="TimeSpan" /> value.
	/// </summary>
	/// <param name="this">The start Date.</param>
	/// <param name="toSubtract">The to Subtract.</param>
	/// <returns>The result.</returns>
	public static DateTime DecreaseTime(this DateTime @this, TimeSpan toSubtract) => @this - toSubtract;

	/// <summary>
	/// Returns the very end of the given day (the last millisecond of the last hour for the given
	/// <see cref="DateTime" /> ).
	/// </summary>
	/// <param name="this">The date.</param>
	/// <returns>A date/time with time at the end of the day. (23:59:59.999)</returns>
	public static DateTime EndOfDay(this DateTime @this) =>
		new(@this.Year, @this.Month, @this.Day, TwentyThree, FiftyNine, FiftyNine, NineHundredNinetyNine);

	/// <summary>
	/// Sets the day of the <see cref="DateTime" /> to the first day in that month.
	/// </summary>
	/// <param name="this">The current <see cref="DateTime" /> to be changed.</param>
	/// <returns>
	/// given <see cref="DateTime" /> with the day part set to the first day in that month.
	/// </returns>
	public static DateTime FirstDayOfMonth(this DateTime @this) => @this.SetDay(1);

	/// <summary>
	/// Rounds the specified DateTime down to the next TimeSpan
	/// </summary>
	/// <param name="this">The DateTime to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTime.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Floor(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTime Floor(this DateTime @this, TimeSpan span) =>
		new(@this.Ticks / span.Ticks * span.Ticks);

	/// <summary>
	/// Increases the <see cref="DateTime" /> object with given <see cref="TimeSpan" /> value.
	/// </summary>
	/// <param name="this">The start Date.</param>
	/// <param name="toAdd">The to Add.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime IncreaseTime(this DateTime @this, TimeSpan toAdd) => @this + toAdd;

	/// <summary>
	/// Determines whether the specified <see cref="DateTime" /> value is After then current value.
	/// </summary>
	/// <param name="this">The current value.</param>
	/// <param name="toCompareWith">Value to compare with.</param>
	/// <returns><c>true</c> if the specified current is after; otherwise, <c>false</c>.</returns>
	public static bool IsAfter(this DateTime @this, DateTime toCompareWith) => @this > toCompareWith;

	/// <summary>
	/// Determines whether the specified <see cref="DateTime" /> is before then current value.
	/// </summary>
	/// <param name="this">The current value.</param>
	/// <param name="toCompareWith">Value to compare with.</param>
	/// <returns><c>true</c> if the specified current is before; otherwise, <c>false</c>.</returns>
	public static bool IsBefore(this DateTime @this, DateTime toCompareWith) => @this < toCompareWith;

	/// <summary>
	/// Determine if a <see cref="DateTime" /> is in the future.
	/// </summary>
	/// <param name="this">The date to be checked.</param>
	/// <returns><c>true</c> if <paramref name="this" /> is in the future; otherwise <c>false</c>.</returns>
	public static bool IsInFuture(this DateTime @this) => @this > DateTime.Now;

	/// <summary>
	/// Determine if a <see cref="DateTime" /> is in the past.
	/// </summary>
	/// <param name="this">The date to be checked.</param>
	/// <returns><c>true</c> if <paramref name="this" /> is in the past; otherwise <c>false</c>.</returns>
	public static bool IsInPast(this DateTime @this) => @this < DateTime.Now;

	/// <summary>
	/// Sets the day of the <see cref="DateTime" /> to the last day in that month.
	/// </summary>
	/// <param name="this">The current DateTime to be changed.</param>
	/// <returns>
	/// given <see cref="DateTime" /> with the day part set to the last day in that month.
	/// </returns>
	public static DateTime LastDayOfMonth(this DateTime @this) =>
		@this.SetDay(DateTime.DaysInMonth(@this.Year, @this.Month));

	/// <summary>
	/// Returns original <see cref="DateTime" /> value with time part set to midnight (alias for
	/// <see cref="BeginningOfDay" /> method).
	/// </summary>
	/// <param name="this">The value.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime Midnight(this DateTime @this) => @this.BeginningOfDay();

	/// <summary>
	/// Returns the number of months between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of months between the specified dates.</returns>
	public static int MonthsBetween(this DateTime @this, DateTime second) => Math.Abs((second.DateValue() - @this.DateValue()) / ThirtyOne);

	/// <summary>
	/// Returns the number of months between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of months between the specified dates.</returns>
	public static int MonthsBetween(this DateTime @this, DateTime second, bool includeLastDay)
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
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime Next(this DateTime @this, DayOfWeek day)
	{
		do
		{
			@this = @this.NextDay();
		}
		while (@this.DayOfWeek != day);

		return @this;
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> increased by 24 hours i.e. Next Day.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime NextDay(this DateTime @this) => @this + 1.Days();

	/// <summary>
	/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar year.
	/// If that day does not exist in next year in same month, number of missing days is added to
	/// the last day in same month next year.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime NextYear(this DateTime @this)
	{
		var nextYear = @this.Year + 1;
		var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, @this.Month);

		if (numberOfDaysInSameMonthNextYear < @this.Day)
		{
			var differenceInDays = @this.Day - numberOfDaysInSameMonthNextYear;
			var dateTime = new DateTime(
				nextYear,
				@this.Month,
				numberOfDaysInSameMonthNextYear,
				@this.Hour,
				@this.Minute,
				@this.Second,
				@this.Millisecond);
			return dateTime + differenceInDays.Days();
		}

		return new DateTime(
			nextYear,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			@this.Second,
			@this.Millisecond);
	}

	/// <summary>
	/// Returns original <see cref="DateTime" /> value with time part set to Noon (12:00:00h).
	/// </summary>
	/// <param name="this">The <see cref="DateTime" /> find Noon for.</param>
	/// <returns>A <see cref="DateTime" /> value with time part set to Noon (12:00:00h).</returns>
	public static DateTime Noon(this DateTime @this) => @this.SetTime(MonthsInAYear, 0, 0, 0);

	/// <summary>
	/// Returns first next occurrence of specified <see cref="DayOfWeek" />.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <param name="day">The day of the week.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime Previous(this DateTime @this, DayOfWeek day)
	{
		do
		{
			@this = @this.PreviousDay();
		}
		while (@this.DayOfWeek != day);

		return @this;
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> decreased by 24h period i.e. Previous Day.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime PreviousDay(this DateTime @this) => @this - 1.Days();

	/// <summary>
	/// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous calendar
	/// year. If that day does not exist in previous year in same month, number of missing days is
	/// added to the last day in same month previous year.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime PreviousYear(this DateTime @this)
	{
		var previousYear = @this.Year - 1;
		var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, @this.Month);

		if (numberOfDaysInSameMonthPreviousYear < @this.Day)
		{
			var differenceInDays = @this.Day - numberOfDaysInSameMonthPreviousYear;
			var dateTime = new DateTime(
				previousYear,
				@this.Month,
				numberOfDaysInSameMonthPreviousYear,
				@this.Hour,
				@this.Minute,
				@this.Second,
				@this.Millisecond);
			return dateTime + differenceInDays.Days();
		}

		return new DateTime(
			previousYear,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			@this.Second,
			@this.Millisecond);
	}

	/// <summary>
	/// Rounds the specified DateTime to the nearest TimeSpan
	/// </summary>
	/// <param name="this">The DateTime to round.</param>
	/// <param name="span">The TimeSpan to round by.</param>
	/// <returns>The rounded DateTime.</returns>
	/// <example>
	/// Rounds to nearest minute (replace 1 for 10 to get nearest 10 minutes)
	/// <code>
	///MyDate.Floor(new TimeSpan(0,1,0));
	/// </code>
	/// </example>
	public static DateTime Round(this DateTime @this, TimeSpan span) =>
		new(((@this.Ticks / span.Ticks) + (span.Ticks / 2) + 1) * span.Ticks);

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year to set.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDate(this DateTime @this, int year)
	{
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTime(year, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year and Month part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year.</param>
	/// <param name="month">The month.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDate(this DateTime @this, int year, int month)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= MonthsInAYear);
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTime(year, month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year, Month and Day part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year.</param>
	/// <param name="month">The month.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDate(this DateTime @this, int year, int month, int day)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= MonthsInAYear);
		Contract.Requires<ArgumentOutOfRangeException>(day >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTime(year, month, day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Day part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="day">The day.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetDay(this DateTime @this, int day)
	{
		Contract.Requires<ArgumentOutOfRangeException>(day >= 1);

		return new DateTime(@this.Year, @this.Month, day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Hour part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="hour">The hour.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetHour(this DateTime @this, int hour)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);

		return new DateTime(
			@this.Year,
			@this.Month,
			@this.Day,
			hour,
			@this.Minute,
			@this.Second,
			@this.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Millisecond part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="millisecond">The millisecond.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetMillisecond(this DateTime @this, int millisecond)
	{
		Contract.Requires<ArgumentOutOfRangeException>(millisecond >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

		return new DateTime(
			@this.Year,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			@this.Second,
			millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Minute part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="minute">The minute.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetMinute(this DateTime @this, int minute)
	{
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);

		return new DateTime(
			@this.Year,
			@this.Month,
			@this.Day,
			@this.Hour,
			minute,
			@this.Second,
			@this.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Month part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="month">The month.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetMonth(this DateTime @this, int month)
	{
		Contract.Requires<ArgumentOutOfRangeException>(month >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(month <= MonthsInAYear);

		return new DateTime(@this.Year, month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Second part.
	/// </summary>
	/// <param name="this">The original Date.</param>
	/// <param name="second">The second.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetSecond(this DateTime @this, int second)
	{
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second <= MinutesInAnHour);

		return new DateTime(
			@this.Year,
			@this.Month,
			@this.Day,
			@this.Hour,
			@this.Minute,
			second,
			@this.Millisecond);
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="hour">The number of hours.</param>
	/// <returns>The date/time with the time set as specified.</returns>
	public static DateTime SetTime(this DateTime @this, int hour)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
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
	public static DateTime SetTime(this DateTime @this, int hour, int minute)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
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
	public static DateTime SetTime(this DateTime @this, int hour, int minute, int second)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
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
	public static DateTime SetTime(this DateTime @this, int hour, int minute, int second, int millisecond)
	{
		Contract.Requires<ArgumentOutOfRangeException>(hour >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(minute >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(second >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond >= 0);
		Contract.Requires<ArgumentOutOfRangeException>(hour <= HoursInADay);
		Contract.Requires<ArgumentOutOfRangeException>(minute <= MinutesInAnHour);
		Contract.Requires<ArgumentOutOfRangeException>(second <= MinutesInAnHour);
		Contract.Requires<ArgumentOutOfRangeException>(millisecond < 1000);

		return new DateTime(@this.Year, @this.Month, @this.Day, hour, minute, second, millisecond);
	}

	/// <summary>
	/// Returns <see cref="DateTime" /> with changed Year part.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="year">The year.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime SetYear(this DateTime @this, int year)
	{
		Contract.Requires<ArgumentOutOfRangeException>(year >= 1);
		Contract.Requires<ArgumentOutOfRangeException>(year <= NineThousandNineHundredNinetyNine);

		return new DateTime(year, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
	}

	/// <summary>
	/// Returns a DateTime adjusted to the beginning of the week.
	/// </summary>
	/// <param name="this">The DateTime to adjust</param>
	/// <returns>A DateTime instance adjusted to the beginning of the current week</returns>
	/// <remarks>the beginning of the week is controlled by the current Culture</remarks>
	[SuppressMessage("Style", "IDE0022:Use expression body for method", Justification = "Rules are in conflict here.")]
	public static DateTime StartOfWeek(this DateTime @this)
	{
		return @this.AddDays(-(DateTime.Today.DayOfWeek - Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
	}

	/// <summary>
	/// Subtracts the given number of business days to the <see cref="DateTime" />.
	/// </summary>
	/// <param name="this">The date to be changed.</param>
	/// <param name="days">Number of business days to be subtracted.</param>
	/// <returns>A <see cref="DateTime" /> increased by a given number of business days.</returns>
	public static DateTime SubtractBusinessDays(this DateTime @this, int days) => AddBusinessDays(@this, -days);

	/// <summary>
	/// Returns the relative date string.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <returns>The relative date string.</returns>
	public static string ToRelativeDateString(this DateTime @this) => GetRelativeDateValue(@this, DateTime.Now);

	/// <summary>
	/// Returns the relative date string for UTC time.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <returns>The relative date string for UTC time.</returns>
	public static string ToRelativeDateStringUtc(this DateTime @this) => GetRelativeDateValue(@this, DateTime.UtcNow);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTime? @this) => @this.ToString(default, DateTimeFormatInfo.CurrentInfo);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="format">The format string.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTime? @this, string? format) =>
		@this.ToString(format, DateTimeFormatInfo.CurrentInfo);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="provider">The format provider.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(this DateTime? @this, IFormatProvider? provider) => @this.ToString(null, provider);

	/// <summary>
	/// Returns a <see cref="string" /> that represents this instance.
	/// </summary>
	/// <param name="this">The date/time.</param>
	/// <param name="format">The format string.</param>
	/// <param name="provider">The format provider.</param>
	/// <returns>A <see cref="string" /> that represents this instance.</returns>
	public static string ToString(
		this DateTime? @this,
		string? format,
		IFormatProvider? provider) => @this?.ToString(format, provider) ?? string.Empty;

	/// <summary>
	/// Increases supplied <see cref="DateTime" /> for 7 days i.e. returns the Next Week.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime WeekAfter(this DateTime @this) => @this + 1.Weeks();

	/// <summary>
	/// Decreases supplied <see cref="DateTime" /> for 7 days i.e. returns the Previous Week.
	/// </summary>
	/// <param name="this">The start.</param>
	/// <returns>The <see cref="DateTime" />.</returns>
	public static DateTime WeekEarlier(this DateTime @this) => @this - 1.Weeks();

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <returns>The number of weeks between the specified dates.</returns>
	public static int WeeksBetween(this DateTime @this, DateTime second) => @this.DaysBetween(second) / DaysInAWeek;

	/// <summary>
	/// Returns the number of weeks between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of weeks between the specified dates.</returns>
	public static int WeeksBetween(this DateTime @this, DateTime second, bool includeLastDay) => @this.DaysBetween(second, includeLastDay) / DaysInAWeek;

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
	public static int WeeksBetween(this DateTime @this, DateTime second, bool includeLastDay, out int excessDays)
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
	public static int YearsBetween(this DateTime @this, DateTime second) => @this.MonthsBetween(second) / MonthsInAYear;

	/// <summary>
	/// Returns the number of years between the specified dates.
	/// </summary>
	/// <param name="this">The first date.</param>
	/// <param name="second">The second date.</param>
	/// <param name="includeLastDay">
	/// A value indicating whether to include the last day in the calculation.
	/// </param>
	/// <returns>The number of years between the specified dates.</returns>
	public static int YearsBetween(this DateTime @this, DateTime second, bool includeLastDay) => @this.MonthsBetween(second, includeLastDay) / MonthsInAYear;

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
	public static int YearsBetween(this DateTime @this, DateTime second, bool includeLastDay, out int excessMonths)
	{
		var months = @this.MonthsBetween(second, includeLastDay);
		excessMonths = months % MonthsInAYear;
		return months / MonthsInAYear;
	}

	/// <summary>
	/// Returns the date value as an Integer.
	/// </summary>
	/// <param name="input">The input date time.</param>
	/// <returns>The date value as an Integer.</returns>
	private static int DateValue(this DateTime input) => (input.Year * ThreeHundredSeventyTwo) + ((input.Month - 1) * ThirtyOne) + input.Day - 1;

	/// <summary>
	/// Gets the relative date value.
	/// </summary>
	/// <param name="date">The date/time.</param>
	/// <param name="comparedTo">The date/time to compare to.</param>
	/// <returns>The relative date value.</returns>
	private static string GetRelativeDateValue(DateTime date, DateTime comparedTo)
	{
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
