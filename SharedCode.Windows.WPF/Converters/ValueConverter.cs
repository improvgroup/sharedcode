namespace SharedCode.Windows.WPF.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows.Markup;

	/// <summary>
	/// An abstract value converter class that allows direct XAML usage.
	/// </summary>
	/// <typeparam name="T">The type of the derived converter.</typeparam>
	public abstract class ValueConverter<T> : MarkupExtension, IValueConverter
		where T : new()
	{
		/// <summary>
		/// A single static instance of this value converter.
		/// </summary>
		private static T? Converter;

		/// <summary>Converts a value.</summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);

		/// <summary>Converts a value.</summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		public abstract object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);

		/// <summary>When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.</summary>
		/// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
		/// <returns>The object value to set on the property where the extension is applied.</returns>
		public override object? ProvideValue(IServiceProvider serviceProvider) => Converter ??= new T();
	}
}
