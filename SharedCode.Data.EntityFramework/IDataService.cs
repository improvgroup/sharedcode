namespace SharedCode.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// The data service interface.
	/// </summary>
	/// <typeparam name="T">The type of entities on which this data service operates.</typeparam>
	public interface IDataService<T> where T : class
	{
		/// <summary>
		/// Creates a new database row for the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The created entity.</returns>
		Task<T> Create(T entity, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes the data row corresponding to the specified primary key.
		/// </summary>
		/// <typeparam name="TKey">The type of the primary key.</typeparam>
		/// <param name="key">The primary key.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>A value indicating whether the entity was successfully deleted.</returns>
		Task<bool> Delete<TKey>(TKey key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes the data row corresponding to the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>A value indicating whether the entity was successfully deleted.</returns>
		Task<bool> Delete(T entity, CancellationToken cancellationToken = default);


		/// <summary>
		/// Gets the entity for the specified primary key.
		/// </summary>
		/// <typeparam name="TKey">The type of the primary key.</typeparam>
		/// <param name="key">The primary key.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The entity, or null if not found.</returns>
		[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Common data service terminology.")]
		Task<T?> Get<TKey>(TKey key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets an enumerable, optionally filtered using the specified expression.
		/// </summary>
		/// <param name="expression">The filter expression.</param>
		/// <returns>An asynchronous enumerable.</returns>
		IAsyncEnumerable<T> Get(Expression<Func<T, bool>>? expression = null);

		/// <summary>
		/// Queries the database, optionally filtered using the specified expression.
		/// </summary>
		/// <param name="expression">The filter expression.</param>
		/// <returns>A queryable sequence.</returns>
		IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>The updated entity.</returns>
		Task<T> Update(T entity, CancellationToken cancellationToken = default);
	}
}
