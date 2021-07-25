// <copyright file="IPersistable.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.WPF
{
	using System.Windows.Input;

	/// <summary>
	/// The persistable interface.
	/// </summary>
	public interface IPersistable
	{
		/// <summary>
		/// Gets the save command.
		/// </summary>
		/// <value>The save command.</value>
		ICommand SaveCommand { get; }
	}
}
