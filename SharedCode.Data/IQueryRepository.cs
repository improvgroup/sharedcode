
namespace SharedCode.Data;

using SharedCode.Models;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The query repository interface.
/// </summary>
public interface IQueryRepository
{
	/// <summary>
	/// Gets the specified identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>Entity.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	Entity Get(int id);

	/// <summary>
	/// Gets the specified page size.
	/// </summary>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	IQueryResult<Entity> Get(int pageSize, int pageIndex);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	IQueryResult<Entity> Get(Func<Entity, bool> predicate);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	IQueryResult<Entity> Get(Func<Entity, bool> predicate, int pageSize, int pageIndex);

	/// <summary>
	/// Gets all.
	/// </summary>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	IQueryResult<Entity> GetAll();
}
