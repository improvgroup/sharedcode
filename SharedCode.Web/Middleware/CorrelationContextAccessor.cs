namespace SharedCode.Web.Middleware;

/// <summary>
/// Provides an implementation of <see cref="ICorrelationContextAccessor"/> that uses
/// <see cref="AsyncLocal{T}"/> to store the correlation context.
/// </summary>
/// <seealso cref="ICorrelationContextAccessor"/>
[SuppressMessage(
    "Performance",
    "CA1812",
    Justification = "This class is used by dependency injection and does not need to be instantiated directly.")]
internal sealed class CorrelationContextAccessor : ICorrelationContextAccessor
{
    private static readonly AsyncLocal<CorrelationContext> _correlationContext = new();

    /// <inheritdoc/>
    public CorrelationContext CorrelationContext
    {
        get => _correlationContext.Value
            ?? throw new InvalidOperationException("Correlation context has not been initialized.");
        set => _correlationContext.Value = value
            ?? throw new ArgumentNullException(nameof(value));
    }
}
