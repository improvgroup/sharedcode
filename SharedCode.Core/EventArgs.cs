namespace SharedCode;

/// <summary>
/// An event arguments class with a value of T.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <remarks>Initializes a new instance of <see cref="EventArgs{T}"/>.</remarks>
/// <param name="value">The value.</param>
public class EventArgs<T>(T? value = default) : EventArgs
{
	/// <summary>
	/// The value.
	/// </summary>
	public T? Value { get; init; } = value;
}
