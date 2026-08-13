namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="IntExtensions"/> class.
/// </summary>
public class IntExtensionsTests
{
	[Test]
	public async Task KB_ReturnsValueMultipliedBy1024()
	{
		await Assert.That(1.KB()).IsEqualTo(1024);
		await Assert.That(2.KB()).IsEqualTo(2048);
		await Assert.That(0.KB()).IsEqualTo(0);
	}

	[Test]
	public async Task MB_ReturnsValueInMegabytes()
	{
		await Assert.That(1.MB()).IsEqualTo(1024 * 1024);
		await Assert.That(2.MB()).IsEqualTo(2 * 1024 * 1024);
	}

	[Test]
	public async Task GB_ReturnsValueInGigabytes()
	{
		await Assert.That(1.GB()).IsEqualTo(1024 * 1024 * 1024);
	}

	[Test]
	public async Task TB_ReturnsValueInTerabytes()
	{
		await Assert.That(1.TB()).IsEqualTo(1024L * 1024 * 1024 * 1024);
	}

	[Test]
	[Arguments(2, true)]
	[Arguments(3, true)]
	[Arguments(5, true)]
	[Arguments(7, true)]
	[Arguments(11, true)]
	[Arguments(13, true)]
	[Arguments(17, true)]
	[Arguments(97, true)]
	[Arguments(1, false)]
	[Arguments(4, false)]
	[Arguments(6, false)]
	[Arguments(9, false)]
	[Arguments(15, false)]
	[Arguments(100, false)]
	public async Task IsPrime_ReturnsExpectedResult(int number, bool expected)
	{
		await Assert.That(number.IsPrime()).IsEqualTo(expected);
	}

	[Test]
	public async Task IsPrime_EvenNumberExcept2_ReturnsFalse()
	{
		await Assert.That(8.IsPrime()).IsFalse();
		await Assert.That(100.IsPrime()).IsFalse();
		await Assert.That(2.IsPrime()).IsTrue();
	}
}
