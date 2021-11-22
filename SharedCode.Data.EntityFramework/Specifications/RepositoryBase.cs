// <copyright file="RepositoryBase.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data.EntityFramework.Specifications;

using Microsoft.EntityFrameworkCore;

using SharedCode.Data.EntityFramework.Specifications.Evaluators;
using SharedCode.Specifications;
using SharedCode.Specifications.Evaluators;
using SharedCode.Specifications.Exceptions;

/// <inheritdoc />
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
	private readonly DbContext dbContext;
	private readonly ISpecificationEvaluator specificationEvaluator;

	/// <summary>
	/// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
	/// </summary>
	/// <param name="dbContext">The database context.</param>
	protected RepositoryBase(DbContext dbContext)
		: this(dbContext, SpecificationEvaluator.Default)
	{
	}

	/// <inheritdoc />
	protected RepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
	{
		this.dbContext = dbContext;
		this.specificationEvaluator = specificationEvaluator;
	}

	/// <inheritdoc />
	public virtual async Task<T> AddAsync(T entity, CancellationToken? cancellationToken = default)
	{
		_ = this.dbContext.Set<T>().Add(entity);

		await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return entity;
	}

	/// <inheritdoc />
	public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken? cancellationToken = default) =>
		await this.ApplySpecification(specification, true).CountAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

	/// <inheritdoc />
	public virtual async Task<int> CountAsync(CancellationToken? cancellationToken = default) =>
		await this.dbContext.Set<T>().CountAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

	/// <inheritdoc />
	public virtual async Task RemoveAsync(T entity, CancellationToken? cancellationToken = default)
	{
		_ = this.dbContext.Set<T>().Remove(entity);

		await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public virtual async Task RemoveAsync(IEnumerable<T> entities, CancellationToken? cancellationToken = default)
	{
		this.dbContext.Set<T>().RemoveRange(entities);

		await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken? cancellationToken = default)
		where TId : notnull =>
		await this.dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken: cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

	/// <inheritdoc />
	public virtual async Task<T?> GetBySpecAsync<TSpecification>(TSpecification specification, CancellationToken? cancellationToken = default)
		where TSpecification : ISpecification<T>, ISingleResultSpecification =>
		await this.ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

	/// <inheritdoc />
	public virtual async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken? cancellationToken = default) =>
		await this.ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

	/// <inheritdoc />
	public virtual async Task<List<T>> ListAsync(CancellationToken? cancellationToken = default) =>
		await this.dbContext.Set<T>().ToListAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

	/// <inheritdoc />
	public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken? cancellationToken = default)
	{
		var queryResult = await this.ApplySpecification(specification).ToListAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

		return specification?.PostProcessingAction is null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
	}

	/// <inheritdoc />
	public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken? cancellationToken = default)
	{
		var queryResult = await this.ApplySpecification(specification).ToListAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

		return specification?.PostProcessingAction is null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
	}

	/// <inheritdoc />
	public virtual async Task SaveChangesAsync(CancellationToken? cancellationToken = default) =>
		await this.dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

	/// <inheritdoc />
	public virtual async Task UpdateAsync(T entity, CancellationToken? cancellationToken = default)
	{
		this.dbContext.Entry(entity).State = EntityState.Modified;

		await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Filters the entities of <typeparamref name="T" />, to those that match the encapsulated
	/// query logic of the <paramref name="specification" />.
	/// </summary>
	/// <param name="specification">The encapsulated query logic.</param>
	/// <param name="evaluateCriteriaOnly">A value indicating whether to evaluate criteria only.</param>
	/// <returns>The filtered entities as an <see cref="IQueryable{T}" />.</returns>
	protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false) =>
		this.specificationEvaluator.GetQuery(this.dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);

	/// <summary>
	/// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic
	/// of the <paramref name="specification" />, from the database.
	/// <para>Projects each entity into a new form, being <typeparamref name="TResult" />.</para>
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
	/// <param name="specification">The encapsulated query logic.</param>
	/// <returns>The filtered projected entities as an <see cref="IQueryable{T}" />.</returns>
	/// <exception cref="ArgumentNullException">specification</exception>
	/// <exception cref="SelectorNotFoundException">The selector was not found.</exception>
	protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
	{
		_ = specification ?? throw new ArgumentNullException(nameof(specification), "Specification is required");
		_ = specification.Selector ?? throw new SelectorNotFoundException();

		return this.specificationEvaluator.GetQuery(this.dbContext.Set<T>().AsQueryable(), specification);
	}
}
