
using SharedCode.Properties;



namespace SharedCode.Notifications;
/// <summary>
/// The notification base record.
/// </summary>
public abstract record Notification
{
	/// <summary>
	/// Gets the name.
	/// </summary>
	/// <value>The name.</value>
	protected virtual string Name => this.GetType().Name;

	/// <inheritdoc />
	public override string ToString() => string.IsNullOrWhiteSpace(this.Name) ? Resources.Unknown : this.Name;
}
