// <copyright file="ExceptionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode
{
	using SharedCode.Reflection;
	using SharedCode.Text;

	using System;
	using System.Collections;
	using System.Diagnostics.Contracts;
	using System.Text;

	/// <summary>
	/// The exception extensions class
	/// </summary>
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Adds the specified data to this exception.
		/// </summary>
		/// <param name="this">The exception.</param>
		/// <param name="dictionary">The dictionary.</param>
		/// <exception cref="ArgumentNullException"></exception>
		[SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "Non-generic inference does not work so well.")]
		public static void AddData(this Exception @this, IDictionary dictionary)
		{
			_ = @this ?? throw new ArgumentNullException(nameof(@this));
			if (dictionary is not null)
			{
				foreach (DictionaryEntry item in dictionary)
				{
					@this.Data.Add(item.Key, item.Value);
				}
			}
		}

		/// <summary>
		/// Adds or updates the key/value pair in this exception's data list.
		/// </summary>
		/// <param name="this">The exception.</param>
		/// <param name="key">The data key.</param>
		/// <param name="value">The data value.</param>
		/// <exception cref="ArgumentNullException">this</exception>
		public static void AddOrUpdateData(this Exception @this, string key, string value)
		{
			_ = @this ?? throw new ArgumentNullException(nameof(@this));
			if (@this.Data.Contains(key))
			{
				(@this.Data[key] as List<string>)?.Add(value);
			}
			else
			{
				@this.Data.Add(key, new List<string> { value });
			}
		}

		/// <summary>
		/// Returns a value indicating whether the data <see cref="IDictionary" /> of this instance
		/// is equal to the specified dictionary.
		/// </summary>
		/// <param name="this">The exception.</param>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns><c>true</c> if the data dictionaries are equal, <c>false</c> otherwise.</returns>
		public static bool DataEquals(this Exception @this, IDictionary? dictionary)
		{
			(var result, _) = @this.DataEqualsWithDetail(dictionary);
			return result;
		}

		/// <summary>
		/// Determines if the specified data <see cref="IDictionary" /> is equal to this instance's
		/// data dictionary.
		/// </summary>
		/// <param name="this">The exception.</param>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns>System.ValueTuple&lt;System.Boolean, System.String&gt;.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static (bool IsEqual, string Message) DataEqualsWithDetail(this Exception @this, IDictionary? dictionary)
		{
			_ = @this ?? throw new ArgumentNullException(nameof(@this));

			var isEqual = true;
			StringBuilder messageStringBuilder = new();
			isEqual = @this.CompareDataKeys(dictionary, isEqual, messageStringBuilder);

			return (isEqual, messageStringBuilder.ToString());
		}

		/// <summary>
		/// Determines whether the specified origin is from.
		/// </summary>
		/// <param name="this">The exception.</param>
		/// <param name="origin">The origin.</param>
		/// <returns><c>true</c> if the specified origin is from; otherwise, <c>false</c>.</returns>
		public static bool IsFrom(this Exception @this, string origin) => @this?.TargetSite?.ReflectedType?.Name == origin;

		/// <summary>
		/// Determines whether [is not from] [the specified origin].
		/// </summary>
		/// <param name="this">The exception.</param>
		/// <param name="origin">The origin.</param>
		/// <returns><c>true</c> if [is not from] [the specified origin]; otherwise, <c>false</c>.</returns>
		public static bool IsNotFrom(this Exception @this, string origin) => @this?.TargetSite?.ReflectedType?.Name != origin;

		/// <summary>
		/// Sames the exception as.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="otherException">The other exception.</param>
		/// <returns><c>true</c> if the exceptions are the same, <c>false</c> otherwise.</returns>
		public static bool SameExceptionAs(this Exception exception, Exception otherException)
		{
			return (
					exception is null &&
					otherException is null
				) || (
					exception?.GetType()?.FullName == otherException?.GetType()?.FullName &&
					exception?.Message == otherException?.Message &&
					(exception?.DataEquals(otherException?.Data) ?? false) &&
					(
						(
							exception?.InnerException is null &&
							otherException?.InnerException is null
						) || (
							exception?.InnerException?.GetType()?.FullName == otherException?.InnerException?.GetType()?.FullName &&
							exception?.InnerException?.Message == otherException?.InnerException?.Message &&
							(
								exception?.InnerException?.DataEquals(otherException?.InnerException?.Data) ?? false
							)
						)
					)
				);
		}

		/// <summary>
		/// Throws if this exception contains errors.
		/// </summary>
		/// <param name="this">The exception.</param>
		/// <exception cref="ArgumentNullException">this</exception>
		public static void ThrowIfContainsErrors(this Exception @this)
		{
			_ = @this ?? throw new ArgumentNullException(nameof(@this));
			if (@this.Data.Count > 0)
			{
				throw @this;
			}
		}

		/// <summary>
		/// Creates a log-string from the Exception.
		/// <para>
		/// The result includes the stacktrace, innerexception et cetera, separated by <seealso
		/// cref="Environment.NewLine" />.
		/// </para>
		/// </summary>
		/// <param name="this">The exception to create the string from.</param>
		/// <param name="additionalMessage">
		/// Additional message to place at the top of the string, maybe be empty or null.
		/// </param>
		/// <returns>System.String.</returns>
		public static string ToLogString(this Exception? @this, string? additionalMessage)
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

			if (@this is null)
			{
				return msg.ToString();
			}
			var orgEx = @this;

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

			if (@this.Data is not null)
			{
				foreach (var i in @this.Data)
				{
					_ = msg
						.Append("Data :")
						.Append(i.ToString())
						.Append(Environment.NewLine);
				}
			}

			if (@this.StackTrace is not null)
			{
				_ = msg
					.Append("StackTrace:")
					.Append(Environment.NewLine)
					.Append(@this.StackTrace)
					.Append(Environment.NewLine);
			}

			if (@this.Source is not null)
			{
				_ = msg
					.Append("Source:")
					.Append(Environment.NewLine)
					.Append(@this.Source)
					.Append(Environment.NewLine);
			}

			if (@this.TargetSite is not null)
			{
				_ = msg
					.Append("TargetSite:")
					.Append(Environment.NewLine)
					.Append(@this.TargetSite.ToString())
					.Append(Environment.NewLine);
			}

			var baseException = @this.GetBaseException();
			if (baseException is not null)
			{
				_ = msg
					.Append("BaseException:")
					.Append(Environment.NewLine)
					.Append(@this.GetBaseException());
			}

			return msg.ToString();
		}

		private static bool CompareDataKeys(this Exception @this, IDictionary? dictionary, bool isEqual, StringBuilder messageStringBuilder)
		{
			if (dictionary is null)
			{
				isEqual = @this.Data.Count == 0 ? isEqual : false;
				_ = messageStringBuilder.AppendFormat(CultureInfo.CurrentCulture, "- Expected data item count to be null, but found {0}.", @this.Data.Count);
				return isEqual;
			}

			if (@this.Data.Count == 0 && dictionary.Count == 0)
			{
				return isEqual;
			}

			if (@this.Data.Count != dictionary.Count)
			{
				isEqual = false;

				messageStringBuilder.Append("- Expected data item count to be {0}, but found {1}.", dictionary.Count, @this.Data.Count);
			}

			(var additionalItems, var missingItems, var sharedItems) = @this.GetDataDifferences(dictionary);
			isEqual = EvaluateAdditionalKeys(isEqual, messageStringBuilder, additionalItems);
			isEqual = EvaluateMissingKeys(isEqual, messageStringBuilder, missingItems);
			isEqual = @this.EvaluateSharedKeys(isEqual, messageStringBuilder, sharedItems);

			return isEqual;
		}

		[SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "Non-generic inference does not work so well.")]
		private static bool EvaluateAdditionalKeys(bool isEqual, StringBuilder messageStringBuilder, IDictionary? additionalItems)
		{
			if (additionalItems?.Count > 0)
			{
				isEqual = false;

				foreach (DictionaryEntry dictionaryEntry in additionalItems)
				{
					_ = messageStringBuilder.AppendFormat(CultureInfo.CurrentCulture, "- Did not expect to find key '{0}'.", dictionaryEntry.Key);
				}
			}

			return isEqual;
		}

		[SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "Non-generic inference does not work so well.")]
		private static bool EvaluateMissingKeys(bool isEqual, StringBuilder messageStringBuilder, IDictionary? missingItems)
		{
			if (missingItems?.Count > 0)
			{
				isEqual = false;

				foreach (DictionaryEntry dictionaryEntry in missingItems)
				{
					_ = messageStringBuilder.AppendFormat(CultureInfo.CurrentCulture, "- Expected to find key '{0}'.", dictionaryEntry.Key);
				}
			}

			return isEqual;
		}

		[SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "Non-generic inference does not work so well.")]
		private static bool EvaluateSharedKeys(this Exception @this, bool isEqual, StringBuilder messageStringBuilder, IDictionary? sharedItems)
		{
			_ = @this ?? throw new ArgumentNullException(nameof(@this));
			_ = messageStringBuilder ?? throw new ArgumentNullException(nameof(messageStringBuilder));
			if (sharedItems?.Count > 0)
			{
				foreach (DictionaryEntry dictionaryEntry in sharedItems)
				{
					var expectedValues = (dictionaryEntry.Value as List<string> ?? new List<string>())
						.Select(value => value).Aggregate((t1, t2) => $"{t1}','{t2}");

					var actualValues = (@this.Data[dictionaryEntry.Key!] as List<string> ?? new List<string>())
						.Select(value => value).Aggregate((t1, t2) => $"{t1}','{t2}");

					if (actualValues != expectedValues)
					{
						isEqual = false;

						var isNotEmpty =
							!string.IsNullOrWhiteSpace(dictionaryEntry.Key?.ToString()) &&
							!string.IsNullOrWhiteSpace(expectedValues);

						_ = messageStringBuilder
							.AppendIf("- Expected to find key '", isNotEmpty)
							.AppendIf(dictionaryEntry.Key?.ToString(), isNotEmpty)
							.AppendIf("' with value(s) ['", isNotEmpty)
							.AppendIf(expectedValues, isNotEmpty)
							.AppendIf("'], but found value(s) ['", isNotEmpty)
							.AppendIf(actualValues, isNotEmpty)
							.AppendIf("'].", isNotEmpty);
					}
				}
			}

			return isEqual;
		}

		[SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "Non-generic inference does not work so well.")]
		private static (IDictionary AdditionalItems, IDictionary MissingItems, IDictionary SharedItems) GetDataDifferences(this Exception @this, IDictionary dictionary)
		{
			IDictionary additionalItems = @this.Data.DeepClone() ?? new Dictionary<object, object?>();
			IDictionary missingItems = dictionary.DeepClone() ?? new Dictionary<object, object?>();
			IDictionary sharedItems = dictionary.DeepClone() ?? new Dictionary<object, object?>();

			foreach (DictionaryEntry dictionaryEntry in dictionary)
			{
				additionalItems.Remove(dictionaryEntry.Key);
			}

			foreach (DictionaryEntry dictionaryEntry in @this.Data)
			{
				missingItems.Remove(dictionaryEntry.Key);
			}

			foreach (DictionaryEntry dictionaryEntry in additionalItems)
			{
				sharedItems.Remove(dictionaryEntry.Key);
			}

			foreach (DictionaryEntry dictionaryEntry in missingItems)
			{
				sharedItems.Remove(dictionaryEntry.Key);
			}

			return (additionalItems, missingItems, sharedItems);
		}
	}
}
