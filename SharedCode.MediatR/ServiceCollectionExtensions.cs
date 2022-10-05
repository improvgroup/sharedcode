namespace SharedCode.MediatR;

using global::MediatR;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The service collection extension methods class.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds the MediatR commands.
	/// </summary>
	/// <param name="services">The services.</param>
	public static void AddMediatRCommands(this IServiceCollection services) => services.AddMediatR(AssemblyReference.Assembly);
}
