﻿
namespace SharedCode.Events;

/// <summary>
/// The domain event class. Implements the <see cref="INotification" />.
/// </summary>
/// <seealso cref="INotification" />
public abstract class DomainEvent : INotification
{
	/// <summary>
	/// Gets or sets the time the event occurred.
	/// </summary>
	/// <value>The time the event occurred.</value>
	public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
}
