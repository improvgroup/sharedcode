namespace SharedCode.Data.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="PageBoundry"/>.
/// </summary>
public class PageBoundryTests
{
	[Test]
	public async Task Constructor_SetsFirstAndLastItemIndex()
	{
		var pageBoundry = new PageBoundry(0, 9);
		await Assert.That(pageBoundry.FirstItemZeroIndex).IsEqualTo(0);
		await Assert.That(pageBoundry.LastItemZeroIndex).IsEqualTo(9);
	}

	[Test]
	public async Task Constructor_SecondPage_SetsCorrectBoundaries()
	{
		var pageBoundry = new PageBoundry(10, 19);
		await Assert.That(pageBoundry.FirstItemZeroIndex).IsEqualTo(10);
		await Assert.That(pageBoundry.LastItemZeroIndex).IsEqualTo(19);
	}

	[Test]
	public async Task Constructor_SingleItem_FirstEqualsLast()
	{
		var pageBoundry = new PageBoundry(5, 5);
		await Assert.That(pageBoundry.FirstItemZeroIndex).IsEqualTo(5);
		await Assert.That(pageBoundry.LastItemZeroIndex).IsEqualTo(5);
	}
}
