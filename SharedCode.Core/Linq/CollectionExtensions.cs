// <copyright file="CollectionExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Linq;

using SharedCode.Properties;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
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
	/// <param name="collection">The collection.</param>
	/// <param name="items">The items to be added.</param>
	/// <returns>The collection.</returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static TCollection AddRange<T, TCollection>([NotNull] this TCollection collection, [NotNull] IEnumerable<T> items)
		where TCollection : ICollection<T>
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (items is null)
		{
			throw new ArgumentNullException(nameof(items));
		}

		foreach (var value in items)
		{
			collection.Add(value);
		}

		return collection;
	}

	/// <summary>
	/// Adds all of the given items to this collection if and only if the values object is not
	/// null. See <see cref="AddRange{T, TCollection}(TCollection, IEnumerable{T})" /> for more details.
	/// </summary>
	/// <typeparam name="T">The type of the items in the collection.</typeparam>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="items">The items to be added.</param>
	/// <returns>The collection.</returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static TCollection AddRangeIfRangeNotNull<T, TCollection>(this TCollection collection, IEnumerable<T> items)
		where TCollection : ICollection<T>
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

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
	/// <param name="collection">The collection.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The object or a new object of type T.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	public static T Find<T>([NotNull] this ICollection<T> collection, [NotNull] Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		foreach (var item in collection.Where(item => predicate?.Invoke(item) ?? default))
		{
			return item;
		}

#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
#pragma warning disable CS8603 // Possible null reference return.
		return default;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
	}

	/// <summary>
	/// Finds all.
	/// </summary>
	/// <typeparam name="T">Type of object to look for.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>A new collection of T.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	public static Collection<T> FindAll<T>([NotNull] this ICollection<T> collection, [NotNull] Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		var all = new Collection<T>();
		foreach (var item in collection.Where(item => predicate?.Invoke(item) ?? default))
		{
			all.Add(item);
		}

		return all;
	}

	/// <summary>
	/// Finds the index.
	/// </summary>
	/// <typeparam name="T">The type of elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The find index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentException">The collection cannot be empty.</exception>
	public static int FindIndex<T>([NotNull] this ICollection<T> collection, [NotNull] Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		if (collection.Count == 0)
		{
			throw new ArgumentException(Resources.TheCollectionIsEmpty, nameof(collection));
		}

		return FindIndex(collection, 0, predicate);
	}

	/// <summary>
	/// Finds the index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The find index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindIndex<T>([NotNull] this ICollection<T> collection, int startIndex, [NotNull] Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		if (startIndex > 0 || startIndex >= collection.Count)
		{
			throw new ArgumentOutOfRangeException(nameof(startIndex));
		}

		return FindIndex(collection, startIndex, collection.Count, predicate);
	}

	/// <summary>
	/// Finds the index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="count">The count.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The find index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindIndex<T>(
		this ICollection<T> collection,
		int startIndex,
		int count,
		Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		if (startIndex < 0 || startIndex >= count)
		{
			throw new ArgumentNullException(nameof(startIndex));
		}

		for (var i = startIndex; i < count; i++)
		{
			if (i < 0 || i >= collection.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(collection));
			}

			if (predicate?.Invoke(collection.ElementAt(i)) ?? default)
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
	/// <param name="collection">The collection.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The element or a default instance of the type searched for.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	public static T FindLast<T>(this ICollection<T> collection, Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		for (var i = collection.Count - 1; i >= 0; i--)
		{
			if (predicate?.Invoke(collection.ElementAt(i)) ?? default)
			{
				return collection.ElementAt(i);
			}
		}

