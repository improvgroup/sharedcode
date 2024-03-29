﻿namespace SharedCode.Data.EntityFramework;

/// <summary>
/// The audited entity interface. Implements the <see cref="IAuditableEntity{TIdentityKey}" />.
/// </summary>
/// <typeparam name="TIdentityKey">The type of the identity key.</typeparam>
/// <seealso cref="IAuditableEntity{TIdentityKey}" />
/// <remarks>
/// This is the audit history entity.
/// </remarks>
public interface IAuditedEntity<TIdentityKey> : IAuditableEntity<TIdentityKey> where TIdentityKey : notnull
{
	/// <summary>
	/// Gets or sets the datetime when this entity was audited.
	/// </summary>
	/// <value>The datetime when this entity was audited.</value>
	DateTimeOffset AuditedAt { get; set; }
}
