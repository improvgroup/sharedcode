namespace SharedCode.Data.CosmosDb;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;

/// <summary>
/// The query class.
/// </summary>
public class Query
{
	private const int DefaultSlowRequestSeconds = 5;
	private const int DefaultThroughput = 400;

	private readonly ILogger<Query> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="Query"/> class.
	/// </summary>
	/// <param name="logger">The logger.</param>
	/// <exception cref="ArgumentNullException">logger</exception>
	public Query(ILogger<Query> logger) => this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

	/// <summary>
	/// Gets or sets the authentication key.
	/// </summary>
	public required string AuthKey { get; set; }

	/// <summary>
	/// Gets or sets the database name.
	/// </summary>
	public required string DatabaseName { get; set; }

	/// <summary>
	/// Gets or sets the server name.
	/// </summary>
	public required string ServerName { get; set; }

	/// <summary>
	/// Gets or sets the slow request time span.
	/// </summary>
	public TimeSpan SlowRequestTimeSpan { get; set; } = TimeSpan.FromSeconds(DefaultSlowRequestSeconds);

	/// <summary>
	/// Gets or sets the throughput request units per second.
	/// </summary>
	public int Throughput { get; set; } = DefaultThroughput;

	/// <summary>
	/// Enumerates the results of the specified SQL query.
	/// </summary>
	/// <typeparam name="T">The type of the entity being queried.</typeparam>
	/// <param name="containerName">The name of the container.</param>
	/// <param name="partitionKeyPath">The path of the partition key.</param>
	/// <param name="sql">The SQL query string.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The enumerable.</returns>
	public async IAsyncEnumerable<T> EnumerateAsync<T>(string containerName, string partitionKeyPath, string sql, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		using var client = new CosmosClient($"https://{this.ServerName}.documents.azure.com:443/", this.AuthKey);

		Database database = await client.CreateDatabaseIfNotExistsAsync(this.DatabaseName, cancellationToken: cancellationToken).ConfigureAwait(false);
		Container container = await database.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath, this.Throughput, cancellationToken: cancellationToken).ConfigureAwait(false);

		using var feedIterator = container.GetItemQueryIterator<dynamic>(sql);
		while (feedIterator.HasMoreResults)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield break;
			}

			var response = await feedIterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
			if (response.Diagnostics.GetClientElapsedTime() > this.SlowRequestTimeSpan)
			{
				this._logger.LogSlowQueryDiagnostics(response.Diagnostics.ToString());
			}

			foreach (var item in response)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					yield break;
				}

				yield return item;
			}
		}
	}
}
