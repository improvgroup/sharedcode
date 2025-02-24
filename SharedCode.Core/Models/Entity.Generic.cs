
namespace SharedCode.Models;

using SharedCode.Events;

using System;
using System.Diagnostics;

/// <summary>
/// The item class. Implements the <see cref="IEquatable{T}" /> where T is <see cref="Entity{TKey}" />.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <seealso cref="IEquatable{T}" />
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class Entity<TKey> : IEquatable<Entity<TKey>?>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Entity{TKey}" /> class.
	/// </summary>
	/// <param name="id">The identifier.</param>
	public Entity(TKey id) => this.Id = id;

	/// <summary>
	/// Gets the events.
	/// </summary>
	/// <value>The events.</value>
	public IList<DomainEvent> Events { get; } = new List<DomainEvent>();

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public TKey Id { get; set; }

	/// <summary>
	/// Implements the != operator.
	/// </summary>
	/// <param name="left">The left value.</param>
	/// <param name="right">The right value.</param>
	/// <returns>The result of the operator.</returns>
	public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right) =>
		!(left == right);

	/// <summary>
	/// Implements the == operator.
	/// </summary>
	/// <param name="left">The left value.</param>
	/// <param name="right">The right value.</param>
	/// <returns>The result of the operator.</returns>
	public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right) =>
		(left is null && right is null) ||
		(left is not null && right is not null && EqualityComparer<Entity<TKey>>.Default.Equals(left, right));

	/// <inheritdoc />
	public override bool Equals(object? obj) => this.Equals(obj as Entity<TKey>);

	/// <summary>
	/// Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">An object to compare with this object.</param>
	/// <returns>
	/// <see langword="true" /> if the current object is equal to the <paramref name="other" />
	/// parameter; otherwise, <see langword="false" />.
	/// </returns>
	public bool Equals(Entity<TKey>? other) =>
		other is not null &&
		EqualityComparer<TKey>.Default.Equals(this.Id, other.Id);

	/// <inheritdoc />
	public override int GetHashCode() => HashCode.Combine(this.Events, this.Id);

	/// <inheritdoc />
	public override string ToString() => this.Id?.ToString() ?? string.Empty;

	/// <summary>
	/// Gets the debugger display.
	/// </summary>
	/// <returns>System.String.</returns>
	private string GetDebuggerDisplay() => this.ToString() ?? string.Empty;
}
