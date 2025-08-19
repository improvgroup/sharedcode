namespace SharedCode.Domain;

/// <summary>
/// The error result class. Implements the <see cref="IErrorResult"/>.
/// </summary>
/// <seealso cref="IErrorResult"/>
public record class ErrorResult : IErrorResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResult"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ErrorResult(string message) : this(message, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResult"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The list of errors.</param>
    public ErrorResult(string message, IReadOnlyCollection<Error> errors)
    {
        this.Message = message;
        this.Success = false;
        this.Errors = errors ?? [];
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<Error> Errors { get; }

    /// <inheritdoc/>
    public string Message { get; }

    /// <inheritdoc/>
    public bool Success { get; }
}