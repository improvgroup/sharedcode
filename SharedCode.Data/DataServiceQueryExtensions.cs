// <copyright file="DataServiceQueryExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

/// <summary>
/// The data service query extensions class
/// </summary>
public static class DataServiceQueryExtensions
{
	/// <summary>
	/// Execute as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="this">The data service query.</param>
	/// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async IAsyncEnumerable<TResult> ExecuteAsync<TResult>(this DataServiceQuery<TResult> @this)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		foreach (var item in await Task.Factory.FromAsync(
			@this.BeginExecute(null, null),
			(queryAsyncResult) => @this.EndExecute(queryAsyncResult)).ConfigureAwait(false))
		{
			yield return item;
		}
	}

	/// <summary>
	/// Execute as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="this">The data service context.</param>
	/// <param name="requestUri">The request URI.</param>
	/// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async IAsyncEnumerable<TResult> ExecuteAsync<TResult>(this DataServiceContext @this, Uri requestUri)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		foreach (var item in await Task.Factory.FromAsync(
			@this.BeginExecute<TResult>(requestUri, null, null),
			(queryAsyncResult) => @this.EndExecute<TResult>(queryAsyncResult)).ConfigureAwait(false))
		{
			yield return item;
		}
	}

	/// <summary>
	/// Execute as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="this">The context.</param>
	/// <param name="queryContinuationToken">The query continuation token.</param>
	/// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async IAsyncEnumerable<TResult> ExecuteAsync<TResult>(this DataServiceContext @this, DataServiceQueryContinuation<TResult> queryContinuationToken)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		foreach (var item in await Task.Factory.FromAsync(
			@this.BeginExecute(queryContinuationToken, null, null),
			(queryAsyncResult) => @this.EndExecute<TResult>(queryAsyncResult)).ConfigureAwait(false))
		{
			yield return item;
		}
	}

	/// <summary>
	/// Execute batch as an asynchronous operation.
	/// </summary>
	/// <param name="this">The context.</param>
	/// <param name="requests">The data service requests.</param>
	/// <returns>A Task&lt;DataServiceResponse&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async Task<DataServiceResponse> ExecuteBatchAsync(this DataServiceContext @this, params DataServiceRequest[] requests)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		var queryTask = Task.Factory.FromAsync(
			@this.BeginExecuteBatch(null, null, requests),
			(queryAsyncResult) => @this.EndExecuteBatch(queryAsyncResult));

		return await queryTask.ConfigureAwait(false);
	}

	/// <summary>
	/// Load property as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="this">The context.</param>
	/// <param name="entity">The entity on which we are looking for a property.</param>
	/// <param name="propertyName">Name of the property.</param>
	/// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async IAsyncEnumerable<TResult> LoadPropertyAsync<TResult>(this DataServiceContext @this, object entity, string propertyName)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		var response = await Task.Factory.FromAsync(
			@this.BeginLoadProperty(entity, propertyName, null, null),
			(loadPropertyAsyncResult) => (IEnumerable<TResult>?)@this.EndLoadProperty(loadPropertyAsyncResult)).ConfigureAwait(false) ?? Enumerable.Empty<TResult>();

		foreach (var item in response)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Load property as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="this">The context.</param>
	/// <param name="entity">The entity on which we are looking for a property.</param>
	/// <param name="propertyName">Name of the property.</param>
	/// <param name="continuation">The data service query continuation.</param>
	/// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async IAsyncEnumerable<TResult> LoadPropertyAsync<TResult>(this DataServiceContext @this, object entity, string propertyName, DataServiceQueryContinuation continuation)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		var response = await Task.Factory.FromAsync(
			@this.BeginLoadProperty(entity, propertyName, continuation, null, null),
			(loadPropertyAsyncResult) => (IEnumerable<TResult>?)@this.EndLoadProperty(loadPropertyAsyncResult)).ConfigureAwait(false) ?? Enumerable.Empty<TResult>();

		foreach (var item in response)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Load property as an asynchronous operation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="this">The context.</param>
	/// <param name="entity">The entity on which we are looking for a property.</param>
	/// <param name="propertyName">Name of the property.</param>
	/// <param name="nextLinkUri">The next link URI.</param>
	/// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async IAsyncEnumerable<TResult> LoadPropertyAsync<TResult>(this DataServiceContext @this, object entity, string propertyName, Uri nextLinkUri)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		var response = await Task.Factory.FromAsync(
			@this.BeginLoadProperty(entity, propertyName, nextLinkUri, null, null),
			(loadPropertyAsyncResult) => (IEnumerable<TResult>?)@this.EndLoadProperty(loadPropertyAsyncResult)).ConfigureAwait(false) ?? Enumerable.Empty<TResult>();

		foreach (var item in response)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Executes the data service query asynchronously.
	/// </summary>
	/// <typeparam name="T">The type of the items in the resulting enumerable.</typeparam>
	/// <param name="this">The data service query.</param>
	/// <returns>A task returning an enumerable.</returns>
	/// <exception cref="ArgumentNullException">query</exception>
	public static async IAsyncEnumerable<T> QueryAsync<T>(this DataServiceQuery<T> @this)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));
		Contract.Ensures(Contract.Result<IAsyncEnumerable<T>>() is not null);

		var result = await Task<IEnumerable<T>>.Factory.FromAsync(@this.BeginExecute, @this.EndExecute, null).ConfigureAwait(false);
		foreach (var item in result)
		{
			yield return item;
		}
	}

	/// <summary>
	/// Save changes as an asynchronous operation.
	/// </summary>
	/// <param name="this">The context.</param>
	/// <returns>A Task&lt;DataServiceResponse&gt; representing the asynchronous operation.</returns>
	public static async Task<DataServiceResponse> SaveChangesAsync(this DataServiceContext @this) => await SaveChangesAsync(@this, SaveChangesOptions.None).ConfigureAwait(false);

	/// <summary>
	/// Save changes as an asynchronous operation.
	/// </summary>
	/// <param name="this">The context.</param>
	/// <param name="options">The save changes options.</param>
	/// <returns>A Task&lt;DataServiceResponse&gt; representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">this</exception>
	public static async Task<DataServiceResponse> SaveChangesAsync(this DataServiceContext @this, SaveChangesOptions options)
	{
		_ = @this ?? throw new ArgumentNullException(nameof(@this));

		var queryTask = Task.Factory.FromAsync(
			@this.BeginSaveChanges(options, null, null),
			(queryAsyncResult) => @this.EndSaveChanges(queryAsyncResult));

		return await queryTask.ConfigureAwait(false);
	}
}
