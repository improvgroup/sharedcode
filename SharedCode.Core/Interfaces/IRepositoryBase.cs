
using SharedCode.Specifications;

namespace SharedCode.Interfaces;
/// <summary>
/// <para>
/// A <see cref="IRepositoryBase{T}" /> can be used to query and save instances of <typeparamref
/// name="T" />. An <see cref="ISpecification{T}" /> (or derived) is used to encapsulate the LINQ
/// queries against the database.
/// </para>
/// </summary>
/// <typeparam name="T">The type of entity being operated on by this repository.</typeparam>
public interface IRepositoryBase<T> : IReadRepositoryBase<T> where T : class
{
	/// <summary>
	/// Adds an entity in the database.
	/// </summary>
	/// <param name="entity">The entity to add.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// A <see cref="Task"/> that represents the asynchronous operation. The <see cref="Task"/> result contains the
	/// <typeparamref name="T" />.
	/// </returns>
	Task<T> AddAsync(T entity, CancellationToken? cancellationToken = default);

	/// <summary>
	/// Removes an entity in the database
	/// </summary>
	/// <param name="entity">The entity to delete.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task RemoveAsync(T entity, CancellationToken? cancellationToken = default);

	/// <summary>
	/// Removes the given entities in the database
	/// </summary>
	/// <param name="entities">The entities to remove.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task RemoveAsync(IEnumerable<T> entities, CancellationToken? cancellationToken = default);

	/// <summary>
	/// Persists changes to the database.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task SaveChangesAsync(CancellationToken? cancellationToken = default);

	/// <summary>
	/// Updates an entity in the database
	/// </summary>
	/// <param name="entity">The entity to update.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task UpdateAsync(T entity, CancellationToken? cancellationToken = default);
}
