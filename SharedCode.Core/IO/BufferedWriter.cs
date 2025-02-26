﻿
namespace SharedCode.IO;

using System;
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
	public BufferedWriter(TextWriter writer) =>
		this.writer = writer ?? throw new ArgumentNullException(nameof(writer));

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
	public StringBuilder StringBuilder { get; } = new StringBuilder(BufferLength * 3 / 2);

	/// <inheritdoc />
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
			await this.writer.WriteAsync(this.StringBuilder, token).ConfigureAwait(false);
			_ = this.StringBuilder.Clear();
		}

		token.ThrowIfCancellationRequested();
#if NET8_0_OR_GREATER
		await this.writer.FlushAsync(token).ConfigureAwait(false);
#else
		await this.writer.FlushAsync().ConfigureAwait(false);
#endif
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
	public async Task StrongFlushAsync(CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		if (this.StringBuilder.Length > 0)
		{
			await this.writer.WriteAsync(this.StringBuilder, token).ConfigureAwait(true);
			_ = this.StringBuilder.Clear();
		}
	}

	/// <summary>
	/// Performs a weak flush of the buffer.
	/// </summary>
	public void WeakFlush()
	{
		if (this.StringBuilder.Length > BufferLength)
		{
			this.writer.Write(this.StringBuilder.ToString());
			_ = this.StringBuilder.Clear();
		}
	}

	/// <summary>
	/// Performs a weak flush of the buffer as an asynchronous operation.
	/// </summary>
	/// <param name="token">The cancellation token.</param>
	/// <returns>A Task.</returns>
	public async Task WeakFlushAsync(CancellationToken token = default)
	{
		if (this.StringBuilder.Length > BufferLength)
		{
			await this.writer.WriteAsync(this.StringBuilder, token).ConfigureAwait(true);
			_ = this.StringBuilder.Clear();
		}
	}

	/// <summary>
	/// Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing">
	/// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
	/// unmanaged resources.
	/// </param>
	protected virtual void Dispose(bool disposing)
	{
		if (this.disposedValue)
		{
			return;
		}

		if (disposing)
		{
			// dispose managed state (managed objects).
			this.writer?.Dispose();
		}

		// free unmanaged resources (unmanaged objects) and override a finalizer below. set large
		// fields to null.
		this.disposedValue = true;
	}
}
