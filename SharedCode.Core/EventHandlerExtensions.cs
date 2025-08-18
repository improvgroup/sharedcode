namespace SharedCode;
/// <summary>
/// The event handler extension methods class.
/// </summary>
public static class EventHandlerExtensions
{
	/// <summary>
	/// Executes the handler with the specified sender and empty arguments.
	/// </summary>
	/// <param name="this">The event handler.</param>
	/// <param name="sender">The sender object.</param>
	[SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "This is applied to event handlers.")]
	public static void Raise(this EventHandler @this, object sender) =>
		@this?.Invoke(sender, EventArgs.Empty);

	/// <summary>
	/// Executes the handler with the specified sender and arguments.
	/// </summary>
	/// <typeparam name="T">The type of the value in the arguments.</typeparam>
	/// <param name="this">The event handler.</param>
	/// <param name="sender">The sender object.</param>
	/// <param name="value">The argument value.</param>
	[SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "This is applied to event handlers.")]
	public static void Raise<T>(this EventHandler<EventArgs<T>> @this, object sender, T value) =>
		@this?.Invoke(sender, new EventArgs<T>(value));

	/// <summary>
	/// Executes the handler with the specified sender and arguments.
	/// </summary>
	/// <typeparam name="T">The type of the arguments.</typeparam>
	/// <param name="this">The event handler.</param>
	/// <param name="sender">The sender object.</param>
	/// <param name="value">The event arguments.</param>
	[SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "This is applied to event handlers.")]
	public static void Raise<T>(this EventHandler<T> @this, object sender, T value) where T : EventArgs =>
		@this?.Invoke(sender, value);

	/// <summary>
	/// Executes the handler with the specified sender and arguments.
	/// </summary>
	/// <typeparam name="T">The type of the value in the arguments.</typeparam>
	/// <param name="this">The event handler.</param>
	/// <param name="sender">The sender object.</param>
	/// <param name="value">The event arguments.</param>
	[SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "This is applied to event handlers.")]
	public static void Raise<T>(this EventHandler<EventArgs<T>> @this, object sender, EventArgs<T> value) =>
		@this?.Invoke(sender, value);
}
