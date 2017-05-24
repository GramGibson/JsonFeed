using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JsonFeed.Tests
{
	public class FeedItemTests
	{
		[Fact]
		public void NullForNullId()
		{
			var json = new Dictionary<string, object>
			{
				{ "id", "" },
				{ "title", "Should Be Null" }
			};
			var feedItem = new FeedItem(json);

			Assert.Null(feedItem.Url);
			Assert.Null(feedItem.Title);
		}

		[Fact]
		public void ParseJson()
		{
			var json = new Dictionary<string, object>
			{
				{ "id", "https://jsonfeed.org/2017/05/17/announcing_json_feed" },
				{ "url", "https://jsonfeed.org/2017/05/17/announcing_json_feed" },
				{ "title", "Announcing JSON Feed" },
				{ "content_html", "Content..." },
				{ "date_published", "2017-05-17T08:02:12-07:00" }
			};
			var feedItem = new FeedItem(json);

			Assert.Equal("https://jsonfeed.org/2017/05/17/announcing_json_feed", feedItem.Id);
			Assert.Equal("https://jsonfeed.org/2017/05/17/announcing_json_feed", feedItem.Url);
			Assert.Equal("Announcing JSON Feed", feedItem.Title);
			Assert.Equal("Content...", feedItem.ContentHtml);
			Assert.Equal("5/17/2017 11:02:12 AM", feedItem.DatePublished.ToString());
		}

		[Fact]
		public void ParseJsonObjects()
		{
			var json = new Dictionary<string, object>
			{
				{ "id", "https://jsonfeed.org/2017/05/17/announcing_json_feed" },
				{ "content_text", "Content..." }
			};

			json.Add("author", new Dictionary<string, object>
			{
				{ "name", "Author Name" },
				{ "url", "https://jsonfeed.org" }
			});

			var feedItem = new FeedItem(json);

			Assert.Equal("Author Name", feedItem.Author.Name);
			Assert.Equal("https://jsonfeed.org", feedItem.Author.Url);
		}

		[Fact]
		public void IgnoreInvalidAttachments()
		{
			var json = new Dictionary<string, object>
			{
				{ "id", "https://jsonfeed.org/2017/05/17/announcing_json_feed" },
				{ "content_text", "Content..." }
			};

			json.Add("attachments", new[] {
				new Dictionary <string, object>
				{
					{ "url", "https://jsonfeed.org/attachment.png" }
				}
			});

			var feedItem = new FeedItem(json);

			Assert.Equal(0, feedItem.Attachments.Count());
		}

		[Fact]
		public void ParseValidAttachments()
		{
			var json = new Dictionary<string, object>
			{
				{ "id", "https://jsonfeed.org/2017/05/17/announcing_json_feed" },
				{ "content_text", "Content..." }
			};

			json.Add("attachments", new object[]
			{
				new Dictionary <string, object>
				{
					{ "url", "https://jsonfeed.org/attachment.png" },
					{ "mime_type", "image/png" },
					{ "title", "An Image" },
				}
			});

			var feedItem = new FeedItem(json);

			Assert.Equal(1, feedItem.Attachments.Count());
			Assert.Equal("https://jsonfeed.org/attachment.png", feedItem.Attachments.FirstOrDefault().Url);
			Assert.Equal("image/png", feedItem.Attachments.FirstOrDefault().MimeType);
			Assert.Equal("An Image", feedItem.Attachments.FirstOrDefault().Title);
		}
	}
}
