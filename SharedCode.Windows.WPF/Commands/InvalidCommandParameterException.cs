namespace SharedCode.Windows.WPF.Commands
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// The InvalidCommandParameterException class. Implements the <see cref="Exception" />
	/// </summary>
	/// <seealso cref="Exception" />
	[Serializable]
	public class InvalidCommandParameterException : Exception
	{
		/// <summary>
		/// The type1
		/// </summary>
		private readonly Type type1 = typeof(object);

		/// <summary>
		/// The type2
		/// </summary>
		private readonly Type? type2 = typeof(object);

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCommandParameterException" /> class.
		/// </summary>
		public InvalidCommandParameterException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCommandParameterException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InvalidCommandParameterException(string? message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCommandParameterException" /> class.
		/// </summary>
		/// <param name="type1">The type1.</param>
		/// <param name="type2">The type2.</param>
		public InvalidCommandParameterException(Type type1, Type? type2)
		{
			this.type1 = type1;
			this.type2 = type2;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCommandParameterException" /> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a null reference ( <see
		/// langword="Nothing" /> in Visual Basic) if no inner exception is specified.
		/// </param>
		public InvalidCommandParameterException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCommandParameterException" /> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the
		/// serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains
		/// contextual information about the source or destination.
		/// </param>
		protected InvalidCommandParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
