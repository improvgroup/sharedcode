using Microsoft.Extensions.Primitives;

namespace SharedCode.Web.Middleware;

/// <summary>
/// Represents a middleware that adds a correlation identifier to the HTTP request and response
/// headers.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
/// </remarks>
/// <param name="next">The next.</param>
public class CorrelationIdMiddleware(RequestDelegate next)
{
    private readonly string _header = "X-Correlation-ID";

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="correlationContextAccessor">The correlation context accessor.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task Invoke(HttpContext context, ICorrelationContextAccessor correlationContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(context);

        var correlationId = this.GetCorrelationId(context);

        context.TraceIdentifier = correlationId.ToString();
        correlationContextAccessor?.CorrelationContext = (CorrelationContext)correlationId;

        context.Response.OnStarting(
            () =>
            {
                if (!context.Response.Headers.ContainsKey(this._header))
                {
                    context.Response.Headers.Append(this._header, correlationId);
                }

                return Task.CompletedTask;
            });

        await next(context).ConfigureAwait(true);
    }

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Pattern.")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Pattern.")]
    private static StringValues GenerateCorrelationId(string traceIdentifier) => Guid.NewGuid().ToString();

    private static bool RequiresGenerationOfCorrelationId(bool idInHeader, StringValues idFromHeader) =>
        !idInHeader || string.IsNullOrWhiteSpace(idFromHeader);

    private StringValues GetCorrelationId(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var correlationIdFoundInRequestHeader =
            context.Request.Headers.TryGetValue(this._header, out var correlationId);

        if (RequiresGenerationOfCorrelationId(correlationIdFoundInRequestHeader, correlationId))
        {
            correlationId = GenerateCorrelationId(context.TraceIdentifier);
        }

        return correlationId;
    }
}
