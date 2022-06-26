// <copyright file="Hasher.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Security;
public static partial class Hasher
{
	/// <summary>
	/// Supported hash algorithms
	/// </summary>
	public enum EHashType
	{
		/// <summary>
		/// The HMAC
		/// </summary>
		HMAC = 0,

		/// <summary>
		/// The HMAC MD5 //DevSkim: ignore DS126858
		/// </summary>
		HMACMD5 = 1, // DevSkim: ignore DS126858

		/// <summary>
		/// The HMAC SHA1 //DevSkim: ignore DS126858
		/// </summary>
		HMACSHA1 = 2, // DevSkim: ignore DS126858

		/// <summary>
		/// The HMAC SHA256
		/// </summary>
		HMACSHA256 = 3,

		/// <summary>
		/// The HMAC SHA384
		/// </summary>
		HMACSHA384 = 4,

		/// <summary>
		/// The HMAC SHA512
		/// </summary>
		HMACSHA512 = 5,

		/// <summary>
		/// The MAC Triple DES
		/// </summary>
		MACTripleDES = 6,

		/// <summary>
		/// The MD5 //DevSkim: ignore DS126858
		/// </summary>
		MD5 = 7, // DevSkim: ignore DS126858

		/// <summary>
		/// The RIPEMD160 //DevSkim: ignore DS126858
		/// </summary>
		RIPEMD160 = 8, // DevSkim: ignore DS126858

		/// <summary>
		/// The SHA1 //DevSkim: ignore DS126858
		/// </summary>
		SHA1 = 9, // DevSkim: ignore DS126858

		/// <summary>
		/// The SHA256
		/// </summary>
		SHA256 = 10,

		/// <summary>
		/// The SHA384
		/// </summary>
		SHA384 = 11,

		/// <summary>
		/// The SHA512
		/// </summary>
		SHA512 = 12
	}
}
