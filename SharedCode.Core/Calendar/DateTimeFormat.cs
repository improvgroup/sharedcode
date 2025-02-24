namespace SharedCode.Calendar
{
	using Attributes;

	/// <summary>
	/// Enum DateTimeFormat
	/// </summary>
	public enum DateTimeFormat
	{
		/// <summary>
		/// The unknown specific
		/// </summary>
		[StringValue("unknown")]
		UnknownSpecific = 0,

		/// <summary>
		/// The short date
		/// </summary>
		[StringValue("d")]
		ShortDate = 1,

		/// <summary>
		/// The long date
		/// </summary>
		[StringValue("D")]
		LongDate = 2,

		/// <summary>
		/// The short time
		/// </summary>
		[StringValue("t")]
		ShortTime = 3,

		/// <summary>
		/// The long time
		/// </summary>
		[StringValue("T")]
		LongTime = 4,

		/// <summary>
		/// The full short date time
		/// </summary>
		[StringValue("f")]
		FullShortDateTime = 5,

		/// <summary>
		/// The full long date time
		/// </summary>
		[StringValue("F")]
		FullLongDateTime = 6,

		/// <summary>
		/// The general short date time
		/// </summary>
		[StringValue("g")]
		GeneralShortDateTime = 7,

		/// <summary>
		/// The general long date time
		/// </summary>
		[StringValue("G")]
		GeneralLongDateTime = 8,

		/// <summary>
		/// The month day lower case
		/// </summary>
		[StringValue("m")]
		MonthDayLowerCase = 9,

		/// <summary>
		/// The month day upper case
		/// </summary>
		[StringValue("M")]
		MonthDayUpperCase = 10,

		/// <summary>
		/// The RFC1123 lower case
		/// </summary>
		[StringValue("r")]
		Rfc1123LowerCase = 11,

		/// <summary>
		/// The RFC1123 upper case
		/// </summary>
		[StringValue("R")]
		Rfc1123UpperCase = 12,

		/// <summary>
		/// The sortable date time iso8601
		/// </summary>
		[StringValue("s")]
		SortableDateTimeIso8601 = 13,

		/// <summary>
		/// The universal sortable date time
		/// </summary>
		[StringValue("U")]
		UniversalSortableDateTime = 14,

		/// <summary>
		/// The year month lower case
		/// </summary>
		[StringValue("y")]
		YearMonthLowerCase = 15,

		/// <summary>
		/// The year month upper case
		/// </summary>
		[StringValue("Y")]
		YearMonthUpperCase = 16
	}
}
