namespace SharedCode.Data.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="PagingDescriptor"/>.
/// </summary>
public class PagingDescriptorTests
{
	[Test]
	public async Task Constructor_SetsAllProperties()
	{
		var boundaries = new List<PageBoundry>
		{
			new(0, 9),
			new(10, 19),
		};
		var descriptor = new PagingDescriptor(actualPageSize: 10, numberOfPages: 2, pagesBoundries: boundaries);

		await Assert.That(descriptor.ActualPageSize).IsEqualTo(10);
		await Assert.That(descriptor.NumberOfPages).IsEqualTo(2);
		await Assert.That(descriptor.PagesBoundries.Count).IsEqualTo(2);
	}

	[Test]
	public async Task Constructor_SinglePage_NumberOfPagesIs1()
	{
		var boundaries = new List<PageBoundry> { new(0, 4) };
		var descriptor = new PagingDescriptor(actualPageSize: 5, numberOfPages: 1, pagesBoundries: boundaries);

		await Assert.That(descriptor.NumberOfPages).IsEqualTo(1);
		await Assert.That(descriptor.ActualPageSize).IsEqualTo(5);
	}

	[Test]
	public async Task Constructor_EmptyBoundaries_ZeroPages()
	{
		var boundaries = new List<PageBoundry>();
		var descriptor = new PagingDescriptor(actualPageSize: 10, numberOfPages: 0, pagesBoundries: boundaries);

		await Assert.That(descriptor.NumberOfPages).IsEqualTo(0);
		await Assert.That(descriptor.PagesBoundries.Count).IsEqualTo(0);
	}

	[Test]
	public async Task PagesBoundries_ContainsCorrectBoundaries()
	{
		var boundaries = new List<PageBoundry>
		{
			new(0, 9),
			new(10, 19),
			new(20, 24),
		};
		var descriptor = new PagingDescriptor(actualPageSize: 10, numberOfPages: 3, pagesBoundries: boundaries);

		var boundaryList = descriptor.PagesBoundries.ToList();
		await Assert.That(boundaryList[0].FirstItemZeroIndex).IsEqualTo(0);
		await Assert.That(boundaryList[0].LastItemZeroIndex).IsEqualTo(9);
		await Assert.That(boundaryList[1].FirstItemZeroIndex).IsEqualTo(10);
		await Assert.That(boundaryList[2].FirstItemZeroIndex).IsEqualTo(20);
	}
}
