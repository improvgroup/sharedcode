using Microsoft.Extensions.Primitives;

namespace SharedCode.Web.Middleware;

/// <summary>
/// Represents the correlation context for a request, containing the correlation ID.
/// </summary>
public record class CorrelationContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationContext"/> class.
    /// </summary>
    /// <param name="correlationId">
    /// The correlation identifier to use. If null, empty, or whitespace, a new GUID string is
    /// generated.
    /// </param>
    public CorrelationContext(string correlationId) =>
        this.CorrelationId = string.IsNullOrWhiteSpace(correlationId)
            ? Guid.NewGuid().ToString()
            : correlationId;

    /// <summary>
    /// Gets the correlation ID associated with the request.
    /// </summary>
    public string CorrelationId { get; }

    /// <summary>
    /// Implicitly converts a correlation ID string to a <see cref="CorrelationContext"/>.
    /// </summary>
    /// <param name="correlationId">The correlation identifier to wrap.</param>
    /// <returns>
    /// A <see cref="CorrelationContext"/> containing the specified correlation ID.
    /// </returns>
    public static implicit operator CorrelationContext(string correlationId) => FromString(correlationId);

    /// <summary>
    /// Converts a correlation ID string to a <see cref="CorrelationContext"/>.
    /// </summary>
    /// <param name="correlationId">The correlation identifier to wrap.</param>
    /// <returns>
    /// A <see cref="CorrelationContext"/> containing the specified correlation ID.
    /// </returns>
    public static CorrelationContext FromString(string correlationId) => new(correlationId);

    /// <summary>
    /// Implicitly converts <see cref="StringValues"/> to a <see cref="CorrelationContext"/>.
    /// </summary>
    /// <param name="correlationId">The correlation identifier values to wrap.</param>
    /// <returns>
    /// A <see cref="CorrelationContext"/> containing the specified correlation ID.
    /// </returns>
    public static implicit operator CorrelationContext(StringValues correlationId) => FromStringValues(correlationId);

    /// <summary>
    /// Converts <see cref="StringValues"/> to a <see cref="CorrelationContext"/>.
    /// </summary>
    /// <param name="correlationId">The correlation identifier values to wrap.</param>
    /// <returns>
    /// A <see cref="CorrelationContext"/> containing the specified correlation ID.
    /// </returns>
    public static CorrelationContext FromStringValues(StringValues correlationId) => new(correlationId.ToString());

    /// <summary>
    /// Implicitly converts a <see cref="Guid"/> to a <see cref="CorrelationContext"/>.
    /// </summary>
    /// <param name="correlationId">The correlation identifier to wrap.</param>
    /// <returns>
    /// A <see cref="CorrelationContext"/> containing the specified correlation ID.
    /// </returns>
    public static implicit operator CorrelationContext(Guid correlationId) => FromGuid(correlationId);

    /// <summary>
    /// Converts a <see cref="Guid"/> to a <see cref="CorrelationContext"/>.
    /// </summary>
    /// <param name="correlationId">The correlation identifier to wrap.</param>
    /// <returns>
    /// A <see cref="CorrelationContext"/> containing the specified correlation ID.
    /// </returns>
    public static CorrelationContext FromGuid(Guid correlationId) => new(correlationId.ToString());

    /// <summary>
    /// Implicitly converts a <see cref="CorrelationContext"/> to its correlation ID string.
    /// </summary>
    /// <param name="correlationId">The correlation context to convert.</param>
    /// <returns>The correlation ID value.</returns>
    public static implicit operator string(CorrelationContext correlationId) => ToStringValue(correlationId);

    /// <summary>
    /// Converts a <see cref="CorrelationContext"/> to its correlation ID string.
    /// </summary>
    /// <param name="correlationId">The correlation context to convert.</param>
    /// <returns>The correlation ID value.</returns>
    public static string ToStringValue(CorrelationContext correlationId) => correlationId?.CorrelationId ?? string.Empty;
}
