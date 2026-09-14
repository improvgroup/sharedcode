using System.Net;

namespace SharedCode.Web.Middleware;

/// <summary>
/// Represents errors that occur during application execution related to HTTP requests and
/// responses. Implements the <see cref="Exception"/>
/// </summary>
/// <seealso cref="Exception"/>
public class HttpException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    public HttpException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    public HttpException(HttpStatusCode httpStatusCode) => this.StatusCode = (int)httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    public HttpException(int httpStatusCode) => this.StatusCode = httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public HttpException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The message.</param>
    public HttpException(int httpStatusCode, string message) : base(message) => this.StatusCode = httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The message.</param>
    public HttpException(HttpStatusCode httpStatusCode, string message) : base(message) =>
        this.StatusCode = (int)httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference (
    /// <see langword="Nothing"/> in Visual Basic) if no inner exception is specified.
    /// </param>
    public HttpException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public HttpException(int httpStatusCode, string message, Exception innerException)
        : base(message, innerException) => this.StatusCode = httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public HttpException(HttpStatusCode httpStatusCode, string message, Exception innerException)
        : base(message, innerException) => this.StatusCode = (int)httpStatusCode;

    /// <summary>
    /// Gets the status code.
    /// </summary>
    /// <value>The status code.</value>
    public int StatusCode { get; }
}
