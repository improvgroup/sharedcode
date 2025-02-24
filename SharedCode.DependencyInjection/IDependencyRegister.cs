
namespace SharedCode.DependencyInjection;

/// <summary>
/// The dependency register interface.
/// </summary>
public interface IDependencyRegister
{
	/// <summary>
	/// Adds the scoped type.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	void AddScoped<TService>() where TService : class;

	/// <summary>
	/// Adds the scoped service and implementation.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	/// <typeparam name="TImplementation">The type of the implementation.</typeparam>
	void AddScoped<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService;

	/// <summary>
	/// Adds the scoped service for multiple implementations.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	/// <typeparam name="TImplementation">The type of the implementation.</typeparam>
	void AddScopedForMultiImplementation<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService;

	/// <summary>
	/// Adds the singleton service.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	void AddSingleton<TService>() where TService : class;

	/// <summary>
	/// Adds the singleton service and implementation.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	/// <typeparam name="TImplementation">The type of the implementation.</typeparam>
	void AddSingleton<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService;

	/// <summary>
	/// Adds the transient service.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	void AddTransient<TService>() where TService : class;

	/// <summary>
	/// Adds the transient service.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	/// <typeparam name="TImplementation">The type of the implementation.</typeparam>
	void AddTransient<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService;

	/// <summary>
	/// Adds the transient service for multiple implementations.
	/// </summary>
	/// <typeparam name="TService">The type of the service.</typeparam>
	/// <typeparam name="TImplementation">The type of the implementation.</typeparam>
	void AddTransientForMultiImplementation<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService;
}
