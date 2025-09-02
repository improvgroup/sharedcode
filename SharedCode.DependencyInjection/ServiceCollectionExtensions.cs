using Microsoft.Extensions.DependencyInjection;

namespace SharedCode.DependencyInjection;

/// <summary>
/// The service collection extension methods class.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the catalog.
    /// </summary>
    /// <typeparam name="T">The type of the catalog.</typeparam>
    /// <param name="services">The services.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection RegisterCatalog<T>(this IServiceCollection services) where T : Catalog
    {
        _ = Activator.CreateInstance(typeof(T), services);
        return services;
    }

    /// <summary>
    /// Adds registrations to the <paramref name="services"/> collection using conventions specified
    /// using the <paramref name="action"/>.
    /// </summary>
    /// <param name="services">The services to add to.</param>
    /// <param name="action">The configuration action.</param>
    /// <returns>The service collection.</returns>
    /// <exception cref="ArgumentNullException">services</exception>
    /// <exception cref="ArgumentNullException">action</exception>
    public static IServiceCollection Scan(this IServiceCollection services, Action<ITypeSourceSelector> action)
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        var selector = new TypeSourceSelector(services);

        action(selector);

        return services;
    }
}
