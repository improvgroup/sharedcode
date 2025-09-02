using Microsoft.Extensions.DependencyInjection;

namespace SharedCode.DependencyInjection;

/// <summary>
/// The service catalog class.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="Catalog"/> class.</remarks>
/// <param name="services">The service collection.</param>
public abstract class Catalog(IServiceCollection services)
{
    /// <summary>
    /// Gets the service collection.
    /// </summary>
    /// <value>The service collection.</value>
    public IServiceCollection Services { get; } = services;
}
