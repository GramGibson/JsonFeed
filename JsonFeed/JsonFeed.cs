using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonFeed
{
	public class JsonFeed
	{
		public string Version { get; set; }
		public string Title { get; set; }
		public string HomePageUrl { get; set; }
		public string FeedUrl { get; set; }
		public string Description { get; set; }
		public string UserComment { get; set; }
		public string NextUrl { get; set; }
		public string Icon { get; set; }
		public string Favicon { get; set; }
		public Author Author { get; set; }
		public bool Expired { get; set; }
		public IEnumerable<Hub> Hubs { get; set; }
		public IEnumerable<FeedItem> Items { get; set; }

		public static JsonFeed Parse(string jsonString)
		{
			if (string.IsNullOrWhiteSpace(jsonString))
			{
				throw new ArgumentNullException(nameof(jsonString));
			}

			var json = SimpleJson.DeserializeObject(jsonString) as IDictionary<string, object>;

			return Parse(json);
		}

		public static JsonFeed Parse(IDictionary<string, object> json)
		{
			if (json == null)
			{
				return null;
			}

			var version = json.GetValue<string>("version");
			var title = json.GetValue<string>("title");

			if (string.IsNullOrWhiteSpace(version) || version != Extensions.CurrentJsonFeedVersion)
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

			var feed = new JsonFeed
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

		public static string Serialize(JsonFeed feed)
		{
			if (string.IsNullOrWhiteSpace(feed.Version) || feed.Version != Extensions.CurrentJsonFeedVersion)
			{
				throw new ArgumentException("Invalid version");
			}

			if (string.IsNullOrWhiteSpace(feed.Title))
			{
				throw new ArgumentException("Invalid title");
			}

			if (feed.Items == null || !feed.Items.Any())
			{
				throw new ArgumentException("No items found");
			}

			return SimpleJson.SerializeObject(feed);
		}
	}
}