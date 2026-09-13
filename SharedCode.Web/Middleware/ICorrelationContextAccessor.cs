namespace SharedCode.Web.Middleware;

/// <summary>
/// Defines an interface for accessing the correlation context.
/// </summary>
public interface ICorrelationContextAccessor
{
    /// <summary>
    /// Gets or sets the correlation context.
    /// </summary>
    /// <value>The correlation context.</value>
    CorrelationContext CorrelationContext { get; set; }
}
