
namespace SharedCode.Security;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// The hasher class
/// </summary>
public static partial class Hasher
{
	/// <summary>
	/// Gets the hash bytes.
	/// </summary>
	/// <param name="input">The input string.</param>
	/// <param name="hash">The hash type.</param>
	/// <returns>The hash byte array.</returns>
	[SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "Computing a hash. May not need to be cryptographically safe.")]
	[SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "Computing a hash. May not need to be cryptographically safe.")]
	[SuppressMessage("Roslynator", "RCS1136:Merge switch sections with equivalent content.", Justification = "CA rules overlap here. Have to disable this or that.")]
	[SuppressMessage("Roslynator", "RCS1069:Remove unnecessary case label.", Justification = "For completeness we want to keep the extra cases.")]
	private static byte[] GetHash(string input, EHashType hash)
	{
		var inputBytes = Encoding.ASCII.GetBytes(input);

		switch (hash)
		{
#pragma warning disable SYSLIB0007 // Type or member is obsolete
			case EHashType.HMAC:
				var hmac = HMAC.Create();
				var output = hmac.ComputeHash(inputBytes);
				hmac.Dispose();
				return output;

			case EHashType.HMACMD5:
				var hmacMd5 = HMAC.Create();
				var moutputHmacMd5 = hmacMd5.ComputeHash(inputBytes);
				hmacMd5.Dispose();
				return moutputHmacMd5;

			case EHashType.HMACSHA1:
				var hmacSha1 = HMAC.Create();
				var outputHmacSha1 = hmacSha1.ComputeHash(inputBytes);
				hmacSha1.Dispose();
				return outputHmacSha1;

			case EHashType.HMACSHA256:
				var hmacSha256 = HMAC.Create();
				var outputHmacSha256 = hmacSha256.ComputeHash(inputBytes);
				hmacSha256.Dispose();
				return outputHmacSha256;

			case EHashType.HMACSHA384:
				var hmacSha384 = HMAC.Create();
				var outputHmacSha384 = hmacSha384.ComputeHash(inputBytes);
				hmacSha384.Dispose();
				return outputHmacSha384;

			case EHashType.HMACSHA512:
				var hmacSha512 = HMAC.Create();
				var outputHmacSha512 = hmacSha512.ComputeHash(inputBytes);
				hmacSha512.Dispose();
				return outputHmacSha512;
#pragma warning restore SYSLIB0007 // Type or member is obsolete

			case EHashType.MD5:
#if NET6_0_OR_GREATER
				var outputMd5 = MD5.HashData(inputBytes);
#else
				var md5 = MD5.Create();
				var outputMd5 = md5.ComputeHash(inputBytes);
				md5.Dispose();
#endif
				return outputMd5;

			case EHashType.SHA1:
#if NET6_0_OR_GREATER
				var outputSha1 = SHA1.HashData(inputBytes);
#else
				var sha1 = SHA1.Create();
				var outputSha1 = sha1.ComputeHash(inputBytes);
				sha1.Dispose();
#endif
				return outputSha1;

			case EHashType.SHA256:
#if NET6_0_OR_GREATER
				var outputSha256 = SHA256.HashData(inputBytes);
#else
				var sha256 = SHA256.Create();
				var outputSha256 = sha256.ComputeHash(inputBytes);
				sha256.Dispose();
#endif
				return outputSha256;

			case EHashType.SHA384:
#if NET6_0_OR_GREATER
				var outputSha384 = SHA384.HashData(inputBytes);
#else
				var sha384 = SHA384.Create();
				var outputSha384 = sha384.ComputeHash(inputBytes);
				sha384.Dispose();
#endif
				return outputSha384;

			case EHashType.SHA512:
#if NET6_0_OR_GREATER
				var outputSha512 = SHA512.HashData(inputBytes);
#else
				var sha512 = SHA512.Create();
				var outputSha512 = sha512.ComputeHash(inputBytes);
				sha512.Dispose();
#endif
				return outputSha512;

			case EHashType.MACTripleDES:
			case EHashType.RIPEMD160:
			default:
				return inputBytes;
		}
	}

	/// <summary>
	/// Computes the hash of the string using a specified hash algorithm
	/// </summary>
	/// <param name="input">The string to hash</param>
	/// <param name="hashType">The hash algorithm to use</param>
	/// <returns>The resulting hash or an empty string on error</returns>
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
	public static string ComputeHash(this string input, EHashType hashType)
	{
		try
		{
			var hash = GetHash(input, hashType);
			var stringBuilder = new StringBuilder();

			for (var i = 0; i < hash?.Length; i++)
			{
				_ = stringBuilder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
			}

			return stringBuilder.ToString();
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}
}
