
namespace SharedCode.Specifications.Builders;

/// <summary>
/// The specification builder class. Implements the <see cref="SpecificationBuilder{T}" />.
/// Implements the <see cref="ISpecificationBuilder{T, TResult}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <seealso cref="SpecificationBuilder{T}" />
/// <seealso cref="ISpecificationBuilder{T, TResult}" />
public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpecificationBuilder{T, TResult}" /> class.
	/// </summary>
	/// <param name="specification">The specification.</param>
	public SpecificationBuilder(Specification<T, TResult> specification)
		: base(specification) => this.Specification = specification;

	/// <summary>
	/// Gets the specification.
	/// </summary>
	/// <value>The specification.</value>
	public new Specification<T, TResult> Specification { get; }
}
