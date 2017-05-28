using System.Collections.Generic;

namespace JsonFeed
{
	public static class Extensions
	{
		public static string CurrentJsonFeedVersion { get; } = "https://jsonfeed.org/version/1";

		public static T GetValue<T>(this IDictionary<string, object> json, string key) where T : class
		{
			if (json.TryGetValue(key, out object value))
			{
				return value as T;
			}
			else
			{
				return default(T);
			}
		}

		public static IEnumerable<T> GetList<T>(this IDictionary<string, object> json, string key) where T : class
		{
			if (json.TryGetValue(key, out object value))
			{
				foreach (var item in value as IEnumerable<object>)
				{
					yield return item as T;
				}
			}
		}
	}
}
