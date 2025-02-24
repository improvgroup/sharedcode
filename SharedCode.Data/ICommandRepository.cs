
namespace SharedCode.Data;

using SharedCode.Models;

using System.Collections.Generic;

/// <summary>
/// The command repository interface.
/// </summary>
public interface ICommandRepository
{
	/// <summary>
	/// Adds the specified entity.
	/// </summary>
	/// <param name="entity">The entity.</param>
	/// <returns>System.Int32.</returns>
	int Add(Entity entity);

	/// <summary>
	/// Adds the specified entities.
	/// </summary>
	/// <param name="entities">The entities.</param>
	/// <returns>IEnumerable&lt;System.Int32&gt;.</returns>
	IEnumerable<int> Add(IEnumerable<Entity> entities);

	/// <summary>
	/// Adds the or update.
	/// </summary>
	/// <param name="entity">The entity.</param>
	/// <returns>IAddOrUpdateDescriptor.</returns>
	IAddOrUpdateDescriptor AddOrUpdate(Entity entity);

	/// <summary>
	/// Adds the or update.
	/// </summary>
	/// <param name="entities">The entities.</param>
	/// <returns>IEnumerable&lt;IAddOrUpdateDescriptor&gt;.</returns>
	IEnumerable<IAddOrUpdateDescriptor> AddOrUpdate(IEnumerable<Entity> entities);

	/// <summary>
	/// Removes the specified identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	bool Remove(int id);

	/// <summary>
	/// Removes the specified entity.
	/// </summary>
	/// <param name="entity">The entity.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	bool Remove(Entity entity);

	/// <summary>
	/// Removes the specified entities.
	/// </summary>
	/// <param name="entities">The entities.</param>
	/// <returns>IDictionary&lt;System.Int32, System.Boolean&gt;.</returns>
	IDictionary<int, bool> Remove(IEnumerable<Entity> entities);

	/// <summary>
	/// Updates the specified entity.
	/// </summary>
	/// <param name="entity">The entity.</param>
	void Update(Entity entity);

	/// <summary>
	/// Updates the specified entities.
	/// </summary>
	/// <param name="entities">The entities.</param>
	void Update(IEnumerable<Entity> entities);
}
