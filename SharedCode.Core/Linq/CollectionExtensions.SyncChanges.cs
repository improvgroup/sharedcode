namespace SharedCode.Linq;
public static partial class CollectionExtensions
{
	/// <summary>
	/// Describes changes made while syncing a collection.
	/// </summary>
	/// <typeparam name="T">The type of item synced.</typeparam>
	[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "<Pending>")]
	public sealed class SyncChanges<T>
	{
		/// <summary>
		/// The added items.
		/// </summary>
		private readonly IList<T> added = new List<T>();

		/// <summary>
		/// The removed items.
		/// </summary>
		private readonly IList<T> removed = new List<T>();

		/// <summary>
		/// Gets the items added during the sync.
		/// </summary>
		/// <value>The added items.</value>
		public IList<T> Added
		{
			get
			{
				Contract.Ensures(Contract.Result<IList<T>>() is not null);

				return this.added;
			}
		}

		/// <summary>
		/// Gets the items removed during the sync.
		/// </summary>
		/// <value>The removed items.</value>
		public IList<T> Removed
		{
			get
			{
				Contract.Ensures(Contract.Result<IList<T>>() is not null);

				return this.removed;
			}
		}
	}
}
