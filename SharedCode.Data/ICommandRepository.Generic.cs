
namespace SharedCode.Data;

using SharedCode.Models;

using System.Collections.Generic;

/// <summary>
/// The command repository interface. Implements the <see cref="ICommandRepository" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="ICommandRepository" />
public interface ICommandRepository<in TEntity> : ICommandRepository where TEntity : Entity
{
	/// <summary>
	/// Adds the specified entity.
	/// </summary>
	/// <param name="entity">The entity.</param>
	/// <returns>System.Int32.</returns>
	int Add(TEntity entity);

	/// <summary>
	/// Adds the specified entities.
	/// </summary>
	/// <param name="entities">The entities.</param>
	/// <returns>IEnumerable&lt;System.Int32&gt;.</returns>
	IEnumerable<int> Add(IEnumerable<TEntity> entities);

	/// <inheritdoc />
	abstract int ICommandRepository.Add(Entity entity);

	/// <inheritdoc />
	abstract IEnumerable<int> ICommandRepository.Add(IEnumerable<Entity> entities);

	/// <summary>
	/// Adds the or update.
	/// </summary>
	/// <param name="entity">The entity.</param>
	/// <returns>IAddOrUpdateDescriptor.</returns>
	IAddOrUpdateDescriptor AddOrUpdate(TEntity entity);

	/// <summary>
	/// Adds the or update.
	/// </summary>
	/// <param name="entities">The entities.</param>
	/// <returns>IEnumerable&lt;IAddOrUpdateDescriptor&gt;.</returns>
	IEnumerable<IAddOrUpdateDescriptor> AddOrUpdate(IEnumerable<TEntity> entities);

	/// <inheritdoc />
	abstract IAddOrUpdateDescriptor ICommandRepository.AddOrUpdate(Entity entity);

	/// <inheritdoc />
	abstract IEnumerable<IAddOrUpdateDescriptor> ICommandRepository.AddOrUpdate(IEnumerable<Entity> entities);

	/// <summary>
	/// Removes the specified entity.
	/// </summary>
	/// <param name="entity">The entity.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	bool Remove(TEntity entity);

	/// <summary>
	/// Removes the specified entities.
	/// </summary>
	/// <param name="entities">The entities.</param>
	/// <returns>IDictionary&lt;System.Int32, System.Boolean&gt;.</returns>
	IDictionary<int, bool> Remove(IEnumerable<TEntity> entities);

	/// <inheritdoc />
	abstract bool ICommandRepository.Remove(Entity entity);

	/// <inheritdoc />
	abstract IDictionary<int, bool> ICommandRepository.Remove(IEnumerable<Entity> entities);

	/// <summary>
	/// Updates the specified entity.
	/// </summary>
	/// <param name="entity">The entity.</param>
	void Update(TEntity entity);

	/// <summary>
	/// Updates the specified entities.
	/// </summary>
	/// <param name="entities">The entities.</param>
	void Update(IEnumerable<TEntity> entities);

	/// <inheritdoc />
	abstract void ICommandRepository.Update(Entity entity);

	/// <inheritdoc />
	abstract void ICommandRepository.Update(IEnumerable<Entity> entities);
}
