// <copyright file="CacheSpecificationBuilder.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Builders;

/// <summary>
/// The cache specification builder class. Implements the <see cref="ICacheSpecificationBuilder{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="ICacheSpecificationBuilder{T}" />
public class CacheSpecificationBuilder<T> : ICacheSpecificationBuilder<T> where T : class
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CacheSpecificationBuilder{T}" /> class.
	/// </summary>
	/// <param name="specification">The specification.</param>
	public CacheSpecificationBuilder(Specification<T> specification) => this.Specification = specification;

	/// <summary>
	/// Gets the specification.
	/// </summary>
	/// <value>The specification.</value>
	public Specification<T> Specification { get; }
}
