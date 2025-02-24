


namespace SharedCode.Notifications;

/// <summary>
/// The consumer interface.
/// </summary>
public interface IConsumer
{
	/// <summary>
	/// Consumes the specified type of objects.
	/// </summary>
	/// <typeparam name="T">The type of the objects being consumed.</typeparam>
	/// <returns>An <see cref="IObservable{T}" />.</returns>
	IObservable<T> Consume<T>();
}
