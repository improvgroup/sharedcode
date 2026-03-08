namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="NumberExtensions"/> class.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class NumberExtensionsTests
{
	[TestMethod]
	public void Days_Int_ReturnsDaysTimeSpan()
	{
		var result = 3.Days();
		Assert.AreEqual(TimeSpan.FromDays(3), (TimeSpan)result);
	}

	[TestMethod]
	public void Days_Double_ReturnsDaysTimeSpan()
	{
		var result = 1.5.Days();
		Assert.AreEqual(TimeSpan.FromDays(1.5), (TimeSpan)result);
	}

	[TestMethod]
	public void Hours_Int_ReturnsHoursTimeSpan()
	{
		var result = 2.Hours();
		Assert.AreEqual(TimeSpan.FromHours(2), (TimeSpan)result);
	}

	[TestMethod]
	public void Hours_Double_ReturnsHoursTimeSpan()
	{
		var result = 2.5.Hours();
		Assert.AreEqual(TimeSpan.FromHours(2.5), (TimeSpan)result);
	}

	[TestMethod]
	public void Minutes_Int_ReturnsMinutesTimeSpan()
	{
		var result = 30.Minutes();
		Assert.AreEqual(TimeSpan.FromMinutes(30), (TimeSpan)result);
	}

	[TestMethod]
	public void Minutes_Double_ReturnsMinutesTimeSpan()
	{
		var result = 30.5.Minutes();
		Assert.AreEqual(TimeSpan.FromMinutes(30.5), (TimeSpan)result);
	}

	[TestMethod]
	public void Seconds_Int_ReturnsSecondsTimeSpan()
	{
		var result = 45.Seconds();
		Assert.AreEqual(TimeSpan.FromSeconds(45), (TimeSpan)result);
	}

	[TestMethod]
	public void Seconds_Double_ReturnsSecondsTimeSpan()
	{
		var result = 45.5.Seconds();
		Assert.AreEqual(TimeSpan.FromSeconds(45.5), (TimeSpan)result);
	}

	[TestMethod]
	public void Milliseconds_Int_ReturnsMillisecondsTimeSpan()
	{
		var result = 500.Milliseconds();
		Assert.AreEqual(TimeSpan.FromMilliseconds(500), (TimeSpan)result);
	}

	[TestMethod]
	public void Milliseconds_Double_ReturnsMillisecondsTimeSpan()
	{
		var result = 500.5.Milliseconds();
		Assert.AreEqual(TimeSpan.FromMilliseconds(500.5), (TimeSpan)result);
	}

	[TestMethod]
	public void Weeks_Int_ReturnsWeeksAsSevenDaysTimeSpan()
	{
		var result = 2.Weeks();
		Assert.AreEqual(TimeSpan.FromDays(14), (TimeSpan)result);
	}

	[TestMethod]
	public void Weeks_Double_ReturnsWeeksAsSevenDaysTimeSpan()
	{
		var result = 1.5.Weeks();
		Assert.AreEqual(TimeSpan.FromDays(10.5), (TimeSpan)result);
	}

	[TestMethod]
	public void Months_Int_ReturnsFluentTimeSpanWithMonths()
	{
		var result = 3.Months();
		Assert.AreEqual(3, result.Months);
	}

	[TestMethod]
	public void Years_Int_ReturnsFluentTimeSpanWithYears()
	{
		var result = 2.Years();
		Assert.AreEqual(2, result.Years);
	}

	[TestMethod]
	public void Ticks_Int_ReturnsTicksTimeSpan()
	{
		var result = 1000.Ticks();
		Assert.AreEqual(TimeSpan.FromTicks(1000), (TimeSpan)result);
	}

	[TestMethod]
	public void Ticks_Long_ReturnsTicksTimeSpan()
	{
		var result = 1000L.Ticks();
		Assert.AreEqual(TimeSpan.FromTicks(1000L), (TimeSpan)result);
	}
}
