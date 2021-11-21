namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;

public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty> where T : class
{
	public Specification<T> Specification { get; }

	public IncludableSpecificationBuilder(Specification<T> specification) => this.Specification = specification;
}
