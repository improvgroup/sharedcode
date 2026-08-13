namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="NumberExtensions"/> class.
/// </summary>
public class NumberExtensionsTests
{
	[Test]
	public async Task Days_Int_ReturnsDaysTimeSpan()
	{
		var result = 3.Days();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromDays(3));
	}

	[Test]
	public async Task Days_Double_ReturnsDaysTimeSpan()
	{
		var result = 1.5.Days();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromDays(1.5));
	}

	[Test]
	public async Task Hours_Int_ReturnsHoursTimeSpan()
	{
		var result = 2.Hours();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromHours(2));
	}

	[Test]
	public async Task Hours_Double_ReturnsHoursTimeSpan()
	{
		var result = 2.5.Hours();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromHours(2.5));
	}

	[Test]
	public async Task Minutes_Int_ReturnsMinutesTimeSpan()
	{
		var result = 30.Minutes();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromMinutes(30));
	}

	[Test]
	public async Task Minutes_Double_ReturnsMinutesTimeSpan()
	{
		var result = 30.5.Minutes();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromMinutes(30.5));
	}

	[Test]
	public async Task Seconds_Int_ReturnsSecondsTimeSpan()
	{
		var result = 45.Seconds();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromSeconds(45));
	}

	[Test]
	public async Task Seconds_Double_ReturnsSecondsTimeSpan()
	{
		var result = 45.5.Seconds();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromSeconds(45.5));
	}

	[Test]
	public async Task Milliseconds_Int_ReturnsMillisecondsTimeSpan()
	{
		var result = 500.Milliseconds();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromMilliseconds(500));
	}

	[Test]
	public async Task Milliseconds_Double_ReturnsMillisecondsTimeSpan()
	{
		var result = 500.5.Milliseconds();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromMilliseconds(500.5));
	}

	[Test]
	public async Task Weeks_Int_ReturnsWeeksAsSevenDaysTimeSpan()
	{
		var result = 2.Weeks();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromDays(14));
	}

	[Test]
	public async Task Weeks_Double_ReturnsWeeksAsSevenDaysTimeSpan()
	{
		var result = 1.5.Weeks();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromDays(10.5));
	}

	[Test]
	public async Task Months_Int_ReturnsFluentTimeSpanWithMonths()
	{
		var result = 3.Months();
		await Assert.That(result.Months).IsEqualTo(3);
	}

	[Test]
	public async Task Years_Int_ReturnsFluentTimeSpanWithYears()
	{
		var result = 2.Years();
		await Assert.That(result.Years).IsEqualTo(2);
	}

	[Test]
	public async Task Ticks_Int_ReturnsTicksTimeSpan()
	{
		var result = 1000.Ticks();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromTicks(1000));
	}

	[Test]
	public async Task Ticks_Long_ReturnsTicksTimeSpan()
	{
		var result = 1000L.Ticks();
		await Assert.That((TimeSpan)result).IsEqualTo(TimeSpan.FromTicks(1000L));
	}
}
