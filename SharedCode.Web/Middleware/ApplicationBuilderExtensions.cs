namespace SharedCode.Web.Middleware;

/// <summary>
/// Provides extension methods for configuring the application builder to use correlation ID
/// middleware.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Uses the correlation identifier.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>Microsoft.AspNetCore.Builder.IApplicationBuilder.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.ApplicationServices.GetService(typeof(ICorrelationContextAccessor)) is null
            ? throw new InvalidOperationException("ICorrelationContextAccessor is not registered. Please call services.AddCorrelationContext() in ConfigureServices.")
            : app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
