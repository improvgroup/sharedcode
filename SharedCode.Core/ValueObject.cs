
using System.Reflection;

namespace SharedCode;
/// <summary>
/// The value object class. Implements the <see cref="IEquatable{ValueObject}" />.
/// </summary>
/// <seealso cref="IEquatable{ValueObject}" />
public abstract class ValueObject : IEquatable<ValueObject>
{
	/// <summary>
	/// The fields
	/// </summary>
	private List<FieldInfo>? fields;

	/// <summary>
	/// The properties
	/// </summary>
	private List<PropertyInfo>? properties;

	/// <summary>
	/// Implements the != operator.
	/// </summary>
	/// <param name="obj1">The first object.</param>
	/// <param name="obj2">The second object.</param>
	/// <returns>The result of the operator.</returns>
	public static bool operator !=(ValueObject obj1, ValueObject obj2) => !(obj1 == obj2);

	/// <summary>
	/// Implements the == operator.
	/// </summary>
	/// <param name="obj1">The first object.</param>
	/// <param name="obj2">The second object.</param>
	/// <returns>The result of the operator.</returns>
	public static bool operator ==(ValueObject obj1, ValueObject obj2) => Equals(obj1, null) ? Equals(obj2, null) : obj1.Equals(obj2);

	/// <summary>
	/// Returns true if the specified object equals this object.
	/// </summary>
	/// <param name="other">The object.</param>
	/// <returns><c>true</c> if the two objects are equal, <c>false</c> otherwise.</returns>
	public bool Equals(ValueObject? other) => this.Equals(other as object);

	/// <inheritdoc />
	public override bool Equals(object? obj) =>
		obj is not null && this.GetType() == obj.GetType() &&
		this.GetProperties().All(p => this.PropertiesAreEqual(obj, p)) &&
		this.GetFields().All(f => this.FieldsAreEqual(obj, f));

	/// <inheritdoc />
	public override int GetHashCode()
	{
		unchecked // allow overflow
		{
			var hash = 17;
			foreach (var prop in this.GetProperties())
			{
				var value = prop.GetValue(this, null);
				hash = HashValue(hash, value);
			}

			foreach (var field in this.GetFields())
			{
				var value = field.GetValue(this);
				hash = HashValue(hash, value);
			}

			return hash;
		}
	}

	/// <summary>
	/// Hashes the value.
	/// </summary>
	/// <param name="seed">The seed.</param>
	/// <param name="value">The value.</param>
	/// <returns>System.Int32.</returns>
	private static int HashValue(int seed, object? value) => (seed * 23) + ((value?.GetHashCode()) ?? 0);

	/// <summary>
	/// Fieldses the are equal.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="f">The f.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private bool FieldsAreEqual(object obj, FieldInfo f) => Equals(f.GetValue(this), f.GetValue(obj));

	/// <summary>
	/// Gets the fields.
	/// </summary>
	/// <returns>IEnumerable&lt;FieldInfo&gt;.</returns>
	private IEnumerable<FieldInfo> GetFields()
	{
		return this.fields ??= this.GetType()
			.GetFields(BindingFlags.Instance | BindingFlags.Public)
			.Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute)))
			.ToList();
	}

	/// <summary>
	/// Gets the properties.
	/// </summary>
	/// <returns>IEnumerable&lt;PropertyInfo&gt;.</returns>
	private IEnumerable<PropertyInfo> GetProperties()
	{
		return this.properties ??= this.GetType()
			.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute)))
			.ToList();
	}

	/// <summary>
	/// Propertieses the are equal.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="p">The p.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private bool PropertiesAreEqual(object obj, PropertyInfo p) => Equals(p.GetValue(this, null), p.GetValue(obj, null));
}
