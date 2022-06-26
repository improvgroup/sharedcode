// <copyright file="SecureStringExtensions.cs" company="improvGroup, LLC">
//     Copyright © 2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.WPF.Security
{
	using System;
	using System.Runtime.InteropServices;
	using System.Security;

	/// <summary>
	/// The secure string extension method holder class.
	/// </summary>
	public static class SecureStringExtensions
	{
		/// <summary>
		/// Unsecures the specified secure string.
		/// </summary>
		/// <param name="this">The secure string.</param>
		/// <returns>System.Nullable&lt;System.String&gt;.</returns>
		public static string? Unsecure(this SecureString @this)
		{
			if (@this is null)
			{
				return string.Empty;
			}

			var unmanagedString = IntPtr.Zero;
			try
			{
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(@this);
				return Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}
	}
}
