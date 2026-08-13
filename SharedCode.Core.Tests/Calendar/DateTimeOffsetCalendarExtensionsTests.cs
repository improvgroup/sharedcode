namespace SharedCode.Tests.Calendar;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Calendar;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="DateTimeOffsetExtensions"/> class in the Calendar namespace.
/// </summary>
public class DateTimeOffsetCalendarExtensionsTests
{
	[Test]
	public async Task FirstDayOfMonth_ReturnsFirstDayOfTheMonth()
	{
		var date = new DateTimeOffset(2023, 5, 15, 10, 30, 0, TimeSpan.Zero);
		var result = date.FirstDayOfMonth();
		await Assert.That(result.Day).IsEqualTo(1);
		await Assert.That(result.Month).IsEqualTo(5);
		await Assert.That(result.Year).IsEqualTo(2023);
	}

	[Test]
	public async Task LastDayOfMonth_ReturnsLastDayOfTheMonth()
	{
		var date = new DateTimeOffset(2023, 2, 15, 0, 0, 0, TimeSpan.Zero);
		var result = date.LastDayOfMonth();
		await Assert.That(result.Day).IsEqualTo(28);
		await Assert.That(result.Month).IsEqualTo(2);
		await Assert.That(result.Year).IsEqualTo(2023);
	}

	[Test]
	public async Task LastDayOfMonth_LeapYear_Returns29()
	{
		var date = new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero);
		var result = date.LastDayOfMonth();
		await Assert.That(result.Day).IsEqualTo(29);
	}

	[Test]
	public async Task IsWeekend_SaturdayDate_ReturnsTrue()
	{
		var saturday = new DateTimeOffset(2023, 12, 9, 0, 0, 0, TimeSpan.Zero); // Saturday
		await Assert.That(saturday.IsWeekend()).IsTrue();
	}

	[Test]
	public async Task IsWeekend_SundayDate_ReturnsTrue()
	{
		var sunday = new DateTimeOffset(2023, 12, 10, 0, 0, 0, TimeSpan.Zero); // Sunday
		await Assert.That(sunday.IsWeekend()).IsTrue();
	}

	[Test]
	public async Task IsWeekend_MondayDate_ReturnsFalse()
	{
		var monday = new DateTimeOffset(2023, 12, 11, 0, 0, 0, TimeSpan.Zero); // Monday
		await Assert.That(monday.IsWeekend()).IsFalse();
	}

	[Test]
	public async Task IsBetween_DateInRange_ReturnsTrue()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 12, 31, 0, 0, 0, TimeSpan.Zero);
		var middle = new DateTimeOffset(2023, 6, 15, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(middle.IsBetween(start, end)).IsTrue();
	}

	[Test]
	public async Task IsBetween_DateOutOfRange_ReturnsFalse()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 6, 30, 0, 0, 0, TimeSpan.Zero);
		var outside = new DateTimeOffset(2023, 7, 1, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(outside.IsBetween(start, end)).IsFalse();
	}

	[Test]
	public async Task IsBetween_WithCompareTime_DateInRange_ReturnsTrue()
	{
		var start = new DateTimeOffset(2023, 6, 1, 9, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 6, 1, 17, 0, 0, TimeSpan.Zero);
		var middle = new DateTimeOffset(2023, 6, 1, 12, 0, 0, TimeSpan.Zero);
		await Assert.That(middle.IsBetween(start, end, compareTime: true)).IsTrue();
	}

	[Test]
	public async Task Intersects_RangesOverlap_ReturnsTrue()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 6, 30, 0, 0, 0, TimeSpan.Zero);
		var intersectStart = new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero);
		var intersectEnd = new DateTimeOffset(2023, 9, 30, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(start.Intersects(end, intersectStart, intersectEnd)).IsTrue();
	}

	[Test]
	public async Task Intersects_RangesDoNotOverlap_ReturnsFalse()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 3, 31, 0, 0, 0, TimeSpan.Zero);
		var intersectStart = new DateTimeOffset(2023, 5, 1, 0, 0, 0, TimeSpan.Zero);
		var intersectEnd = new DateTimeOffset(2023, 9, 30, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(start.Intersects(end, intersectStart, intersectEnd)).IsFalse();
	}

	[Test]
	public async Task GetDateRangeTo_ReturnsCorrectNumberOfDates()
	{
		var from = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var to = new DateTimeOffset(2023, 1, 5, 0, 0, 0, TimeSpan.Zero);
		var range = from.GetDateRangeTo(to).ToList();
		await Assert.That(range.Count).IsEqualTo(4);
	}

	[Test]
	public async Task DateDiff_DayPart_ReturnsExpectedDays()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 11, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(start.DateDiff("day", end)).IsEqualTo(10L);
		await Assert.That(start.DateDiff("dd", end)).IsEqualTo(10L);
		await Assert.That(start.DateDiff("d", end)).IsEqualTo(10L);
	}

	[Test]
	public async Task DateDiff_YearPart_ReturnsExpectedYears()
	{
		var start = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(start.DateDiff("year", end)).IsEqualTo(3L);
		await Assert.That(start.DateDiff("yy", end)).IsEqualTo(3L);
		await Assert.That(start.DateDiff("yyyy", end)).IsEqualTo(3L);
	}

	[Test]
	public async Task DateDiff_MonthPart_ReturnsExpectedMonths()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(start.DateDiff("month", end)).IsEqualTo(3L);
		await Assert.That(start.DateDiff("mm", end)).IsEqualTo(3L);
	}

	[Test]
	public async Task DateDiff_HourPart_ReturnsExpectedHours()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 1, 3, 0, 0, TimeSpan.Zero);
		await Assert.That(start.DateDiff("hour", end)).IsEqualTo(3L);
		await Assert.That(start.DateDiff("hh", end)).IsEqualTo(3L);
	}

	[Test]
	public async Task DateDiff_UnknownPart_ThrowsException()
	{
		var start = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var end = new DateTimeOffset(2023, 1, 2, 0, 0, 0, TimeSpan.Zero);
		await Assert.That(() => start.DateDiff("unknown", end)).ThrowsExactly<Exception>();
	}

	[Test]
	public async Task ComputeTimeZoneVariance_UtcOffset_ReturnsZero()
	{
		var utcDate = new DateTimeOffset(2023, 6, 1, 12, 0, 0, TimeSpan.Zero);
		await Assert.That(utcDate.ComputeTimeZoneVariance()).IsEqualTo(0);
	}

	[Test]
	public async Task ToUnixTimestamp_UnixEpoch_ReturnsZeroOrNearZero()
	{
		// Unix epoch: 1970-01-01 00:00:00 UTC (using local offset)
		var localOffset = DateTimeOffset.UtcNow.Offset;
		var epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, localOffset);
		await Assert.That(epoch.ToUnixTimestamp()).IsEqualTo(0L);
	}

	[Test]
	public async Task AddWorkdays_AddsPositiveWorkdays()
	{
		var monday = new DateTimeOffset(2023, 12, 11, 0, 0, 0, TimeSpan.Zero); // Monday
		var result = monday.AddWorkdays(5);
		await Assert.That(result.DayOfWeek).IsEqualTo(DayOfWeek.Monday);
		await Assert.That(result.Day).IsEqualTo(18);
	}
}
