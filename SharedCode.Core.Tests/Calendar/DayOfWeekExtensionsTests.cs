namespace SharedCode.Tests.Calendar;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Calendar;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="SharedCode.Calendar.DayOfWeekExtensions"/> class.
/// </summary>
public class DayOfWeekExtensionsTests
{
	[Test]
	[Arguments(DayOfWeek.Monday)]
	[Arguments(DayOfWeek.Tuesday)]
	[Arguments(DayOfWeek.Wednesday)]
	[Arguments(DayOfWeek.Thursday)]
	[Arguments(DayOfWeek.Friday)]
	public async Task IsWeekday_WeekdayDays_ReturnsTrue(DayOfWeek day)
	{
		await Assert.That(day.IsWeekday()).IsTrue();
	}

	[Test]
	[Arguments(DayOfWeek.Saturday)]
	[Arguments(DayOfWeek.Sunday)]
	public async Task IsWeekday_WeekendDays_ReturnsFalse(DayOfWeek day)
	{
		await Assert.That(day.IsWeekday()).IsFalse();
	}

	[Test]
	[Arguments(DayOfWeek.Saturday)]
	[Arguments(DayOfWeek.Sunday)]
	public async Task IsWeekend_WeekendDays_ReturnsTrue(DayOfWeek day)
	{
		await Assert.That(day.IsWeekend()).IsTrue();
	}

	[Test]
	[Arguments(DayOfWeek.Monday)]
	[Arguments(DayOfWeek.Tuesday)]
	[Arguments(DayOfWeek.Wednesday)]
	[Arguments(DayOfWeek.Thursday)]
	[Arguments(DayOfWeek.Friday)]
	public async Task IsWeekend_WeekdayDays_ReturnsFalse(DayOfWeek day)
	{
		await Assert.That(day.IsWeekend()).IsFalse();
	}
}
