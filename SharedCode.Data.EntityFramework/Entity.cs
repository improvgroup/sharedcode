namespace SharedCode.Data.EntityFramework;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

/// <summary>
/// The entity base class.
/// </summary>
/// <typeparam name="TIdentityKey">The type of key used to identify the mutator of this entity.</typeparam>
public abstract class Entity<TIdentityKey> : IAuditableEntity<TIdentityKey> where TIdentityKey : notnull
{
	/// <inheritdoc />
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

	/// <inheritdoc />
	public TIdentityKey CreatedBy { get; set; }

	/// <inheritdoc />
	public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;

	/// <inheritdoc />
	public TIdentityKey ModifiedBy { get; set; }

	/// <summary>
	/// Gets or sets the audit histories.
	/// </summary>
	/// <value>The audit histories.</value>
	public virtual ICollection<IAuditedEntity<TIdentityKey>> AuditHistories { get; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