#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
#pragma warning disable CS8603 // Possible null reference return.
		return default;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
	}

	/// <summary>
	/// Finds the last index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The find last index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindLastIndex<T>([NotNull] this ICollection<T> collection, [NotNull] Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		if (collection.Count - 1 < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(collection));
		}

		return FindLastIndex(collection, collection.Count - 1, predicate);
	}

	/// <summary>
	/// Finds the last index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The find last index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindLastIndex<T>([NotNull] this ICollection<T> collection, int startIndex, [NotNull] Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		if (startIndex < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(startIndex));
		}

		return FindLastIndex(collection, startIndex, startIndex + 1, predicate);
	}

	/// <summary>
	/// Finds the last index.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="startIndex">The start index.</param>
	/// <param name="count">The count.</param>
	/// <param name="predicate">The predicate.</param>
	/// <returns>The find last index.</returns>
	/// <exception cref="ArgumentNullException">collection or predicate</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The startIndex is out of range of the collection.
	/// </exception>
	public static int FindLastIndex<T>(
		this ICollection<T> collection,
		int startIndex,
		int count,
		Predicate<T> predicate)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (predicate is null)
		{
			throw new ArgumentNullException(nameof(predicate));
		}

		if (startIndex < 0 || startIndex >= count)
		{
			throw new ArgumentOutOfRangeException(nameof(startIndex));
		}

		for (var i = startIndex; i >= startIndex - count; i--)
		{
			if (i < 0 || i >= collection.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(collection));
			}

			if (predicate?.Invoke(collection.ElementAt(i)) ?? default)
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
	/// <param name="collection">The collection.</param>
	/// <param name="action">The action.</param>
	/// <exception cref="ArgumentNullException">collection or action</exception>
	public static void ForEach<T>([NotNull] this ICollection<T> collection, [NotNull] Action<T> action)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (action is null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		foreach (var item in collection)
		{
			action?.Invoke(item);
		}
	}

	/// <summary>
	/// Determines whether the specified collection is null original empty.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <returns>
	/// <c>true</c> if the specified collection is null original empty; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsNullOrEmpty<T>(this ICollection<T>? collection) => collection is null || collection.Count == 0;

	/// <summary>
	/// Removes all.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="match">The match.</param>
	/// <returns>The remove all.</returns>
	/// <exception cref="ArgumentNullException">collection or match</exception>
	public static int RemoveAll<T>(this ICollection<T> collection, Predicate<T> match)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (match is null)
		{
			throw new ArgumentNullException(nameof(match));
		}

		var count = 0;
		for (var i = 0; i < collection.Count; i++)
		{
			if (!match?.Invoke(collection.ElementAt(i)) ?? default)
			{
				continue;
			}

			_ = collection.Remove(collection.ElementAt(i));
			count++;
			i--;
		}

		return count;
	}

	/// <summary>
	/// Removes the specified range of items from the collection.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="items">The items.</param>
	/// <returns>
	/// An <c>IEnumerable{System.Boolean}</c> indicating whether the items were found and removed.
	/// </returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static IEnumerable<bool> RemoveRange<T>([NotNull] this ICollection<T> collection, [NotNull] IEnumerable<T> items)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (items is null)
		{
			throw new ArgumentNullException(nameof(items));
		}

		Contract.Ensures(Contract.Result<IEnumerable<bool>>() != null);

		return items.Select(collection.Remove);
	}

	/// <summary>
	/// Removes the specified range of items from the collection.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="items">The items.</param>
	/// <returns>
	/// An <c>IEnumerable{System.Boolean}</c> indicating whether the items were found and removed.
	/// </returns>
	/// <exception cref="ArgumentNullException">collection or items</exception>
	public static IEnumerable<bool> RemoveRange<T>([NotNull] this ICollection<T> collection, [NotNull] params T[] items)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (items is null)
		{
			throw new ArgumentNullException(nameof(items));
		}

		Contract.Ensures(Contract.Result<IEnumerable<bool>>() != null);

		return RemoveRange(collection, items.AsEnumerable());
	}

	/// <summary>
	/// Synchronizes the items in two collections, performing a minimal number of operations.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	/// <typeparam name="TKey">The type of the attribute key.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="source">The source collection.</param>
	/// <param name="getKey">The function used to get the attribute keys.</param>
	/// <returns>A <c>SyncChanges{T}</c> object containing the changes.</returns>
	/// <remarks>
	/// This preserves existing items in the collection, so selections are not lost when used on
	/// an ObservableCollection.
	/// </remarks>
	/// <exception cref="ArgumentNullException">collection or source or getKey</exception>
	/// <exception cref="ArgumentException">source cannot be empty.</exception>
	public static SyncChanges<T> SyncFrom<T, TKey>(
		[NotNull] this ICollection<T> collection,
		[NotNull] IEnumerable<T> source,
		[NotNull] Func<T, TKey> getKey)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
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

		var keep = new HashSet<TKey>(collection.Select(getKey));

		var sourceArray = source as T[] ?? source.ToArray();

		keep.IntersectWith(sourceArray.Select(getKey));

		foreach (var item in collection
			.Where(
				x =>
				{
					var handler = getKey;
					return handler is not null && !keep.Contains(handler(x));
				})
			.ToArray())
		{
			_ = collection.Remove(item);
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
			collection.Add(item);
			returnValue.Added.Add(item);
		}

		return returnValue;
	}

	/// <summary>
	/// Synchronizes the items in two collections, performing a minimal number of operations.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	/// <typeparam name="TKey">The type of the attribute key.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="newKeys">The new attribute keys.</param>
	/// <param name="getKey">The function used to get the attribute keys.</param>
	/// <param name="getObject">The function used to get the objects.</param>
	/// <returns>A <c>SyncChanges{T}</c> object containing the changes.</returns>
	/// <remarks>
	/// This preserves existing items in the collection, so selections are not lost when used on
	/// an ObservableCollection.
	/// </remarks>
	/// <exception cref="ArgumentNullException">collection or newKeys or getKey or getObject</exception>
	public static SyncChanges<T> SyncFrom<T, TKey>(
		[NotNull] this ICollection<T> collection,
		[NotNull] IEnumerable<TKey> newKeys,
		[NotNull] Func<T, TKey> getKey,
		[NotNull] Func<TKey, T> getObject)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
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
		var keep = new HashSet<TKey>(collection.Select(getKey));
		var newKeysArray = newKeys as TKey[] ?? newKeys.ToArray();

		keep.IntersectWith(newKeysArray);

		foreach (var item in collection
			.Where(
				x =>
				{
					var handler = getKey;
					return handler is not null && !keep.Contains(handler(x));
				})
			.ToArray())
		{
			_ = collection.Remove(item);
			returnValue.Removed.Add(item);
		}

		foreach (var item in newKeysArray.Except(keep).Select(getObject))
		{
			collection.Add(item);
			returnValue.Added.Add(item);
		}

		return returnValue;
	}

	/// <summary>
	/// Trues for all.
	/// </summary>
	/// <typeparam name="T">The type of the elements to find.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="match">The match.</param>
	/// <returns>The true for all.</returns>
	/// <exception cref="ArgumentNullException">collection or match</exception>
	public static bool TrueForAll<T>([NotNull] this ICollection<T> collection, [NotNull] Predicate<T> match)
	{
		if (collection is null)
		{
			throw new ArgumentNullException(nameof(collection));
		}

		if (match is null)
		{
			throw new ArgumentNullException(nameof(match));
		}

		return collection.All(item => match?.Invoke(item) ?? default);
	}
}
