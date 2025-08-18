namespace SharedCode.Windows.WPF.Converters;

using System;
using System.Globalization;

/// <summary>
/// A number to string converter for one way binding.
/// </summary>
public class NumberToStringConverter : ValueConverter<BooleanToVisibilityConverter>
{
	/// <inheritdoc />
	public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return targetType == typeof(string)
			? value switch
			{
				null => null,
				short s => s.ToString(culture),
				int i => i.ToString(culture),
				long l => l.ToString(culture),
				float f => f.ToString(culture),
				double d => d.ToString(culture),
				decimal c => c.ToString(culture),
				_ => System.Convert.ToString(value, culture),
			}
			: System.Convert.ChangeType(value, targetType, culture);
	}

	/// <inheritdoc />
	public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
