
namespace SharedCode.Events;

/// <summary>
/// Defines a handler for a notification
/// </summary>
/// <typeparam name="TNotification">The type of the notification.</typeparam>
public interface INotificationHandler<in TNotification>
	where TNotification : INotification
{
	/// <summary>
	/// Handles the specified notification.
	/// </summary>
	/// <param name="notification">The notification to handle.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	Task Handle(TNotification notification, CancellationToken? cancellationToken = default);
}
