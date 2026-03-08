namespace SharedCode.Tests.Security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Security;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="Hasher"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class HasherTests
{
	[TestMethod]
	public void ComputeHash_MD5_ReturnsNonEmptyString()
	{
		var result = "hello".ComputeHash(Hasher.EHashType.MD5);
		Assert.IsFalse(string.IsNullOrEmpty(result));
	}

	[TestMethod]
	public void ComputeHash_MD5_SameInput_ReturnsSameHash()
	{
		var hash1 = "hello world".ComputeHash(Hasher.EHashType.MD5);
		var hash2 = "hello world".ComputeHash(Hasher.EHashType.MD5);
		Assert.AreEqual(hash1, hash2);
	}

	[TestMethod]
	public void ComputeHash_MD5_DifferentInput_ReturnsDifferentHash()
	{
		var hash1 = "hello".ComputeHash(Hasher.EHashType.MD5);
		var hash2 = "world".ComputeHash(Hasher.EHashType.MD5);
		Assert.AreNotEqual(hash1, hash2);
	}

	[TestMethod]
	public void ComputeHash_SHA256_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA256);
		Assert.IsFalse(string.IsNullOrEmpty(result));
	}

	[TestMethod]
	public void ComputeHash_SHA256_KnownValue_ReturnsExpected()
	{
		// SHA256 of "test" = 9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08
		var result = "test".ComputeHash(Hasher.EHashType.SHA256);
		Assert.AreEqual("9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08", result);
	}

	[TestMethod]
	public void ComputeHash_SHA512_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA512);
		Assert.IsFalse(string.IsNullOrEmpty(result));
		Assert.AreEqual(128, result.Length);
	}

	[TestMethod]
	public void ComputeHash_SHA384_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA384);
		Assert.IsFalse(string.IsNullOrEmpty(result));
		Assert.AreEqual(96, result.Length);
	}

	[TestMethod]
	public void ComputeHash_SHA1_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA1);
		Assert.IsFalse(string.IsNullOrEmpty(result));
		Assert.AreEqual(40, result.Length);
	}

	[TestMethod]
	public void ComputeHash_MD5_EmptyString_ReturnsHash()
	{
		var result = string.Empty.ComputeHash(Hasher.EHashType.MD5);
		Assert.IsFalse(string.IsNullOrEmpty(result));
		// MD5 of empty string is d41d8cd98f00b204e9800998ecf8427e
		Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", result);
	}
}
