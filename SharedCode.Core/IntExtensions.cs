
namespace SharedCode
{
	using System;

	/// <summary>
	/// The integer extensions class
	/// </summary>
	public static class IntExtensions
	{
		/// <summary>
		/// Gigabytes
		/// </summary>
		/// <param name="value">The number of bytes.</param>
		/// <returns>The value in GB.</returns>
		public static int GB(this int value) => value.MB() * 1024;

		/// <summary>
		/// Determines whether the specified number is prime.
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns><c>true</c> if the specified number is prime; otherwise, <c>false</c>.</returns>
		public static bool IsPrime(this int number)
		{
			if (number % 2 == 0)
				return number == 2;

			var sqrt = (int)Math.Sqrt(number);
			for (var t = 3; t <= sqrt; t += 2)
			{
				if (number % t == 0)
					return false;
			}

			return number != 1;
		}

		/// <summary>
		/// Kilobytes
		/// </summary>
		/// <param name="value">The number of bytes.</param>
		/// <returns>The value in KB.</returns>
		public static int KB(this int value) => value * 1024;

		/// <summary>
		/// Megabytes
		/// </summary>
		/// <param name="value">The number of bytes.</param>
		/// <returns>The value in MB.</returns>
		public static int MB(this int value) => value.KB() * 1024;

		/// <summary>
		/// Terabytes
		/// </summary>
		/// <param name="value">The number of bytes.</param>
		/// <returns>The value in TB.</returns>
		public static long TB(this int value) => (long)value.GB() * 1024;
	}
}
