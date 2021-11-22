// <copyright file="SpecificationBuilder.Generic.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;

/// <summary>
/// The specification builder class. Implements the <see cref="ISpecificationBuilder{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="ISpecificationBuilder{T}" />
public class SpecificationBuilder<T> : ISpecificationBuilder<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SpecificationBuilder{T}" /> class.
	/// </summary>
	/// <param name="specification">The specification.</param>
	public SpecificationBuilder(Specification<T> specification) => this.Specification = specification;

	/// <inheritdoc />
	public Specification<T> Specification { get; }
}
