using Microsoft.AspNetCore.Mvc;

namespace SharedCode.Web;

/// <summary>
/// A base controller class. Implements the <see cref="ControllerBase"/>
/// </summary>
/// <seealso cref="Controller"/>
/// <remarks>Initializes a new instance of the <see cref="BaseController"/> class.</remarks>
/// <param name="httpClientFactory">The HTTP client factory.</param>
public abstract class BaseController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    /// <summary>
    /// Gets the HTTP client factory.
    /// </summary>
    /// <value>The HTTP client factory.</value>
    protected IHttpClientFactory HttpClientFactory { get; } = httpClientFactory;
}
