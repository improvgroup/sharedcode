namespace SharedCode
{
	/// <summary>
	/// Represents a result that contains invalid data.
	/// </summary>
	public interface IResultInvalid
	{
		/// <summary>
		/// Gets a value indicating whether the result is invalid.
		/// </summary>
		/// <value>
		/// <c>true</c> if the result is invalid; otherwise, <c>false</c>.
		/// </value>
		bool IsInvalid { get; }

		/// <summary>
		/// Adds an invalid message to the result.
		/// </summary>
		/// <param name="message">
		/// The invalid message to add.
		/// </param>
		void AddInvalidMessage(string message);

		/// <summary>
		/// Adds an invalid message with a key and value to the result.
		/// </summary>
		/// <param name="key">
		/// The key for the invalid message.
		/// </param>
		/// <param name="value">
		/// The value for the invalid message.
		/// </param>
		void AddInvalidMessage(string key, string value);
	}
}
