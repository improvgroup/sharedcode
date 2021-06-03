// <copyright file="BufferedWriter.cs" company="improvGroup, LLC">
//     Copyright © 2013-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.IO
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides a base for buffered data writers.
	/// </summary>
	public class BufferedWriter : IDisposable
	{
		/// <summary>
		/// The buffer length
		/// </summary>
		private const int BufferLength = 512;

		/// <summary>
		/// The finished task
		/// </summary>
		private static readonly Task FinishedTask = Task.FromResult(true);

		/// <summary>
		/// The writer
		/// </summary>
		private readonly TextWriter writer;

		/// <summary>
		/// To detect redundant calls
		/// </summary>
		private bool disposedValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="BufferedWriter" /> class.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public BufferedWriter([NotNull] TextWriter writer)
		{
			Contract.Requires<ArgumentNullException>(writer != null);
			this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="BufferedWriter" /> class.
		/// </summary>
		~BufferedWriter()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Gets the string builder.
		/// </summary>
		/// <value>The string builder.</value>
		[NotNull]
		public StringBuilder StringBuilder { get; } = new StringBuilder(BufferLength * 3 / 2);

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting
		/// unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Performs a full flush of the buffer.
		/// </summary>
		public void FullFlush()
		{
			this.StrongFlush();
			this.writer.Flush();
		}

		/// <summary>
		/// Performs a full flush of the buffer as an asynchronous operation.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns>A Task.</returns>
		public async Task FullFlushAsync(CancellationToken token)
		{
			token.ThrowIfCancellationRequested();

			if (this.StringBuilder.Length > 0)
			{
				var builtString = this.StringBuilder.ToString();

				this.StringBuilder.Clear();
				await this.writer.WriteAsync(builtString).ConfigureAwait(true);
			}

			token.ThrowIfCancellationRequested();
			await this.writer.FlushAsync().ConfigureAwait(true);
		}

		/// <summary>
		/// Performs a strong flush of the buffer.
		/// </summary>
		public void StrongFlush()
		{
			if (this.StringBuilder.Length > 0)
			{
				this.writer.Write(this.StringBuilder.ToString());
				_ = this.StringBuilder.Clear();
			}
		}

		/// <summary>
		/// Performs a strong flush of the buffer as an asynchronous operation.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns>A Task.</returns>
		public Task StrongFlushAsync(CancellationToken token)
		{
			token.ThrowIfCancellationRequested();

			if (this.StringBuilder.Length > 0)
			{
				var builtString = this.StringBuilder.ToString();

				this.StringBuilder.Clear();
				return this.writer.WriteAsync(builtString);
			}

			return FinishedTask;
		}

		/// <summary>
		/// Performs a weak flush of the buffer.
		/// </summary>
		public void WeakFlush()
		{
			if (this.StringBuilder.Length > BufferLength)
			{
				this.writer.Write(this.StringBuilder.ToString());
				this.StringBuilder.Clear();
			}
		}

		/// <summary>
		/// Performs a weak flush of the buffer as an asynchronous operation.
		/// </summary>
		/// <returns>A Task.</returns>
		public Task WeakFlushAsync()
		{
			if (this.StringBuilder.Length > BufferLength)
			{
				var builtString = this.StringBuilder.ToString();

				this.StringBuilder.Clear();
				return this.writer.WriteAsync(builtString);
			}

			return FinishedTask;
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
		/// only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					// dispose managed state (managed objects).
					this.writer?.Dispose();
				}

				// free unmanaged resources (unmanaged objects) and override a finalizer below. set
				// large fields to null.
				this.disposedValue = true;
			}
		}
	}
}
