namespace SharedCode.Notifications;
/// <summary>
/// The notification service interface. Implements the <see cref="IDisposable" />.
/// </summary>
/// <seealso cref="IDisposable" />
public interface INotificationService : IDisposable
{
	/// <summary>
	/// Consumes a type of notification.
	/// </summary>
	/// <typeparam name="TNotification">The type of notifications you are consuming.</typeparam>
	/// <returns>An <see cref="IObservable{TNotification}"/>.</returns>
	IObservable<TNotification> Subscribe<TNotification>() where TNotification : Notification;

	/// <summary>
	/// Consumes a type of notification.
	/// </summary>
	/// <typeparam name="TNotification">The type of notifications you are consuming.</typeparam>
	/// <param name="handler">Action to invoke when a notification is produced.</param>
	/// <returns>The <see cref="IDisposable" /> subscription.</returns>
	/// <remarks>Disposing the return value unsubscribes from the notifications.</remarks>
	IDisposable Subscribe<TNotification>(Action<TNotification> handler) where TNotification : Notification;

	/// <summary>
	/// Consumes a type of notification.
	/// </summary>
	/// <typeparam name="TNotification">The type of the notification.</typeparam>
	/// <param name="handler">Action to invoke when notification is produced.</param>
	/// <param name="errorHandler">The error handler.</param>
	/// <returns>The <see cref="IDisposable" /> subscription.</returns>
	/// <remarks>Disposing the return value unsubscribes from the notifications.</remarks>
	IDisposable Subscribe<TNotification>(Func<TNotification, Task> handler, Action<Exception>? errorHandler = null)
		where TNotification : Notification;

	/// <summary>
	/// Produces the specified notification.
	/// </summary>
	/// <typeparam name="TNotification">The type of the notification.</typeparam>
	/// <param name="notification">The notification to produce.</param>
	void Publish<TNotification>(TNotification notification) where TNotification : notnull, Notification;
}
