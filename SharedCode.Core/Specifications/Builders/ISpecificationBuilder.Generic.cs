
namespace SharedCode.Specifications.Builders;

/// <summary>
/// The specification builder class.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISpecificationBuilder<T>
{
	/// <summary>
	/// Gets the specification.
	/// </summary>
	/// <value>The specification.</value>
	Specification<T> Specification { get; }
}
