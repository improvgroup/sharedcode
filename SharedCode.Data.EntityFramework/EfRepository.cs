
namespace SharedCode.Data.EntityFramework;

using Microsoft.EntityFrameworkCore;

using SharedCode.Data.EntityFramework.Specifications;
using SharedCode.Interfaces;

/// <summary>
/// The Entity Framework repository class. Implements the <see cref="RepositoryBase{T}" />.
/// Implements the <see cref="IReadRepository{T}" />. Implements the <see cref="IRepository{T}" />.
/// </summary>
/// <typeparam name="T">The type of entities in the repository.</typeparam>
/// <seealso cref="RepositoryBase{T}" />
/// <seealso cref="IReadRepository{T}" />
/// <seealso cref="IRepository{T}" />
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EfRepository{T}" /> class.
	/// </summary>
	/// <param name="dbContext">The database context.</param>
	public EfRepository(DbContext dbContext) : base(dbContext)
	{
	}
}
