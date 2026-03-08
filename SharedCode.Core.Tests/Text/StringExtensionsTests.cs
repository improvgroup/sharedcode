namespace SharedCode.Tests.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Text;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="StringExtensions"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class StringExtensionsTests
{
	private static readonly char[] ExclamationChar = ['!'];
	private static readonly char[] ExclamationAndAt = ['!', '@'];

	[TestMethod]
	public void Contains_CaseSensitive_FindsSubstring()
	{
		Assert.IsTrue("Hello World".Contains("World", StringComparison.Ordinal));
		Assert.IsFalse("Hello World".Contains("world", StringComparison.Ordinal));
	}

	[TestMethod]
	public void Contains_CaseInsensitive_FindsSubstring()
	{
		Assert.IsTrue("Hello World".Contains("world", StringComparison.OrdinalIgnoreCase));
	}

	[TestMethod]
	public void ContainsAny_CharacterPresent_ReturnsTrue()
	{
		Assert.IsTrue("Hello!".ContainsAny(ExclamationChar));
	}

	[TestMethod]
	public void ContainsAny_CharacterNotPresent_ReturnsFalse()
	{
		Assert.IsFalse("Hello".ContainsAny(ExclamationAndAt));
	}

	[TestMethod]
	public void ContainsAny_NullCharacters_ThrowsArgumentNullException()
	{
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => "Hello".ContainsAny(null!));
	}

	[TestMethod]
	public void In_ValueInArray_ReturnsTrue()
	{
		Assert.IsTrue("apple".In("apple", "banana", "cherry"));
	}

	[TestMethod]
	public void In_ValueNotInArray_ReturnsFalse()
	{
		Assert.IsFalse("grape".In("apple", "banana", "cherry"));
	}

	[TestMethod]
	public void In_CaseSensitive_ReturnsFalseForWrongCase()
	{
		Assert.IsFalse("Apple".In("apple", "banana"));
	}

	[TestMethod]
	public void IsNullOrEmpty_NullString_ReturnsTrue()
	{
		Assert.IsTrue(((string?)null)!.IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNullOrEmpty_EmptyString_ReturnsTrue()
	{
		Assert.IsTrue(string.Empty.IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNullOrEmpty_NonEmptyString_ReturnsFalse()
	{
		Assert.IsFalse("hello".IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNotNullOrEmpty_NonEmptyString_ReturnsTrue()
	{
		Assert.IsTrue("hello".IsNotNullOrEmpty());
	}

	[TestMethod]
	public void IsNotNullOrEmpty_EmptyString_ReturnsFalse()
	{
		Assert.IsFalse(string.Empty.IsNotNullOrEmpty());
	}

	[TestMethod]
	public void IsNullOrWhiteSpace_WhitespaceString_ReturnsTrue()
	{
		Assert.IsTrue("   ".IsNullOrWhiteSpace());
	}

	[TestMethod]
	public void IsNullOrWhiteSpace_NonWhitespaceString_ReturnsFalse()
	{
		Assert.IsFalse("hello".IsNullOrWhiteSpace());
	}

	[TestMethod]
	public void IsNotNullOrWhiteSpace_NonWhitespaceString_ReturnsTrue()
	{
		Assert.IsTrue("hello".IsNotNullOrWhiteSpace());
	}

	[TestMethod]
	public void IsNumeric_NumericString_ReturnsTrue()
	{
		Assert.IsTrue("12345".IsNumeric());
		Assert.IsTrue("-100".IsNumeric());
	}

	[TestMethod]
	public void IsNumeric_NonNumericString_ReturnsFalse()
	{
		Assert.IsFalse("12.34".IsNumeric());
		Assert.IsFalse("abc".IsNumeric());
	}

	[TestMethod]
	public void IsValidEmailAddress_ValidEmail_ReturnsTrue()
	{
		Assert.IsTrue("user@example.com".IsValidEmailAddress());
	}

	[TestMethod]
	public void IsValidEmailAddress_InvalidEmail_ReturnsFalse()
	{
		Assert.IsFalse("not-an-email".IsValidEmailAddress());
		Assert.IsFalse("@nodomain".IsValidEmailAddress());
	}

	[TestMethod]
	public void IsValidIPAddress_ValidIPv4_ReturnsTrue()
	{
		Assert.IsTrue("192.168.1.1".IsValidIPAddress());
	}

	[TestMethod]
	public void IsValidIPAddress_InvalidIP_ReturnsFalse()
	{
		Assert.IsFalse("999.999.999.999".IsValidIPAddress());
		Assert.IsFalse("not-an-ip".IsValidIPAddress());
	}

	[TestMethod]
	public void IsValidUrl_ValidUrl_ReturnsTrue()
	{
		Assert.IsTrue("http://www.example.com".IsValidUrl());
		Assert.IsTrue("https://example.com/path?q=1".IsValidUrl());
	}

	[TestMethod]
	public void IsValidUrl_InvalidUrl_ReturnsFalse()
	{
		Assert.IsFalse("not a url".IsValidUrl());
	}

	[TestMethod]
	public void IsValidUri_ValidUri_ReturnsTrue()
	{
		Assert.IsTrue("http://www.example.com".IsValidUri());
		Assert.IsTrue("/relative/path".IsValidUri());
	}

	[TestMethod]
	public void IsDate_ValidDateString_ReturnsTrue()
	{
		Assert.IsTrue("2023-01-15".IsDate());
		Assert.IsTrue("January 15, 2023".IsDate());
	}

	[TestMethod]
	public void IsDate_InvalidDateString_ReturnsFalse()
	{
		Assert.IsFalse("not a date".IsDate());
		Assert.IsFalse(string.Empty.IsDate());
	}

	[TestMethod]
	public void IsGuid_ValidGuid_ReturnsTrue()
	{
		Assert.IsTrue("a8098c1a-f86e-11da-bd1a-00112444be1e".IsGuid());
	}

	[TestMethod]
	public void IsGuid_InvalidGuid_ReturnsFalse()
	{
		Assert.IsFalse("not-a-guid".IsGuid());
	}

	[TestMethod]
	public void IsLengthAtLeast_LongEnough_ReturnsTrue()
	{
		Assert.IsTrue("hello".IsLengthAtLeast(5));
		Assert.IsTrue("hello world".IsLengthAtLeast(5));
	}

	[TestMethod]
	public void IsLengthAtLeast_TooShort_ReturnsFalse()
	{
		Assert.IsFalse("hi".IsLengthAtLeast(5));
	}

	[TestMethod]
	public void NullIfEmpty_EmptyString_ReturnsNull()
	{
		Assert.IsNull(string.Empty.NullIfEmpty());
	}

	[TestMethod]
	public void NullIfEmpty_NonEmptyString_ReturnsString()
	{
		Assert.AreEqual("hello", "hello".NullIfEmpty());
	}

	[TestMethod]
	public void NullIfWhiteSpace_WhitespaceString_ReturnsNull()
	{
		Assert.IsNull("   ".NullIfWhiteSpace());
	}

	[TestMethod]
	public void NullIfWhiteSpace_NonWhitespaceString_ReturnsString()
	{
		Assert.AreEqual("hello", "hello".NullIfWhiteSpace());
	}

	[TestMethod]
	public void Left_ReturnsLeftNCharacters()
	{
		Assert.AreEqual("He", "Hello".Left(2));
	}

	[TestMethod]
	public void Left_LengthGreaterThanString_ReturnsFullString()
	{
		Assert.AreEqual("Hi", "Hi".Left(10));
	}

	[TestMethod]
	public void Right_ReturnsRightNCharacters()
	{
		Assert.AreEqual("lo", "Hello".Right(2));
	}

	[TestMethod]
	public void Right_LengthGreaterThanString_ReturnsFullString()
	{
		Assert.AreEqual("Hi", "Hi".Right(10));
	}

	[TestMethod]
	public void DefaultIfEmpty_EmptyString_ReturnsDefault()
	{
		Assert.AreEqual("default", string.Empty.DefaultIfEmpty("default"));
	}

	[TestMethod]
	public void DefaultIfEmpty_NonEmptyString_ReturnsOriginal()
	{
		Assert.AreEqual("hello", "hello".DefaultIfEmpty("default"));
	}

	[TestMethod]
	public void DefaultIfEmpty_WhitespaceAndConsiderWhitespace_ReturnsDefault()
	{
		Assert.AreEqual("default", "   ".DefaultIfEmpty("default", considerWhiteSpaceIsEmpty: true));
	}

	[TestMethod]
	public void Mask_DefaultMask_MasksAllCharacters()
	{
		var result = "secret".Mask();
		Assert.AreEqual("******", result);
	}

	[TestMethod]
	public void Mask_WithMaskStyle_MasksCharacters()
	{
		var result = "secret123".Mask(MaskStyle.AlphaNumericOnly);
		Assert.IsNotNull(result);
		Assert.AreEqual(9, result!.Length);
	}

	[TestMethod]
	public void Fill_FormatsStringWithArgument()
	{
		var result = "Hello {0}".Fill("World");
		Assert.AreEqual("Hello World", result);
	}

	[TestMethod]
	public void FillInvariant_FormatsStringWithArgument()
	{
		var result = "Value: {0}".FillInvariant(42);
		Assert.AreEqual("Value: 42", result);
	}

	[TestMethod]
	public void ToDateTime_ValidDateString_ReturnsParsedDate()
	{
		var result = "2023-01-15".ToDateTime();
		Assert.IsNotNull(result);
		Assert.AreEqual(2023, result!.Value.Year);
		Assert.AreEqual(1, result.Value.Month);
		Assert.AreEqual(15, result.Value.Day);
	}

	[TestMethod]
	public void ToDateTime_InvalidDateString_ReturnsNull()
	{
		var result = "not a date".ToDateTime();
		Assert.IsNull(result);
	}

	[TestMethod]
	public void ToDateTimeOffset_ValidDateString_ReturnsParsedDateTimeOffset()
	{
		var result = "2023-01-15T12:00:00+00:00".ToDateTimeOffset();
		Assert.IsNotNull(result);
		Assert.AreEqual(2023, result!.Value.Year);
	}

	[TestMethod]
	public void ToDateTimeOffset_InvalidDateString_ReturnsNull()
	{
		var result = "invalid".ToDateTimeOffset();
		Assert.IsNull(result);
	}

	[TestMethod]
	public void ToEnum_ValidEnumString_ReturnsEnumValue()
	{
		var result = "Monday".ToEnum<DayOfWeek>();
		Assert.AreEqual(DayOfWeek.Monday, result);
	}

	[TestMethod]
	public void ToEnum_NullString_ReturnsDefault()
	{
		var result = ((string?)null)!.ToEnum<DayOfWeek>();
		Assert.AreEqual(default(DayOfWeek), result);
	}
}
