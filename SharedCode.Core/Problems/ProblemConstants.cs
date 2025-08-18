namespace SharedCode.Problems;

/// <summary>
/// All constants related to Problem types.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Constant holder class.")]
public static class ProblemConstants
{
	/// <summary>
	/// Keys for Problem extensions dictionary to avoid string literals.
	/// </summary>
	public static class ExtensionKeys
	{
		/// <summary>
		/// Key for error code in problem extensions
		/// </summary>
		public const string ErrorCode = "errorCode";

		/// <summary>
		/// Key for validation errors in problem extensions
		/// </summary>
		public const string Errors = "errors";

		/// <summary>
		/// Key for error type (enum type name) in problem extensions
		/// </summary>
		public const string ErrorType = "errorType";

		/// <summary>
		/// Key for exception data prefix
		/// </summary>
		public const string ExceptionDataPrefix = "exception.";

		/// <summary>
		/// Key for original exception type in problem extensions
		/// </summary>
		public const string OriginalExceptionType = "originalExceptionType";

		/// <summary>
		/// Key for trace ID in problem extensions
		/// </summary>
		public const string TraceId = "traceId";
	}

	/// <summary>
	/// Standard problem detail messages.
	/// </summary>
	public static class Messages
	{
		/// <summary>
		/// The request could not be understood by the server due to malformed syntax.
		/// </summary>
		public const string BadRequest = "The request could not be understood by the server due to malformed syntax.";

		/// <summary>
		/// You do not have permission to access this resource.
		/// </summary>
		public const string ForbiddenAccess = "You do not have permission to access this resource.";

		/// <summary>
		/// An error occurred.
		/// </summary>
		public const string GenericError = "An error occurred";

		/// <summary>
		/// Invalid.
		/// </summary>
		public const string InvalidMessage = "Invalid";

		/// <summary>
		/// The requested resource was not found.
		/// </summary>
		public const string ResourceNotFound = "The requested resource was not found.";

		/// <summary>
		/// Authentication is required to access this resource.
		/// </summary>
		public const string UnauthorizedAccess = "Authentication is required to access this resource.";

		/// <summary>
		/// One or more validation errors occurred.
		/// </summary>
		public const string ValidationErrors = "One or more validation errors occurred.";
	}

	/// <summary>
	/// Standard problem titles for common HTTP status codes.
	/// </summary>
	public static class Titles
	{
		/// <summary>
		/// Title for HTTP 400 Bad Request.
		/// </summary>
		public const string BadRequest = "Bad Request";

		/// <summary>
		/// Title for HTTP 409 Conflict.
		/// </summary>
		public const string Conflict = "Conflict";

		/// <summary>
		/// Title for a generic error.
		/// </summary>
		public const string Error = "Error";

		/// <summary>
		/// Title for HTTP 403 Forbidden.
		/// </summary>
		public const string Forbidden = "Forbidden";

		/// <summary>
		/// Title for HTTP 504 Gateway Timeout.
		/// </summary>
		public const string GatewayTimeout = "Gateway Timeout";

		/// <summary>
		/// Title for HTTP 500 Internal Server Error.
		/// </summary>
		public const string InternalServerError = "Internal Server Error";

		/// <summary>
		/// Title for HTTP 404 Not Found.
		/// </summary>
		public const string NotFound = "Not Found";

		/// <summary>
		/// Title for HTTP 503 Service Unavailable.
		/// </summary>
		public const string ServiceUnavailable = "Service Unavailable";

		/// <summary>
		/// Title for HTTP 429 Too Many Requests.
		/// </summary>
		public const string TooManyRequests = "Too Many Requests";

		/// <summary>
		/// Title for HTTP 401 Unauthorized.
		/// </summary>
		public const string Unauthorized = "Unauthorized";

		/// <summary>
		/// Title for HTTP 422 Unprocessable Entity.
		/// </summary>
		public const string UnprocessableEntity = "Unprocessable Entity";

		/// <summary>
		/// Title for validation failures.
		/// </summary>
		public const string ValidationFailed = "Validation Failed";
	}

	/// <summary>
	/// Standard problem type URIs according to RFC 7807.
	/// </summary>
	public static class Types
	{
		/// <summary>
		/// URI for a generic problem type.
		/// </summary>
		public const string AboutBlank = "about:blank";

		/// <summary>
		/// URI for validation failed problem type.
		/// </summary>
		public const string ValidationFailed = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

		/// <summary>
		/// Returns a URI for the specified HTTP status code.
		/// </summary>
		/// <param name="statusCode">The HTTP status code.</param>
		/// <returns>A URI string representing the HTTP status code.</returns>
		public static string HttpStatus(int statusCode) => $"https://httpstatuses.io/{statusCode}";
	}

	/// <summary>
	/// Field names for validation errors.
	/// </summary>
	public static class ValidationFields
	{
		/// <summary>
		/// General field name for validation errors that don't belong to a specific field.
		/// </summary>
		public const string General = "_general";
	}
}
