using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonFeed
{
	public class JsonFeed
	{
		public static string Version { get; } = "https://jsonfeed.org/version/1";

		public static Feed Parse(string jsonString)
		{
			if (string.IsNullOrWhiteSpace(jsonString))
			{
				throw new ArgumentNullException(nameof(jsonString));
			}

			var json = SimpleJson.DeserializeObject(jsonString) as IDictionary<string, object>;

			return Parse(json);
		}

		public static Feed Parse(IDictionary<string, object> json)
		{
			if (json == null)
			{
				return null;
			}

			var version = json.GetValue<string>("version");
			var title = json.GetValue<string>("title");

			if (string.IsNullOrWhiteSpace(version) || version != Version)
			{
				throw new ArgumentException("Invalid version");
			}

			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("Invalid title");
			}

			if (!json.ContainsKey("items"))
			{
				throw new ArgumentException("No items found");
			}

			var feed = new Feed
			{
				Version = version,
				Title = title,
				HomePageUrl = json.GetValue<string>("home_page_url"),
				FeedUrl = json.GetValue<string>("feed_url"),
				Description = json.GetValue<string>("description"),
				UserComment = json.GetValue<string>("user_comment"),
				NextUrl = json.GetValue<string>("next_url"),
				Icon = json.GetValue<string>("icon"),
				Favicon = json.GetValue<string>("favicon"),
				Expired = json.GetValue<string>("expired") == "true"
			};

			if (json.ContainsKey("author"))
			{
				feed.Author = new Author(json.GetValue<IDictionary<string, object>>("author"));
			}

			if (json.ContainsKey("hubs"))
			{
				var hubsJson = json.GetList<IDictionary<string, object>>("hubs");

				feed.Hubs = hubsJson
					.Where(w => w["type"] != null && w["url"] != null)
					.Select(s => new Hub(s));
			}

			var itemsJson = json.GetList<IDictionary<string, object>>("items");

			feed.Items = itemsJson
				.Where(w => w["id"] != null)
				.Select(s => new FeedItem(s));

			return feed;
		}

		public static string Serialize(Feed feed)
		{
			return SimpleJson.SerializeObject(feed);
		}
	}
}