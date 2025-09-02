using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace SharedCode.DependencyInjection;

/// <summary>
/// The dependency loader class.
/// </summary>
public static class DependencyLoader
{
    /// <summary>
    /// Loads the dependencies.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="path">The path.</param>
    /// <param name="pattern">The pattern.</param>
    /// <exception cref="TypeLoadException"></exception>
    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
    public static void LoadDependencies(this IServiceCollection serviceCollection, string path, string pattern)
    {
        var directoryCatalog = new DirectoryCatalog(path, pattern);
        var importDefinition = new ImportDefinition(
            _ => true,
            typeof(IDependencyResolver).FullName,
            ImportCardinality.ZeroOrMore,
            false,
            false);

        try
        {
            using var aggregateCatalog = new AggregateCatalog();
            aggregateCatalog.Catalogs.Add(directoryCatalog);

            using var componsitionContainer = new CompositionContainer(aggregateCatalog);
            var exports = componsitionContainer.GetExports(importDefinition);

            var modules =
                exports
                    .Select(export => export.Value as IDependencyResolver)
                    .Where(m => m is not null)
                    .ToList()
                    ?? [];

            var registerComponent = new DependencyRegister(serviceCollection);
            foreach (var module in modules)
            {
                module!.SetUp(registerComponent);
            }
        }
        catch (ReflectionTypeLoadException typeLoadException)
        {
            var builder = new StringBuilder();
            foreach (var loaderException in typeLoadException.LoaderExceptions)
            {
                if (loaderException is not null)
                {
                    _ = builder.AppendFormat(CultureInfo.CurrentCulture, "{0}\n", loaderException.Message);
                }
            }

            throw new TypeLoadException(builder.ToString(), typeLoadException);
        }
    }
}
