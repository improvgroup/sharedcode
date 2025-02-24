﻿
namespace SharedCode.Specifications.Builders;

using SharedCode.Specifications;

/// <summary>
/// Class OrderedSpecificationBuilder. Implements the <see cref="IOrderedSpecificationBuilder{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="IOrderedSpecificationBuilder{T}" />
public class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="OrderedSpecificationBuilder{T}" /> class.
	/// </summary>
	/// <param name="specification">The specification.</param>
	public OrderedSpecificationBuilder(Specification<T> specification) => this.Specification = specification;

	/// <summary>
	/// Gets the specification.
	/// </summary>
	/// <value>The specification.</value>
	public Specification<T> Specification { get; }
}
