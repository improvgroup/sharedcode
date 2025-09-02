using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SharedCode.DependencyInjection;

/// <summary>
/// The type source selector class. Implements the <see cref="ITypeSourceSelector"/>.
/// </summary>
/// <seealso cref="ITypeSourceSelector"/>
/// <remarks>Initializes a new instance of the <see cref="TypeSourceSelector"/> class.</remarks>
/// <param name="services">The service collection.</param>
internal sealed class TypeSourceSelector(IServiceCollection services) : ITypeSourceSelector
{
    /// <summary>
    /// Gets the service collection.
    /// </summary>
    /// <value>The service collection.</value>
    public IServiceCollection Services { get; } = services;

    /// <summary>
    /// Returns a catalog selector from the application dependencies.
    /// </summary>
    /// <returns>
    /// The <see cref="ICatalogSelector"/>.
    /// </returns>
    public ICatalogSelector FromApplicationDependencies() => this.FromApplicationDependencies(_ => true);

    /// <summary>
    /// Returns a catalog selector from the specified predicate.
    /// </summary>
    /// <param name="predicate">
    /// The predicate that determines if the assembly is included by the selector.
    /// </param>
    /// <returns>
    /// The <see cref="ICatalogSelector"/>.
    /// </returns>
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "We are ignoring any load issues and falling back to the entry assembly.")]
    public ICatalogSelector FromApplicationDependencies(Func<Assembly, bool> predicate)
    {
        try
        {
            return this.FromDependencyContext(DependencyContext.Default!, predicate);
        }
        catch
        {
            // No logging is needed
            // Something went wrong when loading the DependencyContext, fall back to loading all
            // referenced assemblies of the entry assembly…
            return this.FromAssemblyDependencies(Assembly.GetEntryAssembly()!);
        }
    }

    /// <summary>
    /// Returns a catalog selector from the specified assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to search.</param>
    /// <returns>The <see cref="ICatalogSelector"/>.</returns>
    /// <exception cref="ArgumentNullException">assemblies</exception>
    public ICatalogSelector FromAssemblies(params Assembly[] assemblies)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(assemblies);
#else
        if (assemblies is null)
        {
            throw new ArgumentNullException(nameof(assemblies));
        }
#endif

        return this.InternalFromAssemblies(assemblies);
    }

    /// <summary>
    /// Returns a catalog selector from the specified assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to search.</param>
    /// <returns>The <see cref="ICatalogSelector"/>.</returns>
    /// <exception cref="ArgumentNullException">assemblies</exception>
    public ICatalogSelector FromAssemblies(IEnumerable<Assembly> assemblies)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(assemblies);
#else
        if (assemblies is null)
        {
            throw new ArgumentNullException(nameof(assemblies));
        }
#endif

        return this.InternalFromAssemblies(assemblies);
    }

    /// <summary>
    /// Returns a catalog selector from the specified assemblies of types.
    /// </summary>
    /// <param name="types">The types to search.</param>
    /// <returns>The <see cref="ICatalogSelector"/>.</returns>
    /// <exception cref="ArgumentNullException">types</exception>
    public ICatalogSelector FromAssembliesOf(params Type[] types)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(types);
#else
        if (types is null)
        {
            throw new ArgumentNullException(nameof(types));
        }
#endif

        return this.InternalFromAssembliesOf(types.Select(x => x.GetTypeInfo()));
    }

    /// <summary>
    /// Returns a catalog selector from the specified assemblies of types.
    /// </summary>
    /// <param name="types">The types to search.</param>
    /// <returns>The <see cref="ICatalogSelector"/>.</returns>
    /// <exception cref="ArgumentNullException">types</exception>
    public ICatalogSelector FromAssembliesOf(IEnumerable<Type> types)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(types);
#else
        if (types is null)
        {
            throw new ArgumentNullException(nameof(types));
        }
#endif

        return this.InternalFromAssembliesOf(types.Select(t => t.GetTypeInfo()));
    }

    /// <summary>
    /// Returns a catalog selector from the specified assembly dependencies.
    /// </summary>
    /// <param name="assembly">The assembly to search.</param>
    /// <returns>The <see cref="ICatalogSelector"/>.</returns>
    /// <exception cref="ArgumentNullException">assembly</exception>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignoring assembly load exceptions.")]
    public ICatalogSelector FromAssemblyDependencies(Assembly assembly)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(assembly);
#else
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }
#endif

        var assemblies = new List<Assembly> { assembly };

        try
        {
            foreach (var dependencyName in assembly.GetReferencedAssemblies())
            {
                try
                {
                    // Try to load the referenced assembly...
                    assemblies.Add(Assembly.Load(dependencyName));
                }
                catch
                {
                    // No logging is needed
                    // Failed to load assembly. Skip it.
                }
            }

            return this.InternalFromAssemblies(assemblies);
        }
        catch
        {
            // No logging is needed
            return this.InternalFromAssemblies(assemblies);
        }
    }

    /// <inheritdoc/>
    public ICatalogSelector FromAssemblyOf<T>() =>
        this.InternalFromAssembliesOf([typeof(T).GetTypeInfo()]);

    /// <inheritdoc/>
    public ICatalogSelector FromCallingAssembly() =>
        this.FromAssemblies(Assembly.GetCallingAssembly());

    /// <inheritdoc/>
    public ICatalogSelector FromDependencyContext(DependencyContext context) =>
        this.FromDependencyContext(context, _ => true);

    /// <inheritdoc/>
    public ICatalogSelector FromDependencyContext(DependencyContext context, Func<Assembly, bool> predicate)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(predicate);
#else
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }
#endif

        var assemblies = context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .Select(Assembly.Load)
            .Where(predicate)
            .ToArray();

        return this.InternalFromAssemblies(assemblies);
    }

    /// <inheritdoc/>
    public ICatalogSelector FromEntryAssembly() =>
        this.FromAssemblies(Assembly.GetEntryAssembly()!);

    /// <inheritdoc/>
    public ICatalogSelector FromExecutingAssembly() => this.FromAssemblies(Assembly.GetExecutingAssembly());

    private CatalogSelector InternalFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        return new(
            assemblies
                .SelectMany(asm => asm.DefinedTypes)
                .Where(x => x.IsAssignableTo(typeof(Catalog)))
                .Select(x => x.AsType()),
            this.Services);
    }

    private CatalogSelector InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos) =>
        this.InternalFromAssemblies(typeInfos.Select(t => t.Assembly));
}
