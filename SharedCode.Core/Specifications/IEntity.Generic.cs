// <copyright file="IEntity.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications;

/// <summary>
/// The <see cref="IEntity{TIdentifier}" /> interface.
/// </summary>
/// <typeparam name="TIdentifier">
/// The type of the unique identifier representing this <see cref="IEntity{TIdentifier}" />.
/// </typeparam>
public interface IEntity<TIdentifier>
{
	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	TIdentifier Id { get; set; }
}
