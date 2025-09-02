using Microsoft.EntityFrameworkCore;

namespace SharedCode.Data.EntityFramework;

/// <summary>
/// The auditable database context class. Implements the <see cref="DbContext"/>
/// </summary>
/// <typeparam name="TMutationIdentityKey">
/// The type of the key for the identity performing mutations on the entities handled by this context.
/// </typeparam>
/// <seealso cref="DbContext"/>
public abstract class AuditableDbContext<TMutationIdentityKey> : DbContext where TMutationIdentityKey : notnull
{
    /// <inheritdoc/>
    public override int SaveChanges()
    {
        this.InitializeAuditablePropertyValues();
        return base.SaveChanges();
    }

    /// <inheritdoc/>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.InitializeAuditablePropertyValues();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc/>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.InitializeAuditablePropertyValues();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.InitializeAuditablePropertyValues();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// Gets the identity performing the mutation of the entities.
    /// </summary>
    /// <returns>The mutation identity.</returns>
    /// <example>
    /// <c title="Sample implementation.">this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";</c>
    /// <c title="Don't forget to add the accessor to your dependency container in your Program.cs or Startup.cs file.">services.AddHttpContextAccessor();</c>
    /// </example>
    protected abstract TMutationIdentityKey GetMutationIdentity();

    /// <summary>
    /// Initializes the audit related property values.
    /// </summary>
    protected virtual void InitializeAuditablePropertyValues()
    {
        foreach (var entry in this.ChangeTracker.Entries())
        {
            if (entry.Entity is IAuditableEntity<TMutationIdentityKey> entity && (entry.State == EntityState.Added || entry.State == EntityState.Modified))
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTimeOffset.UtcNow;
                    entity.CreatedBy = this.GetMutationIdentity();
                }

                entity.ModifiedAt = DateTimeOffset.UtcNow;
                entity.ModifiedBy = this.GetMutationIdentity();

                // Prevent changing the creation information if modifying an existing entity.
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
                    entry.Property(nameof(IAuditableEntity.CreatedBy)).IsModified = false;
                }
            }
        }
    }
}
