namespace SharedCode.Web.Middleware;

/// <summary>
/// Represents a middleware for handling exceptions in an ASP.NET Core application.
/// </summary>
/// <param name="next">The next.</param>
/// <param name="logger">The logger.</param>
[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This class is instantiated by the ASP.NET Core framework.")]
internal sealed partial class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private const string HttpExceptionErrorMessage = "An HTTP error occurred.";
    private const string UnexpectedErrorMessage = "An unexpected error occurred.";

    /// <summary>
    /// Invoke as an asynchronous operation.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "We are deliberatly catching all exceptions to handle them in a centralized manner.")]
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context).ConfigureAwait(true);
        }
        catch (HttpException httpException)
        {
            LogHttpError(logger, httpException);
            await HandleExceptionAsync(
                context,
                httpException,
                httpException.StatusCode).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            LogUnexpectedError(logger, ex);
            await HandleExceptionAsync(context, ex, 500).ConfigureAwait(true);
        }
    }

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "<Pending>")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(
            new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error",
            }.ToString());
    }

    [LoggerMessage(Level = LogLevel.Error, Message = HttpExceptionErrorMessage)]
    private static partial void LogHttpError(ILogger logger, HttpException httpException);

    [LoggerMessage(Level = LogLevel.Error, Message = UnexpectedErrorMessage)]
    private static partial void LogUnexpectedError(ILogger logger, Exception? ex);
}
