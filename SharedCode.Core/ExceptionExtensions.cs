// <copyright file="ExceptionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Text;

	/// <summary>
	/// The exception extensions class
	/// </summary>
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Creates a log-string from the Exception.
		/// <para>
		/// The result includes the stacktrace, innerexception et cetera, separated by <seealso
		/// cref="Environment.NewLine" />.
		/// </para>
		/// </summary>
		/// <param name="ex">The exception to create the string from.</param>
		/// <param name="additionalMessage">
		/// Additional message to place at the top of the string, maybe be empty or null.
		/// </param>
		/// <returns>System.String.</returns>
		public static string ToLogString(this Exception? ex, string? additionalMessage)
		{
			Contract.Ensures(Contract.Result<string>() is not null);

			var msg = new StringBuilder()
				.Append(string.Empty);

			if (!string.IsNullOrEmpty(additionalMessage))
			{
				_ = msg
					.Append(additionalMessage)
					.Append(Environment.NewLine);
			}

			if (ex is null)
			{
				return msg.ToString();
			}
			var orgEx = ex;

			_ = msg
				.Append("Exception:")
				.Append(Environment.NewLine);
			while (orgEx is not null)
			{
				_ = msg
					.Append(orgEx.Message)
					.Append(Environment.NewLine);
				orgEx = orgEx.InnerException;
			}

			if (ex.Data is not null)
			{
				foreach (var i in ex.Data)
				{
					_ = msg
						.Append("Data :")
						.Append(i.ToString())
						.Append(Environment.NewLine);
				}
			}

			if (ex.StackTrace is not null)
			{
				_ = msg
					.Append("StackTrace:")
					.Append(Environment.NewLine)
					.Append(ex.StackTrace)
					.Append(Environment.NewLine);
			}

			if (ex.Source is not null)
			{
				_ = msg
					.Append("Source:")
					.Append(Environment.NewLine)
					.Append(ex.Source)
					.Append(Environment.NewLine);
			}

			if (ex.TargetSite is not null)
			{
				_ = msg
					.Append("TargetSite:")
					.Append(Environment.NewLine)
					.Append(ex.TargetSite.ToString())
					.Append(Environment.NewLine);
			}

			var baseException = ex.GetBaseException();
			if (baseException is not null)
			{
				_ = msg
					.Append("BaseException:")
					.Append(Environment.NewLine)
					.Append(ex.GetBaseException());
			}

			return msg.ToString();
		}
	}
}
