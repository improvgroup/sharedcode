
namespace SharedCode;

/// <summary>
/// The ignore member attribute class. Implements the <see cref="Attribute" />.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class IgnoreMemberAttribute : Attribute
{
}
