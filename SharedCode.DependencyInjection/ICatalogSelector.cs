// <copyright file="ICatalogSelector.cs" company="William Forney">
//     Copyright © 2021 William Forney. All Rights Reserved.

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
