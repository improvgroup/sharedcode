// <copyright file="AssertExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;

/// <summary>
/// The assert extensions class.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public static class AssertExtensions
{
	/// <summary>
	/// Asserts that the expected and actual values are equal using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type being compared.</typeparam>
	/// <param name="_">The assert class.</param>
	/// <param name="expected">The expected value.</param>
	/// <param name="actual">The actual value.</param>
	/// <param name="comparer">The comparer class.</param>
	public static void AreEqual<T>(this Assert _, T expected, T actual, IComparer comparer) =>
		CollectionAssert.AreEqual(new[] { expected }, new[] { actual }, comparer, $"\nExpected: <{expected}>.\nActual: <{actual}>.");

	/// <summary>
	/// Asserts that the expected and actual values are equal using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type being compared.</typeparam>
	/// <param name="_">The assert class.</param>
	/// <param name="expected">The expected value.</param>
	/// <param name="actual">The actual value.</param>
	/// <param name="compareFunction">The compare function.</param>
	public static void AreEqual<T>(this Assert _, T expected, T actual, CompareFunc<T> compareFunction) =>
		CollectionAssert.AreEqual(new[] { expected }, new[] { actual }, new LambdaComparer<T>(compareFunction), $"\nExpected: <{expected}>.\nActual: <{actual}>.");
}
