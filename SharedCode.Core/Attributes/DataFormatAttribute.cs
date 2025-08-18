namespace SharedCode.Attributes;
/// <summary>
/// Specifies a custom format to use when serializing objects supporting IFormattable.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class DataFormatAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DataFormatAttribute" /> class.
	/// </summary>
	/// <param name="format">
	/// If the type of the field/property implements IFormattable, this format will be used when writing.
	/// </param>
	public DataFormatAttribute(string format) => this.Format = format ?? throw new ArgumentNullException(nameof(format));

	/// <summary>
	/// Gets the format to use when serializing objects supporting IFormattable.
	/// </summary>
	/// <value>The format to use when serializing objects supporting IFormattable.</value>
	public string Format { get; }
}
