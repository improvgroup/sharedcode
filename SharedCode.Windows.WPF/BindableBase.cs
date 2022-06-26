// <copyright file="ViewModelBase.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.WPF
{
	using CommunityToolkit.Mvvm.ComponentModel;

	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// The bindable base class. Implements the <see cref="ObservableObject" />.
	/// </summary>
	/// <seealso cref="ObservableObject" />
	public abstract class BindableBase : ObservableObject
	{
		/// <summary>
		/// Checks if a property already matches a desired value. Sets the property and notifies
		/// listeners only when necessary.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="storage">A reference to a property with both getter and setter.</param>
		/// <param name="value">The desired value for the property.</param>
		/// <param name="onChanged">
		/// The action that is called after the property value has been changed.
		/// </param>
		/// <param name="propertyName">
		/// The name of the property used to notify listeners. This value is optional and can be
		/// provided automatically when invoked from compilers that support CallerMemberName.
		/// </param>
		/// <returns>
		/// True if the value was changed, false if the existing value matched the desired value.
		/// </returns>
		protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string? propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
			{
				return false;
			}

			this.OnPropertyChanging(propertyName);

			storage = value;

			onChanged?.Invoke();

			this.OnPropertyChanged(propertyName);

			return true;
		}
	}
}
