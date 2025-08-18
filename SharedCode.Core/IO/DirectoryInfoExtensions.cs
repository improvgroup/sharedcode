namespace SharedCode.IO;
/// <summary>
/// The directory information extensions class
/// </summary>
public static class DirectoryInfoExtensions
{
	/// <summary>
	/// Recursively create directory
	/// </summary>
	/// <param name="dirInfo">Folder path to create.</param>
	/// <exception cref="ArgumentNullException">dirInfo</exception>
	public static void CreateDirectory(this DirectoryInfo dirInfo)
	{
		_ = dirInfo ?? throw new ArgumentNullException(nameof(dirInfo));

		dirInfo.Parent?.CreateDirectory();

		if (!dirInfo.Exists)
			dirInfo.Create();
	}
}
