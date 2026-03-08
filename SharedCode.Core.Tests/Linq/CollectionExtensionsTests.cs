namespace SharedCode.Tests.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Linq;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="CollectionExtensions"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class CollectionExtensionsTests
{
	private static readonly int[] ThreeItems = [3, 4, 5];
	private static readonly int[] TwoItemsToRemove = [2, 4];

	[TestMethod]
	public void AddRange_AddsAllItems()
	{
		var collection = new List<int> { 1, 2 };
		var result = collection.AddRange<int, List<int>>(ThreeItems);
		Assert.AreEqual(5, collection.Count);
		Assert.IsTrue(collection.Contains(3));
		Assert.IsTrue(collection.Contains(5));
		Assert.AreSame(collection, result);
	}

	[TestMethod]
	public void AddRange_NullCollection_ThrowsArgumentNullException()
	{
		List<int>? collection = null;
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => collection!.AddRange<int, List<int>>(ThreeItems));
	}

	[TestMethod]
	public void AddRange_NullItems_ThrowsArgumentNullException()
	{
		var collection = new List<int>();
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => collection.AddRange<int, List<int>>(null!));
	}

	[TestMethod]
	public void AddRangeIfRangeNotNull_NullItems_DoesNotThrow()
	{
		var collection = new List<int> { 1 };
		_ = collection.AddRangeIfRangeNotNull<int, List<int>>(null!);
		Assert.AreEqual(1, collection.Count);
	}

	private static readonly int[] TwoItemsForAdd = [2, 3];

	[TestMethod]
	public void AddRangeIfRangeNotNull_WithItems_AddsAll()
	{
		var collection = new List<int> { 1 };
		_ = collection.AddRangeIfRangeNotNull<int, List<int>>(TwoItemsForAdd);
		Assert.AreEqual(3, collection.Count);
	}

	[TestMethod]
	public void Find_ItemExists_ReturnsItem()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5 };
		var result = collection.Find(x => x == 3);
		Assert.AreEqual(3, result);
	}

	[TestMethod]
	public void Find_ItemDoesNotExist_ReturnsDefault()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3 };
		var result = collection.Find(x => x == 10);
		Assert.AreEqual(default, result);
	}

	[TestMethod]
	public void Find_NullCollection_ThrowsArgumentNullException()
	{
		ICollection<int>? collection = null;
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => collection!.Find(x => x == 1));
	}

	[TestMethod]
	public void Find_NullPredicate_ThrowsArgumentNullException()
	{
		ICollection<int> collection = new List<int> { 1, 2 };
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => collection.Find(null!));
	}

	[TestMethod]
	public void FindAll_MatchingItems_ReturnsAll()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5 };
		var result = collection.FindAll(x => x % 2 == 0);
		Assert.AreEqual(2, result.Count);
		Assert.IsTrue(result.Contains(2));
		Assert.IsTrue(result.Contains(4));
	}

	[TestMethod]
	public void FindIndex_ItemExists_ReturnsIndex()
	{
		ICollection<string> collection = new List<string> { "a", "b", "c" };
		var index = collection.FindIndex(x => x == "b");
		Assert.AreEqual(1, index);
	}

	[TestMethod]
	public void FindIndex_ItemDoesNotExist_ReturnsMinusOne()
	{
		ICollection<string> collection = new List<string> { "a", "b", "c" };
		var index = collection.FindIndex(x => x == "z");
		Assert.AreEqual(-1, index);
	}

	[TestMethod]
	public void FindLast_ItemExists_ReturnsLastMatch()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 2, 1 };
		var result = collection.FindLast(x => x == 2);
		Assert.AreEqual(2, result);
	}

	[TestMethod]
	public void FindLastIndex_ItemExists_ReturnsLastIndex()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 2, 1 };
		var index = collection.FindLastIndex(x => x == 2);
		Assert.AreEqual(3, index);
	}

	[TestMethod]
	public void ForEach_ExecutesActionOnEachItem()
	{
		var collection = new List<int> { 1, 2, 3 };
		var sum = 0;
		collection.ForEach((Action<int>)(x => sum += x));
		Assert.AreEqual(6, sum);
	}

	[TestMethod]
	public void IsNullOrEmpty_EmptyCollection_ReturnsTrue()
	{
		var collection = new List<int>();
		Assert.IsTrue(collection.IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNullOrEmpty_NullCollection_ReturnsTrue()
	{
		List<int>? collection = null;
		Assert.IsTrue(collection.IsNullOrEmpty());
	}

	[TestMethod]
	public void IsNullOrEmpty_NonEmptyCollection_ReturnsFalse()
	{
		var collection = new List<int> { 1 };
		Assert.IsFalse(collection.IsNullOrEmpty());
	}

	[TestMethod]
	public void RemoveAll_RemovesMatchingItems()
	{
		var collection = new List<int> { 1, 2, 3, 4, 5 };
		var removed = collection.RemoveAll(x => x % 2 == 0);
		Assert.AreEqual(2, removed);
		Assert.AreEqual(3, collection.Count);
	}

	[TestMethod]
	public void RemoveRange_RemovesSpecifiedItems()
	{
		var collection = new List<int> { 1, 2, 3, 4, 5 };
		var results = collection.RemoveRange(TwoItemsToRemove).ToList();
		Assert.AreEqual(3, collection.Count);
		Assert.IsTrue(results.All(r => r));
	}

	[TestMethod]
	public void TrueForAll_AllMatch_ReturnsTrue()
	{
		ICollection<int> collection = new List<int> { 2, 4, 6 };
		Assert.IsTrue(collection.TrueForAll(x => x % 2 == 0));
	}

	[TestMethod]
	public void TrueForAll_SomeDoNotMatch_ReturnsFalse()
	{
		ICollection<int> collection = new List<int> { 2, 3, 6 };
		Assert.IsFalse(collection.TrueForAll(x => x % 2 == 0));
	}
}
