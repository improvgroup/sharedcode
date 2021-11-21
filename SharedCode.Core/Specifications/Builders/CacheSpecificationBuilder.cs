namespace SharedCode.Specifications.Builders;
public class CacheSpecificationBuilder<T> : ICacheSpecificationBuilder<T> where T : class
{
	public Specification<T> Specification { get; }

	public CacheSpecificationBuilder(Specification<T> specification) => this.Specification = specification;
}
