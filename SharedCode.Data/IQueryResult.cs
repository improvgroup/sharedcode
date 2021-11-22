// <copyright file="IQueryResult.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using SharedCode.Models;

using System.Collections.Generic;

/// <summary>
/// The query result interface.
/// </summary>
public interface IQueryResult
{
	/// <summary>
	/// Gets the actual index of the page zero.
	/// </summary>
	/// <value>The actual index of the page zero.</value>
	int ActualPageZeroIndex { get; }

	/// <summary>
	/// Gets the paging descriptor.
	/// </summary>
	/// <value>The paging descriptor.</value>
	PagingDescriptor PagingDescriptor { get; }

	/// <summary>
	/// Gets the results.
	/// </summary>
	/// <value>The results.</value>
	IEnumerable<Entity> Results { get; }
}
