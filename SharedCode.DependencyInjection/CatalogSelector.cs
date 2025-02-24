// <copyright file="CatalogSelector.cs" company="William Forney">
//     Copyright © 2021 William Forney. All Rights Reserved.

namespace SharedCode.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The catalog selector class. Implements the <see cref="ICatalogSelector" />.
/// </summary>
/// <seealso cref="ICatalogSelector" />
public class CatalogSelector : ICatalogSelector
{
	/// <summary>
	/// The catalogs
	/// </summary>
	private readonly IEnumerable<Type> _catalogs;

	/// <summary>
	/// The services
	/// </summary>
	private readonly IServiceCollection _services;

	/// <summary>
	/// Initializes a new instance of the <see cref="CatalogSelector" /> class.
	/// </summary>
	/// <param name="catalogs">The catalogs.</param>
	/// <param name="services">The services.</param>
	public CatalogSelector(IEnumerable<Type> catalogs, IServiceCollection services)
	{
		this._catalogs = catalogs;
		this._services = services;
	}

	/// <summary>
	/// Registers the catalogs.
	/// </summary>
	public void RegisterCatalogs()
	{
		foreach (var catalog in this._catalogs)
		{
			_ = Activator.CreateInstance(catalog, this._services);
		}
	}
}
