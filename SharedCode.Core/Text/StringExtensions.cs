using System.ComponentModel;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SharedCode.Text;
/// <summary>
/// The string extensions class.
/// </summary>
public static partial class StringExtensions
{
	/// <summary>
	/// Default masking character used in a mask.
	/// </summary>
	public const char DefaultMaskCharacter = '*';

	/// <summary>
	/// The domain regular expression
	/// </summary>
	private static readonly Regex DomainRegex = new(@"(((?<scheme>http(s)?):\/\/)?([\w-]+?\.\w+)+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\,]*)?)", RegexOptions.Compiled | RegexOptions.Multiline);

	/// <summary>
	/// The email address regular expression
	/// </summary>
	private static readonly Regex EmailAddressRegex = new("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

	private static readonly string[] separator = new[] { "-" };

	/// <summary>
	/// The slug regular expression
	/// </summary>
	private static readonly Regex SlugRegEx = new(@"\s+");

	/// <summary>
	/// The second slug regular expression
	/// </summary>
	private static readonly Regex SlugRegEx2 = new(@"\s{2,}|[^\w]", RegexOptions.ECMAScript);

	/// <summary>
	/// The URL regular expression
	/// </summary>
	private static readonly Regex UrlRegEx = new(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);

	/// <summary>
	/// The valid IP address (v4) regular expression
	/// </summary>
	private static readonly Regex ValidIpRegEx = new(@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
	/// <summary>
	/// Returns a value indicating whether the specified <see cref="string" /> object occurs within
	/// the <paramref name="this" /> string. A parameter specifies the type of search to use for the
	/// specified string.
	/// </summary>
	/// <param name="this">The string to search in</param>
	/// <param name="value">The string to seek</param>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies the rules for the search
	/// </param>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="this" /> or <paramref name="value" /> is <c>null</c>
	/// </exception>
	/// <exception cref="ArgumentException">
	/// <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value
	/// </exception>
	/// <returns>
	/// Returns <c>true</c> if the <paramref name="value" /> parameter occurs within the <paramref
	/// name="this" /> parameter, or if <paramref name="value" /> is the empty string ( <c>""</c>);
	/// otherwise, <c>false</c>.
	/// </returns>
	/// <remarks>
	/// The <paramref name="comparisonType" /> parameter specifies to search for the value parameter
	/// using the current or invariant culture, using a case-sensitive or case-insensitive search,
	/// and using word or ordinal comparison rules.
	/// </remarks>
	public static bool Contains(this string @this, string value, StringComparison comparisonType) => @this?.IndexOf(value, comparisonType) > -1;

	/// <summary>
	/// Determines whether the specified string contains any of the specified characters.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <param name="characters">The characters to check.</param>
	/// <returns>
	/// A value indicating whether the specified string contains any of the specified characters.
	/// </returns>
	/// <exception cref="ArgumentNullException">characters</exception>
	public static bool ContainsAny(this string @this, IEnumerable<char> characters)
	{
		_ = characters ?? throw new ArgumentNullException(nameof(characters));

		return characters.Any(character => @this?.Contains(character.ToString(), StringComparison.CurrentCulture) ?? false);
	}

	/// <summary>
	/// Converts the input to the specified type.
	/// </summary>
	/// <typeparam name="T">The type to convert to.</typeparam>
	/// <param name="this">The input string.</param>
	/// <returns>The output.</returns>
	public static T? ConvertTo<T>(this string @this)
	{
		try
		{
			var converter = TypeDescriptor.GetConverter(typeof(T));
			return converter is null ? default : (T?)converter.ConvertFromString(@this);
		}
		catch (NotSupportedException)
		{
			return default;
		}
	}

	/// <summary>
	/// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
	/// </summary>
	/// <param name="this">String that must be decrypted.</param>
	/// <param name="key">Decryptionkey.</param>
	/// <returns>The decrypted string or null if decryption failed.</returns>
	/// <exception cref="ArgumentException">Occurs when this or key is empty.</exception>
	/// <exception cref="ArgumentNullException">this or key</exception>
	[SupportedOSPlatform("windows")]
	public static string Decrypt(this string @this, string key)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = key ?? throw new ArgumentNullException(nameof(key));
		Contract.Ensures(Contract.Result<string>() is not null);

		if (string.IsNullOrEmpty(@this))
		{
			throw new ArgumentException("An empty string value cannot be encrypted.");
		}

		if (string.IsNullOrEmpty(key))
		{
			throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
		}

		using var rsa = new RSACryptoServiceProvider(new CspParameters { KeyContainerName = key }) { PersistKeyInCsp = true };
		var decryptArray = @this.Split(separator, StringSplitOptions.None);
		var decryptByteArray = Array.ConvertAll(decryptArray, s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber, CultureInfo.CurrentCulture)));

