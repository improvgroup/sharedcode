namespace SharedCode.Data.EntityFramework;

/// <summary>
/// The entity base class.
/// </summary>
/// <typeparam name="TIdentityKey">The type of key used to identify the mutator of this entity.</typeparam>
public abstract class Entity<TIdentityKey> : IAuditableEntity<TIdentityKey> where TIdentityKey : notnull
{
    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public TIdentityKey CreatedBy { get; set; } = default!;

    /// <inheritdoc />
    public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public TIdentityKey ModifiedBy { get; set; } = default!;

    /// <summary>
    /// Gets or sets the audit histories.
    /// </summary>
    /// <value>The audit histories.</value>
    public virtual ICollection<IAuditedEntity<TIdentityKey>> AuditHistories { get; } = [];
}
