using Microsoft.Extensions.DependencyInjection;

namespace SharedCode.DependencyInjection;

/// <summary>
/// The dependency register class. Implements the <see cref="IDependencyRegister"/>.
/// </summary>
/// <seealso cref="IDependencyRegister"/>
/// <remarks>Initializes a new instance of the <see cref="DependencyRegister"/> class.</remarks>
/// <param name="serviceCollection">The service collection.</param>
public sealed class DependencyRegister(IServiceCollection serviceCollection) : IDependencyRegister
{
    /// <inheritdoc/>
    void IDependencyRegister.AddScoped<TService>() => serviceCollection.AddScoped<TService>();

    /// <inheritdoc/>
    void IDependencyRegister.AddScoped<TService, TImplementation>() => serviceCollection.AddScoped<TService, TImplementation>();

    /// <inheritdoc/>
    void IDependencyRegister.AddScopedForMultiImplementation<TService, TImplementation>()
    {
        _ = serviceCollection
            .AddScoped<TImplementation>()
            .AddScoped<TService, TImplementation>(
                s =>
                    s.GetService<TImplementation>()
                        ?? throw new ArgumentException("Resolved TImplementation service instance cannot be null.", nameof(TImplementation)));
    }

    /// <inheritdoc/>
    void IDependencyRegister.AddSingleton<TService>() => serviceCollection.AddSingleton<TService>();

    /// <inheritdoc/>
    void IDependencyRegister.AddSingleton<TService, TImplementation>() => serviceCollection.AddSingleton<TService, TImplementation>();

    /// <inheritdoc/>
    void IDependencyRegister.AddTransient<TService>() => serviceCollection.AddTransient<TService>();

    /// <inheritdoc/>
    void IDependencyRegister.AddTransient<TService, TImplementation>() => serviceCollection.AddTransient<TService, TImplementation>();

    /// <inheritdoc/>
    void IDependencyRegister.AddTransientForMultiImplementation<TService, TImplementation>()
    {
        _ = serviceCollection
            .AddTransient<TImplementation>()
            .AddTransient<TService, TImplementation>(
                s =>
                    s.GetService<TImplementation>()
                        ?? throw new ArgumentException("Resolved TImplementation service instance cannot be null.", nameof(TImplementation)));
    }
}
