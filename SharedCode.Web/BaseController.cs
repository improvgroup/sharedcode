namespace SharedCode.Web;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// A base controller class. Implements the <see cref="ControllerBase" />
/// </summary>
/// <seealso cref="Controller" />
public abstract class BaseController : ControllerBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseController"/> class.
	/// </summary>
	/// <param name="httpClientFactory">The HTTP client factory.</param>
	protected BaseController(IHttpClientFactory httpClientFactory) => this.HttpClientFactory = httpClientFactory;

	/// <summary>
	/// Gets the HTTP client factory.
	/// </summary>
	/// <value>The HTTP client factory.</value>
	protected IHttpClientFactory HttpClientFactory { get; }
}
