namespace SharedCode.Domain;

/// <summary>
/// Represents a result of an operation that can either succeed or fail.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="IResult"/> is success.
    /// </summary>
    /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
    bool Success { get; }
}

/// <summary>
/// Represents a result of an operation that can either succeed or fail, with a value.
/// </summary>
/// <typeparam name="T">The type of the value contained in the result.</typeparam>
public interface IResult<out T> : IResult
{
    /// <summary>
    /// Gets the value of the result if it is successful.
    /// </summary>
    T? Value { get; }
}
