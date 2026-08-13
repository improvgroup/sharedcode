namespace SharedCode.Tests.Security;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Security;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="Hasher"/>.
/// </summary>
public class HasherTests
{
	[Test]
	public async Task ComputeHash_MD5_ReturnsNonEmptyString()
	{
		var result = "hello".ComputeHash(Hasher.EHashType.MD5);
		await Assert.That(string.IsNullOrEmpty(result)).IsFalse();
	}

	[Test]
	public async Task ComputeHash_MD5_SameInput_ReturnsSameHash()
	{
		var hash1 = "hello world".ComputeHash(Hasher.EHashType.MD5);
		var hash2 = "hello world".ComputeHash(Hasher.EHashType.MD5);
		await Assert.That(hash2).IsEqualTo(hash1);
	}

	[Test]
	public async Task ComputeHash_MD5_DifferentInput_ReturnsDifferentHash()
	{
		var hash1 = "hello".ComputeHash(Hasher.EHashType.MD5);
		var hash2 = "world".ComputeHash(Hasher.EHashType.MD5);
		await Assert.That(hash2).IsNotEqualTo(hash1);
	}

	[Test]
	public async Task ComputeHash_SHA256_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA256);
		await Assert.That(string.IsNullOrEmpty(result)).IsFalse();
	}

	[Test]
	public async Task ComputeHash_SHA256_KnownValue_ReturnsExpected()
	{
		// SHA256 of "test" = 9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08
		var result = "test".ComputeHash(Hasher.EHashType.SHA256);
		await Assert.That(result).IsEqualTo("9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08");
	}

	[Test]
	public async Task ComputeHash_SHA512_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA512);
		await Assert.That(string.IsNullOrEmpty(result)).IsFalse();
		await Assert.That(result.Length).IsEqualTo(128);
	}

	[Test]
	public async Task ComputeHash_SHA384_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA384);
		await Assert.That(string.IsNullOrEmpty(result)).IsFalse();
		await Assert.That(result.Length).IsEqualTo(96);
	}

	[Test]
	public async Task ComputeHash_SHA1_ReturnsNonEmptyString()
	{
		var result = "test".ComputeHash(Hasher.EHashType.SHA1);
		await Assert.That(string.IsNullOrEmpty(result)).IsFalse();
		await Assert.That(result.Length).IsEqualTo(40);
	}

	[Test]
	public async Task ComputeHash_MD5_EmptyString_ReturnsHash()
	{
		var result = string.Empty.ComputeHash(Hasher.EHashType.MD5);
		await Assert.That(string.IsNullOrEmpty(result)).IsFalse();
		// MD5 of empty string is d41d8cd98f00b204e9800998ecf8427e
		await Assert.That(result).IsEqualTo("d41d8cd98f00b204e9800998ecf8427e");
	}
}
