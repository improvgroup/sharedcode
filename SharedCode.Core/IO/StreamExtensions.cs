// <copyright file="StreamExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.IO;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

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

		using var ep = new EncoderParameters();
		ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);

		var memoryStream = new MemoryStream();

#pragma warning disable CS8604 // Possible null reference argument.
		targetImage.Save(memoryStream, info, ep);
#pragma warning restore CS8604 // Possible null reference argument.

		// Rewind the stream to the beginning so consumers don't have to do it.
		_ = memoryStream.Seek(0, SeekOrigin.Begin);

		return memoryStream;
	}
}
