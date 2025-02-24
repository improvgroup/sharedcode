
namespace SharedCode;

using System;

/// <summary>
/// Static class containing Fluent <see cref="DateTime" /> extension methods.
/// </summary>
public static class NumberExtensions
{
	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Days.
	/// </summary>
	/// <param name="days">The days.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Days(this int days) => new() { TimeSpan = TimeSpan.FromDays(days) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Days.
	/// </summary>
	/// <param name="days">The days.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Days(this double days) => new() { TimeSpan = TimeSpan.FromDays(days) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Hours.
	/// </summary>
	/// <param name="hours">The hours.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Hours(this int hours) => new() { TimeSpan = TimeSpan.FromHours(hours) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Hours.
	/// </summary>
	/// <param name="hours">The hours.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Hours(this double hours) => new() { TimeSpan = TimeSpan.FromHours(hours) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Milliseconds.
	/// </summary>
	/// <param name="milliseconds">The milliseconds.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Milliseconds(this int milliseconds) => new() { TimeSpan = TimeSpan.FromMilliseconds(milliseconds) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Milliseconds.
	/// </summary>
	/// <param name="milliseconds">The milliseconds.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Milliseconds(this double milliseconds) => new() { TimeSpan = TimeSpan.FromMilliseconds(milliseconds) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Minutes.
	/// </summary>
	/// <param name="minutes">The minutes.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Minutes(this int minutes) => new() { TimeSpan = TimeSpan.FromMinutes(minutes) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Minutes.
	/// </summary>
	/// <param name="minutes">The minutes.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Minutes(this double minutes) => new() { TimeSpan = TimeSpan.FromMinutes(minutes) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> value for given number of Months.
	/// </summary>
	/// <param name="months">The months.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Months(this int months) => new() { Months = months };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Seconds.
	/// </summary>
	/// <param name="seconds">The seconds.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Seconds(this int seconds) => new() { TimeSpan = TimeSpan.FromSeconds(seconds) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Seconds.
	/// </summary>
	/// <param name="seconds">The seconds.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Seconds(this double seconds) => new() { TimeSpan = TimeSpan.FromSeconds(seconds) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of ticks.
	/// </summary>
	/// <param name="ticks">The ticks.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Ticks(this int ticks) => new() { TimeSpan = TimeSpan.FromTicks(ticks) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of ticks.
	/// </summary>
	/// <param name="ticks">The ticks.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Ticks(this long ticks) => new() { TimeSpan = TimeSpan.FromTicks(ticks) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Weeks (number of weeks * 7).
	/// </summary>
	/// <param name="weeks">The weeks.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Weeks(this int weeks) => new() { TimeSpan = TimeSpan.FromDays(weeks * 7) };

	/// <summary>
	/// Returns <see cref="TimeSpan" /> for given number of Weeks (number of weeks * 7).
	/// </summary>
	/// <param name="weeks">The weeks.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Weeks(this double weeks) => new() { TimeSpan = TimeSpan.FromDays(weeks * 7) };

	/// <summary>
	/// Generates <see cref="TimeSpan" /> value for given number of Years.
	/// </summary>
	/// <param name="years">The years.</param>
	/// <returns>A fluent time span.</returns>
	public static FluentTimeSpan Years(this int years) => new() { Years = years };
}
