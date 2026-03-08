namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="EnumExtensions"/> class.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
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

	[TestMethod]
	public void IsSet_FlagIsSet_ReturnsTrue()
	{
		var value = TestFlags.A | TestFlags.B;
		Assert.IsTrue(value.IsSet(TestFlags.A));
		Assert.IsTrue(value.IsSet(TestFlags.B));
	}

	[TestMethod]
	public void IsSet_FlagIsNotSet_ReturnsFalse()
	{
		var value = TestFlags.A | TestFlags.B;
		Assert.IsFalse(value.IsSet(TestFlags.C));
	}

	[TestMethod]
	public void IsSet_NoneFlag_ReturnsFalse()
	{
		var value = TestFlags.A;
		Assert.IsFalse(value.IsSet(TestFlags.None));
	}
}
