// <copyright file="ListExtensionMethods.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using System.Collections;
using System.Linq;

/// <summary>
/// The list extension methods class.
/// </summary>
public static class ListExtensions
{
	/// <summary>
	/// Pages the specified page size.
	/// </summary>
	/// <param name="list">The list.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <returns>PagingDescriptor.</returns>
	public static PagingDescriptor Page(this IList list, int pageSize)
	{
		var actualPageSize = pageSize;

		if (actualPageSize <= 0)
		{
			actualPageSize = list?.Count ?? 0;
		}

		var maxNumberOfPages = (int)Math.Round(Math.Max(1, Math.Ceiling(((float)(list?.Count ?? 0)) / actualPageSize)));

		return new PagingDescriptor(
			actualPageSize,
			maxNumberOfPages,
			Enumerable
				.Range(0, maxNumberOfPages)
				.Select(pageZeroIndex => new PageBoundry(
					pageZeroIndex * actualPageSize,
					Math.Min((pageZeroIndex * actualPageSize) + (actualPageSize - 1), (list?.Count ?? 0) - 1)))
				.ToArray()
		);
	}
}
