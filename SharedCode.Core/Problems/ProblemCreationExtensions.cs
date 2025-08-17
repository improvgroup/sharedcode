namespace SharedCode.Problems
{
	using System;
	using System.Net;

	/// <summary>
	/// Extension methods for creating <see cref="Problem"/> instances from various sources.
	/// </summary>
	public static class ProblemCreationExtensions
	{
		/// <summary>
		/// Converts a <see cref="Problem"/> to a <see cref="ProblemException"/>.
		/// </summary>
		/// <param name="problem">The <see cref="Problem"/> to convert.</param>
		/// <returns>A <see cref="ProblemException"/> representing the problem.</returns>
		public static Exception ToException(this Problem problem) => new ProblemException(problem);

		/// <summary>
		/// Creates a <see cref="Problem"/> from an <see cref="Exception"/>.
		/// </summary>
		/// <param name="exception">The exception to convert.</param>
		/// <returns>A <see cref="Problem"/> representing the exception.</returns>
		public static Problem ToProblem(this Exception exception) => Problem.Create(exception);

		/// <summary>
		/// Creates a <see cref="Problem"/> from an <see cref="Exception"/> with a status code.
		/// </summary>
		/// <param name="exception">The exception to convert.</param>
		/// <param name="statusCode">The HTTP status code for the problem.</param>
		/// <returns>A <see cref="Problem"/> representing the exception.</returns>
		public static Problem ToProblem(this Exception exception, int statusCode) => Problem.Create(exception, statusCode);

		/// <summary>
		/// Creates a <see cref="Problem"/> from an <see cref="Exception"/> with an <see cref="HttpStatusCode"/>.
		/// </summary>
		/// <param name="exception">The exception to convert.</param>
		/// <param name="statusCode">The HTTP status code for the problem.</param>
		/// <returns>A <see cref="Problem"/> representing the exception.</returns>
		public static Problem ToProblem(this Exception exception, HttpStatusCode statusCode) => Problem.Create(exception, (int)statusCode);

		/// <summary>
		/// Creates a <see cref="Problem"/> from an enum value.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="errorCode">The enum value representing the error code.</param>
		/// <returns>A <see cref="Problem"/> representing the error code.</returns>
		public static Problem ToProblem<TEnum>(this TEnum errorCode) where TEnum : Enum => Problem.Create(errorCode);

		/// <summary>
		/// Creates a <see cref="Problem"/> from an enum value with a detail message.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="errorCode">The enum value representing the error code.</param>
		/// <param name="detail">
		/// A human-readable explanation specific to this occurrence of the problem.
		/// </param>
		/// <returns>A <see cref="Problem"/> representing the error code and detail.</returns>
		public static Problem ToProblem<TEnum>(this TEnum errorCode, string detail) where TEnum : Enum => Problem.Create(errorCode, detail);

		/// <summary>
		/// Creates a <see cref="Problem"/> from an enum value with a detail message and status code.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="errorCode">The enum value representing the error code.</param>
		/// <param name="detail">
		/// A human-readable explanation specific to this occurrence of the problem.
		/// </param>
		/// <param name="statusCode">The HTTP status code for the problem.</param>
		/// <returns>
		/// A <see cref="Problem"/> representing the error code, detail, and status code.
		/// </returns>
		public static Problem ToProblem<TEnum>(this TEnum errorCode, string detail, int statusCode) where TEnum : Enum => Problem.Create(errorCode, detail, statusCode);

		/// <summary>
		/// Creates a <see cref="Problem"/> from an enum value with a detail message and <see cref="HttpStatusCode"/>.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="errorCode">The enum value representing the error code.</param>
		/// <param name="detail">
		/// A human-readable explanation specific to this occurrence of the problem.
		/// </param>
		/// <param name="statusCode">The HTTP status code for the problem.</param>
		/// <returns>
		/// A <see cref="Problem"/> representing the error code, detail, and status code.
		/// </returns>
		public static Problem ToProblem<TEnum>(this TEnum errorCode, string detail, HttpStatusCode statusCode) where TEnum : Enum => Problem.Create(errorCode, detail, (int)statusCode);
	}
}
