// <copyright file="IOrderedSpecificationBuilder.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Builders;

/// <summary>
/// Interface IOrderedSpecificationBuilder. Implements the <see cref="ISpecificationBuilder{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="ISpecificationBuilder{T}" />
public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
}
