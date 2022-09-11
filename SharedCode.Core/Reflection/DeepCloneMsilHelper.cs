namespace SharedCode.Reflection;

using System;
using System.Reflection;

/// <summary>
/// The deep clone MSIL helper class.
/// </summary>
internal static class DeepCloneMsilHelper
{
	/// <summary>
	/// Determines whether [is constructor do nothing] [the specified type].
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="constructor">The constructor.</param>
	/// <returns>
	/// <c>true</c> if [is constructor do nothing] [the specified type]; otherwise, <c>false</c>.
	/// </returns>
	[SuppressMessage("Design", "GCop179:Do not hardcode numbers, strings or other values. Use constant fields, enums, config files or database as appropriate.", Justification = "<Pending>")]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
	public static bool IsConstructorDoNothing(Type type, ConstructorInfo constructor)
	{
		if (constructor is null)
			return false;

		try
		{
			// will not try to determine body for this types
			if (type.IsGenericType || type.IsContextful || type.IsCOMObject || type.Assembly.IsDynamic)
				return false;

			var methodBody = constructor.GetMethodBody();

			// this situation can be for com
			if (methodBody is null)
				return false;

			var ilAsByteArray = methodBody.GetILAsByteArray();
			if (ilAsByteArray?.Length == 7
				&& ilAsByteArray[0] == 0x02 // Ldarg_0
				&& ilAsByteArray[1] == 0x28 // newobj
				&& ilAsByteArray[6] == 0x2a // ret
				&& type.Module.ResolveMethod(BitConverter.ToInt32(ilAsByteArray, 2)) == typeof(object).GetConstructor(Type.EmptyTypes)) // call object
			{
				return true;
			}
			else if (ilAsByteArray?.Length == 1 && ilAsByteArray[0] == 0x2a) // ret
			{
				return true;
			}

			return false;
		}
		catch (Exception)
		{
			// no permissions or something similar
			return false;
		}
	}
}
