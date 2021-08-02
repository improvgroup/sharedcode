namespace SharedCode.Windows.WPF.Security
{
	using System;
	using System.Runtime.InteropServices;
	using System.Security;

	public static class SecureStringExtensions
	{
		public static string? Unsecure(this SecureString secureString)
		{
			if (secureString is null)
				return string.Empty;

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
