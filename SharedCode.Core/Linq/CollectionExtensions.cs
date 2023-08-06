// <copyright file="CollectionExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Linq;

using SharedCode.Properties;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

/// <summary>
/// Extensions for the ICollection interface.
/// </summary>
public static partial class CollectionExtensions
{
	/// <summary>
	/// Adds all of the given items to this collection. Can be used with dictionaries, which
	/// implement <see cref="ICollection{T}" /> and <see cref="IEnumerable{T}" /> where
	/// <typeparamref name="T" /> is <see cref="KeyValuePair{TKey, TValue}" />.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <param name="collection">The generic collection.</param>
	/// <param name="items">The items to be added.</param>
	/// <returns>The collection.</returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static TCollection AddRange<T, TCollection>(this TCollection collection, IEnumerable<T> items)
		where TCollection : ICollection<T>
	{
		_ = collection ?? throw new ArgumentNullException(nameof(collection));
		_ = items ?? throw new ArgumentNullException(nameof(items));

		foreach (var value in items)
		{
			collection.Add(value);
		}

		return collection;
	}

	/// <summary>
	/// Adds all of the given items to this collection if and only if the values object is not null.
	/// See <see cref="AddRange{T, TCollection}(TCollection, IEnumerable{T})" /> for more details.
	/// </summary>
	/// <typeparam name="T">The type of the items in the collection.</typeparam>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <param name="collection">The generic collection.</param>
	/// <param name="items">The items to be added.</param>
	/// <returns>The collection.</returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static TCollection AddRangeIfRangeNotNull<T, TCollection>(this TCollection collection, IEnumerable<T> items)
		where TCollection : ICollection<T>
	{
		_ = collection ?? throw new ArgumentNullException(nameof(collection));

		if (items is not null)
		{
			_ = collection.AddRange(items);
		}

		return collection;
	}

	/// <summary>
	/// Finds the specified collection.
	/// </summary>
	/// <typeparam name="T">Type of object to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The object or a new object of type T.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	public static T? Find<T>(this ICollection<T> @this, Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		foreach (var item in @this.Where(item => predicate?.Invoke(item) ?? default))
		{
			return item;
		}