		var bytes = rsa.Decrypt(decryptByteArray, fOAEP: true);

		return Encoding.UTF8.GetString(bytes);
	}

	/// <summary>
	/// Returns this string or the specified default value if the string is empty.
	/// </summary>
	/// <param name="this">The string.</param>
	/// <param name="defaultValue">The default value.</param>
	/// <param name="considerWhiteSpaceIsEmpty">
	/// if set to <c>true</c> then consider white space as empty.
	/// </param>
	/// <returns>The output string.</returns>
	public static string DefaultIfEmpty(this string @this, string defaultValue, bool considerWhiteSpaceIsEmpty = false) =>
		(considerWhiteSpaceIsEmpty ? string.IsNullOrWhiteSpace(@this) : string.IsNullOrEmpty(@this)) ? defaultValue : @this;

	/// <summary>
	/// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
	/// </summary>
	/// <param name="this">String that must be encrypted.</param>
	/// <param name="key">Encryptionkey.</param>
	/// <returns>A string representing a byte array separated by a minus sign.</returns>
	/// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
	/// <exception cref="ArgumentNullException">this or key</exception>
#if NET5_0_OR_GREATER
	[SupportedOSPlatform("windows")]
#endif
	public static string Encrypt(this string @this, string key)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = key ?? throw new ArgumentNullException(nameof(key));
		Contract.Ensures(Contract.Result<string>() is not null);

		if (string.IsNullOrEmpty(@this))
		{
			throw new ArgumentException("An empty string value cannot be encrypted.");
		}

		if (string.IsNullOrEmpty(key))
		{
			throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
		}

		using var rsa = new RSACryptoServiceProvider(new CspParameters { KeyContainerName = key }) { PersistKeyInCsp = true };
		var bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(@this), fOAEP: true);

		return BitConverter.ToString(bytes);
	}

	/// <summary>
	/// Fills the specified format string using the specified argument.
	/// </summary>
	/// <param name="this">The format.</param>
	/// <param name="arg">The argument.</param>
	/// <returns>The formatted string.</returns>
	public static string? Fill(this string @this, object? arg) => string.Format(CultureInfo.CurrentCulture, @this, arg);

	/// <summary>
	/// Fills the specified format string using the specified arguments.
	/// </summary>
	/// <param name="this">The format.</param>
	/// <param name="args">The arguments.</param>
	/// <returns>The formatted string.</returns>
	public static string? Fill(this string @this, params object?[] args) => string.Format(CultureInfo.CurrentCulture, @this, args);

	/// <summary>
	/// Fills the specified format string with the provider using the specified argument.
	/// </summary>
	/// <param name="this">The format.</param>
	/// <param name="provider">The format provider.</param>
	/// <param name="arg">The argument.</param>
	/// <returns>The formatted string.</returns>
	public static string? Fill(this string @this, IFormatProvider provider, object? arg) => string.Format(provider, @this, arg);

	/// <summary>
	/// Fills the specified format string with the provider using the specified arguments.
	/// </summary>
	/// <param name="this">The format.</param>
	/// <param name="provider">The format provider.</param>
	/// <param name="args">The arguments.</param>
	/// <returns>The formatted string.</returns>
	public static string? Fill(this string @this, IFormatProvider provider, params object?[] args) => string.Format(provider, @this, args);

	/// <summary>
	/// Fills the specified format string with the invariant culture provider using the specified argument.
	/// </summary>
	/// <param name="this">The format.</param>
	/// <param name="arg">The argument.</param>
	/// <returns>The formatted string.</returns>
	public static string? FillInvariant(this string @this, object? arg) => string.Format(CultureInfo.InvariantCulture, @this, arg);

	/// <summary>
	/// Fills the specified format string with the invariant culture provider using the specified arguments.
	/// </summary>
	/// <param name="this">The format.</param>
	/// <param name="args">The arguments.</param>
	/// <returns>The formatted string.</returns>
	public static string? FillInvariant(this string @this, params object?[] args) => string.Format(CultureInfo.InvariantCulture, @this, args);

	/// <summary>
	/// Replaces the format item in a specified System.String with the text equivalent of the value
	/// of a specified System.Object instance.
	/// </summary>
	/// <param name="this">A composite format string</param>
	/// <param name="arg0">An System.Object to format</param>
	/// <returns>
	/// A copy of format in which the first format item has been replaced by the System.String
	/// equivalent of arg0
	/// </returns>
	public static string Format(this string @this, object arg0) => string.Format(CultureInfo.CurrentCulture, @this, arg0);

	/// <summary>
	/// Replaces the format item in a specified <see cref="string" /> with the text equivalent of
	/// the value of a specified <see cref="object" /> instance.
	/// </summary>
	/// <param name="this">A composite format string.</param>
	/// <param name="args">An <see cref="object" /> array containing zero or more objects to format.</param>
	/// <returns>
	/// A copy of format in which the format items have been replaced by the <see cref="string" />
	/// equivalent of the corresponding instances of System.Object in args.
	/// </returns>
	public static string? Format(this string? @this, params object[] args) => @this is null ? null : string.Format(CultureInfo.CurrentCulture, @this, args);

	/// <summary>
	/// Formats the string according to the specified mask
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
	/// <returns>The formatted string</returns>
	/// <exception cref="ArgumentNullException">mask</exception>
	public static string FormatWithMask(this string @this, string mask)
	{
		_ = mask ?? throw new ArgumentNullException(nameof(mask));
		if (string.IsNullOrEmpty(@this))
		{
			return @this;
		}

		var output = string.Empty;
		var index = 0;
		var builder = new StringBuilder().Append(output);
		foreach (var m in mask)
		{
			if (m == '#')
			{
				if (index < @this.Length)
				{
					_ = builder.Append(@this[index]);
					index++;
				}
			}
			else
			{
				_ = builder.Append(m);
			}
		}

		return builder.ToString();
	}

	/// <summary>
	/// Gets the enumeration value description.
	/// </summary>
	/// <typeparam name="T">The type of the enumeration.</typeparam>
	/// <param name="value">The enumeration value.</param>
	/// <returns>The enumeration value description.</returns>
	public static string GetEnumDescription<T>(string value)
	{
		Contract.Ensures(Contract.Result<string>() is not null);

		var type = typeof(T);
		var name = Array.Find(Enum.GetNames(type), f => f.Equals(value, StringComparison.OrdinalIgnoreCase));
		if (name is null)
		{
			return string.Empty;
		}

		var field = type.GetField(name);
		var customAttribute = field?.GetCustomAttributes<DescriptionAttribute>(inherit: false).ToList();
		return customAttribute?.Count > 0 ? customAttribute[0].Description : name;
	}

	/// <summary>
	/// Checks string object's value to array of string values
	/// </summary>
	/// <param name="this">The input value.</param>
	/// <param name="stringValues">Array of string values to compare</param>
	/// <returns>Return true if any string value matches</returns>
	public static bool In(this string @this, params string[] stringValues) => stringValues.Any(otherValue => string.CompareOrdinal(@this, otherValue) == 0);

	/// <summary>
	/// Determines whether the input string can be converted to the target type.
	/// </summary>
	/// <typeparam name="T">The target type.</typeparam>
	/// <param name="this">The input string.</param>
	/// <returns>
	/// A value indicating whether the input string was successfully converted to the target type.
	/// </returns>
	public static bool Is<T>(this string @this)
	{
		try
		{
			var converter = TypeDescriptor.GetConverter(typeof(T));
			if (converter is null)
			{
				return false;
			}

			var output = (T?)converter.ConvertFromString(@this);
			return output is not null;
		}
		catch (NotSupportedException)
		{
			return false;
		}
	}

	/// <summary>
	/// Determines whether the input string can be converted to the target type.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <param name="targetType">The target type.</param>
	/// <returns>
	/// A value indicating whether the input string was successfully converted to the target type.
	/// </returns>
	/// <exception cref="ArgumentNullException">targetType</exception>
	public static bool Is(this string @this, Type targetType)
	{
		_ = targetType ?? throw new ArgumentNullException(nameof(targetType));

		try
		{
			var converter = TypeDescriptor.GetConverter(targetType);
			if (converter is null)
			{
				return false;
			}

			var output = converter.ConvertFromString(@this);
			return output is not null;
		}
		catch (NotSupportedException)
		{
			return false;
		}
	}

	/// <summary>
	/// Determines whether the specified input string is a date.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>Returns <c>true</c> if the specified input string is a date; otherwise, <c>false</c>.</returns>
	public static bool IsDate(this string @this) => !string.IsNullOrEmpty(@this) && DateTime.TryParse(@this, out var _);

	/// <summary>
	/// Converts the string representation of a Guid to its Guid equivalent. A return value
	/// indicates whether the operation succeeded.
	/// </summary>
	/// <param name="this">A string containing a Guid to convert.</param>
	/// <returns>
	/// When this method returns, contains the Guid value equivalent to the Guid contained in
	/// <paramref name="this" />, if the conversion succeeded, or <see cref="Guid.Empty" /> if the
	/// conversion failed. The conversion fails if the <paramref name="this" /> parameter is a <see
	/// langword="null" /> reference ( <see langword="Nothing" /> in Visual Basic), or is not of the
	/// correct format. <c>true</c> if <paramref name="this" /> was converted successfully;
	/// otherwise, <c>false</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown if <pararef name="s" /> is <see langword="null" />.
	/// </exception>
	/// <remarks>
	/// Original code at <seealso
	/// href="https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=94072&amp;wa=wsignin1.0#tabs" />
	/// </remarks>
	public static bool IsGuid(this string @this)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		var format = new Regex(
			"^[A-Fa-f0-9]{32}$|" + "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|"
			+ "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
		var match = format.Match(@this);

		return match.Success;
	}

	/// <summary>
	/// Returns true if the string is non-null and at least the specified number of characters.
	/// </summary>
	/// <param name="this">The string to check.</param>
	/// <param name="length">The minimum length.</param>
	/// <returns>True if string is non-null and at least the length specified.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// throws ArgumentOutOfRangeException if length is not a non-negative number.
	/// </exception>
	public static bool IsLengthAtLeast(this string @this, int length) =>
		length < 0
			? throw new ArgumentOutOfRangeException(nameof(length), length, "The length must be a non-negative number.")
			: @this is not null && @this.Length >= length;

	/// <summary>
	/// Determines whether the specified input string is not null or empty.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns><c>true</c> if the specified input string is not null or empty; otherwise, <c>false</c>.</returns>
	public static bool IsNotNullOrEmpty(this string @this) => !string.IsNullOrEmpty(@this);

	/// <summary>
	/// Determines whether the specified input string is not null or white space.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>
	/// <c>true</c> if the specified input string is not null or white space; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsNotNullOrWhiteSpace(this string @this) => !string.IsNullOrWhiteSpace(@this);

	/// <summary>
	/// Determines whether the specified input string is null or empty.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns><c>true</c> if the specified input string is null or empty; otherwise, <c>false</c>.</returns>
	public static bool IsNullOrEmpty(this string @this) => string.IsNullOrEmpty(@this);

	/// <summary>
	/// Determines whether the specified input string is null or white space.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>
	/// <c>true</c> if the specified input string is null or white space; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsNullOrWhiteSpace(this string @this) => string.IsNullOrWhiteSpace(@this);

	/// <summary>
	/// Determines whether the specified string is numeric.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>Returns <c>true</c> if the specified string is numeric; otherwise, <c>false</c>.</returns>
	public static bool IsNumeric(this string @this) => long.TryParse(@this, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out _);

	/// <summary>
	/// Determines whether the input string is a valid email address.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>
	/// Returns <c>true</c> if the input string is a valid email address; otherwise, <c>false</c>.
	/// </returns>
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "")]
	public static bool IsValidEmailAddress(this string @this)
	{
		if (string.IsNullOrWhiteSpace(@this))
		{
			return false;
		}

		try
		{
			var addr = new MailAddress(@this);
			return true;
		}
		catch (Exception)
		{
			// No logging is needed
			return false;
		}
	}

	/// <summary>
	/// Determines whether the specified string is a valid email address.
	/// </summary>
	/// <param name="this">The s.</param>
	/// <returns><c>true</c> if the specified string is a valid email address; otherwise, <c>false</c>.</returns>
	public static bool IsValidEmailAddress2(this string @this) => EmailAddressRegex.IsMatch(@this);

	/// <summary>
	/// Determines whether the specified input string is a valid IP address.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>
	/// <c>true</c> if the specified input string is a valid IP address; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsValidIPAddress(this string @this) => ValidIpRegEx.IsMatch(@this);

	/// <summary>
	/// Determines whether the input text is a valid URI.
	/// </summary>
	/// <param name="this">The input text.</param>
	/// <returns><c>true</c> if the input text is a valid URI; otherwise, <c>false</c>.</returns>
	public static bool IsValidUri(this string @this) => Uri.TryCreate(@this, UriKind.RelativeOrAbsolute, out var _);

	/// <summary>
	/// Determines whether the input text is a valid URL.
	/// </summary>
	/// <param name="this">The input text.</param>
	/// <returns><c>true</c> if the input text is a valid URL; otherwise, <c>false</c>.</returns>
	public static bool IsValidUrl(this string @this) => UrlRegEx.IsMatch(@this);

	/// <summary>
	/// Returns characters from left of the value of specified length
	/// </summary>
	/// <param name="this">The string value.</param>
	/// <param name="length">The maximum number of charaters to return.</param>
	/// <returns>The string from left.</returns>
