namespace SharedCode.Collections.Generic;

/// <summary>
/// The enumeration utilities class.
/// </summary>
public static class EnumerationUtilities
{
    /// <summary>
    /// Returns a list of the values in the specified enumeration.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <returns>The list of the values in the specified enumeration.</returns>
    [SuppressMessage("Refactoring", "GCop659:Use 'var' instead of explicit type.", Justification = "It is a non-generic array so the type cannot be inferred using var.")]
    public static IList<T> ToList<T>() where T : struct, Enum
    {
#if NET6_0_OR_GREATER
        return [.. Enum.GetValues<T>()];
#else
        var values = Enum.GetValues(typeof(T));
        var list = new List<T>(values.Length);
        foreach (T value in values)
        {
            list.Add(value);
        }

        return list;
#endif
    }
}
