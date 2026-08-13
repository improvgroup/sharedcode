namespace SharedCode.Tests.Linq;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Linq;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="CollectionExtensions"/>.
/// </summary>
public class CollectionExtensionsTests
{
	private static readonly int[] ThreeItems = [3, 4, 5];
	private static readonly int[] TwoItemsToRemove = [2, 4];

	[Test]
	public async Task AddRange_AddsAllItems()
	{
		var collection = new List<int> { 1, 2 };
		var result = collection.AddRange<int, List<int>>(ThreeItems);
		await Assert.That(collection.Count).IsEqualTo(5);
		await Assert.That(collection.Contains(3)).IsTrue();
		await Assert.That(collection.Contains(5)).IsTrue();
		await Assert.That(result).IsSameReferenceAs(collection);
	}

	[Test]
	public async Task AddRange_NullCollection_ThrowsArgumentNullException()
	{
		List<int>? collection = null;
		await Assert.That(() => collection!.AddRange<int, List<int>>(ThreeItems)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task AddRange_NullItems_ThrowsArgumentNullException()
	{
		var collection = new List<int>();
		await Assert.That(() => collection.AddRange<int, List<int>>(null!)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task AddRangeIfRangeNotNull_NullItems_DoesNotThrow()
	{
		var collection = new List<int> { 1 };
		_ = collection.AddRangeIfRangeNotNull<int, List<int>>(null!);
		await Assert.That(collection.Count).IsEqualTo(1);
	}

	private static readonly int[] TwoItemsForAdd = [2, 3];

	[Test]
	public async Task AddRangeIfRangeNotNull_WithItems_AddsAll()
	{
		var collection = new List<int> { 1 };
		_ = collection.AddRangeIfRangeNotNull<int, List<int>>(TwoItemsForAdd);
		await Assert.That(collection.Count).IsEqualTo(3);
	}

	[Test]
	public async Task Find_ItemExists_ReturnsItem()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5 };
		var result = collection.Find(x => x == 3);
		await Assert.That(result).IsEqualTo(3);
	}

	[Test]
	public async Task Find_ItemDoesNotExist_ReturnsDefault()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3 };
		var result = collection.Find(x => x == 10);
		await Assert.That(result).IsEqualTo(default);
	}

	[Test]
	public async Task Find_NullCollection_ThrowsArgumentNullException()
	{
		ICollection<int>? collection = null;
		await Assert.That(() => collection!.Find(x => x == 1)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task Find_NullPredicate_ThrowsArgumentNullException()
	{
		ICollection<int> collection = new List<int> { 1, 2 };
		await Assert.That(() => collection.Find(null!)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task FindAll_MatchingItems_ReturnsAll()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5 };
		var result = collection.FindAll(x => x % 2 == 0);
		await Assert.That(result.Count).IsEqualTo(2);
		await Assert.That(result.Contains(2)).IsTrue();
		await Assert.That(result.Contains(4)).IsTrue();
	}

	[Test]
	public async Task FindIndex_ItemExists_ReturnsIndex()
	{
		ICollection<string> collection = new List<string> { "a", "b", "c" };
		var index = collection.FindIndex(x => x == "b");
		await Assert.That(index).IsEqualTo(1);
	}

	[Test]
	public async Task FindIndex_ItemDoesNotExist_ReturnsMinusOne()
	{
		ICollection<string> collection = new List<string> { "a", "b", "c" };
		var index = collection.FindIndex(x => x == "z");
		await Assert.That(index).IsEqualTo(-1);
	}

	[Test]
	public async Task FindLast_ItemExists_ReturnsLastMatch()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 2, 1 };
		var result = collection.FindLast(x => x == 2);
		await Assert.That(result).IsEqualTo(2);
	}

	[Test]
	public async Task FindLastIndex_ItemExists_ReturnsLastIndex()
	{
		ICollection<int> collection = new List<int> { 1, 2, 3, 2, 1 };
		var index = collection.FindLastIndex(x => x == 2);
		await Assert.That(index).IsEqualTo(3);
	}

	[Test]
	public async Task ForEach_ExecutesActionOnEachItem()
	{
		var collection = new List<int> { 1, 2, 3 };
		var sum = 0;
		collection.ForEach((Action<int>)(x => sum += x));
		await Assert.That(sum).IsEqualTo(6);
	}

	[Test]
	public async Task IsNullOrEmpty_EmptyCollection_ReturnsTrue()
	{
		var collection = new List<int>();
		await Assert.That(collection.IsNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNullOrEmpty_NullCollection_ReturnsTrue()
	{
		List<int>? collection = null;
		await Assert.That(collection.IsNullOrEmpty()).IsTrue();
	}

	[Test]
	public async Task IsNullOrEmpty_NonEmptyCollection_ReturnsFalse()
	{
		var collection = new List<int> { 1 };
		await Assert.That(collection.IsNullOrEmpty()).IsFalse();
	}

	[Test]
	public async Task RemoveAll_RemovesMatchingItems()
	{
		var collection = new List<int> { 1, 2, 3, 4, 5 };
		var removed = collection.RemoveAll(x => x % 2 == 0);
		await Assert.That(removed).IsEqualTo(2);
		await Assert.That(collection.Count).IsEqualTo(3);
	}

	[Test]
	public async Task RemoveRange_RemovesSpecifiedItems()
	{
		var collection = new List<int> { 1, 2, 3, 4, 5 };
		var results = collection.RemoveRange(TwoItemsToRemove).ToList();
		await Assert.That(collection.Count).IsEqualTo(3);
		await Assert.That(results.All(r => r)).IsTrue();
	}

	[Test]
	public async Task TrueForAll_AllMatch_ReturnsTrue()
	{
		ICollection<int> collection = new List<int> { 2, 4, 6 };
		await Assert.That(collection.TrueForAll(x => x % 2 == 0)).IsTrue();
	}

	[Test]
	public async Task TrueForAll_SomeDoNotMatch_ReturnsFalse()
	{
		ICollection<int> collection = new List<int> { 2, 3, 6 };
		await Assert.That(collection.TrueForAll(x => x % 2 == 0)).IsFalse();
	}
}
