// <copyright file="NotificationService.cs" company="improvGroup, LLC">
//     Copyright © 2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Notifications;

using System;
using System.Threading.Tasks;

/// <summary>
/// The notification service class. Implements the <see cref="INotificationService" />.
/// </summary>
/// <seealso cref="INotificationService" />
[Obsolete("Use CommunityToolkit.Mvvm WeakReferenceMessenger instead.")]
public class NotificationService : INotificationService
{
	/// <summary>
	/// The producer/consumer.
	/// </summary>
	private readonly ProducerConsumer producerConsumer = new();

	/// <summary>
	/// A value indicating whether this instance has been disposed.
	/// </summary>
	private bool disposed;

	/// <summary>
	/// Finalizes an instance of the <see cref="NotificationService" /> class.
	/// </summary>
	~NotificationService()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		this.Dispose(disposing: false);
	}

	/// <inheritdoc />
	public IObservable<TNotification> Subscribe<TNotification>() where TNotification : Notification => this.producerConsumer.Consume<TNotification>();

	/// <inheritdoc />
	public IDisposable Subscribe<TNotification>(Action<TNotification> handler) where TNotification : Notification => this.producerConsumer.Consume(handler);

	/// <inheritdoc />
	public IDisposable Subscribe<TNotification>(Func<TNotification, Task> handler, Action<Exception>? errorHandler = null) where TNotification : Notification => this.producerConsumer.Consume(handler, errorHandler);

	/// <inheritdoc />
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		this.Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <inheritdoc />
	public void Publish<TNotification>(TNotification notification) where TNotification : notnull, Notification => this.producerConsumer.Produce(notification);

	/// <summary>
	/// Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing">
	/// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
	/// unmanaged resources.
	/// </param>
	protected virtual void Dispose(bool disposing)
	{
		if (!this.disposed)
		{
			if (disposing)
			{
				// dispose managed state (managed objects)
				this.producerConsumer?.Dispose();
			}

			// free unmanaged resources (unmanaged objects) and override finalizer, set large fields
			// to null
			this.disposed = true;
		}
	}
}
