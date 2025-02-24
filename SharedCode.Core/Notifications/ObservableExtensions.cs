


namespace SharedCode.Notifications;

using System.Reactive;
using System.Reactive.Linq;

/// <summary>
/// The observable extension methods class.
/// </summary>
public static class ObservableExtensions
{
	/// <summary>
	/// Subscribes to the observable source with the specified asynchronous action.
	/// </summary>
	/// <typeparam name="T">The type of items in the source.</typeparam>
	/// <param name="source">The observable source.</param>
	/// <param name="handler">The asynchronous handler action.</param>
	/// <param name="errorHandler">The error handler method (optional).</param>
	/// <returns>A <see cref="IDisposable" /> subscription.</returns>
	public static IDisposable Subscribe<T>(this IObservable<T> source, Func<Task> handler, Action<Exception>? errorHandler = null)
	{
		async Task<Unit> Wrapped(T _)
		{
			await handler().ConfigureAwait(false);
			return Unit.Default;
		}

		return errorHandler is null
			? source.SelectMany(Wrapped).Subscribe(_ => { })
			: source.SelectMany(Wrapped).Subscribe(_ => { }, errorHandler);
	}

	/// <summary>
	/// Subscribes to the observable source with the specified asynchronous handler function.
	/// </summary>
	/// <typeparam name="T">The type of items in the source.</typeparam>
	/// <param name="source">The source.</param>
	/// <param name="handler">The asynchronous handler function.</param>
	/// <param name="errorHandler">The error handler (optional).</param>
	/// <returns>A <see cref="IDisposable" /> subscription.</returns>
	public static IDisposable Subscribe<T>(this IObservable<T> source, Func<T, Task> handler, Action<Exception>? errorHandler = null)
	{
		async Task<Unit> Wrapped(T t)
		{
			await handler(t).ConfigureAwait(false);
			return Unit.Default;
		}

		return errorHandler is null
			? source.SelectMany(Wrapped).Subscribe(_ => { })
			: source.SelectMany(Wrapped).Subscribe(_ => { }, errorHandler);
	}
}
