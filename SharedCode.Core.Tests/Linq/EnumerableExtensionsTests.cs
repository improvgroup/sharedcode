namespace SharedCode.Tests.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Linq;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="EnumerableExtensions"/> in the SharedCode.Linq namespace.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class EnumerableExtensionsTests
{
	[TestMethod]
	public void Aggregate_WithItems_ReturnsAggregatedResult()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.Aggregate((a, b) => a + b);
		Assert.AreEqual(15, result);
	}

	[TestMethod]
	public void Aggregate_EmptyList_ReturnsDefault()
	{
		var items = Array.Empty<int>();
		var result = items.Aggregate((a, b) => a + b);
		Assert.AreEqual(default, result);
	}

	[TestMethod]
	public void Aggregate_WithDefaultValue_EmptyList_ReturnsDefault()
	{
		var items = Array.Empty<int>();
		var result = items.Aggregate(42, (a, b) => a + b);
		Assert.AreEqual(42, result);
	}

	[TestMethod]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling explicitly.")]
	public void IsNullOrEmpty_NullEnumerable_ReturnsTrue()
	{
		IEnumerable<int>? items = null;
		Assert.IsTrue(items!.IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNullOrEmpty_EmptyEnumerable_ReturnsTrue()
	{
		var items = Array.Empty<int>();
		Assert.IsTrue(items.IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNullOrEmpty_NonEmptyEnumerable_ReturnsFalse()
	{
		var items = new[] { 1, 2, 3 };
		Assert.IsFalse(items.IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNotNullOrEmpty_NonEmptyEnumerable_ReturnsTrue()
	{
		var items = new[] { 1 };
		Assert.IsTrue(items.IsNotNullOrEmpty());
	}

	[TestMethod]
	public void IsNotNullOrEmpty_EmptyEnumerable_ReturnsFalse()
	{
		var items = Array.Empty<int>();
		Assert.IsFalse(items.IsNotNullOrEmpty());
	}

	[TestMethod]
	public void Distinct_ByKey_ReturnsUniqueItems()
	{
		var items = new[]
		{
			new { Id = 1, Name = "A" },
			new { Id = 2, Name = "B" },
			new { Id = 1, Name = "C" },
		};
		var result = items.Distinct(x => x.Id).ToList();
		Assert.AreEqual(2, result.Count);
	}

	[TestMethod]
	public void ForEach_ExecutesActionOnEachItem()
	{
		var items = new[] { 1, 2, 3 };
		var sum = 0;
		items.ForEach((Action<int>)(x => sum += x));
		Assert.AreEqual(6, sum);
	}

	[TestMethod]
	public void IndexOf_ItemExists_ReturnsIndex()
	{
		var items = new[] { "a", "b", "c" };
		Assert.AreEqual(1, items.IndexOf("b"));
	}

	[TestMethod]
	public void IndexOf_ItemDoesNotExist_ReturnsMinusOne()
	{
		var items = new[] { "a", "b", "c" };
		Assert.AreEqual(-1, items.IndexOf("z"));
	}

	[TestMethod]
	public void Randomize_ReturnsAllItemsInSomeOrder()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.Randomize().ToList();
		Assert.AreEqual(5, result.Count);
		Assert.IsTrue(items.All(i => result.Contains(i)));
	}

	[TestMethod]
	public void ToCollection_ReturnsCollectionWithAllItems()
	{
		var items = new[] { 1, 2, 3 };
		var collection = items.ToCollection();
		Assert.AreEqual(3, collection.Count);
	}

	[TestMethod]
	public void OrderBy_ByKeyDescending_ReturnsDescendingOrder()
	{
		var items = new[] { 3, 1, 4, 1, 5, 9, 2 };
		var result = items.OrderBy(x => x, descending: true).ToList();
		Assert.AreEqual(9, result[0]);
		Assert.AreEqual(5, result[1]);
	}

	[TestMethod]
	public void OrderBy_ByKeyAscending_ReturnsAscendingOrder()
	{
		var items = new[] { 3, 1, 4, 1, 5 };
		var result = items.OrderBy(x => x, descending: false).ToList();
		Assert.AreEqual(1, result[0]);
		Assert.AreEqual(5, result[^1]);
	}

	[TestMethod]
	public void Slice_ReturnsSubset()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.Slice(1, 4).ToList();
		Assert.AreEqual(3, result.Count);
		Assert.AreEqual(2, result[0]);
		Assert.AreEqual(4, result[2]);
	}

	[TestMethod]
	public void StdDev_IntEnumerable_ReturnsExpectedDeviation()
	{
		// Sample standard deviation (n-1): sqrt(32/7) ≈ 2.138
		var values = new[] { 2, 4, 4, 4, 5, 5, 7, 9 };
		var stdDev = values.StdDev();
		Assert.AreEqual(2.138, stdDev, 0.001);
	}

	[TestMethod]
	public void StdDev_DoubleEnumerable_ReturnsExpectedDeviation()
	{
		// Sample standard deviation (n-1): sqrt(32/7) ≈ 2.138
		var values = new[] { 2.0, 4.0, 4.0, 4.0, 5.0, 5.0, 7.0, 9.0 };
		var stdDev = values.StdDev();
		Assert.AreEqual(2.138, stdDev, 0.001);
	}

	[TestMethod]
	public void SelectRandom_NonEmptyList_ReturnsItemFromList()
	{
		var items = new[] { 1, 2, 3, 4, 5 };
		var result = items.SelectRandom();
		Assert.IsTrue(items.Contains(result));
	}

	[TestMethod]
	public void Cache_ReturnsAllItems()
	{
		var items = new[] { 1, 2, 3 };
		var cached = items.Cache().ToList();
		Assert.AreEqual(3, cached.Count);
	}
}
