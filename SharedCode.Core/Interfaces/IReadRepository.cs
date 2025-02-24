


namespace SharedCode.Interfaces;

/// <summary>
/// The read repository interface. Implements the <see cref="IReadRepositoryBase{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="IReadRepositoryBase{T}" />
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
