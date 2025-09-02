using Microsoft.Extensions.DependencyInjection;

namespace SharedCode.DependencyInjection;

/// <summary>
/// The catalog selector class. Implements the <see cref="ICatalogSelector"/>.
/// </summary>
/// <seealso cref="ICatalogSelector"/>
/// <remarks>Initializes a new instance of the <see cref="CatalogSelector"/> class.</remarks>
/// <param name="typeCatalog">The type catalogs.</param>
/// <param name="services">The services collection.</param>
public class CatalogSelector(IEnumerable<Type> typeCatalog, IServiceCollection services) : ICatalogSelector
{
    /// <summary>
    /// Registers the catalogs.
    /// </summary>
    public void RegisterCatalogs()
    {
        foreach (var type in typeCatalog)
        {
            _ = Activator.CreateInstance(type, services);
        }
    }
}
