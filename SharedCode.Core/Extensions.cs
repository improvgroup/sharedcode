// <copyright file="Extensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode
{
	using ProtoBuf;

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data.SqlTypes;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Diagnostics.Contracts;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Runtime.Serialization.Json;
	using System.Text;
	using System.Text.Json;
	using System.Xml;
	using System.Xml.Serialization;

	/// <summary>
	/// The extensions class.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// The XML serializers
		/// </summary>
		private static readonly Dictionary<RuntimeTypeHandle, XmlSerializer> XmlSerializers = new();

		/// <summary>
		/// Determines whether the specified <paramref name="value" /> is between the <paramref
		/// name="from" /> and <paramref name="to" /> values.
		/// </summary>
		/// <typeparam name="T">The type of the values to be compared.</typeparam>
		/// <param name="value">The value to be compared.</param>
		/// <param name="from">The lower bound value.</param>
		/// <param name="to">The upper bound value.</param>
		/// <returns>
		/// Returns <c>true</c> if the specified <paramref name="value" /> is between the <paramref
		/// name="from" /> and <paramref name="to" /> values, <c>false</c> otherwise.
		/// </returns>
		public static bool Between<T>(this T value, T from, T to) where T : IComparable<T> => value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;

		/// <summary>
		/// Converts any type to another.
		/// </summary>
		/// <typeparam name="T">The type to convert to.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="returnValueIfException">The return value if exception.</param>
		/// <returns>The output.</returns>
		[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
		public static T? ChangeType<T>(this object source, T returnValueIfException)
		{
			try
			{
				return source.ChangeType<T>();
			}
			catch (Exception)
			{
				return returnValueIfException;
			}
		}

		/// <summary>
		/// Converts any type to another.
		/// </summary>
		/// <typeparam name="T">The type to convert to.</typeparam>
		/// <param name="source">The source object.</param>
		/// <returns>The output.</returns>
		public static T? ChangeType<T>(this object source)
		{
			if (source is T u)
			{
				return u;
			}

			var destinationType = typeof(T);
			if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				destinationType = new NullableConverter(destinationType).UnderlyingType;
			}

			return (T?)Convert.ChangeType(source, destinationType, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Makes a copy of the object using Protobuf serialization.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="this">Object to be copied.</param>
		/// <returns>The copied object.</returns>
		/// <remarks>The <see cref="BinaryFormatter" /> method was removed due to security issues.</remarks>
		public static T? Clone<T>(this object @this)
		{
			var bytes = @this.ToByteArray();
			return bytes is null ? default : bytes.ToObject<T>();
		}

		/// <summary>
		/// Converts the value to the specified type.
		/// </summary>
		/// <typeparam name="T">The type to convert to.</typeparam>
		/// <param name="value">The input value.</param>
		/// <returns>The output value.</returns>
		public static T? ConvertTo<T>(this IConvertible value) => (T?)Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture);

		/// <summary>
		/// Converts the value to the specified type.
		/// </summary>
		/// <typeparam name="T">The type to convert to.</typeparam>
		/// <param name="value">The input value.</param>
		/// <returns>The output value.</returns>
		public static T? ConvertTo<T>(this object value) => (T?)Convert.ChangeType(value, typeof(T), CultureInfo.CurrentCulture);

		/// <summary>
		/// Returns a deep copy of this object.
		/// </summary>
		/// <typeparam name="T">The type of the input object.</typeparam>
		/// <param name="input">The input object.</param>
		/// <returns>The output.</returns>
		/// <remarks>The <see cref="BinaryFormatter" /> method was removed due to security issues.</remarks>
		public static T? DeepClone<T>(this T input) where T : ISerializable
		{
			var bytes = input.ToByteArray();
			return bytes is null ? default : bytes.ToObject<T>();
		}

		/// <summary>
		/// Converts a JSON string to the specified type.
		/// </summary>
		/// <typeparam name="T">The type of the object represented in the JSON string.</typeparam>
		/// <param name="jsonString">The JSON string.</param>
		/// <returns>The output.</returns>
		public static T? FromJson<T>(this object jsonString) => jsonString is not string s ? default : JsonSerializer.Deserialize<T>(s);

		/// <summary>
		/// Gets the property information.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="propertyLambda">The property lambda.</param>
		/// <returns>The property information.</returns>
		/// <exception cref="ArgumentNullException">propertyLambda</exception>
		/// <exception cref="ArgumentException">
		/// Expression propertyLambda refers to a method or field, not a property.
		/// </exception>
		public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
		{
			_ = propertyLambda ?? throw new ArgumentNullException(nameof(propertyLambda));

			var type = typeof(TSource);

			if (propertyLambda.Body is not MemberExpression member)
			{
				throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
			}

			if (member.Member is not PropertyInfo propInfo)
			{
				throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
			}

			return propInfo.ReflectedType switch
			{
				null => throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property."),
				_ => type == propInfo.ReflectedType || type.IsSubclassOf(propInfo.ReflectedType) || propInfo.ReflectedType.IsAssignableFrom(type)
					? propInfo
					: throw new ArgumentException($"Expresion '{propertyLambda}' refers to a property that is not from type {type}.")
			};
		}

		/// <summary>
		/// Gets the property value from the source object using reflection.
		/// </summary>
		/// <typeparam name="T">The type of the property value.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="property">The property name.</param>
		/// <returns>The property value.</returns>
		/// <exception cref="ArgumentNullException">source or property</exception>
		public static T? GetPropertyValue<T>(this object source, string property)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (property is null)
			{
				throw new ArgumentNullException(nameof(property));
			}

			var sourceType = source.GetType();
			var sourceProperties = sourceType.GetProperties();

			return (T?)sourceProperties
				.Where(s => s.Name.Equals(property, StringComparison.OrdinalIgnoreCase))
				.Select(s => s.GetValue(source, null))
				.FirstOrDefault();
		}

		/// <summary>
		/// If the object this method is called on is not null, runs the given function and returns
		/// the value. If the object is null, returns default(TResult)
		/// </summary>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="target">The target object.</param>
		/// <param name="getValue">The get value function.</param>
		/// <returns>The result.</returns>
		public static TResult? IfNotNull<T, TResult>(this T target, Func<T, TResult> getValue)
		{
			var handler = getValue;
			return handler is null || EqualityComparer<T>.Default.Equals(target, default) ? default : handler(target);
		}

		/// <summary>
		/// Determines if this value is in the specified list of parameters.
		/// </summary>
		/// <typeparam name="T">The type of values being compared.</typeparam>
		/// <param name="value">This value.</param>
		/// <param name="parameters">The list of parameters.</param>
		/// <returns>
		/// <c>true</c> if this value is in the specified list of parameters, <c>false</c> otherwise.
		/// </returns>
		public static bool In<T>(this T value, params T[] parameters) => parameters?.Contains(value) ?? false;

		/// <summary>
		/// Determines whether the specified value is between the low and high values.
		/// </summary>
		/// <typeparam name="T">The types being compared.</typeparam>
		/// <param name="value">The value to compare.</param>
		/// <param name="low">The low value.</param>
		/// <param name="high">The high value.</param>
		/// <returns>
		/// <c>true</c> if the specified value is between the low and high values; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsBetween<T>(this T value, T low, T high) where T : IComparable<T> => value.CompareTo(low) >= 0 && value.CompareTo(high) <= 0;

		/// <summary>
		/// Determines whether the specified source object is not null.
		/// </summary>
		/// <param name="source">The source object.</param>
		/// <returns><c>true</c> if the specified source object is not null; otherwise, <c>false</c>.</returns>
		public static bool IsNotNull(this object source) => source != null;

		/// <summary>
		/// Determines whether the specified source object is null.
		/// </summary>
		/// <param name="source">The source object.</param>
		/// <returns><c>true</c> if the specified source object is null; otherwise, <c>false</c>.</returns>
		public static bool IsNull(this object source) => source is null;

		/// <summary>
		/// Unified advanced generic check for: DbNull.Value, INullable.IsNull,
		/// !Nullable&lt;&gt;.HasValue, null reference. Omits boxing for value types.
		/// </summary>
		/// <typeparam name="T">The type of the value we are checking.</typeparam>
		/// <param name="value">The value to check.</param>
		/// <returns>A value indicating whether or not this value is null.</returns>
		[DebuggerStepThrough]
		public static bool IsNull<T>(this T value)
		{
			if (value is INullable nullable && nullable.IsNull)
			{
				return true;
			}

			var type = typeof(T);
			if (type.IsValueType)
			{
				if (Nullable.GetUnderlyingType(type) is not null && value?.GetHashCode() == 0)
				{
					return true;
				}
			}
			else
			{
				if (EqualityComparer<T>.Default.Equals(value, default))
				{
					return true;
				}

				if (Convert.IsDBNull(value))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the specified value is null.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="value">The value being checked.</param>
		/// <returns>A value indicating whether the specified value is null.</returns>
		[DebuggerStepThrough]
		public static bool IsNull<T>(this T? value) where T : struct => !value.HasValue;

		/// <summary>
		/// Convert an <see cref="object" /> to a <see cref="byte" /> array, using Protobuf.
		/// </summary>
		/// <param name="this">The <see cref="object" />.</param>
		public static byte[]? ToByteArray(this object @this)
		{
			if (@this is null)
			{
				return null;
			}

			using var stream = new MemoryStream();
			Serializer.Serialize(stream, @this);
			return stream.ToArray();
		}

		/// <summary>
		/// Turns any object into an exception.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>The exception.</returns>
		[SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
		public static Exception ToException(this object obj) => new(obj?.ToString() ?? string.Empty);

		/// <summary>
		/// Convert an object to JSON.
		/// </summary>
		/// <param name="input">The input object.</param>
		/// <returns>The JSON representation of the object.</returns>
		public static string ToJson(this object input) => JsonSerializer.Serialize(input);

		/// <summary>
		/// Converts this item to a JSON string.
		/// </summary>
		/// <typeparam name="T">The type of the item.</typeparam>
		/// <param name="item">The item to be converted.</param>
		/// <param name="encoding">The string encoding.</param>
		/// <param name="serializer">The JSON serializer.</param>
		/// <returns>The JSON string.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static string ToJson<T>(
			this T item,
			Encoding? encoding = null,
			DataContractJsonSerializer? serializer = null)
		{
			_ = item ?? throw new ArgumentNullException(nameof(item));
			Contract.Ensures(Contract.Result<string>() != null);

			encoding ??= Encoding.Default;
			serializer ??= new DataContractJsonSerializer(typeof(T));

			using var stream = new MemoryStream();
			serializer.WriteObject(stream, item);
			return encoding.GetString(stream.ToArray());
		}

		/// <summary>
		/// Convert a <see cref="byte" /> array to an <see cref="object" /> of <typeparamref
		/// name="T" />, using Protobuf.
		/// </summary>
		/// <typeparam name="T">The type of the resulting object.</typeparam>
		/// <param name="this">The <see cref="byte" /> array.</param>
		public static T? ToObject<T>(this byte[] @this)
		{
			if (@this is null || @this.Length == 0)
			{
				return default;
			}

			using var stream = new MemoryStream();

			stream.Write(@this, 0, @this.Length);

			// Ensure that our stream is at the beginning.
			_ = stream.Seek(0, SeekOrigin.Begin);

			return Serializer.Deserialize<T>(stream);
		}

		/// <summary>
		/// Serialize object to xml string by <see cref="XmlSerializer" />
		/// </summary>
		/// <typeparam name="T">The type of the input value.</typeparam>
		/// <param name="value">The input value.</param>
		/// <returns>The XML representation of the input value.</returns>
		/// <exception cref="ArgumentNullException">value</exception>
		public static string ToXml<T>(this T value) where T : new()
		{
			_ = value ?? throw new ArgumentNullException(nameof(value));
			Contract.Ensures(Contract.Result<string>() != null);

			var serializer = GetXmlSerializer(typeof(T));
			using var stream = new MemoryStream();
			using var writer = new XmlTextWriter(stream, new UTF8Encoding());
			serializer.Serialize(writer, value);
			return Encoding.UTF8.GetString(stream.ToArray());
		}

		/// <summary>
		/// Serialize object to xml string by <see cref="XmlSerializer" />
		/// </summary>
		/// <typeparam name="T">The type of the input value.</typeparam>
		/// <param name="value">The input value.</param>
		/// <param name="stream">The output stream.</param>
		/// <exception cref="ArgumentNullException">value or stream</exception>
		public static void ToXml<T>(this T value, Stream stream) where T : new()
		{
			_ = value ?? throw new ArgumentNullException(nameof(value));
			_ = stream ?? throw new ArgumentNullException(nameof(stream));

			var serializer = GetXmlSerializer(typeof(T));
			serializer.Serialize(stream, value);
		}

		/// <summary>
		/// Serializes the input object to an XML string.
		/// </summary>
		/// <param name="input">The input object.</param>
		/// <returns>The XML string.</returns>
		/// <exception cref="ArgumentNullException">input</exception>
		public static string ToXml(this object input)
		{
			_ = input ?? throw new ArgumentNullException(nameof(input));
			Contract.Ensures(Contract.Result<string>() != null);

			// Serialize an object into an xml string
			var xmlSerializer = new XmlSerializer(input.GetType());

			// use new UTF8Encoding here, not Encoding.UTF8.
			using var memoryStream = new MemoryStream();
			using var xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding());

			xmlSerializer.Serialize(xmlTextWriter, input);
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}

		/// <summary>
		/// Gets the XML serializer for the specified <paramref name="type" />.
		/// </summary>
		/// <param name="type">The type handled by the serializer.</param>
		/// <returns>The <see cref="XmlSerializer" /> for the <paramref name="type" />.</returns>
		/// <exception cref="ArgumentNullException">type</exception>
		private static XmlSerializer GetXmlSerializer(Type type)
		{
			_ = type ?? throw new ArgumentNullException(nameof(type));

			if (XmlSerializers.TryGetValue(type.TypeHandle, out var serializer))
			{
				return serializer;
			}

			lock (XmlSerializers)
			{
				if (XmlSerializers.TryGetValue(type.TypeHandle, out serializer))
				{
					return serializer;
				}

				serializer = new XmlSerializer(type);
				XmlSerializers.Add(type.TypeHandle, serializer);
			}

			return serializer;
		}
	}
}
