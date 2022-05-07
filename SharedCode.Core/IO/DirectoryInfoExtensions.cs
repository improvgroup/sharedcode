// <copyright file="DirectoryInfoExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.IO;

using System.IO;

/// <summary>
/// The directory information extensions class
/// </summary>
public static class DirectoryInfoExtensions
{
	/// <summary>
	/// Recursively create directory
	/// </summary>
	/// <param name="dirInfo">Folder path to create.</param>
	public static void CreateDirectory(this DirectoryInfo dirInfo)
	{
		_ = dirInfo ?? throw new ArgumentNullException(nameof(dirInfo));

		dirInfo.Parent?.CreateDirectory();

		if (!dirInfo.Exists)
			dirInfo.Create();
	}
}
