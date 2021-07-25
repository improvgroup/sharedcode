// <copyright file="IRefreshable.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.WPF
{
	using System.Windows.Input;

	/// <summary>
	/// The refreshable interface.
	/// </summary>
	public interface IRefreshable
	{
		/// <summary>
		/// Gets the refresh command.
		/// </summary>
		/// <value>The refresh command.</value>
		ICommand RefreshCommand { get; }
	}
}
