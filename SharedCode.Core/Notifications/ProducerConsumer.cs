


namespace SharedCode.Notifications;

using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

/// <summary>
/// The producer/consumer class.
/// </summary>
public class ProducerConsumer : IConsumer, IDisposable, IProducer
{
	/// <summary>
	/// A value indicating whether this instance has been disposed.
	/// </summary>
	private bool disposed;

	/// <summary>
	/// The subject
	/// </summary>
	private readonly ReplaySubject<object> subject = new();

	/// <summary>
	/// The subscriptions
	/// </summary>
	private readonly ConcurrentBag<IDisposable> subscriptions = new();

	/// <summary>
	/// Finalizes an instance of the <see cref="ProducerConsumer" /> class.
	/// </summary>
	~ProducerConsumer()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		this.Dispose(disposing: false);
	}

	/// <inheritdoc />
	public IObservable<T> Consume<T>() => this.subject.OfType<T>().AsObservable();

	/// <summary>
	/// Consumes the specified type with the handler action.
	/// </summary>
	/// <typeparam name="T">The type of the thing being consumed.</typeparam>
	/// <param name="handler">The handler action.</param>
	/// <returns>A <see cref="IDisposable"/> subscription.</returns>
	/// <remarks>
	/// The handler is unsubscribed when the subscription is disposed.
	/// </remarks>
	public IDisposable Consume<T>(Action<T> handler)
	{
		var subscription = this.Consume<T>().Subscribe(handler);
		this.subscriptions.Add(subscription);
		return subscription;
	}

	/// <summary>
	/// Consumes the specified type with the handler function.
	/// </summary>
	/// <typeparam name="T">The type of the thing being consumed.</typeparam>
	/// <param name="handler">The handler function.</param>
	/// <param name="errorHandler">The error handler.</param>
	/// <returns>A <see cref="IDisposable"/> subscription.</returns>
	/// <remarks>
	/// The handler is unsubscribed when the subscription is disposed.
	/// </remarks>
	/// <returns>IDisposable.</returns>
	public IDisposable Consume<T>(Func<T, Task> handler, Action<Exception>? errorHandler = null)
	{
		var subscription = this.Consume<T>().Subscribe(handler, errorHandler);
		this.subscriptions.Add(subscription);
		return subscription;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		this.Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <inheritdoc />
	public void Produce<T>(T produced) where T : notnull => this.subject.OnNext(produced);

	/// <summary>
	/// Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing">
	/// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
	/// unmanaged resources.
	/// </param>
	protected virtual void Dispose(bool disposing)
	{
		if (this.disposed)
		{
			return;
		}

		if (disposing)
		{
			// dispose managed state (managed objects)
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			foreach (var subscription in this.subscriptions)
			{
				subscription?.Dispose();
			}

			this.subscriptions.Clear();
#else
			while (!this.subscriptions.IsEmpty)
			{
				if (this.subscriptions.TryTake(out var subscription))
				{
					subscription?.Dispose();
				}
			}
#endif
			this.subject?.Dispose();
		}

		// free unmanaged resources (unmanaged objects) and override finalizer, set large fields to null
		this.disposed = true;
	}
}
