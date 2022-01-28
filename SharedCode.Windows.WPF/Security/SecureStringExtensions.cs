// <copyright file="SecureStringExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.WPF.Security
{
	using System;
	using System.Runtime.InteropServices;
	using System.Security;

	/// <summary>
	/// Class SecureStringExtensions.
	/// </summary>
	public static class SecureStringExtensions
	{
		/// <summary>
		/// Unsecures the specified secure string.
		/// </summary>
		/// <param name="secureString">The secure string.</param>
		/// <returns>System.Nullable&lt;System.String&gt;.</returns>
		public static string? Unsecure(this SecureString secureString)
		{
			if (secureString is null)
			{
				return string.Empty;
			}

			var unmanagedString = IntPtr.Zero;
			try
			{
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
				return Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}
	}
}
