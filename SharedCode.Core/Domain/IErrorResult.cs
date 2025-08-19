namespace SharedCode.Domain;

/// <summary>
/// The error result interface.
/// </summary>
public interface IErrorResult : IResult
{
    /// <summary>
    /// Gets the errors.
    /// </summary>
    /// <value>The errors.</value>
    IReadOnlyCollection<Error> Errors { get; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    string Message { get; }
}

/// <summary>
/// Represents a result of an operation that can either succeed or fail, with a value and errors.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public interface IErrorResult<out T> : IErrorResult, IResult<T>
{
}