namespace SharedCode;

using System;

/// <summary>
/// An event arguments class with a value of T.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class EventArgs<T> : EventArgs
{
	/// <summary>
	/// Initializes a new instance of <see cref="EventArgs{T}" />.
	/// </summary>
	/// <param name="value">The value.</param>
	public EventArgs(T? value = default) => this.Value = value;

	/// <summary>
	/// The value.
	/// </summary>
#if NET5_0_OR_GREATER
	public T? Value { get; init; }
#else
	public T? Value { get; set; }
#endif
}
