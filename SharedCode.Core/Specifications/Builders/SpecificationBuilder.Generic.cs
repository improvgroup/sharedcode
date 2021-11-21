namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;

public class SpecificationBuilder<T> : ISpecificationBuilder<T>
{
	public Specification<T> Specification { get; }

	public SpecificationBuilder(Specification<T> specification) => this.Specification = specification;
}
