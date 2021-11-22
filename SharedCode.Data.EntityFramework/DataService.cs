// <copyright file="DataService.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework
{
	using Microsoft.EntityFrameworkCore;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// The data service.
	/// </summary>
	/// <typeparam name="T">The type of entities on which the data service operates.</typeparam>
	public class DataService<T> : IDataService<T> where T : class
	{
		private readonly IDbContextFactory<DbContext> dbContextFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataService{T}" /> class.
		/// </summary>
		/// <param name="dbContextFactory">The data context factory.</param>
		public DataService(IDbContextFactory<DbContext> dbContextFactory) =>
			this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

		/// <inheritdoc />
		public async Task<T> Create(T entity, CancellationToken cancellationToken = default)
		{
			using var context = this.dbContextFactory.CreateDbContext();

			var entry = await context.Set<T>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
			_ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return entry.Entity;
		}

		/// <inheritdoc />
		public async Task<bool> Delete<TKey>(TKey key, CancellationToken cancellationToken = default)
		{
			using var context = this.dbContextFactory.CreateDbContext();

			var entity = await context.Set<T>().FindAsync(new object?[] { key }, cancellationToken: cancellationToken).ConfigureAwait(false);
			if (entity is null)
				return false;

			_ = context.Set<T>().Remove(entity);
			var result = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			return result > 0;
		}

		/// <inheritdoc />
		public async Task<bool> Delete(T entity, CancellationToken cancellationToken = default)
		{
			using var context = this.dbContextFactory.CreateDbContext();
			var entry = context.Set<T>().Attach(entity);
			entry.State = EntityState.Deleted;
			var result = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return result > 0;
		}

		/// <inheritdoc />
		public async Task<T?> Get<TKey>(TKey key, CancellationToken cancellationToken = default)
		{
			using var context = this.dbContextFactory.CreateDbContext();
			return await context.Set<T>().FindAsync(new object?[] { key }, cancellationToken: cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		public IAsyncEnumerable<T> Get(Expression<Func<T, bool>>? expression = null)
		{
			using var context = this.dbContextFactory.CreateDbContext();
			return expression is null ? context.Set<T>().AsNoTracking().AsAsyncEnumerable() : context.Set<T>().Where(expression).AsNoTracking().AsAsyncEnumerable();
		}

		/// <inheritdoc />
		public IQueryable<T> Query(Expression<Func<T, bool>>? expression = null)
		{
			using var context = this.dbContextFactory.CreateDbContext();
			return expression is null ? context.Set<T>().AsNoTracking() : context.Set<T>().Where(expression).AsNoTracking();
		}

		/// <inheritdoc />
		public async Task<T> Update(T entity, CancellationToken cancellationToken = default)
		{
			using var context = this.dbContextFactory.CreateDbContext();

			var entry = context.Set<T>().Attach(entity);
			entry.State = EntityState.Modified;

			var result = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return result > 0 ? entry.Entity : entity;
		}
	}
}
