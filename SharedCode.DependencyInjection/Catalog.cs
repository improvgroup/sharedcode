// <copyright file="Catalog.cs" company="William Forney">
//     Copyright © 2021 William Forney. All Rights Reserved.

namespace SharedCode.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The service catalog class.
/// </summary>
public abstract class Catalog
{
	/// <summary>
	/// Gets the services.
	/// </summary>
	/// <value>The services.</value>
	public IServiceCollection Services { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Catalog"/> class.
	/// </summary>
	/// <param name="services">The services.</param>
	protected Catalog(IServiceCollection services) => this.Services = services;
}
