namespace SharedCode.Models;
/// <summary>
/// The item class. Implements the <see cref="Entity{Guid}" />.
/// </summary>
/// <seealso cref="Entity{Guid}" />
public class Entity : Entity<Guid>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Entity" /> class.
	/// </summary>
	public Entity() : base(Guid.NewGuid())
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Entity" /> class.
	/// </summary>
	/// <param name="id">The identifier.</param>
	public Entity(Guid id) : base(id)
	{
	}
}
