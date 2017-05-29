using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JsonFeed
{
	public class JsonFeed
	{
		/// <summary>
		/// The URL of the version the feed uses
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		///  The name of the feed
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The URL of the resource that the feed describes
		/// </summary>
		public string HomePageUrl { get; set; }

		/// <summary>
		/// The URL of the feed, and serves as the unique identifier for the feed
		/// </summary>
		public string FeedUrl { get; set; }

		/// <summary>
		/// Provides more detail, beyond the title, on what the feed is about
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Description of the purpose of the feed
		/// </summary>
		public string UserComment { get; set; }

		/// <summary>
		/// The URL of a feed that provides the next n items, where n is determined by the publisher
		/// </summary>
		public string NextUrl { get; set; }

		/// <summary>
		/// The URL of an image for the feed suitable to be used in a timeline, much the way an avatar might be used
		/// </summary>
		public string Icon { get; set; }

		/// <summary>
		///  The URL of an image for the feed suitable to be used in a source list
		/// </summary>
		public string Favicon { get; set; }

		/// <summary>
		/// Specifies the feed author
		/// </summary>
		public Author Author { get; set; }

		/// <summary>
		/// Says whether or not the feed is finished — that is, whether or not it will ever update again
		/// </summary>
		public bool Expired { get; set; }

		/// <summary>
		/// Describes endpoints that can be used to subscribe to real-time notifications from the publisher of this feed
		/// </summary>
		public IEnumerable<Hub> Hubs { get; set; }

		/// <summary>
		/// Individual feed items
		/// </summary>
		public IEnumerable<FeedItem> Items { get; set; }

		/// <summary>
		/// Load a <c>JsonFeed</c> by URL
		/// </summary>
		/// <param name="url">URL of the feed</param>
		/// <param name="preRequest">Optional, allows modification of the <c>HttpRequestMessage</c> before the request has been made — can be used to append cookies, specify different HTTP methods, etc.</param>
		/// <returns>An instance of <c>JsonFeed</c>, null if the request failed or throws if parsing failed</returns>
		public static async Task<JsonFeed> Load(string url, Action<HttpRequestMessage> preRequest = null)
		{
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
			preRequest?.Invoke(httpRequestMessage);

			var client = new HttpClient();
			var response = await client.SendAsync(httpRequestMessage);

			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			return Parse(await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Parse a <c>JsonFeed</c> from the specified string
		/// </summary>
		/// <param name="jsonString">The string to parse</param>
		/// <param name="strictParsing">Optional, defaults to true, specifies whether to enforce rules laid out in the spec when attempting to parse the feed</param>
		/// <returns></returns>
		public static JsonFeed Parse(string jsonString, bool strictParsing = true)
		{
			if (string.IsNullOrWhiteSpace(jsonString))
			{
				throw new ArgumentNullException(nameof(jsonString));
			}

			var json = SimpleJson.DeserializeObject(jsonString) as IDictionary<string, object>;

			return Parse(json, strictParsing);
		}

		/// <summary>
		/// Parse a <c>JsonFeed</c> from the specified dictionary
		/// </summary>
		/// <param name="json">The dictionary to parse</param>
		/// <param name="strictParsing">Optional, defaults to true, specifies whether to enforce rules laid out in the spec when attempting to parse the feed</param>
		/// <returns></returns>
		public static JsonFeed Parse(IDictionary<string, object> json, bool strictParsing = true)
		{
			if (json == null)
			{
				return null;
			}

			var version = json.GetValue<string>("version");
			var title = json.GetValue<string>("title");

			if (strictParsing && (string.IsNullOrWhiteSpace(version) || version != Extensions.CurrentJsonFeedVersion))
			{
				throw new ArgumentException("Invalid version");
			}

			if (strictParsing && string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("Invalid title");
			}

			if (strictParsing && !json.ContainsKey("items"))
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

				if (strictParsing)
				{
					feed.Hubs = hubsJson
						.Where(w => w["type"] != null && w["url"] != null)
						.Select(s => new Hub(s));
				}
				else
				{
					feed.Hubs = hubsJson.Select(s => new Hub(s));
				}
			}

			var itemsJson = json.GetList<IDictionary<string, object>>("items");

			if (strictParsing)
			{
				feed.Items = itemsJson
					.Where(w => w["id"] != null)
					.Select(s => new FeedItem(s, strictParsing));
			}
			else
			{
				feed.Items = itemsJson.Select(s => new FeedItem(s));
			}

			return feed;
		}

		/// <summary>
		/// Serialize an instance of <c>JsonFeed</c> into a string
		/// </summary>
		/// <param name="feed">The <c>JsonFeed</c> to serialize</param>
		/// <returns>A string representing the serialized <c>JsonFeed</c></returns>
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