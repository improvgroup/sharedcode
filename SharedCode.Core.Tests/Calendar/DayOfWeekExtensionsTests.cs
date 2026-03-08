namespace SharedCode.Tests.Calendar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Calendar;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="SharedCode.Calendar.DayOfWeekExtensions"/> class.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class DayOfWeekExtensionsTests
{
	[TestMethod]
	[DataRow(DayOfWeek.Monday)]
	[DataRow(DayOfWeek.Tuesday)]
	[DataRow(DayOfWeek.Wednesday)]
	[DataRow(DayOfWeek.Thursday)]
	[DataRow(DayOfWeek.Friday)]
	public void IsWeekday_WeekdayDays_ReturnsTrue(DayOfWeek day)
	{
		Assert.IsTrue(day.IsWeekday());
	}

	[TestMethod]
	[DataRow(DayOfWeek.Saturday)]
	[DataRow(DayOfWeek.Sunday)]
	public void IsWeekday_WeekendDays_ReturnsFalse(DayOfWeek day)
	{
		Assert.IsFalse(day.IsWeekday());
	}

	[TestMethod]
	[DataRow(DayOfWeek.Saturday)]
	[DataRow(DayOfWeek.Sunday)]
	public void IsWeekend_WeekendDays_ReturnsTrue(DayOfWeek day)
	{
		Assert.IsTrue(day.IsWeekend());
	}

	[TestMethod]
	[DataRow(DayOfWeek.Monday)]
	[DataRow(DayOfWeek.Tuesday)]
	[DataRow(DayOfWeek.Wednesday)]
	[DataRow(DayOfWeek.Thursday)]
	[DataRow(DayOfWeek.Friday)]
	public void IsWeekend_WeekdayDays_ReturnsFalse(DayOfWeek day)
	{
		Assert.IsFalse(day.IsWeekend());
	}
}
