namespace SharedCode.Domain;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The error class.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "<Pending>")]
public class Error
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Error" /> class.
	/// </summary>
	/// <param name="details">The error details.</param>
	public Error(string details) : this(null, details)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Error" /> class.
	/// </summary>
	/// <param name="code">The error code.</param>
	/// <param name="details">The error details.</param>
	public Error(string? code, string? details)
	{
		this.Code = code;
		this.Details = details;
	}

	/// <summary>
	/// Gets the code.
	/// </summary>
	/// <value>The code.</value>
	public string? Code { get; }

	/// <summary>
	/// Gets the details.
	/// </summary>
	/// <value>The details.</value>
	public string? Details { get; }
}
