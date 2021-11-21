// <copyright file="IInMemoryEvaluator.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Specifications.Evaluators;

using SharedCode.Specifications;

/// <summary>
/// The in-memory evaluator interface.
/// </summary>
public interface IInMemoryEvaluator
{
	/// <summary>
	/// Evaluates the specified query.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="query">The query.</param>
	/// <param name="specification">The specification.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification);
}
