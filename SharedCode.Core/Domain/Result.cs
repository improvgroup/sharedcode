namespace SharedCode.Domain;

/// <summary>
/// Represents a result of an operation that can either succeed or fail.
/// </summary>
/// <param name="Success">
/// A boolean indicating whether the operation was successful. Defaults to true.
/// </param>
[SuppressMessage("Naming", "GCop201:Use camelCasing when declaring {0}", Justification = "Not valid on record types with primary constructors.")]
public readonly record struct Result(bool Success = true) : IResult;

/// <summary>
/// Represents a result of an operation that can either succeed or fail, with a value.
/// </summary>
/// <typeparam name="T">The type of the value contained in the result.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Per type semantics are understood.")]
public record class Result<T> : IResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with a specified value and
    /// success status.
    /// </summary>
    /// <param name="value">
    /// The value of the result if it is successful. If the operation fails, this can be null.
    /// </param>
    /// <param name="success">
    /// A boolean indicating whether the operation was successful. Defaults to true.
    /// </param>
    public Result(T? value = default, bool success = true)
    {
        this.Value = value;
        this.Success = success;
    }

    /// <inheritdoc/>
    public bool Success { get; }

    /// <inheritdoc/>
    public T? Value { get; }

    /// <summary>
    /// Implicit conversion from <see typeref="T"/> to <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<T>(T value) => ToResult(value);

    /// <summary>
    /// Converts a nullable value to a <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">The nullable value to convert to a result.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the value if it is not null, otherwise a failed result.
    /// </returns>
    public static Result<T> ToResult(T? value) => new(value, value is not null);
}