// <copyright file="QueryRepository.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using SharedCode.Models;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The query repository. Implements the <see cref="IQueryRepository{TEntity}" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="IQueryRepository{TEntity}" />
public abstract class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : Entity
{
	/// <summary>
	/// Gets the specified page size.
	/// </summary>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Common repository nomenclature.")]
	public abstract IQueryResult<TEntity> Get(int pageSize, int pageIndex);

	/// <summary>
	/// Gets the specified identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>TEntity.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Common repository nomenclature.")]
	public abstract TEntity Get(int id);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Common repository nomenclature.")]
	public abstract IQueryResult<TEntity> Get(Func<TEntity, bool> predicate);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Common repository nomenclature.")]
	public abstract IQueryResult<TEntity> Get(Func<TEntity, bool> predicate, int pageSize, int pageIndex);

	/// <summary>
	/// Gets the specified identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>Entity.</returns>
	Entity IQueryRepository.Get(int id) => this.Get(id);

	/// <summary>
	/// Gets the specified page size.
	/// </summary>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	IQueryResult<Entity> IQueryRepository.Get(int pageSize, int pageIndex) => this.Get(pageSize, pageIndex);

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	/// <exception cref="ArgumentException">
	/// The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"
	/// </exception>
	IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate)
	{
		var passedInType = predicate.Method.GetParameters()[0].ParameterType;

		return typeof(TEntity).IsAssignableFrom(passedInType)
			? this.Get(predicate)
			: throw new ArgumentException($"The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"");
	}

	/// <summary>
	/// Gets the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	/// <exception cref="ArgumentException">
	/// The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"
	/// </exception>
	IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate, int pageSize, int pageIndex)
	{
		var passedInType = predicate.Method.GetParameters()[0].ParameterType;

		return typeof(TEntity).IsAssignableFrom(passedInType)
			? this.Get(predicate, pageSize, pageIndex)
			: throw new ArgumentException($"The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"");
	}

	/// <summary>
	/// Gets all.
	/// </summary>
	/// <returns>IQueryResult&lt;TEntity&gt;.</returns>
	public abstract IQueryResult<TEntity> GetAll();

	/// <summary>
	/// Gets all.
	/// </summary>
	/// <returns>IQueryResult&lt;Entity&gt;.</returns>
	IQueryResult<Entity> IQueryRepository.GetAll() => this.GetAll();
}