#if NET5_0_OR_GREATER
	public static string? Left(this string? @this, int length = 0) => @this is not null && @this.Length > length ? @this[..length] : @this;
#else
	public static string? Left(this string? @this, int length = 0) => @this is not null && @this.Length > length ? @this.Substring(0, length) : @this;
#endif

	/// <summary>
	/// Takes a string of text and replaces text matching a link pattern to a hyperlink.
	/// </summary>
	/// <param name="this">The input text.</param>
	/// <param name="target">The link target.</param>
	/// <returns>The output.</returns>
	/// <exception cref="ArgumentNullException">target</exception>
	public static string Linkify(this string @this, string target = "_self")
	{
		_ = target ?? throw new ArgumentNullException(nameof(target));
		return DomainRegex.Replace(
			@this,
			match =>
			{
				var link = match.ToString();
				var scheme = match.Groups["scheme"].Value == "https" ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

				var url = new UriBuilder(link) { Scheme = scheme }.Uri.ToString();

				return $@"<a href=""{url}"" target=""{target}"">{link}</a>";
			}
		);
	}

	/// <summary>
	/// Mask the source string with the mask char except for the last exposed digits.
	/// </summary>
	/// <param name="this">Original string to mask.</param>
	/// <param name="maskChar">The character to use to mask the source.</param>
	/// <param name="numExposed">Number of characters exposed in masked value.</param>
	/// <param name="style">The masking style to use (all characters or just alpha-nums).</param>
	/// <returns>The masked account number.</returns>
	public static string? Mask(this string @this, char maskChar, int numExposed = 0, MaskStyle style = MaskStyle.All)
	{
		if (@this is null)
		{
			return @this;
		}

		if (!@this.IsLengthAtLeast(numExposed))
		{
			return @this;
		}

		var builder = new StringBuilder(@this.Length);
		var index = @this.Length - numExposed;

		if (style == MaskStyle.AlphaNumericOnly)
		{
			CreateAlphaNumMask(builder, @this, maskChar, index);
		}
		else
		{
			_ = builder.Append(maskChar, index);
		}

#if NET5_0_OR_GREATER
		_ = builder.Append(@this.AsSpan(index));
#else
		_ = builder.Append(@this, index, @this.Length - index);
#endif

		return builder.ToString();
	}

	/// <summary>
	/// Mask the source string with the default mask char.
	/// </summary>
	/// <param name="this">Original string to mask.</param>
	/// <returns>The masked account number.</returns>
	public static string? Mask(this string @this) => @this.Mask(DefaultMaskCharacter);

	/// <summary>
	/// Mask the source string with the mask char.
	/// </summary>
	/// <param name="this">Original string to mask.</param>
	/// <param name="maskChar">The character to use to mask the source.</param>
	/// <param name="style">The masking style to use (all characters or just alpha-nums).</param>
	/// <returns>The masked account number.</returns>
	public static string? Mask(this string @this, char maskChar, MaskStyle style) => @this.Mask(maskChar, 0, style);

	/// <summary>
	/// Mask the source string with the default mask char except for the last exposed digits.
	/// </summary>
	/// <param name="this">Original string to mask.</param>
	/// <param name="numExposed">Number of characters exposed in masked value.</param>
	/// <param name="style">The masking style to use (all characters or just alpha-nums).</param>
	/// <returns>The masked account number.</returns>
	public static string? Mask(this string @this, int numExposed, MaskStyle style) => @this.Mask(DefaultMaskCharacter, numExposed, style);

	/// <summary>
	/// Mask the source string with the default mask char.
	/// </summary>
	/// <param name="this">Original string to mask.</param>
	/// <param name="style">The masking style to use (all characters or just alpha-nums).</param>
	/// <returns>The masked account number.</returns>
	public static string? Mask(this string @this, MaskStyle style) => @this.Mask(DefaultMaskCharacter, 0, style);

	/// <summary>
	/// Returns the string with the specified value or null if the value is empty.
	/// </summary>
	/// <param name="this">The string value.</param>
	/// <returns>The result.</returns>
	public static string? NullIfEmpty(this string @this) => string.IsNullOrEmpty(@this) ? default : @this;

	/// <summary>
	/// Returns the string with the specified value or null if the value is white space.
	/// </summary>
	/// <param name="this">The string value.</param>
	/// <returns>The result.</returns>
	public static string? NullIfWhiteSpace(this string @this) => string.IsNullOrWhiteSpace(@this) ? default : @this;

	/// <summary>
	/// Parses the specified value.
	/// </summary>
	/// <typeparam name="T">The type to parse the string into.</typeparam>
	/// <param name="this">The string value.</param>
	/// <returns>The parsed output.</returns>
	public static T? Parse<T>(this string @this)
	{
		// Get default value for type so if string is empty then we can return default value.
		var result = default(T);
		if (string.IsNullOrEmpty(@this))
		{
			return result;
		}

		// We are not going to handle exceptions here. If you need SafeParse then you should create
		// another method specially for that.
		var tc = TypeDescriptor.GetConverter(typeof(T));
		return (T?)tc.ConvertFrom(@this);
	}

	/// <summary>
	/// Returns characters from right of the value of the specified length
	/// </summary>
	/// <param name="this">The string value.</param>
	/// <param name="length">The maximum number of charaters to return.</param>
	/// <returns>The string from right.</returns>
