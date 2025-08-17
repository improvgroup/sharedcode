namespace SharedCode
{
	/// <summary>
	/// Represents a result that contains a collection of items.
	/// </summary>
	/// <typeparam name="T">
	/// The type of items in the collection.
	/// </typeparam>
	public interface ICollectionResult<T> : IResult
	{
		/// <summary>
		/// Gets the collection of items.
		/// </summary>
		/// <value>
		/// The collection of items, or <c>null</c> if there are no items.
		/// </value>
		ICollection<T>? Collection { get; }

		/// <summary>
		/// Gets a value indicating whether the collection is empty.
		/// </summary>
		/// <value>
		/// <c>true</c> if the collection is empty; otherwise, <c>false</c>.
		/// </value>
		bool IsEmpty { get; }
	}
}
