
namespace SharedCode.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

/// <summary>
/// The type source selector class. Implements the <see cref="ITypeSourceSelector" />.
/// </summary>
/// <seealso cref="ITypeSourceSelector" />
/// <remarks>
/// Initializes a new instance of the <see cref="TypeSourceSelector" /> class.
/// </remarks>
/// <param name="services">The service collection.</param>
internal sealed class TypeSourceSelector(IServiceCollection services) : ITypeSourceSelector
{
	/// <summary>
	/// Gets the services.
	/// </summary>
	/// <value>The services.</value>
	public IServiceCollection Services { get; } = services;

	/// <summary>
	/// Froms the application dependencies.
	/// </summary>
	/// <returns>ICatalogSelector.</returns>
	public ICatalogSelector FromApplicationDependencies() => this.FromApplicationDependencies(_ => true);

	/// <summary>
	/// Returns a catalog selector from the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate that determines if the assembly is included by the selector.</param>
	/// <returns>ICatalogSelector.</returns>
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
			// Something went wrong when loading the DependencyContext, fall back to loading all
			// referenced assemblies of the entry assembly…
			return this.FromAssemblyDependencies(Assembly.GetEntryAssembly()!);
		}
	}

	/// <summary>
	/// Froms the assemblies.
	/// </summary>
	/// <param name="assemblies">The assemblies.</param>
	/// <returns>ICatalogSelector.</returns>
	/// <exception cref="ArgumentNullException">assemblies</exception>
	public ICatalogSelector FromAssemblies(params Assembly[] assemblies)
	{
		ArgumentNullException.ThrowIfNull(assemblies);

		return this.InternalFromAssemblies(assemblies);
	}

	/// <summary>
	/// Froms the assemblies.
	/// </summary>
	/// <param name="assemblies">The assemblies.</param>
	/// <returns>ICatalogSelector.</returns>
	/// <exception cref="ArgumentNullException">assemblies</exception>
	public ICatalogSelector FromAssemblies(IEnumerable<Assembly> assemblies)
	{
		ArgumentNullException.ThrowIfNull(assemblies);

		return this.InternalFromAssemblies(assemblies);
	}

	/// <summary>
	/// Froms the assemblies of.
	/// </summary>
	/// <param name="types">The types.</param>
	/// <returns>ICatalogSelector.</returns>
	/// <exception cref="ArgumentNullException">types</exception>
	public ICatalogSelector FromAssembliesOf(params Type[] types)
	{
		ArgumentNullException.ThrowIfNull(types);

		return this.InternalFromAssembliesOf(types.Select(x => x.GetTypeInfo()));
	}

	/// <summary>
	/// Froms the assemblies of.
	/// </summary>
	/// <param name="types">The types.</param>
	/// <returns>ICatalogSelector.</returns>
	/// <exception cref="ArgumentNullException">types</exception>
	public ICatalogSelector FromAssembliesOf(IEnumerable<Type> types)
	{
		ArgumentNullException.ThrowIfNull(types);

		return this.InternalFromAssembliesOf(types.Select(t => t.GetTypeInfo()));
	}

	/// <summary>
	/// Froms the assembly dependencies.
	/// </summary>
	/// <param name="assembly">The assembly.</param>
	/// <returns>ICatalogSelector.</returns>
	/// <exception cref="ArgumentNullException">assembly</exception>
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignoring assembly load exceptions.")]
	public ICatalogSelector FromAssemblyDependencies(Assembly assembly)
	{
		ArgumentNullException.ThrowIfNull(assembly);

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
					// Failed to load assembly. Skip it.
				}
			}

			return this.InternalFromAssemblies(assemblies);
		}
		catch
		{
			return this.InternalFromAssemblies(assemblies);
		}
	}

	/// <inheritdoc />
	public ICatalogSelector FromAssemblyOf<T>() =>
		this.InternalFromAssembliesOf(new[] { typeof(T).GetTypeInfo() });

	/// <inheritdoc />
	public ICatalogSelector FromCallingAssembly() =>
		this.FromAssemblies(Assembly.GetCallingAssembly());

	/// <inheritdoc />
	public ICatalogSelector FromDependencyContext(DependencyContext context) =>
		this.FromDependencyContext(context, _ => true);

	/// <inheritdoc />
	public ICatalogSelector FromDependencyContext(DependencyContext context, Func<Assembly, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(context);
		ArgumentNullException.ThrowIfNull(predicate);

		var assemblies = context.RuntimeLibraries
			.SelectMany(library => library.GetDefaultAssemblyNames(context))
			.Select(Assembly.Load)
			.Where(predicate)
			.ToArray();

		return this.InternalFromAssemblies(assemblies);
	}

	/// <inheritdoc />
	public ICatalogSelector FromEntryAssembly() =>
		this.FromAssemblies(Assembly.GetEntryAssembly()!);

	/// <inheritdoc />
	public ICatalogSelector FromExecutingAssembly() => this.FromAssemblies(Assembly.GetExecutingAssembly());

	/// <summary>
	/// Internals from assemblies.
	/// </summary>
	/// <param name="assemblies">The assemblies.</param>
	/// <returns>ICatalogSelector.</returns>
	private CatalogSelector InternalFromAssemblies(IEnumerable<Assembly> assemblies) =>
		new(
			assemblies
				.SelectMany(asm => asm.DefinedTypes)
				.Where(x => x.IsAssignableTo(typeof(Catalog)))
				.Select(x => x.AsType()),
			this.Services);

	/// <summary>
	/// Internals from assemblies of.
	/// </summary>
	/// <param name="typeInfos">The type infos.</param>
	/// <returns>ICatalogSelector.</returns>
	private CatalogSelector InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos) =>
		this.InternalFromAssemblies(typeInfos.Select(t => t.Assembly));
}
