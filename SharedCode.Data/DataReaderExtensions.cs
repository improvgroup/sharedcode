using System.Data;
using System.Data.Common;

namespace SharedCode.Data;

/// <summary>
/// The data reader extensions class.
/// </summary>
public static class DataReaderExtensions
{
    /// <summary>
    /// Enumerates the specified database data reader.
    /// </summary>
    /// <typeparam name="T">The type of the objects being enumerated.</typeparam>
    /// <param name="this">The database data reader.</param>
    /// <returns>The enumerable sequence.</returns>
    /// <exception cref="ArgumentNullException">dbDataReader</exception>
    public static IEnumerable<T> Enumerate<T>(this IDataReader @this) where T : new()
    {
        return @this is null
            ? throw new ArgumentNullException(nameof(@this))
            : EnumerateImpl();

        IEnumerable<T> EnumerateImpl()
        {
            var converter = new DataRecordConverter<T>(@this);
            while (@this.Read())
            {
                yield return converter.ConvertRecordToItem();
            }
        }
    }

    /// <summary>
    /// Enumerates the specified database data reader as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the objects being enumerated.</typeparam>
    /// <param name="this">The database data reader.</param>
    /// <returns>The asynchronous enumerable sequence.</returns>
    /// <exception cref="ArgumentNullException">dbDataReader</exception>
    public static IAsyncEnumerable<T> EnumerateAsync<T>(this DbDataReader @this) where T : new()
    {
        return @this is null
            ? throw new ArgumentNullException(nameof(@this))
            : EnumerateAsyncImpl();

        async IAsyncEnumerable<T> EnumerateAsyncImpl()
        {
            var converter = new DataRecordConverter<T>(@this);
            while (await @this.ReadAsync().ConfigureAwait(true))
            {
                yield return converter.ConvertRecordToItem();
            }
        }
    }

    /// <summary>
    /// Maps the specified database data record to the specified object type <typeparamref
    /// name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the object being mapped.</typeparam>
    /// <param name="this">The database data record.</param>
    /// <returns>The resulting object.</returns>
    /// <exception cref="ArgumentNullException">dbDataRecord</exception>
    public static T Map<T>(this IDataRecord @this) where T : new()
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(@this);
#else
        if (@this is null)
        {
            throw new ArgumentNullException(nameof(@this));
        }
#endif

        var converter = new DataRecordConverter<T>(@this);
        return converter.ConvertRecordToItem();
    }
}
