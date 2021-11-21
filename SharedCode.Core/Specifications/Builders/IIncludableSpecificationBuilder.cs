// <copyright file="IIncludableSpecificationBuilder.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Builders;

/// <summary>
/// Interface IIncludableSpecificationBuilder. Implements the <see cref="ISpecificationBuilder{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TProperty">The type of the property.</typeparam>
/// <seealso cref="ISpecificationBuilder{T}" />
public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
}
