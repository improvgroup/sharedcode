namespace SharedCode.Web;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

using System.Net;

/// <summary>
/// The service collection extension methods class.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds a resilient HTTP client to the service collection.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="clientName">The name of the client.</param>
	/// <returns>The HTTP client builder.</returns>
	public static IHttpClientBuilder AddResilientHttpClient(this IServiceCollection services, string clientName)
	{
		var policy = HttpPolicyExtensions
			.HandleTransientHttpError()
			.Or<HttpRequestException>()
			.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
			.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5));

		return services
			.AddHttpClient(clientName)
			.SetHandlerLifetime(TimeSpan.FromMinutes(5))
			.AddPolicyHandler(policy);
	}
}
