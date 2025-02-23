// <copyright file="LambdaComparer.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Tests
{
	/// <summary>
	/// The compare function delegate.
	/// </summary>
	/// <typeparam name="T">The type of the objects being compared.</typeparam>
	/// <param name="object1">The first object.</param>
	/// <param name="object2">The second object.</param>
	/// <returns><c>true</c> if the objects are equivalent, <c>false</c> otherwise.</returns>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
	public delegate bool CompareFunc<in T>(T object1, T object2);
}
