
namespace SharedCode.Data;

using SharedCode.Models;

using System.Collections.Generic;

/// <summary>
/// The query result class. Implements the <see cref="IQueryResult{TEntity}" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="IQueryResult{TEntity}" />
public class QueryResult<TEntity> : IQueryResult<TEntity> where TEntity : Entity
{
	/// <summary>
	/// Initializes a new instance of the <see cref="QueryResult{TEntity}" /> class.
	/// </summary>
	/// <param name="pagingDescriptor">The paging descriptor.</param>
	/// <param name="actualPageZeroIndex">Actual index of the page zero.</param>
	/// <param name="results">The results.</param>
	public QueryResult(
		PagingDescriptor pagingDescriptor,
		int actualPageZeroIndex,
		IEnumerable<TEntity> results)
	{
		this.PagingDescriptor = pagingDescriptor;
		this.ActualPageZeroIndex = actualPageZeroIndex;
		this.Results = results;
	}

	/// <inheritdoc />
	public int ActualPageZeroIndex { get; }

	/// <inheritdoc />
	public PagingDescriptor PagingDescriptor { get; }

	/// <inheritdoc />
	public IEnumerable<TEntity> Results { get; }

	/// <inheritdoc />
	IEnumerable<Entity> IQueryResult.Results => this.Results;
}
