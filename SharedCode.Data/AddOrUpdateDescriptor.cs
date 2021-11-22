// <copyright file="AddOrUpdateDescriptor.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

/// <summary>
/// The add or update descriptor class. Implements the <see cref="IAddOrUpdateDescriptor" />.
/// </summary>
/// <seealso cref="IAddOrUpdateDescriptor" />
public class AddOrUpdateDescriptor : IAddOrUpdateDescriptor
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AddOrUpdateDescriptor" /> class.
	/// </summary>
	/// <param name="actionType">Type of the action.</param>
	/// <param name="id">The identifier.</param>
	public AddOrUpdateDescriptor(AddOrUpdate actionType, int id)
	{
		this.ActionType = actionType;
		this.Id = id;
	}

	/// <inheritdoc />
	public AddOrUpdate ActionType { get; }

	/// <inheritdoc />
	public int Id { get; }
}
