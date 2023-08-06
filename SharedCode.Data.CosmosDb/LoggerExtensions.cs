namespace SharedCode.Data.CosmosDb;

using Microsoft.Extensions.Logging;

/// <summary>
/// The logger extension methods class.
/// </summary>
public static class LoggerExtensions
{
	private static readonly Action<ILogger, string?, Exception?> s_logSlowQueryDiagnostics =
		LoggerMessage.Define<string?>(
			LogLevel.Warning,
			new EventId(0, nameof(LogSlowQueryDiagnostics)),
			"Slow query: {Diagnostics}");

	/// <summary>
	/// Logs the slow query diagnostics.
	/// </summary>
	/// <param name="this">The logger.</param>
	/// <param name="diagnostics">The diagnostics.</param>
	public static void LogSlowQueryDiagnostics(this ILogger @this, string diagnostics) => s_logSlowQueryDiagnostics(@this, diagnostics, default);
}