#if NET5_0_OR_GREATER
	public static string? Right(this string? @this, int length = 0) => @this is not null && @this.Length > length ? @this[^length..] : @this;
#else
	public static string? Right(this string? @this, int length = 0) => @this is not null && @this.Length > length ? @this.Substring(@this.Length - length) : @this;
#endif

	/// <summary>
	/// Strips the HTML from the input string.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>The output string.</returns>
	public static string? StripHtml(this string @this)
	{
		if (@this is null)
		{
			return @this;
		}

		// Will this simple expression replace all tags???
#if NET8_0_OR_GREATER
		var tagsExpression = HtmlTagRegex();
#else
		var tagsExpression = new Regex("</?.+?>");
#endif
		return tagsExpression.Replace(@this, " ");
	}

	/// <summary>
	/// Converts the specified input string to a date time.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>The date time or null if conversion failed.</returns>
	public static DateTime? ToDateTime(this string @this) => DateTime.TryParse(@this, out var result) ? result : new DateTime?();

	/// <summary>
	/// Converts the specified input string to a date time offset.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>The date time offset or null if conversion failed.</returns>
	public static DateTimeOffset? ToDateTimeOffset(this string @this) => DateTimeOffset.TryParse(@this, out var result) ? result : new DateTimeOffset?();

	/// <summary>
	/// Converts the string value to the specified enumeration type.
	/// </summary>
	/// <typeparam name="T">The type of enumeration.</typeparam>
	/// <param name="this">The string value to convert.</param>
	/// <returns>Returns enumeration value.</returns>
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
	public static T ToEnum<T>(this string @this) where T : struct => @this is null ? default : Enum.Parse<T>(@this, ignoreCase: true);
