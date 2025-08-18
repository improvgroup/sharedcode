using System.Reflection;

namespace SharedCode;
/// <summary>
/// The utilities class
/// </summary>
public static class Utilities
{
	/// <summary>
	/// Gets the constant name from value.
	/// </summary>
	/// <param name="type">The input type.</param>
	/// <param name="val">The input value.</param>
	/// <returns>The constant name.</returns>
	/// <remarks>See also <seealso href="https://stackoverflow.com/a/10261848/1449056" /></remarks>
	/// <exception cref="ArgumentNullException">type or val</exception>
	public static string? GetConstantNameFromValue(Type type, object val)
	{
		_ = type ?? throw new ArgumentNullException(nameof(type));
		_ = val ?? throw new ArgumentNullException(nameof(val));
		Contract.Ensures(Contract.Result<string>() != null);

		// Gets all public and static fields This tells it to get the fields from all base types as
		// well Go through the list and only pick out the constants
		foreach (var fi in type.GetFields(
			BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
		{
			// remove deprecated / obsolete fields/properties
			if (fi.GetCustomAttributes<ObsoleteAttribute>(inherit: true).Any())
			{
				continue;
			}

			// IsLiteral determines if its value is written at compile time and not changeable
			// IsInitOnly determine if the field can be set in the body of the constructor for C# a
			// field which is readonly keyword would have both true but a const field would have
			// only IsLiteral equal to true
			if (fi.IsLiteral && !fi.IsInitOnly)
			{
				var value = fi.GetRawConstantValue();
				if (value?.Equals(val) ?? false)
				{
					return fi.Name;
				}
			}
		}

		return val.ToString();
	}

	/// <summary>
	/// Swaps the left reference with the right reference.
	/// </summary>
	/// <typeparam name="T">The type of the references.</typeparam>
	/// <param name="left">The left reference.</param>
	/// <param name="right">The right reference.</param>
	/// <exception cref="ArgumentNullException">left or right</exception>
	public static void Swap<T>(ref T left, ref T right)
	{
		_ = left ?? throw new ArgumentNullException(nameof(left));
		_ = right ?? throw new ArgumentNullException(nameof(right));

		(right, left) = (left, right);
	}
}
