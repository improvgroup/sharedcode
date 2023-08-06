namespace SharedCode.Domain;

/// <summary>
/// The validation error class. Implements the <see cref="Error" />.
/// </summary>
/// <seealso cref="Error" />
public class ValidationError : Error
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationError" /> class.
	/// </summary>
	/// <param name="propertyName">The name of the property.</param>
	/// <param name="details">The error details.</param>
	public ValidationError(string propertyName, string details) : base(null, details) => this.PropertyName = propertyName;

	/// <summary>
	/// Gets the name of the property.
	/// </summary>
	/// <value>The name of the property.</value>
	public string PropertyName { get; }
}
