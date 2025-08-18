namespace SharedCode.Collections;
/// <summary>
/// The list extensions class
/// </summary>
public static class ListExtensions
{
	/// <summary>
	/// Determines whether a list is not null or empty.
	/// </summary>
	/// <typeparam name="T">The item type.</typeparam>
	/// <param name="this">The list.</param>
	/// <returns><c>true</c> if this list is not null or empty; otherwise, <c>false</c>.</returns>
	public static bool IsNotNullOrEmpty<T>(this IList<T> @this) => @this?.Count > 0;

	/// <summary>
	/// Determines whether a list is null or empty.
	/// </summary>
	/// <typeparam name="T">The item type.</typeparam>
	/// <param name="this">The list.</param>
	/// <returns><c>true</c> if this list is null or empty; otherwise, <c>false</c>.</returns>
	public static bool IsNullOrEmpty<T>(this IList<T> @this) => @this is null || @this.Count == 0;

	/// <summary>
	/// This extension method replaces an item in a collection that implements the IList interface.
	/// </summary>
	/// <typeparam name="T">The type of the field that we are manipulating</typeparam>
	/// <param name="this">The input list</param>
	/// <param name="position">The position of the old item</param>
	/// <param name="item">The item we are goint to put in it's place</param>
	/// <returns>True in case of a replace, false if failed</returns>
	public static bool Replace<T>(this IList<T?> @this, int position, T? item)
	{
		// only process if inside the range of this list
		if (@this is null || position > @this.Count - 1)
		{
			return false;
		}

		// remove the old item
		@this.RemoveAt(position);

		// insert the new item at its position
		@this.Insert(position, item);

		// return success
		return true;
	}
}
