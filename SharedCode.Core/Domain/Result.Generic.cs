namespace SharedCode.Domain;

/// <summary>
/// The result class. Implements the <see cref="Result" />.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <seealso cref="Result" />
public abstract class Result<T> : Result
{
	[SuppressMessage("Style", "GCop406:Mark {0} field as read-only.", Justification = "Backing field for mutable property so readonly is inappropriate here.")]
	private T? _data;

	/// <summary>
	/// Initializes a new instance of the <see cref="Result{T}" /> class.
	/// </summary>
	/// <param name="data">The result data.</param>
	protected Result(T? data) => this.Data = data;

	/// <summary>
	/// Gets or sets the data.
	/// </summary>
	/// <value>The result data.</value>
	/// <exception cref="InvalidOperationException">
	/// You cannot access this.Data when this.Success is false.
	/// </exception>
	public T? Data
	{
		get => this.Success ? this._data : throw new InvalidOperationException($"You cannot access .{nameof(this.Data)} when .{nameof(this.Success)} is false.");
		set => this._data = value;
	}
}
