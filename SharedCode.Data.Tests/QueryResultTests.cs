namespace SharedCode.Data.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Models;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="QueryResult{TEntity}"/>.
/// </summary>
public class QueryResultTests
{
	[Test]
	public async Task Constructor_SetsAllProperties()
	{
		var entities = new[] { new Entity(), new Entity(), new Entity() };
		var boundaries = new List<PageBoundry> { new(0, 9) };
		var pagingDescriptor = new PagingDescriptor(10, 1, boundaries);

		var result = new QueryResult<Entity>(pagingDescriptor, actualPageZeroIndex: 0, entities);

		await Assert.That(result.ActualPageZeroIndex).IsEqualTo(0);
		await Assert.That(result.PagingDescriptor).IsSameReferenceAs(pagingDescriptor);
		await Assert.That(result.Results.Count()).IsEqualTo(3);
	}

	[Test]
	public async Task Constructor_SecondPage_ReturnsCorrectPageIndex()
	{
		var entities = new[] { new Entity() };
		var boundaries = new List<PageBoundry> { new(0, 9), new(10, 19) };
		var pagingDescriptor = new PagingDescriptor(10, 2, boundaries);

		var result = new QueryResult<Entity>(pagingDescriptor, actualPageZeroIndex: 1, entities);

		await Assert.That(result.ActualPageZeroIndex).IsEqualTo(1);
	}

	[Test]
	public async Task Constructor_EmptyResults_HasZeroResults()
	{
		var boundaries = new List<PageBoundry>();
		var pagingDescriptor = new PagingDescriptor(10, 0, boundaries);

		var result = new QueryResult<Entity>(pagingDescriptor, actualPageZeroIndex: 0, Array.Empty<Entity>());

		await Assert.That(result.Results.Count()).IsEqualTo(0);
	}

	[Test]
	public async Task Results_ExplicitInterface_ReturnsEntities()
	{
		var entity = new Entity();
		var boundaries = new List<PageBoundry> { new(0, 0) };
		var pagingDescriptor = new PagingDescriptor(1, 1, boundaries);

		SharedCode.Data.IQueryResult queryResult = new QueryResult<Entity>(pagingDescriptor, 0, new[] { entity });

		await Assert.That(queryResult.Results.Count()).IsEqualTo(1);
		await Assert.That(queryResult.Results.First()).IsSameReferenceAs(entity);
	}
}
