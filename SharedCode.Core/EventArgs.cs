namespace SharedCode
{
	using System;

	/// <summary>
	/// An event arguments class with a parameter of T.
	/// </summary>
	/// <typeparam name="T">The type of the parameter.</typeparam>
	public class EventArgs<T> : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of <see cref="EventArgs{T}"/>.
		/// </summary>
		/// <param name="item">The parameter.</param>
		public EventArgs(T? item = default) => this.Item = item;

		/// <summary>
		/// The parameter.
		/// </summary>
		public T? Item { get; set; }
	}
}
