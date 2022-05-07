// <copyright file="TcpClientExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.IO
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Net.Sockets;
	using System.Runtime.InteropServices;

	/// <summary>
	/// The TCP client extensions class
	/// </summary>
	public static class TcpClientExtensions
	{
		/// <summary>
		/// The default keep alive interval
		/// </summary>
		public const uint DefaultKeepAliveInterval = 1000;

		/// <summary>
		/// The default keep alive time
		/// </summary>
		public const uint DefaultKeepAliveTime = 7200000;

		/// <summary>
		/// Using IOControl code to configue socket KeepAliveValues for line disconnection
		/// detection(because default is toooo slow)
		/// </summary>
		/// <param name="tcpClient">TcpClient</param>
		/// <param name="keepAliveTime">The keep alive time. (ms) Defaults to 2 hours.</param>
		/// <param name="keepAliveInterval">The keep alive interval. (ms) Defaults to 1 second.</param>
		/// <exception cref="ArgumentNullException">tcpClient</exception>
		[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Ignoring for now.")]
		public static void SetSocketKeepAliveValues(this TcpClient tcpClient, uint keepAliveTime = DefaultKeepAliveTime, uint keepAliveInterval = DefaultKeepAliveInterval)
		{
			_ = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));

			// KeepAliveTime: default value is 2hr
			// KeepAliveInterval: default value is 1s and Detect 5 times
			const uint dummy = 0; // lenth = 4
			var inOptionValues = new byte[Marshal.SizeOf(dummy) * 3]; // size = lenth * 3 = 12
			const uint OnOff = 1; // OnOff ? 1 : 0

			BitConverter.GetBytes(OnOff).CopyTo(inOptionValues, 0);
			BitConverter.GetBytes(keepAliveTime).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
			BitConverter.GetBytes(keepAliveInterval).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);

			_ = tcpClient.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
		}
	}
}
