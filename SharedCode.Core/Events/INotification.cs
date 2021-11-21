// <copyright file="INotification.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Events;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The notification interface.
/// </summary>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface.")]
public interface INotification
{
}
