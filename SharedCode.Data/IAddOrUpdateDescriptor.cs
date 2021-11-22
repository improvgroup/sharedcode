// <copyright file="IAddOrUpdateDescriptor.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

/// <summary>
/// The add or update descriptor interface.
/// </summary>
public interface IAddOrUpdateDescriptor
{
	/// <summary>
	/// Gets the type of the action.
	/// </summary>
	/// <value>The type of the action.</value>
	AddOrUpdate ActionType { get; }

	/// <summary>
	/// Gets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	int Id { get; }
}
