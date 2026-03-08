namespace SharedCode.Data.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="PagingDescriptor"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class PagingDescriptorTests
{
	[TestMethod]
	public void Constructor_SetsAllProperties()
	{
		var boundaries = new List<PageBoundry>
		{
			new(0, 9),
			new(10, 19),
		};
		var descriptor = new PagingDescriptor(actualPageSize: 10, numberOfPages: 2, pagesBoundries: boundaries);

		Assert.AreEqual(10, descriptor.ActualPageSize);
		Assert.AreEqual(2, descriptor.NumberOfPages);
		Assert.AreEqual(2, descriptor.PagesBoundries.Count);
	}

	[TestMethod]
	public void Constructor_SinglePage_NumberOfPagesIs1()
	{
		var boundaries = new List<PageBoundry> { new(0, 4) };
		var descriptor = new PagingDescriptor(actualPageSize: 5, numberOfPages: 1, pagesBoundries: boundaries);

		Assert.AreEqual(1, descriptor.NumberOfPages);
		Assert.AreEqual(5, descriptor.ActualPageSize);
	}

	[TestMethod]
	public void Constructor_EmptyBoundaries_ZeroPages()
	{
		var boundaries = new List<PageBoundry>();
		var descriptor = new PagingDescriptor(actualPageSize: 10, numberOfPages: 0, pagesBoundries: boundaries);

		Assert.AreEqual(0, descriptor.NumberOfPages);
		Assert.AreEqual(0, descriptor.PagesBoundries.Count);
	}

	[TestMethod]
	public void PagesBoundries_ContainsCorrectBoundaries()
	{
		var boundaries = new List<PageBoundry>
		{
			new(0, 9),
			new(10, 19),
			new(20, 24),
		};
		var descriptor = new PagingDescriptor(actualPageSize: 10, numberOfPages: 3, pagesBoundries: boundaries);

		var boundaryList = descriptor.PagesBoundries.ToList();
		Assert.AreEqual(0, boundaryList[0].FirstItemZeroIndex);
		Assert.AreEqual(9, boundaryList[0].LastItemZeroIndex);
		Assert.AreEqual(10, boundaryList[1].FirstItemZeroIndex);
		Assert.AreEqual(20, boundaryList[2].FirstItemZeroIndex);
	}
}
