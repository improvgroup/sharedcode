// <copyright file="ListExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Collections;

using System.Collections.Generic;

/// <summary>
/// The list extensions class
/// </summary>
public static class ListExtensions
{
	/// <summary>
	/// Determines whether a list is not null or empty.
	/// </summary>
	/// <typeparam name="T">The item type.</typeparam>
	/// <param name="items">The list.</param>
	/// <returns><c>true</c> if this list is not null or empty; otherwise, <c>false</c>.</returns>
	public static bool IsNotNullOrEmpty<T>(this IList<T> items) => items?.Count > 0;

	/// <summary>
	/// Determines whether a list is null or empty.
	/// </summary>
	/// <typeparam name="T">The item type.</typeparam>
	/// <param name="items">The list.</param>
	/// <returns><c>true</c> if this list is null or empty; otherwise, <c>false</c>.</returns>
	public static bool IsNullOrEmpty<T>(this IList<T> items) => items is null || items.Count == 0;

	/// <summary>
	/// This extension method replaces an item in a collection that implements the IList interface.
	/// </summary>
	/// <typeparam name="T">The type of the field that we are manipulating</typeparam>
	/// <param name="thisList">The input list</param>
	/// <param name="position">The position of the old item</param>
	/// <param name="item">The item we are goint to put in it's place</param>
	/// <returns>True in case of a replace, false if failed</returns>
	public static bool Replace<T>(this IList<T?> thisList, int position, T? item)
	{
		// only process if inside the range of this list
		if (thisList is null || position > thisList.Count - 1)
		{
			return false;
		}

		// remove the old item
		thisList.RemoveAt(position);

		// insert the new item at its position
		thisList.Insert(position, item);

		// return success
		return true;
	}
}
