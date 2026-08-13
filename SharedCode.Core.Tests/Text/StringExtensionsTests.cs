namespace SharedCode.Tests.Text;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Text;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="StringExtensions"/>.
/// </summary>
public class StringExtensionsTests
{
	private static readonly char[] ExclamationChar = ['!'];
	private static readonly char[] ExclamationAndAt = ['!', '@'];

	[Test]
	public async Task Contains_CaseSensitive_FindsSubstring()
	{
		await Assert.That("Hello World".Contains("World", StringComparison.Ordinal)).IsTrue();
		await Assert.That("Hello World".Contains("world", StringComparison.Ordinal)).IsFalse();
	}

	[Test]
	public async Task Contains_CaseInsensitive_FindsSubstring()
	{
		await Assert.That("Hello World".Contains("world", StringComparison.OrdinalIgnoreCase)).IsTrue();
	}

	[Test]
	public async Task ContainsAny_CharacterPresent_ReturnsTrue()
	{
		await Assert.That("Hello!".ContainsAny(ExclamationChar)).IsTrue();
	}

	[Test]
	public async Task ContainsAny_CharacterNotPresent_ReturnsFalse()
	{
		await Assert.That("Hello".ContainsAny(ExclamationAndAt)).IsFalse();
	}

	[Test]
	public async Task ContainsAny_NullCharacters_ThrowsArgumentNullException()
	{
		await Assert.That(() => "Hello".ContainsAny(null!)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task In_ValueInArray_ReturnsTrue()
	{
		await Assert.That("apple".In("apple", "banana", "cherry")).IsTrue();
	}

	[Test]
	public async Task In_ValueNotInArray_ReturnsFalse()
	{
		await Assert.That("grape".In("apple", "banana", "cherry")).IsFalse();
	}

	[Test]
	public async Task In_CaseSensitive_ReturnsFalseForWrongCase()
	{
		await Assert.That("Apple".In("apple", "banana")).IsFalse();
	}

	[Test]
	public async Task IsNullOrEmpty_NullString_ReturnsTrue()
	{
		await Assert.That(((string?)null)!.IsNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNullOrEmpty_EmptyString_ReturnsTrue()
	{
		await Assert.That(string.Empty.IsNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNullOrEmpty_NonEmptyString_ReturnsFalse()
	{
		await Assert.That("hello".IsNullOrEmpty()).IsFalse();
	}

	[Test]
	public async Task IsNotNullOrEmpty_NonEmptyString_ReturnsTrue()
	{
		await Assert.That("hello".IsNotNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNotNullOrEmpty_EmptyString_ReturnsFalse()
	{
		await Assert.That(string.Empty.IsNotNullOrEmpty()).IsFalse();
	}

	[Test]
	public async Task IsNullOrWhiteSpace_WhitespaceString_ReturnsTrue()
	{
		await Assert.That("   ".IsNullOrWhiteSpace()).IsTrue();
	}

	[Test]
	public async Task IsNullOrWhiteSpace_NonWhitespaceString_ReturnsFalse()
	{
		await Assert.That("hello".IsNullOrWhiteSpace()).IsFalse();
	}

	[Test]
	public async Task IsNotNullOrWhiteSpace_NonWhitespaceString_ReturnsTrue()
	{
		await Assert.That("hello".IsNotNullOrWhiteSpace()).IsTrue();
	}

	[Test]
	public async Task IsNumeric_NumericString_ReturnsTrue()
	{
		await Assert.That("12345".IsNumeric()).IsTrue();
		await Assert.That("-100".IsNumeric()).IsTrue();
	}

	[Test]
	public async Task IsNumeric_NonNumericString_ReturnsFalse()
	{
		await Assert.That("12.34".IsNumeric()).IsFalse();
		await Assert.That("abc".IsNumeric()).IsFalse();
	}

	[Test]
	public async Task IsValidEmailAddress_ValidEmail_ReturnsTrue()
	{
		await Assert.That("user@example.com".IsValidEmailAddress()).IsTrue();
	}

	[Test]
	public async Task IsValidEmailAddress_InvalidEmail_ReturnsFalse()
	{
		await Assert.That("not-an-email".IsValidEmailAddress()).IsFalse();
		await Assert.That("@nodomain".IsValidEmailAddress()).IsFalse();
	}

	[Test]
	public async Task IsValidIPAddress_ValidIPv4_ReturnsTrue()
	{
		await Assert.That("192.168.1.1".IsValidIPAddress()).IsTrue();
	}

	[Test]
	public async Task IsValidIPAddress_InvalidIP_ReturnsFalse()
	{
		await Assert.That("999.999.999.999".IsValidIPAddress()).IsFalse();
		await Assert.That("not-an-ip".IsValidIPAddress()).IsFalse();
	}

	[Test]
	public async Task IsValidUrl_ValidUrl_ReturnsTrue()
	{
		await Assert.That("http://www.example.com".IsValidUrl()).IsTrue();
		await Assert.That("https://example.com/path?q=1".IsValidUrl()).IsTrue();
	}

	[Test]
	public async Task IsValidUrl_InvalidUrl_ReturnsFalse()
	{
		await Assert.That("not a url".IsValidUrl()).IsFalse();
	}

	[Test]
	public async Task IsValidUri_ValidUri_ReturnsTrue()
	{
		await Assert.That("http://www.example.com".IsValidUri()).IsTrue();
		await Assert.That("/relative/path".IsValidUri()).IsTrue();
	}

	[Test]
	public async Task IsDate_ValidDateString_ReturnsTrue()
	{
		await Assert.That("2023-01-15".IsDate()).IsTrue();
		await Assert.That("January 15, 2023".IsDate()).IsTrue();
	}

	[Test]
	public async Task IsDate_InvalidDateString_ReturnsFalse()
	{
		await Assert.That("not a date".IsDate()).IsFalse();
		await Assert.That(string.Empty.IsDate()).IsFalse();
	}

	[Test]
	public async Task IsGuid_ValidGuid_ReturnsTrue()
	{
		await Assert.That("a8098c1a-f86e-11da-bd1a-00112444be1e".IsGuid()).IsTrue();
	}

	[Test]
	public async Task IsGuid_InvalidGuid_ReturnsFalse()
	{
		await Assert.That("not-a-guid".IsGuid()).IsFalse();
	}

	[Test]
	public async Task IsLengthAtLeast_LongEnough_ReturnsTrue()
	{
		await Assert.That("hello".IsLengthAtLeast(5)).IsTrue();
		await Assert.That("hello world".IsLengthAtLeast(5)).IsTrue();
	}

	[Test]
	public async Task IsLengthAtLeast_TooShort_ReturnsFalse()
	{
		await Assert.That("hi".IsLengthAtLeast(5)).IsFalse();
	}

	[Test]
	public async Task NullIfEmpty_EmptyString_ReturnsNull()
	{
		await Assert.That(string.Empty.NullIfEmpty() is null).IsTrue();
	}

	[Test]
	public async Task NullIfEmpty_NonEmptyString_ReturnsString()
	{
		await Assert.That("hello".NullIfEmpty()).IsEqualTo("hello");
	}

	[Test]
	public async Task NullIfWhiteSpace_WhitespaceString_ReturnsNull()
	{
		await Assert.That("   ".NullIfWhiteSpace() is null).IsTrue();
	}

	[Test]
	public async Task NullIfWhiteSpace_NonWhitespaceString_ReturnsString()
	{
		await Assert.That("hello".NullIfWhiteSpace()).IsEqualTo("hello");
	}

	[Test]
	public async Task Left_ReturnsLeftNCharacters()
	{
		await Assert.That("Hello".Left(2)).IsEqualTo("He");
	}

	[Test]
	public async Task Left_LengthGreaterThanString_ReturnsFullString()
	{
		await Assert.That("Hi".Left(10)).IsEqualTo("Hi");
	}

	[Test]
	public async Task Right_ReturnsRightNCharacters()
	{
		await Assert.That("Hello".Right(2)).IsEqualTo("lo");
	}

	[Test]
	public async Task Right_LengthGreaterThanString_ReturnsFullString()
	{
		await Assert.That("Hi".Right(10)).IsEqualTo("Hi");
	}

	[Test]
	public async Task DefaultIfEmpty_EmptyString_ReturnsDefault()
	{
		await Assert.That(string.Empty.DefaultIfEmpty("default")).IsEqualTo("default");
	}

	[Test]
	public async Task DefaultIfEmpty_NonEmptyString_ReturnsOriginal()
	{
		await Assert.That("hello".DefaultIfEmpty("default")).IsEqualTo("hello");
	}

	[Test]
	public async Task DefaultIfEmpty_WhitespaceAndConsiderWhitespace_ReturnsDefault()
	{
		await Assert.That("   ".DefaultIfEmpty("default", considerWhiteSpaceIsEmpty: true)).IsEqualTo("default");
	}

	[Test]
	public async Task Mask_DefaultMask_MasksAllCharacters()
	{
		var result = "secret".Mask();
		await Assert.That(result).IsEqualTo("******");
	}

	[Test]
	public async Task Mask_WithMaskStyle_MasksCharacters()
	{
		var result = "secret123".Mask(MaskStyle.AlphaNumericOnly);
		await Assert.That(result is not null).IsTrue();
		await Assert.That(result!.Length).IsEqualTo(9);
	}

	[Test]
	public async Task Fill_FormatsStringWithArgument()
	{
		var result = "Hello {0}".Fill("World");
		await Assert.That(result).IsEqualTo("Hello World");
	}

	[Test]
	public async Task FillInvariant_FormatsStringWithArgument()
	{
		var result = "Value: {0}".FillInvariant(42);
		await Assert.That(result).IsEqualTo("Value: 42");
	}

	[Test]
	public async Task ToDateTime_ValidDateString_ReturnsParsedDate()
	{
		var result = "2023-01-15".ToDateTime();
		await Assert.That(result is not null).IsTrue();
		await Assert.That(result!.Value.Year).IsEqualTo(2023);
		await Assert.That(result.Value.Month).IsEqualTo(1);
		await Assert.That(result.Value.Day).IsEqualTo(15);
	}

	[Test]
	public async Task ToDateTime_InvalidDateString_ReturnsNull()
	{
		var result = "not a date".ToDateTime();
		await Assert.That(result is null).IsTrue();
	}

	[Test]
	public async Task ToDateTimeOffset_ValidDateString_ReturnsParsedDateTimeOffset()
	{
		var result = "2023-01-15T12:00:00+00:00".ToDateTimeOffset();
		await Assert.That(result is not null).IsTrue();
		await Assert.That(result!.Value.Year).IsEqualTo(2023);
	}

	[Test]
	public async Task ToDateTimeOffset_InvalidDateString_ReturnsNull()
	{
		var result = "invalid".ToDateTimeOffset();
		await Assert.That(result is null).IsTrue();
	}

	[Test]
	public async Task ToEnum_ValidEnumString_ReturnsEnumValue()
	{
		var result = "Monday".ToEnum<DayOfWeek>();
		await Assert.That(result).IsEqualTo(DayOfWeek.Monday);
	}

	[Test]
	public async Task ToEnum_NullString_ReturnsDefault()
	{
		var result = ((string?)null)!.ToEnum<DayOfWeek>();
		await Assert.That(result).IsEqualTo(default(DayOfWeek));
	}
}
