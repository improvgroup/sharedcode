// <copyright file="IDependencyResolver.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.DependencyInjection;

/// <summary>
/// The dependency resolver interface.
/// </summary>
public interface IDependencyResolver
{
	/// <summary>
	/// Sets up the dependency register.
	/// </summary>
	/// <param name="dependencyRegister">The dependency register.</param>
	void SetUp(IDependencyRegister dependencyRegister);
}
