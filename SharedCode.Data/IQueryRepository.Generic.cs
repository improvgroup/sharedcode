// <copyright file="IQueryRepository1.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using SharedCode.Models;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The query repository. Implements the <see cref="IQueryRepository" />
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <seealso cref="IQueryRepository" />
public interface IQueryRepository<out TEntity> : IQueryRepository where TEntity : Entity
{
	/// <summary>
	/// Gets the specified identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>TEntity.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	new TEntity Get(int id);

	/// <summary>
	/// Gets the specified page size.
	/// </summary>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	new IQueryResult<TEntity> Get(int pageSize, int pageIndex);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	IQueryResult<TEntity> Get(Func<TEntity, bool> predicate);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Normal repository nomenclature.")]
	IQueryResult<TEntity> Get(Func<TEntity, bool> predicate, int pageSize, int pageIndex);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	abstract IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	abstract IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate, int pageSize, int pageIndex);

	/// <summary>
	/// Gets all.
	/// </summary>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	new IQueryResult<TEntity> GetAll();
}
