using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SharedCode.DependencyInjection;

/// <summary>
/// The fluent interface.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IFluentInterface
{
    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(object? obj);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures
    /// like a hash table.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetHashCode();

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <returns>Type.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "It is not appropriate.")]
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Not applicable.")]
    Type GetType();

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    string? ToString();
}
