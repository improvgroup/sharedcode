namespace SharedCode.Web.Middleware;

/// <summary>
/// Represents the details of an error response.
/// </summary>
internal sealed class ErrorDetails
{
    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    public required string Message { get; set; }

    /// <summary>
    /// Gets or sets the status code.
    /// </summary>
    /// <value>The status code.</value>
    public int StatusCode { get; set; }

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}
