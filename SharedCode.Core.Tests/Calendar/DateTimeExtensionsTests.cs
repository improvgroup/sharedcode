
namespace SharedCode.Tests.Calendar;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Calendar;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

/// <summary>
/// The date time extensions tests class
/// </summary>
[NotInParallel]
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

	/// <summary>
	/// The original culture before test execution.
	/// </summary>
	private CultureInfo? originalCulture;

	/// <summary>
	/// The original default culture before test execution.
	/// </summary>
	private CultureInfo? originalDefaultCulture;

	/// <summary>
	/// The original UI culture before test execution.
	/// </summary>
	private CultureInfo? originalUiCulture;

	/// <summary>
	/// The original default UI culture before test execution.
	/// </summary>
	private CultureInfo? originalDefaultUiCulture;

	[Test]
	[Arguments(5, 2022, 12, 11, 2022, 12, 19)]
	[Arguments(3, 2022, 12, 10, 2022, 12, 15)]
	public async Task AddWorkdays_AddsGivenNumberOfWorkdaysAndSkipsWeekends(int workdays, int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
	{
		// Arrange
		var date = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);

		// Act
		DateTimeOffset result = date.AddWorkdays(workdays);

		// Assert
		await Assert.That(result).IsEqualTo(new DateTimeOffset(expectedYear, expectedMonth, expectedDay, 0, 0, 0, TimeSpan.Zero));
	}

	[Test]
	public async Task AddWorkdays_SkipsWeekends()
	{
		// Arrange
		DateTimeOffset date = new DateTimeOffset(2022, 12, 10, 0, 0, 0, TimeSpan.Zero);

		// Act
		DateTimeOffset result = date.AddWorkdays(3);

		// Assert
		await Assert.That(result).IsEqualTo(new DateTimeOffset(2022, 12, 15, 0, 0, 0, TimeSpan.Zero));
	}

	/// <summary>
	/// Determines whether this instance [can get full long date time string].
	/// </summary>
	[Test]
	public async Task Can_Get_Full_Long_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.FullLongDateTime);
		var expected = NormalizeWhitespace(this.originalDateTime.ToString("F", Thread.CurrentThread.CurrentCulture));
		await Assert.That(NormalizeWhitespace(result)).IsEqualTo(expected);
	}

	/// <summary>
	/// Determines whether this instance [can get full short date time string].
	/// </summary>
	[Test]
	public async Task Can_Get_Full_Short_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.FullShortDateTime);
		var expected = NormalizeWhitespace(this.originalDateTime.ToString("f", Thread.CurrentThread.CurrentCulture));
		await Assert.That(NormalizeWhitespace(result)).IsEqualTo(expected);
	}

	/// <summary>
	/// Determines whether this instance [can get general long date time string].
	/// </summary>
	[Test]
	public async Task Can_Get_General_Long_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.GeneralLongDateTime);
		var expected = NormalizeWhitespace(this.originalDateTime.ToString("G", Thread.CurrentThread.CurrentCulture));
		await Assert.That(NormalizeWhitespace(result)).IsEqualTo(expected);
	}

	/// <summary>
	/// Determines whether this instance [can get general short date time string].
	/// </summary>
	[Test]
	public async Task Can_Get_General_Short_Date_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.GeneralShortDateTime);
		var expected = NormalizeWhitespace(this.originalDateTime.ToString("g", Thread.CurrentThread.CurrentCulture));
		await Assert.That(NormalizeWhitespace(result)).IsEqualTo(expected);
	}

	/// <summary>
	/// Determines whether this instance [can get long date string].
	/// </summary>
	[Test]
	public async Task Can_Get_Long_Date_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.LongDate);
		await Assert.That(result).IsEqualTo(this.originalDateTime.ToString("D", Thread.CurrentThread.CurrentCulture));
	}

	/// <summary>
	/// Determines whether this instance [can get long time string].
	/// </summary>
	[Test]
	public async Task Can_Get_Long_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.LongTime);
		var expected = NormalizeWhitespace(this.originalDateTime.ToString("T", Thread.CurrentThread.CurrentCulture));
		await Assert.That(NormalizeWhitespace(result)).IsEqualTo(expected);
	}

	/// <summary>
	/// Determines whether this instance [can get month day lower case string].
	/// </summary>
	[Test]
	public async Task Can_Get_Month_Day_Lower_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.MonthDayLowerCase);
		await Assert.That(result).IsEqualTo(this.originalDateTime.ToString("m", Thread.CurrentThread.CurrentCulture));
	}

	/// <summary>
	/// Determines whether this instance [can get month day upper case string].
	/// </summary>
	[Test]
	public async Task Can_Get_Month_Day_Upper_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.MonthDayUpperCase);
		await Assert.That(result).IsEqualTo(this.originalDateTime.ToString("M", Thread.CurrentThread.CurrentCulture));
	}

	/// <summary>
	/// Determines whether this instance [can get RFC1123 lower case string].
	/// </summary>
	[Test]
	public async Task Can_Get_Rfc1123_Lower_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.Rfc1123LowerCase);
		await Assert.That(result).IsEqualTo("Sun, 11 Mar 1984 02:30:58 GMT");
	}

	/// <summary>
	/// Determines whether this instance [can get RFC1123 upper case string].
	/// </summary>
	[Test]
	public async Task Can_Get_Rfc1123_Upper_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.Rfc1123UpperCase);
		await Assert.That(result).IsEqualTo("Sun, 11 Mar 1984 02:30:58 GMT");
	}

	/// <summary>
	/// Determines whether this instance [can get short date string].
	/// </summary>
	[Test]
	public async Task Can_Get_Short_Date_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.ShortDate);
		await Assert.That(result).IsEqualTo(this.originalDateTime.ToShortDateString());
	}

	/// <summary>
	/// Determines whether this instance [can get short time string].
	/// </summary>
	[Test]
	public async Task Can_Get_Short_Time_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.ShortTime);
		var expected = NormalizeWhitespace(this.originalDateTime.ToString("t", Thread.CurrentThread.CurrentCulture));
		await Assert.That(NormalizeWhitespace(result)).IsEqualTo(expected);
	}

	/// <summary>
	/// Determines whether this instance [can get sortable date time iso8601 string].
	/// </summary>
	[Test]
	public async Task Can_Get_Sortable_DateTime_Iso8601_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.SortableDateTimeIso8601);
		await Assert.That(result).IsEqualTo("1984-03-11T02:30:58");
	}

	/// <summary>
	/// Determines whether this instance [can get universal sortable date time string].
	/// </summary>
	[Test]
	[Skip("Not applicable")]
	public async Task Can_Get_Universal_Sortable_DateTime_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.UniversalSortableDateTime);
		await Assert.That(result).IsEqualTo("Sunday, March 11, 1984 10:30:58 AM");
	}

	/// <summary>
	/// Determines whether this instance [can get year month lower case string].
	/// </summary>
	[Test]
	public async Task Can_Get_Year_Month_Lower_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.YearMonthLowerCase);
		await Assert.That(result).IsEqualTo(this.originalDateTime.ToString("y", Thread.CurrentThread.CurrentCulture));
	}

	/// <summary>
	/// Determines whether this instance [can get year month upper case string].
	/// </summary>
	[Test]
	public async Task Can_Get_Year_Month_Upper_Case_String()
	{
		var result = this.originalDateTime.ToStringFormat(() => DateTimeFormat.YearMonthUpperCase);
		await Assert.That(result).IsEqualTo(this.originalDateTime.ToString("Y", Thread.CurrentThread.CurrentCulture));
	}

	/// <summary>
	/// Initializes the test case.
	/// </summary>
	[Before(Test)]
	public Task InitTestCase()
	{
		this.originalCulture = CultureInfo.CurrentCulture;
		this.originalUiCulture = CultureInfo.CurrentUICulture;
		this.originalDefaultCulture = CultureInfo.DefaultThreadCurrentCulture;
		this.originalDefaultUiCulture = CultureInfo.DefaultThreadCurrentUICulture;
		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
		CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
		CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
		CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
		this.originalDateTime = new DateTime(Year, Month, Day, Hour, Minute, Second);

		return Task.CompletedTask;
	}

	/// <summary>
	/// Teardowns the test case.
	/// </summary>
	[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "This is a special case.")]
	[After(Test)]
	public Task TeardownTestCase()
	{
		if (this.originalDateTime != default)
		{
			GC.SuppressFinalize(this.originalDateTime);
		}

		if (this.originalCulture is not null)
		{
			CultureInfo.CurrentCulture = this.originalCulture;
		}

		if (this.originalUiCulture is not null)
		{
			CultureInfo.CurrentUICulture = this.originalUiCulture;
		}

		CultureInfo.DefaultThreadCurrentCulture = this.originalDefaultCulture;
		CultureInfo.DefaultThreadCurrentUICulture = this.originalDefaultUiCulture;

		return Task.CompletedTask;
	}

	private static string NormalizeWhitespace(string value) => value.Replace('\u202F', ' ');
}