		return default;
	}

	/// <summary>
	/// Finds all.
	/// </summary>
	/// <typeparam name="T">Type of object to look for.</typeparam>
	/// <param name="this">The generic collection.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>A new collection of T.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	public static Collection<T> FindAll<T>(this ICollection<T> @this, Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		var all = new Collection<T>();
		foreach (var item in @this.Where(item => predicate?.Invoke(item) ?? default))
		{
			all.Add(item);
		}

		return all;
	}

	/// <summary>
	/// Finds the index.
	/// </summary>
	/// <typeparam name="T">The type of elements to find.</typeparam>
	/// <param name="this">The generic collection.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The find index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentException">The collection cannot be empty.</exception>
	public static int FindIndex<T>(this ICollection<T> @this, Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		return @this.Count == 0
			? throw new ArgumentException(Resources.TheCollectionIsEmpty, nameof(@this))
			: FindIndex(@this, 0, predicate);
	}

	/// <summary>
	/// Finds the index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The find index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindIndex<T>(this ICollection<T> @this, int startIndex, Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		return startIndex > 0 || startIndex >= @this.Count
			? throw new ArgumentOutOfRangeException(nameof(startIndex))
			: FindIndex(@this, startIndex, @this.Count, predicate);
	}

	/// <summary>
	/// Finds the index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="count">The total count.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The find index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindIndex<T>(
		this ICollection<T> @this,
		int startIndex,
		int count,
		Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		if (startIndex < 0 || startIndex >= count)
		{
			throw new ArgumentNullException(nameof(startIndex));
		}

		for (var i = startIndex; i < count; i++)
		{
			if (i < 0 || i >= @this.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(@this));
			}

			if (predicate?.Invoke(@this.ElementAt(i)) ?? default)
			{
				return i;
			}
		}

		return -1;
	}

	/// <summary>
	/// Finds the last.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The element or a default instance of the type searched for.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	public static T? FindLast<T>(this ICollection<T> @this, Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		for (var i = @this.Count - 1; i >= 0; i--)
		{
			if (predicate?.Invoke(@this.ElementAt(i)) ?? default)
			{
				return @this.ElementAt(i);
			}
		}

		return default;
	}

	/// <summary>
	/// Finds the last index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The find last index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindLastIndex<T>(this ICollection<T> @this, Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		return @this.Count - 1 < 0
			? throw new ArgumentOutOfRangeException(nameof(@this))
			: FindLastIndex(@this, @this.Count - 1, predicate);
	}

	/// <summary>
	/// Finds the last index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The find last index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindLastIndex<T>(this ICollection<T> @this, int startIndex, Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		return startIndex < 0
			? throw new ArgumentOutOfRangeException(nameof(startIndex))
			: FindLastIndex(@this, startIndex, startIndex + 1, predicate);
	}

	/// <summary>
	/// Finds the last index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="count">The total count.</param>
	/// <param name="predicate">The predicate method.</param>
	/// <returns>The find last index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindLastIndex<T>(
		this ICollection<T> @this,
		int startIndex,
		int count,
		Predicate<T> predicate)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = predicate ?? throw new ArgumentNullException(nameof(predicate));

		if (startIndex < 0 || startIndex >= count)
		{
			throw new ArgumentOutOfRangeException(nameof(startIndex));
		}

		for (var i = startIndex; i >= startIndex - count; i--)
		{
			if (i < 0 || i >= @this.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(@this));
			}

			if (predicate?.Invoke(@this.ElementAt(i)) ?? default)
			{
				return i;
			}
		}

		return -1;
	}

	/// <summary>
	/// Loops over the collection performing action on each element.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="action">The action method.</param>
	/// <exception cref="ArgumentNullException">collection or action</exception>
	public static void ForEach<T>(this ICollection<T> @this, Action<T> action)
	{
		if (@this is null)
		{
			throw new ArgumentNullException(nameof(@this));
		}

		if (action is null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		foreach (var item in @this)
		{
			action?.Invoke(item);
		}
	}

	/// <summary>
	/// Determines whether the specified collection is null original empty.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="this">The collection.</param>
	/// <returns><c>true</c> if the specified collection is null original empty; otherwise, <c>false</c>.</returns>
	public static bool IsNullOrEmpty<T>(this ICollection<T>? @this) => @this is null || @this.Count == 0;

	/// <summary>
	/// Removes all.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="match">The matching predicate method.</param>
	/// <returns>The remove all.</returns>
	/// <exception cref="ArgumentNullException">collection or match</exception>
	public static int RemoveAll<T>(this ICollection<T> @this, Predicate<T> match)
	{
		if (@this is null)
		{
			throw new ArgumentNullException(nameof(@this));
		}

		if (match is null)
		{
			throw new ArgumentNullException(nameof(match));
		}

		var count = 0;
		for (var i = 0; i < @this.Count; i++)
		{
			if (!match?.Invoke(@this.ElementAt(i)) ?? default)
			{
				continue;
			}

			_ = @this.Remove(@this.ElementAt(i));
			count++;
			i--;
		}

		return count;
	}

	/// <summary>
	/// Removes the specified range of items from the collection.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="items">The items to remove.</param>
	/// <returns>
	/// An <c>IEnumerable{System.Boolean}</c> indicating whether the items were found and removed.
	/// </returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static IEnumerable<bool> RemoveRange<T>(this ICollection<T> @this, IEnumerable<T> items)
	{
		if (@this is null)
		{
			throw new ArgumentNullException(nameof(@this));
		}

		if (items is null)
		{
			throw new ArgumentNullException(nameof(items));
		}

		Contract.Ensures(Contract.Result<IEnumerable<bool>>() != null);

		return items.Select(@this.Remove);
	}

	/// <summary>
	/// Removes the specified range of items from the collection.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="items">The items to remove.</param>
	/// <returns>
	/// An <c>IEnumerable{System.Boolean}</c> indicating whether the items were found and removed.
	/// </returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static IEnumerable<bool> RemoveRange<T>(this ICollection<T> @this, params T[] items)
	{
		if (@this is null)
		{
			throw new ArgumentNullException(nameof(@this));
		}

		if (items is null)
		{
			throw new ArgumentNullException(nameof(items));
		}

		Contract.Ensures(Contract.Result<IEnumerable<bool>>() != null);

		return RemoveRange(@this, items.AsEnumerable());
	}

	/// <summary>
	/// Synchronizes the items in two collections, performing a minimal number of operations.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	/// <typeparam name="TKey">The type of the attribute key.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="source">The source collection.</param>
	/// <param name="getKey">The function used to get the attribute keys.</param>
	/// <returns>A <c>SyncChanges{T}</c> object containing the changes.</returns>
	/// <remarks>
	/// This preserves existing items in the collection, so selections are not lost when used on an ObservableCollection.
	/// </remarks>
	/// <exception cref="ArgumentNullException">collection or source or getKey</exception>
	/// <exception cref="ArgumentException">source cannot be empty.</exception>
	[SuppressMessage("Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Any is optimized and we do not want to materialize the array until later.")]
	public static SyncChanges<T> SyncFrom<T, TKey>(
		 this ICollection<T> @this,
		 IEnumerable<T> source,
		 Func<T, TKey> getKey)
	{
		if (@this is null)
		{
			throw new ArgumentNullException(nameof(@this));
		}

		if (source is null)
		{
			throw new ArgumentNullException(nameof(source));
		}

		if (source.Any(item => Equals(item, default)))
		{
			throw new ArgumentNullException(nameof(source));
		}

		if (getKey is null)
		{
			throw new ArgumentNullException(nameof(getKey));
		}

		Contract.Ensures(Contract.Result<SyncChanges<T>>() != null);

		var returnValue = new SyncChanges<T>();

		var keep = new HashSet<TKey>(@this.Select(getKey));

		var sourceArray = source as T[] ?? source.ToArray();

		keep.IntersectWith(sourceArray.Select(getKey));

		foreach (var item in @this
			.Where(
				x =>
				{
					var handler = getKey;
					return handler is not null && !keep.Contains(handler(x));
				}))
		{
			_ = @this.Remove(item);
			returnValue.Removed.Add(item);
		}

		foreach (var item in sourceArray
			.Where(
				x =>
				{
					var handler = getKey;
					return handler is not null && !keep.Contains(handler(x));
				}))
		{
			@this.Add(item);
			returnValue.Added.Add(item);
		}

		return returnValue;
	}

	/// <summary>
	/// Synchronizes the items in two collections, performing a minimal number of operations.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	/// <typeparam name="TKey">The type of the attribute key.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="newKeys">The new attribute keys.</param>
	/// <param name="getKey">The function used to get the attribute keys.</param>
	/// <param name="getObject">The function used to get the objects.</param>
	/// <returns>A <c>SyncChanges{T}</c> object containing the changes.</returns>
	/// <remarks>
	/// This preserves existing items in the collection, so selections are not lost when used on an ObservableCollection.
	/// </remarks>
	/// <exception cref="ArgumentNullException">collection or newKeys or getKey or getObject</exception>
	public static SyncChanges<T> SyncFrom<T, TKey>(
		 this ICollection<T> @this,
		 IEnumerable<TKey> newKeys,
		 Func<T, TKey> getKey,
		 Func<TKey, T> getObject)
	{
		if (@this is null)
		{
			throw new ArgumentNullException(nameof(@this));
		}

		if (newKeys is null)
		{
			throw new ArgumentNullException(nameof(newKeys));
		}

		if (getKey is null)
		{
			throw new ArgumentNullException(nameof(getKey));
		}

		if (getObject is null)
		{
			throw new ArgumentNullException(nameof(getObject));
		}

		Contract.Ensures(Contract.Result<SyncChanges<T>>() != null);

		var returnValue = new SyncChanges<T>();
		var keep = new HashSet<TKey>(@this.Select(getKey));
		var newKeysArray = newKeys as TKey[] ?? newKeys.ToArray();

		keep.IntersectWith(newKeysArray);

		foreach (var item in @this
			.Where(
				x =>
				{
					var handler = getKey;
					return handler is not null && !keep.Contains(handler(x));
				})
			.ToArray())
		{
			_ = @this.Remove(item);
			returnValue.Removed.Add(item);
		}

		foreach (var item in newKeysArray.Except(keep).Select(getObject))
		{
			@this.Add(item);
			returnValue.Added.Add(item);
		}

		return returnValue;
	}

	/// <summary>
	/// Trues for all.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="match">The matching predicate.</param>
	/// <returns>The true for all.</returns>
	/// <exception cref="ArgumentNullException">collection or match</exception>
	public static bool TrueForAll<T>(this ICollection<T> @this, Predicate<T> match)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		_ = match ?? throw new ArgumentNullException(nameof(match));

		return @this.All(item => match?.Invoke(item) ?? default);
	}
}
