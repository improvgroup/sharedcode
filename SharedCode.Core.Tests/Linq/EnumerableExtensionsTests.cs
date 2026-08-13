namespace SharedCode.Tests.Linq;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Linq;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="EnumerableExtensions"/> in the SharedCode.Linq namespace.
/// </summary>
public class EnumerableExtensionsTests
{
	[Test]
	public async Task Aggregate_WithItems_ReturnsAggregatedResult()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.Aggregate((a, b) => a + b);
		await Assert.That(result).IsEqualTo(15);
	}

	[Test]
	public async Task Aggregate_EmptyList_ReturnsDefault()
	{
		var items = Array.Empty<int>();
		var result = items.Aggregate((a, b) => a + b);
		await Assert.That(result).IsEqualTo(default);
	}

	[Test]
	public async Task Aggregate_WithDefaultValue_EmptyList_ReturnsDefault()
	{
		var items = Array.Empty<int>();
		var result = items.Aggregate(42, (a, b) => a + b);
		await Assert.That(result).IsEqualTo(42);
	}

	[Test]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling explicitly.")]
	public async Task IsNullOrEmpty_NullEnumerable_ReturnsTrue()
	{
		IEnumerable<int>? items = null;
		await Assert.That(items!.IsNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNullOrEmpty_EmptyEnumerable_ReturnsTrue()
	{
		var items = Array.Empty<int>();
		await Assert.That(items.IsNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNullOrEmpty_NonEmptyEnumerable_ReturnsFalse()
	{
		var items = new[] { 1, 2, 3 };
		await Assert.That(items.IsNullOrEmpty()).IsFalse();
	}

	[Test]
	public async Task IsNotNullOrEmpty_NonEmptyEnumerable_ReturnsTrue()
	{
		var items = new[] { 1 };
		await Assert.That(items.IsNotNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNotNullOrEmpty_EmptyEnumerable_ReturnsFalse()
	{
		var items = Array.Empty<int>();
		await Assert.That(items.IsNotNullOrEmpty()).IsFalse();
	}

	[Test]
	public async Task Distinct_ByKey_ReturnsUniqueItems()
	{
		var items = new[]
		{
			new { Id = 1, Name = "A" },
			new { Id = 2, Name = "B" },
			new { Id = 1, Name = "C" },
		};
		var result = items.Distinct(x => x.Id).ToList();
		await Assert.That(result.Count).IsEqualTo(2);
	}

	[Test]
	public async Task ForEach_ExecutesActionOnEachItem()
	{
		var items = new[] { 1, 2, 3 };
		var sum = 0;
		items.ForEach((Action<int>)(x => sum += x));
		await Assert.That(sum).IsEqualTo(6);
	}

	[Test]
	public async Task IndexOf_ItemExists_ReturnsIndex()
	{
		var items = new[] { "a", "b", "c" };
		await Assert.That(items.IndexOf("b")).IsEqualTo(1);
	}

	[Test]
	public async Task IndexOf_ItemDoesNotExist_ReturnsMinusOne()
	{
		var items = new[] { "a", "b", "c" };
		await Assert.That(items.IndexOf("z")).IsEqualTo(-1);
	}

	[Test]
	public async Task Randomize_ReturnsAllItemsInSomeOrder()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.Randomize().ToList();
		await Assert.That(result.Count).IsEqualTo(5);
		await Assert.That(items.All(i => result.Contains(i))).IsTrue();
	}

	[Test]
	public async Task ToCollection_ReturnsCollectionWithAllItems()
	{
		var items = new[] { 1, 2, 3 };
		var collection = items.ToCollection();
		await Assert.That(collection.Count).IsEqualTo(3);
	}

	[Test]
	public async Task OrderBy_ByKeyDescending_ReturnsDescendingOrder()
	{
		var items = new[] { 3, 1, 4, 1, 5, 9, 2 };
		var result = items.OrderBy(x => x, descending: true).ToList();
		await Assert.That(result[0]).IsEqualTo(9);
		await Assert.That(result[1]).IsEqualTo(5);
	}

	[Test]
	public async Task OrderBy_ByKeyAscending_ReturnsAscendingOrder()
	{
		var items = new[] { 3, 1, 4, 1, 5 };
		var result = items.OrderBy(x => x, descending: false).ToList();
		await Assert.That(result[0]).IsEqualTo(1);
		await Assert.That(result[^1]).IsEqualTo(5);
	}

	[Test]
	public async Task Slice_ReturnsSubset()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.Slice(1, 4).ToList();
		await Assert.That(result.Count).IsEqualTo(3);
		await Assert.That(result[0]).IsEqualTo(2);
		await Assert.That(result[2]).IsEqualTo(4);
	}

	[Test]
	public async Task StdDev_IntEnumerable_ReturnsExpectedDeviation()
	{
		// Sample standard deviation (n-1): sqrt(32/7) ≈ 2.138
		var values = new[] { 2, 4, 4, 4, 5, 5, 7, 9 };
		var stdDev = values.StdDev();
		await Assert.That(Math.Round(stdDev, 3)).IsEqualTo(2.138);
	}

	[Test]
	public async Task StdDev_DoubleEnumerable_ReturnsExpectedDeviation()
	{
		// Sample standard deviation (n-1): sqrt(32/7) ≈ 2.138
		var values = new[] { 2.0, 4.0, 4.0, 4.0, 5.0, 5.0, 7.0, 9.0 };
		var stdDev = values.StdDev();
		await Assert.That(Math.Round(stdDev, 3)).IsEqualTo(2.138);
	}

	[Test]
	public async Task SelectRandom_NonEmptyList_ReturnsItemFromList()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.SelectRandom();
		await Assert.That(items.Contains(result)).IsTrue();
	}

	[Test]
	public async Task Cache_ReturnsAllItems()
	{
		var items = new[] { 1, 2, 3 };
		var cached = items.Cache().ToList();
		await Assert.That(cached.Count).IsEqualTo(3);
	}
}
