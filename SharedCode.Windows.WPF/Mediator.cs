


namespace SharedCode.Windows.WPF;

using System;
using System.Collections.Generic;

/// <summary>
/// Class Mediator.
/// </summary>
public static class Mediator
{
	/// <summary>
	/// The page list dictionary
	/// </summary>
	private static readonly Dictionary<string, List<Action<object?>>> pageListDictionary = [];

	/// <summary>
	/// Notifies the specified token.
	/// </summary>
	/// <param name="token">The page token.</param>
	/// <param name="args">The arguments.</param>
	public static void Notify(string token, object? args = null)
	{
		if (pageListDictionary.TryGetValue(token, out var pageList))
		{
			foreach (var callback in pageList)
			{
				callback(args);
			}
		}
	}

	/// <summary>
	/// Subscribes the specified token.
	/// </summary>
	/// <param name="token">The page token.</param>
	/// <param name="callback">The callback action.</param>
	public static void Subscribe(string token, Action<object?> callback)
	{
		ArgumentNullException.ThrowIfNull(callback);

		if (pageListDictionary.TryGetValue(token, out var value))
		{
			var found = false;
			foreach (var item in value)
			{
				if (item.Method.ToString() == callback?.Method.ToString())
				{
					found = true;
				}
			}

			if (!found)
			{
				value.Add(callback!);
			}
		}
		else
		{
			var list = new List<Action<object?>> { callback };
			pageListDictionary.Add(token, list);
		}
	}

	/// <summary>
	/// Unsubscribes the specified token.
	/// </summary>
	/// <param name="token">The page token.</param>
	/// <param name="callback">The callback action.</param>
	public static void Unsubscribe(string token, Action<object?> callback)
	{
		if (pageListDictionary.TryGetValue(token, out var pageList))
		{
			_ = pageList.Remove(callback);
		}
	}
}
