// <copyright file="IProducer.cs" company="improvGroup, LLC">
//     Copyright © 2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Notifications;

/// <summary>
/// The producer interface.
/// </summary>
public interface IProducer
{
	/// <summary>
	/// Produces the specified object.
	/// </summary>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <param name="produced">The object being produced.</param>
	void Produce<T>(T produced) where T : notnull;
}
