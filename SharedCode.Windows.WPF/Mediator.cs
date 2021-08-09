namespace SharedCode.Windows.WPF
{
	using System;
	using System.Collections.Generic;

	public static class Mediator
	{
		private static readonly IDictionary<string, List<Action<object?>>> pageListDictionary = new Dictionary<string, List<Action<object?>>>();

		public static void Subscribe(string token, Action<object?> callback)
		{
			if (!pageListDictionary.ContainsKey(token))
			{
				var list = new List<Action<object?>> { callback };
				pageListDictionary.Add(token, list);
			}
			else
			{
				var found = false;
				foreach (var item in pageListDictionary[token])
				{
					if (item.Method.ToString() == callback?.Method.ToString())
					{
						found = true;
					}
				}

				if (!found)
				{
					pageListDictionary[token].Add(callback);
				}
			}
		}

		public static void Unsubscribe(string token, Action<object?> callback)
		{
			if (pageListDictionary.ContainsKey(token))
			{
				_ = pageListDictionary[token].Remove(callback);
			}
		}

		public static void Notify(string token, object? args = null)
		{
			if (pageListDictionary.ContainsKey(token))
			{
				foreach (var callback in pageListDictionary[token])
				{
					callback(args);
				}
			}
		}
	}
}
