namespace SharedCode.DependencyInjection;

/// <summary>
/// The catalog selector interface.
/// </summary>
public interface ICatalogSelector
{
    /// <summary>
    /// Registers the catalogs.
    /// </summary>
    void RegisterCatalogs();
}
