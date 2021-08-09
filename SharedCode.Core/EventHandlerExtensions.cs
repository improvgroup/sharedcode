namespace SharedCode
{
	using System;

	/// <summary>
	/// The event handler extension methods class.
	/// </summary>
	public static class EventHandlerExtensions
	{
		/// <summary>
		/// Executes the handler with the specified sender and empty arguments.
		/// </summary>
		/// <param name="handler">The event handler.</param>
		/// <param name="sender">The sender.</param>
		public static void Raise(this EventHandler handler, object sender) => handler?.Invoke(sender, EventArgs.Empty);

		/// <summary>
		/// Executes the handler with the specified sender and arguments.
		/// </summary>
		/// <typeparam name="T">The type of the value in the arguments.</typeparam>
		/// <param name="handler">The event handler.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="value">The value.</param>
		public static void Raise<T>(this EventHandler<EventArgs<T>> handler, object sender, T value) => handler?.Invoke(sender, new EventArgs<T>(value));

		/// <summary>
		/// Executes the handler with the specified sender and arguments.
		/// </summary>
		/// <typeparam name="T">The type of the arguments.</typeparam>
		/// <param name="handler">The event handler.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="value">The event arguments.</param>
		public static void Raise<T>(this EventHandler<T> handler, object sender, T value) where T : EventArgs => handler?.Invoke(sender, value);

		/// <summary>
		/// Executes the handler with the specified sender and arguments.
		/// </summary>
		/// <typeparam name="T">The type of the value in the arguments.</typeparam>
		/// <param name="handler">The event handler.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="value">The event arguments.</param>
		public static void Raise<T>(this EventHandler<EventArgs<T>> handler, object sender, EventArgs<T> value) => handler?.Invoke(sender, value);
	}
}
