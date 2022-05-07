// <copyright file="FileInfoExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.IO
{
	using System;
	using System.IO;
	using System.Security;

	/// <summary>
	/// The file information extensions class
	/// </summary>
	public static class FileInfoExtensions
	{
		/// <summary>
		/// Move current instance and rename current instance when needed
		/// </summary>
		/// <param name="fileInfo">Current instance</param>
		/// <param name="destFileName">
		/// The Path to move current instance to, which can specify a different file name
		/// </param>
		/// <param name="renameWhenExists">
		/// Bool to specify if current instance should be renamed when exists
		/// </param>
		/// <exception cref="IOException">
		/// An I/O error occurs, such as the destination file already exists or the destination
		/// device is not ready.
		/// </exception>
		/// <exception cref="SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="UnauthorizedAccessException">
		/// The <paramref name="destFileName">destFileName</paramref> is read-only or is a directory.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="destFileName">destFileName</paramref> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The <paramref name="destFileName">destFileName</paramref> is empty, contains only white
		/// spaces, or contains invalid characters.
		/// </exception>
		/// <exception cref="FileNotFoundException">The file is not found.</exception>
		/// <exception cref="DirectoryNotFoundException">
		/// The specified path is invalid, such as being on an unmapped drive.
		/// </exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length. For
		/// example, on Windows-based platforms, paths must be less than 248 characters, and file
		/// names must be less than 260 characters.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// The <paramref name="destFileName">destFileName</paramref> contains a colon (:) in the
		/// middle of the string.
		/// </exception>
		public static void MoveTo(this FileInfo fileInfo, string destFileName, bool renameWhenExists = false)
		{
			_ = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
			_ = destFileName ?? throw new ArgumentNullException(nameof(destFileName));

			var newFullPath = string.Empty;

			if (renameWhenExists)
			{
				var count = 1;

				var fileNameOnly = Path.GetFileNameWithoutExtension(fileInfo.FullName);
				var extension = Path.GetExtension(fileInfo.FullName);
				newFullPath = Path.Combine(destFileName, fileInfo.Name);

				while (File.Exists(newFullPath))
				{
					var tempFileName = $"{fileNameOnly}({count++})";
					newFullPath = Path.Combine(destFileName, tempFileName + extension);
				}
			}

			fileInfo.MoveTo(renameWhenExists ? newFullPath : destFileName);
		}
	}
}
