// <copyright file="AssertExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The assert extensions class.
/// </summary>
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
	[SuppressMessage("Naming", "GCop218:For consistency and clarity, use '@this' instead of '{0}' for the first parameter of extension methods", Justification = "The parameter is unused.")]
	[SuppressMessage("Style", "GCop436:As the implementation is relatively long, change this into a standard method implementation.", Justification = "One liner.")]
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
	[SuppressMessage("Naming", "GCop218:For consistency and clarity, use '@this' instead of '{0}' for the first parameter of extension methods", Justification = "The parameter is unused.")]
	[SuppressMessage("Style", "GCop436:As the implementation is relatively long, change this into a standard method implementation.", Justification = "One liner.")]
	public static void AreEqual<T>(this Assert _, T expected, T actual, CompareFunc<T> compareFunction) =>
		CollectionAssert.AreEqual(new[] { expected }, new[] { actual }, new LambdaComparer<T>(compareFunction), $"\nExpected: <{expected}>.\nActual: <{actual}>.");
}
