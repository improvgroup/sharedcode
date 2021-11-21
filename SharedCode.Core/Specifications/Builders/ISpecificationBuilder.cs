// <copyright file="ISpecificationBuilder.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Builders;

/// <summary>
/// The specification builder interface. Implements the <see cref="ISpecificationBuilder{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult">The type of the t result.</typeparam>
/// <seealso cref="ISpecificationBuilder{T}" />
public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T>
{
	/// <summary>
	/// Gets the specification.
	/// </summary>
	/// <value>The specification.</value>
	new Specification<T, TResult> Specification { get; }
}
