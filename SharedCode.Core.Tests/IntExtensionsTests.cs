namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="IntExtensions"/> class.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class IntExtensionsTests
{
	[TestMethod]
	public void KB_ReturnsValueMultipliedBy1024()
	{
		Assert.AreEqual(1024, 1.KB());
		Assert.AreEqual(2048, 2.KB());
		Assert.AreEqual(0, 0.KB());
	}

	[TestMethod]
	public void MB_ReturnsValueInMegabytes()
	{
		Assert.AreEqual(1024 * 1024, 1.MB());
		Assert.AreEqual(2 * 1024 * 1024, 2.MB());
	}

	[TestMethod]
	public void GB_ReturnsValueInGigabytes()
	{
		Assert.AreEqual(1024 * 1024 * 1024, 1.GB());
	}

	[TestMethod]
	public void TB_ReturnsValueInTerabytes()
	{
		Assert.AreEqual(1024L * 1024 * 1024 * 1024, 1.TB());
	}

	[TestMethod]
	[DataRow(2, true)]
	[DataRow(3, true)]
	[DataRow(5, true)]
	[DataRow(7, true)]
	[DataRow(11, true)]
	[DataRow(13, true)]
	[DataRow(17, true)]
	[DataRow(97, true)]
	[DataRow(1, false)]
	[DataRow(4, false)]
	[DataRow(6, false)]
	[DataRow(9, false)]
	[DataRow(15, false)]
	[DataRow(100, false)]
	public void IsPrime_ReturnsExpectedResult(int number, bool expected)
	{
		Assert.AreEqual(expected, number.IsPrime());
	}

	[TestMethod]
	public void IsPrime_EvenNumberExcept2_ReturnsFalse()
	{
		Assert.IsFalse(8.IsPrime());
		Assert.IsFalse(100.IsPrime());
		Assert.IsTrue(2.IsPrime());
	}
}
