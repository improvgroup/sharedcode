namespace SharedCode.Windows.WPF.Converters
{
	using System;
	using System.Globalization;
	using System.Windows;

	/// <summary>
	/// A boolean to visibility converter class.
	/// </summary>
	public class BooleanToVisibilityConverter : ValueConverter<BooleanToVisibilityConverter>
	{
		/// <summary>Converts a value.</summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
			parameter is null
				? (bool?)value ?? false ? Visibility.Visible : (object)Visibility.Collapsed
				: (bool?)value ?? false ? Visibility.Collapsed : (object)Visibility.Visible;

		/// <summary>Converts a value.</summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
			parameter is null
				? (Visibility?)value == Visibility.Visible
				: (object)((Visibility?)value != Visibility.Visible);
	}
}
