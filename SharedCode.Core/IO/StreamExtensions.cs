// <copyright file="StreamExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2009-2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.IO;

using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;

/// <summary>
/// The stream extension methods class.
/// </summary>
public static class StreamExtensions
{
	/// <summary>
	/// Fits an image into the dimensions if it is too large to fit in the specified maximum size.
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <param name="maxWidth">The maximum width.</param>
	/// <param name="maxHeight">The maximum height.</param>
	/// <param name="makeSquare">A value indicating whether the output dimensions should be square.</param>
	/// <param name="quality">The encoder quality in percent (90% recommended, the default).</param>
	/// <returns>The thumbnail image.</returns>
	/// <exception cref="ArgumentNullException">The input stream cannot be null.</exception>
	/// <exception cref="PlatformNotSupportedException">
	/// The Image.FromStream() method is not available on non-Windows platforms.
	/// </exception>
	[SupportedOSPlatform("windows")]
	[SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
	public static MemoryStream FitImage(this Stream stream, int maxWidth = 0, int maxHeight = 0, bool makeSquare = false, long quality = 90L)
	{
		if (stream is null)
		{
			throw new ArgumentNullException(nameof(stream));
		}

		if (stream.CanSeek)
		{
			_ = stream.Seek(0, SeekOrigin.Begin);
		}

		var source = Image.FromStream(stream);

		float targetWidth = source.Width;
		float targetHeight = source.Height;

		maxWidth = maxWidth == 0 ? source.Width : maxWidth;
		maxHeight = maxHeight == 0 ? source.Height : maxHeight;

		float sourceAspectRatio = source.Width / source.Height;

		if (makeSquare)
		{
			// get the height and width for the image up to the specified max values
			targetWidth = maxWidth > source.Width ? source.Width : maxWidth;
			targetHeight = maxHeight > source.Height ? source.Height : maxHeight;

			// get the aspect ratios in case we need to expand or shrink to fit
			var targetAspectRatio = targetWidth / targetHeight;

			// get the larger aspect ratio of the two, if the aspect ratio is 1 then no adjustment
			// is needed
			if (sourceAspectRatio > targetAspectRatio)
			{
				targetHeight = targetWidth / sourceAspectRatio;
			}
			else if (sourceAspectRatio < targetAspectRatio)
			{
				targetWidth = targetHeight * sourceAspectRatio;
			}
		}
		else
		{
			float maxRatio = maxWidth / maxHeight;

			float scaleFactor = maxRatio > sourceAspectRatio
				? maxHeight / source.Height
				: maxWidth / source.Width;

			targetWidth = source.Width * scaleFactor;
			targetHeight = source.Height * scaleFactor;
		}

		var targetRectangle = new Rectangle(0, 0, (int)targetWidth, (int)targetHeight);
		using var targetImage = new Bitmap((int)targetWidth, (int)targetHeight);

		targetImage.SetResolution(source.HorizontalResolution, source.VerticalResolution);

		using var graphics = Graphics.FromImage(targetImage);

		graphics.CompositingMode = CompositingMode.SourceCopy;
		graphics.CompositingQuality = CompositingQuality.HighQuality;
		graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		graphics.SmoothingMode = SmoothingMode.HighQuality;
		graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

		using var wrapMode = new ImageAttributes();
		wrapMode.SetWrapMode(WrapMode.TileFlipXY);

		graphics.DrawImage(source, targetRectangle, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, wrapMode);

		var info = Array.Find(
			ImageCodecInfo.GetImageEncoders(),
			ici => ici.MimeType?.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ?? false);
		if (info is null)
		{
			throw new Exception("No encoder found.");
		}

		using var ep = new EncoderParameters();
		ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);

		var memoryStream = new MemoryStream();

		targetImage.Save(memoryStream, info, ep);

		// Rewind the stream to the beginning so consumers don't have to do it.
		_ = memoryStream.Seek(0, SeekOrigin.Begin);

		return memoryStream;
	}

	/// <summary>
	/// Reads the content of the stream.
	/// </summary>
	/// <param name="stream">The input stream.</param>
	/// <returns>Returns a string with the content of the input stream.</returns>
	/// <exception cref="ArgumentNullException">stream</exception>
	public static string ReadToEnd(this Stream stream)
	{
		_ = stream ?? throw new ArgumentNullException(nameof(stream));

		if (stream.CanSeek)
		{
			_ = stream.Seek(0, SeekOrigin.Begin);
		}

		using var sr = new StreamReader(stream);
		return sr.ReadToEnd();
	}

	/// <summary>
	/// Reads the content of the stream.
	/// </summary>
	/// <param name="stream">The input stream.</param>
	/// <returns>Returns a string with the content of the input stream.</returns>
	/// <exception cref="ArgumentNullException">stream</exception>
	public static async Task<string> ReadToEndAsync(this Stream stream)
	{
		_ = stream ?? throw new ArgumentNullException(nameof(stream));
		Contract.Ensures(Contract.Result<Task<string>>() is not null);

		if (stream.CanSeek)
		{
			_ = stream.Seek(0, SeekOrigin.Begin);
		}

		using var sr = new StreamReader(stream);
		return await sr.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	/// <summary>
	/// Converts the input stream to a byte array.
	/// </summary>
	/// <param name="input">This input stream.</param>
	/// <returns>The byte array.</returns>
	/// <exception cref="ArgumentNullException">input</exception>
	public static byte[] ToByteArray(this Stream input)
	{
		_ = input ?? throw new ArgumentNullException(nameof(input));
		Contract.Ensures(Contract.Result<byte[]>() is not null);

		if (input is MemoryStream stream)
		{
			return stream.ToArray();
		}

		using var ms = new MemoryStream();
		if (input.CanSeek)
		{
			_ = input.Seek(0, SeekOrigin.Begin);
		}

		input.CopyTo(ms);
		return ms.ToArray();
	}

	/// <summary>
	/// Converts the input stream to a byte array.
	/// </summary>
	/// <param name="input">This input stream.</param>
	/// <returns>The byte array.</returns>
	/// <exception cref="ArgumentNullException">input</exception>
	public static async Task<byte[]> ToByteArrayAsync(this Stream input)
	{
		_ = input ?? throw new ArgumentNullException(nameof(input));
		Contract.Ensures(Contract.Result<Task<byte[]>>() is not null);

		if (input is MemoryStream stream)
		{
			return stream.ToArray();
		}

		using var ms = new MemoryStream();
		if (input.CanSeek)
		{
			_ = input.Seek(0, SeekOrigin.Begin);
		}

		await input.CopyToAsync(ms).ConfigureAwait(continueOnCapturedContext: false);
		return ms.ToArray();
	}
}
