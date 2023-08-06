namespace SharedCode.Domain;

using System.Collections.Generic;
using System.Net;

/// <summary>
/// The HTTP error result class. Implements the <see cref="ErrorResult" />.
/// </summary>
/// <seealso cref="ErrorResult" />
public class HttpErrorResult : ErrorResult
{
	/// <summary>
	/// Initializes a new instance of the <see cref="HttpErrorResult" /> class.
	/// </summary>
	/// <param name="message">The error message.</param>
	/// <param name="statusCode">The status code.</param>
	public HttpErrorResult(string message, HttpStatusCode statusCode) : base(message) => this.StatusCode = statusCode;

	/// <summary>
	/// Initializes a new instance of the <see cref="HttpErrorResult" /> class.
	/// </summary>
	/// <param name="message">The error message.</param>
	/// <param name="errors">The list of errors.</param>
	/// <param name="statusCode">The status code.</param>
	public HttpErrorResult(string message, IReadOnlyCollection<Error> errors, HttpStatusCode statusCode) : base(message, errors) => this.StatusCode = statusCode;

	/// <summary>
	/// Gets the status code.
	/// </summary>
	/// <value>The status code.</value>
	public HttpStatusCode StatusCode { get; }
}
