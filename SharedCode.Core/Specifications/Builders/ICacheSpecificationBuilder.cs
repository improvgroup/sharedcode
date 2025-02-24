
namespace SharedCode.Specifications.Builders;

/// <summary>
/// The cache specification builder class. Implements the <see cref="ISpecificationBuilder{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="ISpecificationBuilder{T}" />
public interface ICacheSpecificationBuilder<T> : ISpecificationBuilder<T> where T : class
{
}
