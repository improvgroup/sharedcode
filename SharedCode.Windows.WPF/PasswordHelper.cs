


namespace SharedCode.Windows.WPF
{
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Class PasswordHelper.
	/// </summary>
	public static class PasswordHelper
	{
		/// <summary>
		/// The attach property
		/// </summary>
		public static readonly DependencyProperty AttachProperty =
			DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

		/// <summary>
		/// The password property
		/// </summary>
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.RegisterAttached("Password",
			typeof(string), typeof(PasswordHelper),
			new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

		/// <summary>
		/// The is updating property
		/// </summary>
		private static readonly DependencyProperty IsUpdatingProperty =
			DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

		/// <summary>
		/// Gets the attach.
		/// </summary>
		/// <param name="dependencyObject">The dependency object.</param>
		/// <returns><c>true</c> if attached, <c>false</c> otherwise.</returns>
		public static bool GetAttach(DependencyObject dependencyObject) => (bool)(dependencyObject?.GetValue(AttachProperty) ?? false);

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <param name="dependencyObject">The dependency object.</param>
		/// <returns>System.String.</returns>
		public static string GetPassword(DependencyObject dependencyObject) => (string)(dependencyObject?.GetValue(PasswordProperty) ?? string.Empty);

		/// <summary>
		/// Sets the attach.
		/// </summary>
		/// <param name="dependencyObject">The dependency object.</param>
		/// <param name="value">if set to <c>true</c> [value].</param>
		public static void SetAttach(DependencyObject dependencyObject, bool value) => dependencyObject?.SetValue(AttachProperty, value);

		/// <summary>
		/// Sets the password.
		/// </summary>
		/// <param name="dependencyObject">The dependency object.</param>
		/// <param name="value">The password.</param>
		public static void SetPassword(DependencyObject dependencyObject, string value) => dependencyObject?.SetValue(PasswordProperty, value);

		/// <summary>
		/// Attaches the specified sender.
		/// </summary>
		/// <param name="sender">The dependency object.</param>
		/// <param name="e">
		/// The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.
		/// </param>
		private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is not PasswordBox passwordBox)
				return;

			if ((bool)e.OldValue)
				passwordBox.PasswordChanged -= PasswordChanged;

			if ((bool)e.NewValue)
				passwordBox.PasswordChanged += PasswordChanged;
		}

		/// <summary>
		/// Gets the is updating.
		/// </summary>
		/// <param name="dependencyObject">The password box.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private static bool GetIsUpdating(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsUpdatingProperty);

		/// <summary>
		/// Handles the PasswordPropertyChanged event.
		/// </summary>
		/// <param name="sender">The password box.</param>
		/// <param name="e">
		/// The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.
		/// </param>
		private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is not PasswordBox passwordBox)
			{
				return;
			}

			passwordBox.PasswordChanged -= PasswordChanged;

			if (!GetIsUpdating(passwordBox))
			{
				passwordBox.Password = (string)e.NewValue;
			}

			passwordBox.PasswordChanged += PasswordChanged;
		}

		/// <summary>
		/// Passwords the changed.
		/// </summary>
		/// <param name="sender">The password box.</param>
		/// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (sender is not PasswordBox passwordBox)
			{
				return;
			}

			SetIsUpdating(passwordBox, true);
			SetPassword(passwordBox, passwordBox.Password);
			SetIsUpdating(passwordBox, false);
		}

		/// <summary>
		/// Sets the is updating.
		/// </summary>
		/// <param name="dependencyObject">The dependency object.</param>
		/// <param name="value">if set to <c>true</c> [value].</param>
		private static void SetIsUpdating(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsUpdatingProperty, value);
	}
}
