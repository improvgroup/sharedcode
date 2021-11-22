// <copyright file="PagingDescriptor.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

/// <summary>
/// The paging descriptor class.
/// </summary>
public class PagingDescriptor
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PagingDescriptor" /> class.
	/// </summary>
	/// <param name="actualPageSize">Actual size of the page.</param>
	/// <param name="numberOfPages">The number of pages.</param>
	/// <param name="pagesBoundries">The pages boundries.</param>
	public PagingDescriptor(
		int actualPageSize,
		int numberOfPages,
		ICollection<PageBoundry> pagesBoundries)
	{
		this.ActualPageSize = actualPageSize;
		this.NumberOfPages = numberOfPages;
		this.PagesBoundries = pagesBoundries;
	}

	/// <summary>
	/// Gets the actual size of the page.
	/// </summary>
	/// <value>The actual size of the page.</value>
	public int ActualPageSize { get; }

	/// <summary>
	/// Gets the number of pages.
	/// </summary>
	/// <value>The number of pages.</value>
	public int NumberOfPages { get; }

	/// <summary>
	/// Gets the pages boundries.
	/// </summary>
	/// <value>The pages boundries.</value>
	public ICollection<PageBoundry> PagesBoundries { get; }
}
