using System.Collections.Generic;

namespace JsonFeed
{
	public static class Extensions
	{
		/// <summary>
		/// Current version of the JsonFeed spec supported by this library
		/// </summary>
		public static string CurrentJsonFeedVersion { get; } = "https://jsonfeed.org/version/1";

		/// <summary>
		/// Get a value from the specified JSON object by key
		/// </summary>
		/// <typeparam name="T">The type of the object</typeparam>
		/// <param name="json">The JSON object</param>
		/// <param name="key">The key of the object</param>
		/// <returns></returns>
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

		/// <summary>
		/// Get a list from the specified JSON object by key
		/// </summary>
		/// <typeparam name="T">The type of the list</typeparam>
		/// <param name="json">The JSON object</param>
		/// <param name="key">The key of the list</param>
		/// <returns></returns>
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
