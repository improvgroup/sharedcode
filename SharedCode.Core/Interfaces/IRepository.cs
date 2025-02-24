
namespace SharedCode.Interfaces;

/// <summary>
/// The repository interface. Implements the <see cref="IRepositoryBase{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="IRepositoryBase{T}" />
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
