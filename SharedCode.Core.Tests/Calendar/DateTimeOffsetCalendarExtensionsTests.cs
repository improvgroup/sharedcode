namespace SharedCode.Tests.Calendar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Calendar;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="DateTimeOffsetExtensions"/> class in the Calendar namespace.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class DateTimeOffsetCalendarExtensionsTests
{
	[TestMethod]
	public void FirstDayOfMonth_ReturnsFirstDayOfTheMonth()
	{
		var date = new DateTimeOffset(2023, 5, 15, 10, 30, 0, TimeSpan.Zero);
		var result = date.FirstDayOfMonth();
		Assert.AreEqual(1, result.Day);
		Assert.AreEqual(5, result.Month);
		Assert.AreEqual(2023, result.Year);
	}

	[TestMethod]
	public void LastDayOfMonth_ReturnsLastDayOfTheMonth()
	{
		var date = new DateTimeOffset(2023, 2, 15, 0, 0, 0, TimeSpan.Zero);
		var result = date.LastDayOfMonth();
		Assert.AreEqual(28, result.Day);
		Assert.AreEqual(2, result.Month);
		Assert.AreEqual(2023, result.Year);
	}

	[TestMethod]
	public void LastDayOfMonth_LeapYear_Returns29()
	{
		var date = new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero);
		var result = date.LastDayOfMonth();
		Assert.AreEqual(29, result.Day);
	}

	[TestMethod]
	public void IsWeekend_SaturdayDate_ReturnsTrue()
	{
		var saturday = new DateTimeOffset(2023, 12, 9, 0, 0, 0, TimeSpan.Zero); // Saturday
		Assert.IsTrue(saturday.IsWeekend());
	}

	[TestMethod]
	public void IsWeekend_SundayDate_ReturnsTrue()
	{
		var sunday = new DateTimeOffset(2023, 12, 10, 0, 0, 0, TimeSpan.Zero); // Sunday
		Assert.IsTrue(sunday.IsWeekend());
	}

	[TestMethod]
	public void IsWeekend_MondayDate_ReturnsFalse()
	{
		var monday = new DateTimeOffset(2023, 12, 11, 0, 0, 0, TimeSpan.Zero); // Monday
		Assert.IsFalse(monday.IsWeekend());
	}

	[TestMethod]
	public void IsBetween_DateInRange_ReturnsTrue()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 12, 31, 0, 0, 0, TimeSpan.Zero);
		var middle = new DateTimeOffset(2023, 6, 15, 0, 0, 0, TimeSpan.Zero);
		Assert.IsTrue(middle.IsBetween(start, end));
	}

	[TestMethod]
	public void IsBetween_DateOutOfRange_ReturnsFalse()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 6, 30, 0, 0, 0, TimeSpan.Zero);
		var outside = new DateTimeOffset(2023, 7, 1, 0, 0, 0, TimeSpan.Zero);
		Assert.IsFalse(outside.IsBetween(start, end));
	}

	[TestMethod]
	public void IsBetween_WithCompareTime_DateInRange_ReturnsTrue()
	{
		var start = new DateTimeOffset(2023, 6, 1, 9, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 6, 1, 17, 0, 0, TimeSpan.Zero);
		var middle = new DateTimeOffset(2023, 6, 1, 12, 0, 0, TimeSpan.Zero);
		Assert.IsTrue(middle.IsBetween(start, end, compareTime: true));
	}

	[TestMethod]
	public void Intersects_RangesOverlap_ReturnsTrue()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 6, 30, 0, 0, 0, TimeSpan.Zero);
		var intersectStart = new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero);
		var intersectEnd = new DateTimeOffset(2023, 9, 30, 0, 0, 0, TimeSpan.Zero);
		Assert.IsTrue(start.Intersects(end, intersectStart, intersectEnd));
	}

	[TestMethod]
	public void Intersects_RangesDoNotOverlap_ReturnsFalse()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 3, 31, 0, 0, 0, TimeSpan.Zero);
		var intersectStart = new DateTimeOffset(2023, 5, 1, 0, 0, 0, TimeSpan.Zero);
		var intersectEnd = new DateTimeOffset(2023, 9, 30, 0, 0, 0, TimeSpan.Zero);
		Assert.IsFalse(start.Intersects(end, intersectStart, intersectEnd));
	}

	[TestMethod]
	public void GetDateRangeTo_ReturnsCorrectNumberOfDates()
	{
		var from = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var to = new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.Zero);
		var range = from.GetDateRangeTo(to).ToList();
		Assert.AreEqual(4, range.Count);
	}

	[TestMethod]
	public void DateDiff_DayPart_ReturnsExpectedDays()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 11, 0, 0, 0, TimeSpan.Zero);
		Assert.AreEqual(10L, start.DateDiff("day", end));
		Assert.AreEqual(10L, start.DateDiff("dd", end));
		Assert.AreEqual(10L, start.DateDiff("d", end));
	}

	[TestMethod]
	public void DateDiff_YearPart_ReturnsExpectedYears()
	{
		var start = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		Assert.AreEqual(3L, start.DateDiff("year", end));
		Assert.AreEqual(3L, start.DateDiff("yy", end));
		Assert.AreEqual(3L, start.DateDiff("yyyy", end));
	}

	[TestMethod]
	public void DateDiff_MonthPart_ReturnsExpectedMonths()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero);
		Assert.AreEqual(3L, start.DateDiff("month", end));
		Assert.AreEqual(3L, start.DateDiff("mm", end));
	}

	[TestMethod]
	public void DateDiff_HourPart_ReturnsExpectedHours()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 1, 3, 0, 0, TimeSpan.Zero);
		Assert.AreEqual(3L, start.DateDiff("hour", end));
		Assert.AreEqual(3L, start.DateDiff("hh", end));
	}

	[TestMethod]
	public void DateDiff_UnknownPart_ThrowsException()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 2, 0, 0, 0, TimeSpan.Zero);
		_ = Assert.ThrowsExactly<Exception>(() => start.DateDiff("unknown", end));
	}

	[TestMethod]
	public void ComputeTimeZoneVariance_UtcOffset_ReturnsZero()
	{
		var utcDate = new DateTimeOffset(2023, 6, 1, 12, 0, 0, TimeSpan.Zero);
		Assert.AreEqual(0, utcDate.ComputeTimeZoneVariance());
	}

	[TestMethod]
	public void ToUnixTimestamp_UnixEpoch_ReturnsZeroOrNearZero()
	{
		// Unix epoch: 1970-01-01 00:00:00 UTC (using local offset)
		var localOffset = DateTimeOffset.UtcNow.Offset;
		var epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, localOffset);
		Assert.AreEqual(0L, epoch.ToUnixTimestamp());
	}

	[TestMethod]
	public void AddWorkdays_AddsPositiveWorkdays()
	{
		var monday = new DateTimeOffset(2023, 12, 11, 0, 0, 0, TimeSpan.Zero); // Monday
		var result = monday.AddWorkdays(5);
		Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
		Assert.AreEqual(18, result.Day);
	}
}
