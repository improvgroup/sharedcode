// <copyright file="ViewModelBase.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.WPF
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// An implementation of <see cref="INotifyPropertyChanged" /> and <see
	/// cref="INotifyPropertyChanging" /> to simplify models.
	/// </summary>
	public abstract class BindableBase : INotifyPropertyChanged, INotifyPropertyChanging
	{
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Occurs when a property value is about to change.
		/// </summary>
		public event PropertyChangingEventHandler? PropertyChanging;

		/// <summary>
		/// Called when a property value has changed.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// Handles the <see cref="PropertyChanged" /> event.
		/// </summary>
		/// <param name="e">
		/// The <see cref="PropertyChangedEventArgs" /> instance containing the event data.
		/// </param>
		/// <exception cref="ArgumentNullException">e</exception>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			_ = e ?? throw new ArgumentNullException(nameof(e));
			this.VerifyPropertyName(e.PropertyName);
			this.PropertyChanged?.Invoke(this, e);
		}

		/// <summary>
		/// Called when a property value is about to change.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		protected void OnPropertyChanging([CallerMemberName] string? propertyName = null) =>
			this.OnPropertyChanging(new PropertyChangingEventArgs(propertyName));

		/// <summary>
		/// Handles the <see cref="PropertyChanging" /> event.
		/// </summary>
		/// <param name="e">
		/// The <see cref="PropertyChangingEventArgs" /> instance containing the event data.
		/// </param>
		/// <exception cref="ArgumentNullException">e</exception>
		protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
		{
			_ = e ?? throw new ArgumentNullException(nameof(e));
			this.VerifyPropertyName(e.PropertyName);
			this.PropertyChanging?.Invoke(this, e);
		}

		/// <summary>
		/// Checks if a property already matches a desired value. Sets the property and notifies
		/// listeners only when necessary.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="storage">A reference to a property with both getter and setter.</param>
		/// <param name="value">The desired value for the property.</param>
		/// <param name="propertyName">
		/// The name of the property used to notify listeners. This value is optional and can be
		/// provided automatically when invoked from compilers that support CallerMemberName.
		/// </param>
		/// <returns>
		/// True if the value was changed, false if the existing value matched the desired value.
		/// </returns>
		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
			{
				return false;
			}

			this.OnPropertyChanging(propertyName);

			storage = value;

			this.OnPropertyChanged(propertyName);

			return true;
		}

		/// <summary>
		/// Checks if a property already matches a desired value. Sets the property and notifies
		/// listeners only when necessary.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="storage">A reference to a property with both getter and setter.</param>
		/// <param name="value">The desired value for the property.</param>
		/// <param name="propertyName">
		/// The name of the property used to notify listeners. This value is optional and can be
		/// provided automatically when invoked from compilers that support CallerMemberName.
		/// </param>
		/// <param name="onChanged">
		/// The action that is called after the property value has been changed.
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

		[Conditional("DEBUG")]
		private void VerifyPropertyName(string? propertyName)
		{
			if (propertyName is null)
			{
				throw new ArgumentNullException(nameof(propertyName));
			}

			if (TypeDescriptor.GetProperties(this)[propertyName] is null)
			{
				throw new ArgumentNullException($"{this.GetType().Name} does not contain property: {propertyName}");
			}
		}
	}
}
