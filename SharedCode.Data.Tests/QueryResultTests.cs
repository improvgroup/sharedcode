namespace SharedCode.Data.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Models;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="QueryResult{TEntity}"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class QueryResultTests
{
	[TestMethod]
	public void Constructor_SetsAllProperties()
	{
		var entities = new[] { new Entity(), new Entity(), new Entity() };
		var boundaries = new List<PageBoundry> { new(0, 9) };
		var pagingDescriptor = new PagingDescriptor(10, 1, boundaries);

		var result = new QueryResult<Entity>(pagingDescriptor, actualPageZeroIndex: 0, entities);

		Assert.AreEqual(0, result.ActualPageZeroIndex);
		Assert.AreSame(pagingDescriptor, result.PagingDescriptor);
		Assert.AreEqual(3, result.Results.Count());
	}

	[TestMethod]
	public void Constructor_SecondPage_ReturnsCorrectPageIndex()
	{
		var entities = new[] { new Entity() };
		var boundaries = new List<PageBoundry> { new(0, 9), new(10, 19) };
		var pagingDescriptor = new PagingDescriptor(10, 2, boundaries);

		var result = new QueryResult<Entity>(pagingDescriptor, actualPageZeroIndex: 1, entities);

		Assert.AreEqual(1, result.ActualPageZeroIndex);
	}

	[TestMethod]
	public void Constructor_EmptyResults_HasZeroResults()
	{
		var boundaries = new List<PageBoundry>();
		var pagingDescriptor = new PagingDescriptor(10, 0, boundaries);

		var result = new QueryResult<Entity>(pagingDescriptor, actualPageZeroIndex: 0, Array.Empty<Entity>());

		Assert.AreEqual(0, result.Results.Count());
	}

	[TestMethod]
	public void Results_ExplicitInterface_ReturnsEntities()
	{
		var entity = new Entity();
		var boundaries = new List<PageBoundry> { new(0, 0) };
		var pagingDescriptor = new PagingDescriptor(1, 1, boundaries);

		SharedCode.Data.IQueryResult queryResult = new QueryResult<Entity>(pagingDescriptor, 0, new[] { entity });

		Assert.AreEqual(1, queryResult.Results.Count());
		Assert.AreSame(entity, queryResult.Results.First());
	}
}
