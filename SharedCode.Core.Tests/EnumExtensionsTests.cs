namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="EnumExtensions"/> class.
/// </summary>
public class EnumExtensionsTests
{
	[Flags]
	private enum TestFlags
	{
		None = 0,
		A = 1,
		B = 2,
		C = 4,
	}

	[Test]
	public async Task IsSet_FlagIsSet_ReturnsTrue()
	{
		var value = TestFlags.A | TestFlags.B;
		await Assert.That(value.IsSet(TestFlags.A)).IsTrue();
		await Assert.That(value.IsSet(TestFlags.B)).IsTrue();
	}

	[Test]
	public async Task IsSet_FlagIsNotSet_ReturnsFalse()
	{
		var value = TestFlags.A | TestFlags.B;
		await Assert.That(value.IsSet(TestFlags.C)).IsFalse();
	}

	[Test]
	public async Task IsSet_NoneFlag_ReturnsFalse()
	{
		var value = TestFlags.A;
		await Assert.That(value.IsSet(TestFlags.None)).IsFalse();
	}
}
