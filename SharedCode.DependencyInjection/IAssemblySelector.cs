
namespace SharedCode.DependencyInjection;

using Microsoft.Extensions.DependencyModel;

using System.Reflection;

/// <summary>
/// The assembly selector interface. Implements the <see cref="IFluentInterface" />.
/// </summary>
/// <seealso cref="IFluentInterface" />
public interface IAssemblySelector : IFluentInterface
{
	/// <summary>
	/// Loads and scans all runtime libraries referenced by the currently executing application.
	/// Calling this method is equivalent to calling <see
	/// cref="FromDependencyContext(DependencyContext)" /> with <see
	/// cref="DependencyContext.Default" />.
	/// </summary>
	/// <remarks>
	/// If loading <see cref="DependencyContext.Default" /> fails, this method will fall back to
	/// calling <see cref="FromAssemblyDependencies(Assembly)" />, using the entry assembly.
	/// </remarks>
	ICatalogSelector FromApplicationDependencies();

	/// <summary>
	/// Loads and scans all runtime libraries referenced by the currently executing application.
	/// Calling this method is equivalent to calling <see
	/// cref="FromDependencyContext(DependencyContext, Func{Assembly, bool})" /> with <see
	/// cref="DependencyContext.Default" />.
	/// </summary>
	/// <remarks>
	/// If loading <see cref="DependencyContext.Default" /> fails, this method will fall back to
	/// calling <see cref="FromAssemblyDependencies(Assembly)" />, using the entry assembly.
	/// </remarks>
	/// <param name="predicate">The predicate to match assemblies.</param>
	ICatalogSelector FromApplicationDependencies(Func<Assembly, bool> predicate);

	/// <summary>
	/// Scans for types in each <see cref="Assembly" /> in <paramref name="assemblies" />.
	/// </summary>
	/// <param name="assemblies">The assemblies to should be scanned.</param>
	/// <exception cref="ArgumentNullException">
	/// If the <paramref name="assemblies" /> argument is <c>null</c>.
	/// </exception>
	ICatalogSelector FromAssemblies(params Assembly[] assemblies);

	/// <summary>
	/// Scans for types in each <see cref="Assembly" /> in <paramref name="assemblies" />.
	/// </summary>
	/// <param name="assemblies">The assemblies to should be scanned.</param>
	/// <exception cref="ArgumentNullException">
	/// If the <paramref name="assemblies" /> argument is <c>null</c>.
	/// </exception>
	ICatalogSelector FromAssemblies(IEnumerable<Assembly> assemblies);

	/// <summary>
	/// Scans for types from the assemblies of each <see cref="Type" /> in <paramref name="types" />.
	/// </summary>
	/// <param name="types">The types in which assemblies that should be scanned.</param>
	/// <exception cref="ArgumentNullException">
	/// If the <paramref name="types" /> argument is <c>null</c>.
	/// </exception>
	ICatalogSelector FromAssembliesOf(params Type[] types);

	/// <summary>
	/// Scans for types from the assemblies of each <see cref="Type" /> in <paramref name="types" />.
	/// </summary>
	/// <param name="types">The types in which assemblies that should be scanned.</param>
	/// <exception cref="ArgumentNullException">
	/// If the <paramref name="types" /> argument is <c>null</c>.
	/// </exception>
	ICatalogSelector FromAssembliesOf(IEnumerable<Type> types);

	/// <summary>
	/// Loads and scans all runtime libraries referenced by the currently specified <paramref
	/// name="assembly" />.
	/// </summary>
	/// <param name="assembly">The assembly whose dependencies should be scanned.</param>
	ICatalogSelector FromAssemblyDependencies(Assembly assembly);

	/// <summary>
	/// Scans for types from the assembly of type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The type in which assembly that should be scanned.</typeparam>
	ICatalogSelector FromAssemblyOf<T>();

	/// <summary>
	/// Scans for types from the calling assembly.
	/// </summary>
	ICatalogSelector FromCallingAssembly();

	/// <summary>
	/// Loads and scans all runtime libraries in the given <paramref name="context" />.
	/// </summary>
	/// <param name="context">The dependency context.</param>
	ICatalogSelector FromDependencyContext(DependencyContext context);

	/// <summary>
	/// Loads and scans all runtime libraries in the given <paramref name="context" />.
	/// </summary>
	/// <param name="context">The dependency context.</param>
	/// <param name="predicate">The predicate to match assemblies.</param>
	ICatalogSelector FromDependencyContext(DependencyContext context, Func<Assembly, bool> predicate);

	/// <summary>
	/// Scans for types from the entry assembly.
	/// </summary>
	ICatalogSelector FromEntryAssembly();

	/// <summary>
	/// Scans for types from the currently executing assembly.
	/// </summary>
	ICatalogSelector FromExecutingAssembly();
}
