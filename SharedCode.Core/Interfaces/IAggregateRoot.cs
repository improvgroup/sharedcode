


namespace SharedCode.Interfaces;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The aggregate root interface.
/// </summary>
/// <remarks>
/// Apply this marker interface only to aggregate root entities. Repositories will only work with
/// aggregate roots, not their children.
/// </remarks>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface.")]
public interface IAggregateRoot
{
}