#else
	public static T ToEnum<T>(this string @this) where T : struct => @this is null ? default : (T)Enum.Parse(typeof(T), @this, ignoreCase: true);
#endif

	/// <summary>
	/// متدی برای تبدیل اعداد انگلیسی به فارسی
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>The equivalent persion number.</returns>
	public static string? ToPersianNumber(this string @this)
	{
		if (@this is null)
		{
			return null;
		}

		if (@this.Trim()?.Length == 0)
		{
			return string.Empty;
		}

		//۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
		return @this.Replace("0", "۰", StringComparison.Ordinal)
			.Replace("1", "۱", StringComparison.Ordinal)
			.Replace("2", "۲", StringComparison.Ordinal)
			.Replace("3", "۳", StringComparison.Ordinal)
			.Replace("4", "۴", StringComparison.Ordinal)
			.Replace("5", "۵", StringComparison.Ordinal)
			.Replace("6", "۶", StringComparison.Ordinal)
			.Replace("7", "۷", StringComparison.Ordinal)
			.Replace("8", "۸", StringComparison.Ordinal)
			.Replace("9", "۹", StringComparison.Ordinal);
#else
		return @this.Replace("0", "۰")
			.Replace("1", "۱")
			.Replace("2", "۲")
			.Replace("3", "۳")
			.Replace("4", "۴")
			.Replace("5", "۵")
			.Replace("6", "۶")
			.Replace("7", "۷")
			.Replace("8", "۸")
			.Replace("9", "۹");
#endif
	}

	/// <summary>
	/// Changes the input string to proper case.
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>The proper cased string.</returns>
	public static string ToProperCase(this string @this) => Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(@this);

	/// <summary>
	/// Converts a string into a "SecureString"
	/// </summary>
	/// <param name="this">The input string.</param>
	/// <returns>The secure string.</returns>
	public static SecureString ToSecureString(this string @this)
	{
		Contract.Ensures(Contract.Result<SecureString>() is not null);

		var secureString = new SecureString();

		if (@this is not null)
		{
			foreach (var character in @this)
			{
				secureString.AppendChar(character);
			}
		}

		return secureString;
	}

	/// <summary>
	/// Converts the specified text to a slug for friendly URLs.
	/// </summary>
	/// <param name="this">The text input.</param>
	/// <returns>The slug.</returns>
	/// <exception cref="ArgumentNullException">@this</exception>
	[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "In this use case we want lower case.")]
	public static string ToSlug(this string @this)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		Contract.Ensures(Contract.Result<string>() is not null);

		var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(@this);
		var value = SlugRegEx.Replace(SlugRegEx2.Replace(Encoding.ASCII.GetString(bytes), " ").Trim(), "_");

		return value.ToLowerInvariant();
	}

	/// <summary>
	/// Converts the specified string to title case.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <returns>The string.</returns>
	public static string ToTitleCase(this string @this) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@this);

	/// <summary>
	/// Converts the string to title case using the specified culture information.
	/// </summary>
	/// <param name="this">The value.</param>
	/// <param name="cultureInfo">The culture information.</param>
	/// <returns>string.</returns>
	/// <exception cref="ArgumentNullException">cultureInfo</exception>
	public static string ToTitleCase(this string @this, CultureInfo cultureInfo) =>
		cultureInfo?.TextInfo.ToTitleCase(@this) ??
		throw new ArgumentNullException(nameof(cultureInfo));

	/// <summary>
	/// Truncates the string to a specified length and replace the truncated to a ...
	/// </summary>
	/// <param name="this">The string that will be truncated.</param>
	/// <param name="maxLength">
	/// The total length of characters to maintain before the truncate happens.
	/// </param>
	/// <param name="suffix">The suffix string.</param>
	/// <returns>The truncated string.</returns>
	/// <exception cref="ArgumentNullException">suffix</exception>
	public static string? Truncate(this string @this, int maxLength, string suffix = "...")
	{
		_ = suffix ?? throw new ArgumentNullException(nameof(suffix));

		if (@this is null)
		{
			return null;
		}

		// replaces the truncated string to a ...
		var truncatedString = @this;

		if (maxLength <= 0)
		{
			return truncatedString;
		}

		var strLength = maxLength - suffix.Length;

		if (strLength <= 0)
		{
			return truncatedString;
		}

		if (@this.Length <= maxLength)
		{
			return truncatedString;
		}

		truncatedString = @this.Substring(0, strLength);
		truncatedString = truncatedString.TrimEnd();
		truncatedString += suffix;

		return truncatedString;
	}

	/// <summary>
	/// Upper case the first letter in the string.
	/// </summary>
	/// <param name="this">The string value.</param>
	/// <returns>The upper cased string.</returns>
	public static string? UppercaseFirstLetter(this string @this)
	{
		if (@this is null)
		{
			return @this;
		}

		if (@this.Length > 0)
		{
			var array = @this.ToCharArray();
			array[0] = char.ToUpper(array[0], CultureInfo.CurrentCulture);
			return new string(array);
		}

		return @this;
	}

	/// <summary>
	/// Returns the string with the specified value or an empty string if value is null.
	/// </summary>
	/// <param name="this">The string value.</param>
	/// <returns>The result.</returns>
	public static string ValueOrEmpty(this string @this) => @this ?? string.Empty;

	/// <summary>
	/// Masks all characters for the specified length.
	/// </summary>
	/// <param name="buffer">String builder to store result in.</param>
	/// <param name="source">The source string to pull non-alpha numeric characters.</param>
	/// <param name="mask">Masking character to use.</param>
	/// <param name="length">Length of the mask.</param>
	/// <exception cref="ArgumentNullException">buffer</exception>
	private static void CreateAlphaNumMask(StringBuilder buffer, string source, char mask, int length)
	{
		_ = buffer ?? throw new ArgumentNullException(nameof(buffer));

		if (source is null)
		{
			return;
		}

		for (var i = 0; i < length; i++)
		{
			_ = buffer.Append(char.IsLetterOrDigit(source[i]) ? mask : source[i]);
		}
	}

#if NET8_0_OR_GREATER
	[GeneratedRegex("</?.+?>")]
	private static partial Regex HtmlTagRegex();
#endif
}
