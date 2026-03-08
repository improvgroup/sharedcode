namespace SharedCode.Data.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="PageBoundry"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class PageBoundryTests
{
	[TestMethod]
	public void Constructor_SetsFirstAndLastItemIndex()
	{
		var pageBoundry = new PageBoundry(0, 9);
		Assert.AreEqual(0, pageBoundry.FirstItemZeroIndex);
		Assert.AreEqual(9, pageBoundry.LastItemZeroIndex);
	}

	[TestMethod]
	public void Constructor_SecondPage_SetsCorrectBoundaries()
	{
		var pageBoundry = new PageBoundry(10, 19);
		Assert.AreEqual(10, pageBoundry.FirstItemZeroIndex);
		Assert.AreEqual(19, pageBoundry.LastItemZeroIndex);
	}

	[TestMethod]
	public void Constructor_SingleItem_FirstEqualsLast()
	{
		var pageBoundry = new PageBoundry(5, 5);
		Assert.AreEqual(5, pageBoundry.FirstItemZeroIndex);
		Assert.AreEqual(5, pageBoundry.LastItemZeroIndex);
	}
}
