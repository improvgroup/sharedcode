using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SharedCode.Web.Middleware;

/// <summary>
/// Provides extension methods for configuring the service collection to use correlation ID
/// middleware.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the correlation context.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddCorrelationContext(this IServiceCollection services)
    {
        services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
        return services.AddTransient(sp => sp.GetRequiredService<ICorrelationContextAccessor>().CorrelationContext);
    }
}
