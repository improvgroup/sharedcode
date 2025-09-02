using SharedCode.Data.Exceptions;

using System.Data;
using System.Dynamic;

namespace SharedCode.Data;

/// <summary>
/// The dynamic data record class.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DynamicDataRecord"/> class.
/// </remarks>
/// <param name="record">The data record to be made dynamic.</param>
public class DynamicDataRecord(IDataRecord record) : DynamicObject
{
    /// <inheritdoc/>
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(binder);
#else
        if (binder is null)
        {
            throw new ArgumentNullException(nameof(binder));
        }
#endif

        try
        {
            var rawResult = record.GetValue(record.GetOrdinal(binder.Name));
            result = rawResult == DBNull.Value ? default : rawResult;
            return true;
        }
        catch (IndexOutOfRangeException)
        {
            throw new ColumnIsNotInResultSetException(binder.Name);
        }
    }
}
