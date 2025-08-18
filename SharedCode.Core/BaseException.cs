using System.Collections;

namespace SharedCode;
/// <summary>
/// The base exception class. Implements the <see cref="Exception" />.
/// </summary>
/// <seealso cref="Exception" />
public class BaseException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseException"/> class.
	/// </summary>
	public BaseException() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseException"/> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public BaseException(string message) : base(message) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseException"/> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
	public BaseException(string message, Exception innerException) : base(message, innerException) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseException"/> class.
	/// </summary>
	/// <param name="innerException">The inner exception.</param>
	/// <param name="data">The exception data.</param>
	public BaseException(Exception innerException, IDictionary data) : base(innerException?.Message, innerException) => this.AddData(data);

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseException"/> class.
	/// </summary>
	/// <param name="message">The exception message.</param>
	/// <param name="innerException">The inner exception.</param>
	/// <param name="data">The exception data.</param>
	public BaseException(string message, Exception innerException, IDictionary data) : base(message, innerException) => this.AddData(data);
}
