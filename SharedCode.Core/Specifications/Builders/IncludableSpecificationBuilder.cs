namespace SharedCode.Specifications.Builders;
/// <summary>
/// The includable specification builder class. Implements the <see
/// cref="IIncludableSpecificationBuilder{T, TProperty}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TProperty">The type of the t property.</typeparam>
/// <seealso cref="IIncludableSpecificationBuilder{T, TProperty}" />
public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty> where T : class
{
	/// <summary>
	/// Initializes a new instance of the <see cref="IncludableSpecificationBuilder{T, TProperty}"
	/// /> class.
	/// </summary>
	/// <param name="specification">The specification.</param>
	public IncludableSpecificationBuilder(Specification<T> specification) => this.Specification = specification;

	/// <summary>
	/// Gets the specification.
	/// </summary>
	/// <value>The specification.</value>
	public Specification<T> Specification { get; }
}
