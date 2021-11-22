// <copyright file="IQueryResult.Generic.cs" company="improvGroup, LLC">
//     Copyright © 2013-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

using SharedCode.Models;

/// <summary>
/// The query result interface. Implements the <see cref="IQueryResult" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="IQueryResult" />
public interface IQueryResult<out TEntity> : IQueryResult where TEntity : Entity
{
	/// <summary>
	/// Gets the results.
	/// </summary>
	/// <value>The results.</value>
	new IEnumerable<TEntity> Results { get; }
}
