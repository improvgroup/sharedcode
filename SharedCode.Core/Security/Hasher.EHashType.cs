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
		HMAC,

		/// <summary>
		/// The HMAC MD5 //DevSkim: ignore DS126858
		/// </summary>
		HMACMD5, // DevSkim: ignore DS126858

		/// <summary>
		/// The HMAC SHA1 //DevSkim: ignore DS126858
		/// </summary>
		HMACSHA1, // DevSkim: ignore DS126858

		/// <summary>
		/// The HMAC SHA256
		/// </summary>
		HMACSHA256,

		/// <summary>
		/// The HMAC SHA384
		/// </summary>
		HMACSHA384,

		/// <summary>
		/// The HMAC SHA512
		/// </summary>
		HMACSHA512,

		/// <summary>
		/// The MAC Triple DES
		/// </summary>
		MACTripleDES,

		/// <summary>
		/// The MD5 //DevSkim: ignore DS126858
		/// </summary>
		MD5, // DevSkim: ignore DS126858

		/// <summary>
		/// The RIPEMD160 //DevSkim: ignore DS126858
		/// </summary>
		RIPEMD160, // DevSkim: ignore DS126858

		/// <summary>
		/// The SHA1 //DevSkim: ignore DS126858
		/// </summary>
		SHA1, // DevSkim: ignore DS126858

		/// <summary>
		/// The SHA256
		/// </summary>
		SHA256,

		/// <summary>
		/// The SHA384
		/// </summary>
		SHA384,

		/// <summary>
		/// The SHA512
		/// </summary>
		SHA512
	}
}
