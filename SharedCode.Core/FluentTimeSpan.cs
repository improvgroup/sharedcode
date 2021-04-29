// <copyright file="FluentTimeSpan.cs" company="improvGroup, LLC">
//     Copyright © 2009-2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode
{
	using System;
	using System.ComponentModel;
	using System.Runtime.InteropServices;

	/// <summary>
	/// The fluent time span.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(true)]
	public struct FluentTimeSpan : IEquatable<FluentTimeSpan>, IComparable<TimeSpan>, IComparable<FluentTimeSpan>, IComparable
	{
		/// <summary>
		/// The days per year.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public const int DaysPerYear = 365;

		/// <summary>
		/// Gets or sets Months.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int Months { get; set; }

		/// <summary>
		/// Gets or sets Years.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int Years { get; set; }

		/// <summary>
		/// Gets or sets TimeSpan.
		/// </summary>
		/// <value>TimeSpan.</value>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public TimeSpan TimeSpan { get; set; }

		/// <summary>
		/// Gets the ticks.
		/// </summary>
		/// <value>The ticks.</value>
		public long Ticks => ((TimeSpan)this).Ticks;

		/// <summary>
		/// Gets the days.
		/// </summary>
		/// <value>The days.</value>
		public int Days => ((TimeSpan)this).Days;

		/// <summary>
		/// Gets the hours.
		/// </summary>
		/// <value>The hours.</value>
		public int Hours => ((TimeSpan)this).Hours;

		/// <summary>
		/// Gets the milliseconds.
		/// </summary>
		/// <value>The milliseconds.</value>
		public int Milliseconds => ((TimeSpan)this).Milliseconds;

		/// <summary>
		/// Gets the minutes.
		/// </summary>
		/// <value>The minutes.</value>
		public int Minutes => ((TimeSpan)this).Minutes;

		/// <summary>
		/// Gets the seconds.
		/// </summary>
		/// <value>The seconds.</value>
		public int Seconds => ((TimeSpan)this).Seconds;

		/// <summary>
		/// Gets the total days.
		/// </summary>
		/// <value>The total days.</value>
		public double TotalDays => ((TimeSpan)this).TotalDays;

		/// <summary>
		/// Gets the total hours.
		/// </summary>
		/// <value>The total hours.</value>
		public double TotalHours => ((TimeSpan)this).TotalHours;

		/// <summary>
		/// Gets the total milliseconds.
		/// </summary>
		/// <value>The total milliseconds.</value>
		public double TotalMilliseconds => ((TimeSpan)this).TotalMilliseconds;

		/// <summary>
		/// Gets the total minutes.
		/// </summary>
		/// <value>The total minutes.</value>
		public double TotalMinutes => ((TimeSpan)this).TotalMinutes;

		/// <summary>
		/// Gets the total seconds.
		/// </summary>
		/// <value>The total seconds.</value>
		public double TotalSeconds => ((TimeSpan)this).TotalSeconds;

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static FluentTimeSpan operator +(FluentTimeSpan left, FluentTimeSpan right) => AddInternal(left, right);

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static FluentTimeSpan operator +(FluentTimeSpan left, TimeSpan right) => AddInternal(left, right);

		/// <summary>
		/// Implements the operator +.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static FluentTimeSpan operator +(TimeSpan left, FluentTimeSpan right) => AddInternal(left, right);

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static FluentTimeSpan operator -(FluentTimeSpan left, FluentTimeSpan right) => SubtractInternal(left, right);

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static FluentTimeSpan operator -(TimeSpan left, FluentTimeSpan right) => SubtractInternal(left, right);

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static FluentTimeSpan operator -(FluentTimeSpan left, TimeSpan right) => SubtractInternal(left, right);

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(FluentTimeSpan left, FluentTimeSpan right) =>
			(left.Years == right.Years) && (left.Months == right.Months) && (left.TimeSpan == right.TimeSpan);

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(TimeSpan left, FluentTimeSpan right) => (FluentTimeSpan)left == right;

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(FluentTimeSpan left, TimeSpan right) => left == (FluentTimeSpan)right;

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(FluentTimeSpan left, FluentTimeSpan right) => !(left == right);

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(TimeSpan left, FluentTimeSpan right) => !(left == right);

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(FluentTimeSpan left, TimeSpan right) => !(left == right);

		/// <summary>
		/// Implements the operator -.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operator.</returns>
		public static FluentTimeSpan operator -(FluentTimeSpan value) => value.Negate();

		/// <summary>
		/// Implements the operator &lt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <(FluentTimeSpan left, FluentTimeSpan right) => (TimeSpan)left < (TimeSpan)right;

		/// <summary>
		/// Implements the operator &lt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <(FluentTimeSpan left, TimeSpan right) => (TimeSpan)left < right;

		/// <summary>
		/// Implements the operator &lt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <(TimeSpan left, FluentTimeSpan right) => left < (TimeSpan)right;

		/// <summary>
		/// Implements the operator &lt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <=(FluentTimeSpan left, FluentTimeSpan right) => (TimeSpan)left <= (TimeSpan)right;

		/// <summary>
		/// Implements the operator &lt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <=(FluentTimeSpan left, TimeSpan right) => (TimeSpan)left <= right;

		/// <summary>
		/// Implements the operator &lt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator <=(TimeSpan left, FluentTimeSpan right) => left <= (TimeSpan)right;

		/// <summary>
		/// Implements the operator &gt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >(FluentTimeSpan left, FluentTimeSpan right) => (TimeSpan)left > (TimeSpan)right;

		/// <summary>
		/// Implements the operator &gt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >(FluentTimeSpan left, TimeSpan right) => (TimeSpan)left > right;

		/// <summary>
		/// Implements the operator &gt;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >(TimeSpan left, FluentTimeSpan right) => left > (TimeSpan)right;

		/// <summary>
		/// Implements the operator &gt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >=(FluentTimeSpan left, FluentTimeSpan right) => (TimeSpan)left >= (TimeSpan)right;

		/// <summary>
		/// Implements the operator &gt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >=(FluentTimeSpan left, TimeSpan right) => (TimeSpan)left >= right;

		/// <summary>
		/// Implements the operator &gt;=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator >=(TimeSpan left, FluentTimeSpan right) => left >= (TimeSpan)right;

		/// <summary>
		/// Performs an explicit conversion from <see cref="FluentTimeSpan" /> to <see
		/// cref="TimeSpan" />.
		/// </summary>
		/// <param name="fluentTimeSpan">The FluentTimeSpan.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator TimeSpan(FluentTimeSpan fluentTimeSpan)
		{
			var daysFromYears = DaysPerYear * fluentTimeSpan.Years;
			var daysFromMonths = 30 * fluentTimeSpan.Months;
			var days = daysFromMonths + daysFromYears;
			return new TimeSpan(days, 0, 0, 0) + fluentTimeSpan.TimeSpan;
		}

		/// <summary>
		/// Performs an implicit conversion from a <see cref="TimeSpan" /> to <see
		/// cref="FluentTimeSpan" />.
		/// </summary>
		/// <param name="timeSpan">The <see cref="TimeSpan" /> that will be converted.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator FluentTimeSpan(TimeSpan timeSpan) => new() { TimeSpan = timeSpan };

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// <c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(FluentTimeSpan other) => this == other;

		/// <summary>
		/// Adds two fluentTimeSpan according operator +.
		/// </summary>
		/// <param name="number">The number to add to this fluentTimeSpan.</param>
		/// <returns>The result of the addition operation.</returns>
		public FluentTimeSpan Add(FluentTimeSpan number) => AddInternal(this, number);

		/// <summary>
		/// Subtracts the number according -.
		/// </summary>
		/// <param name="fluentTimeSpan">The matrix to subtract from this fluentTimeSpan.</param>
		/// <returns>The result of the subtraction.</returns>
		public FluentTimeSpan Subtract(FluentTimeSpan fluentTimeSpan) => SubtractInternal(this, fluentTimeSpan);

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone() => new FluentTimeSpan { TimeSpan = this.TimeSpan, Months = this.Months, Years = this.Years };

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="string" /> that represents this instance.</returns>
		public override string ToString() => ((TimeSpan)this).ToString();

		/// <summary>
		/// Determines whether the specified <see cref="object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object? obj)
		{
			if (obj is null)
			{
				return false;
			}

			var type = obj.GetType();
			if (type == typeof(FluentTimeSpan))
			{
				return this == (FluentTimeSpan)obj;
			}

			if (type == typeof(TimeSpan))
			{
				return this == (TimeSpan)obj;
			}

			return false;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data
		/// structures like a hash table.
		/// </returns>
		public override int GetHashCode() => this.Months.GetHashCode() ^ this.Years.GetHashCode() ^ this.TimeSpan.GetHashCode();

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an
		/// integer that indicates whether the current instance precedes, follows, or occurs in the
		/// same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return
		/// value has these meanings:
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>Less than zero</term>
		/// <description>This instance precedes <paramref name="obj" /> in the sort order.</description>
		/// </item>
		/// <item>
		/// <term>Zero</term>
		/// <description>
		/// This instance occurs in the same position in the sort order as <paramref name="obj" />.
		/// </description>
		/// </item>
		/// <item>
		/// <term>Greater than zero</term>
		/// <description>This instance follows <paramref name="obj" /> in the sort order.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <exception cref="ArgumentException">obj</exception>
		public int CompareTo(object? obj) =>
			obj switch
			{
				null => 1,
				TimeSpan x => CompareTo(x),
				_ => throw new ArgumentException("Object is not a FluentTimeSpan.", nameof(obj))
			};

		/// <summary>
		/// Compares to.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns>The compare to.</returns>
		public int CompareTo(TimeSpan other) => ((TimeSpan)this).CompareTo(other);

		/// <summary>
		/// Compares to.
		/// </summary>
		/// <param name="other">The value.</param>
		/// <returns>The compare to.</returns>
		public int CompareTo(FluentTimeSpan other) => ((TimeSpan)this).CompareTo(other);

		/// <summary>
		/// Negates this instance.
		/// </summary>
		/// <returns>The <see cref="TimeSpan" />.</returns>
		public TimeSpan Negate() => new FluentTimeSpan { TimeSpan = -this.TimeSpan, Months = -this.Months, Years = -this.Years };

		/// <summary>
		/// Internal addition function.
		/// </summary>
		/// <param name="left">The left hand side.</param>
		/// <param name="right">The right hand side.</param>
		/// <returns>A fluent time span.</returns>
		private static FluentTimeSpan AddInternal(FluentTimeSpan left, FluentTimeSpan right)
			=> new()
			{
				Years = left.Years + right.Years,
				Months = left.Months + right.Months,
				TimeSpan = left.TimeSpan + right.TimeSpan
			};

		/// <summary>
		/// Internal subtraction function for the subtraction of fluentTimeSpans.
		/// </summary>
		/// <param name="left">The left hand side.</param>
		/// <param name="right">The right hand side.</param>
		/// <returns>A fluent time span.</returns>
		private static FluentTimeSpan SubtractInternal(FluentTimeSpan left, FluentTimeSpan right)
			=> new()
			{
				Years = left.Years - right.Years,
				Months = left.Months - right.Months,
				TimeSpan = left.TimeSpan - right.TimeSpan
			};
	}
}
