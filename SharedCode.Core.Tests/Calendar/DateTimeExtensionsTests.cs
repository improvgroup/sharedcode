
namespace SharedCode.Tests.Calendar;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Calendar;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The date time extensions tests class
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class DateTimeExtensionsTests
{
	private const int Day = 11;
	private const int Hour = 2;
	private const int Minute = 30;
	private const int Month = 3;
	private const int Second = 58;
	private const int Year = 1984;

	/// <summary>
	/// The original date time
	/// </summary>
	private DateTime originalDateTime;

	[TestMethod]
	[DataRow(5, 2022, 12, 11, 2022, 12, 19)]
	[DataRow(3, 2022, 12, 10, 2022, 12, 15)]
	public void AddWorkdays_AddsGivenNumberOfWorkdaysAndSkipsWeekends(int workdays, int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
	{
		// Arrange
		var date = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);

		// Act
		DateTimeOffset result = date.AddWorkdays(workdays);

		// Assert
		Assert.AreEqual(new DateTimeOffset(expectedYear, expectedMonth, expectedDay, 0, 0, 0, TimeSpan.Zero), result);
	}

	[TestMethod]
	public void AddWorkdays_SkipsWeekends()
	{
		// Arrange
		DateTimeOffset date = new DateTimeOffset(2022, 12, 10, 0, 0, 0, TimeSpan.Zero);

		// Act
		DateTimeOffset result = date.AddWorkdays(3);

		// Assert
		Assert.AreEqual(new DateTimeOffset(2022, 12, 15, 0, 0, 0, TimeSpan.Zero), result);
	}

	/// <summary>
	/// Determines whether this instance [can get full long date time string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Full_Long_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.FullLongDateTime);
		Assert.AreEqual("Sunday, March 11, 1984 2:30:58 AM", result);
	}

	/// <summary>
	/// Determines whether this instance [can get full short date time string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Full_Short_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.FullShortDateTime);
		Assert.AreEqual("Sunday, March 11, 1984 2:30 AM", result);
	}

	/// <summary>
	/// Determines whether this instance [can get general long date time string].
	/// </summary>
	[TestMethod]
	public void Can_Get_General_Long_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.GeneralLongDateTime);
		Assert.AreEqual("3/11/1984 2:30:58 AM", result);
	}

	/// <summary>
	/// Determines whether this instance [can get general short date time string].
	/// </summary>
	[TestMethod]
	public void Can_Get_General_Short_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.GeneralShortDateTime);
		Assert.AreEqual("3/11/1984 2:30 AM", result);
	}

	/// <summary>
	/// Determines whether this instance [can get long date string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Long_Date_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.LongDate);
		Assert.AreEqual("Sunday, March 11, 1984", result);
	}

	/// <summary>
	/// Determines whether this instance [can get long time string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Long_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.LongTime);
		Assert.AreEqual("2:30:58 AM", result);
	}

	/// <summary>
	/// Determines whether this instance [can get month day lower case string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Month_Day_Lower_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.MonthDayLowerCase);
		Assert.AreEqual("March 11", result);
	}

	/// <summary>
	/// Determines whether this instance [can get month day upper case string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Month_Day_Upper_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.MonthDayUpperCase);
		Assert.AreEqual("March 11", result);
	}

	/// <summary>
	/// Determines whether this instance [can get RFC1123 lower case string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Rfc1123_Lower_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.Rfc1123LowerCase);
		Assert.AreEqual("Sun, 11 Mar 1984 02:30:58 GMT", result);
	}

	/// <summary>
	/// Determines whether this instance [can get RFC1123 upper case string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Rfc1123_Upper_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.Rfc1123UpperCase);
		Assert.AreEqual("Sun, 11 Mar 1984 02:30:58 GMT", result);
	}

	/// <summary>
	/// Determines whether this instance [can get short date string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Short_Date_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.ShortDate);
		Assert.AreEqual(this.originalDateTime.ToShortDateString(), result);
	}

	/// <summary>
	/// Determines whether this instance [can get short time string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Short_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.ShortTime);
		Assert.AreEqual("2:30 AM", result);
	}

	/// <summary>
	/// Determines whether this instance [can get sortable date time iso8601 string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Sortable_DateTime_Iso8601_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.SortableDateTimeIso8601);
		Assert.AreEqual("1984-03-11T02:30:58", result);
	}

	/// <summary>
	/// Determines whether this instance [can get universal sortable date time string].
	/// </summary>
	[TestMethod]
	[Ignore]
	public void Can_Get_Universal_Sortable_DateTime_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.UniversalSortableDateTime);
		Assert.AreEqual("Sunday, March 11, 1984 10:30:58 AM", result);
	}

	/// <summary>
	/// Determines whether this instance [can get year month lower case string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Year_Month_Lower_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.YearMonthLowerCase);
		Assert.AreEqual("March 1984", result);
	}

	/// <summary>
	/// Determines whether this instance [can get year month upper case string].
	/// </summary>
	[TestMethod]
	public void Can_Get_Year_Month_Upper_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.YearMonthUpperCase);
		Assert.AreEqual("March 1984", result);
	}

	/// <summary>
	/// Initializes the test case.
	/// </summary>
	[TestInitialize]
	public void InitTestCase() => this.originalDateTime = new DateTime(Year, Month, Day, Hour, Minute, Second);

	/// <summary>
	/// Teardowns the test case.
	/// </summary>
	[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "This is a special case.")]
	[TestCleanup]
	public void TeardownTestCase()
	{
		if (this.originalDateTime != default)
		{
			GC.SuppressFinalize(this.originalDateTime);
		}
	}
}
