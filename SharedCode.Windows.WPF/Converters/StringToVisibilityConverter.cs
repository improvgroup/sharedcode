namespace SharedCode.Windows.WPF.Converters
{
	using System;
	using System.Globalization;
	using System.Windows;

	/// <summary>
	/// A string to visibility converter class.
	/// </summary>
	public class StringToVisibilityConverter : ValueConverter<StringToVisibilityConverter>
	{
		/// <summary>Converts a value.</summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			parameter is null
				? string.IsNullOrWhiteSpace(value?.ToString()) ? (object)Visibility.Collapsed : Visibility.Visible
				: string.IsNullOrWhiteSpace(value?.ToString()) ? (object)Visibility.Visible : Visibility.Collapsed;

		/// <summary>Converts a value.</summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			parameter is null
				? ((Visibility?)value == Visibility.Visible).ToString()
				: ((Visibility?)value != Visibility.Visible).ToString();
	}
}
