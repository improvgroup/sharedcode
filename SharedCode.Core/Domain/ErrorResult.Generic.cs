namespace SharedCode.Domain;

/// <summary>
/// The error result class. Implements the <see cref="IErrorResult"/>.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <seealso cref="IErrorResult"/>
public record class ErrorResult<T> : IErrorResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResult{T}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ErrorResult(string message) : this(message, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResult{T}"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The list of errors.</param>
    public ErrorResult(string message, IReadOnlyCollection<Error> errors)
    {
        this.Message = message;
        this.Errors = errors ?? [];
    }

    /// <summary>
    /// Gets the errors.
    /// </summary>
    /// <value>The errors.</value>
    public IReadOnlyCollection<Error> Errors { get; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    public string Message { get; set; }

    /// <inheritdoc/>
    public bool Success { get; }

    /// <inheritdoc/>
    public T? Value { get; }
}