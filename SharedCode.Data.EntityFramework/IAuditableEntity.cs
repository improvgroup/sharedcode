namespace SharedCode.Data.EntityFramework;

/// <summary>
/// The auditable entity interface.
/// </summary>
public interface IAuditableEntity : IAuditableEntity<Guid>
{
}

/// <summary>
/// The auditable entity interface.
/// </summary>
/// <typeparam name="TIdentityKey">The type of the key for the identity mutating this entity.</typeparam>
public interface IAuditableEntity<TIdentityKey> where TIdentityKey : notnull
{
	/// <summary>
	/// Gets or sets the datetime when this entity was created.
	/// </summary>
	/// <value>The datetime when this entity was created.</value>
	DateTimeOffset CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the identity who created this entity.
	/// </summary>
	/// <value>The created at.</value>
	TIdentityKey CreatedBy { get; set; }

	/// <summary>
	/// Gets or sets the datetime when this entity was last modified.
	/// </summary>
	/// <value>The last datetime this entity was modified.</value>
	DateTimeOffset ModifiedAt { get; set; }

	/// <summary>
	/// Gets or sets the identity who last modified this entity.
	/// </summary>
	/// <value>The identity who last modified this entity.</value>
	TIdentityKey ModifiedBy { get; set; }
}
