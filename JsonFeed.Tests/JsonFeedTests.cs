using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonFeed.Tests
{
	public class JsonFeedTests
	{
		[Fact]
		public void EnsureValidVersion()
		{
			var json = new Dictionary<string, object>
			{
				{ "version", "" },
			};

			var ex = Assert.Throws<ArgumentException>(() =>
			{
				var feed = JsonFeed.Parse(json);
			});

			Assert.Equal("Invalid version", ex.Message);
		}

		[Fact]
		public void EnsureValidTitle()
		{
			var json = new Dictionary<string, object>
			{
				{ "version", Extensions.CurrentJsonFeedVersion },
				{ "title", "" },
			};

			var ex = Assert.Throws<ArgumentException>(() =>
			{
				var feed = JsonFeed.Parse(json);
			});

			Assert.Equal("Invalid title", ex.Message);
		}

		[Fact]
		public void EnsureValidItems()
		{
			var json = new Dictionary<string, object>
			{
				{ "version", Extensions.CurrentJsonFeedVersion },
				{ "title", "Feed Title" },
			};

			var ex = Assert.Throws<ArgumentException>(() =>
			{
				var feed = JsonFeed.Parse(json);
			});

			Assert.Equal("No items found", ex.Message);
		}

		[Fact]
		public void ParseExampleJsonString()
		{
			var file = File.ReadAllText("../../Feeds/jsonfeed.json");
			var feed = JsonFeed.Parse(file);

			Assert.Equal("JSON Feed", feed.Title);
			Assert.Equal("JSON Feed is a pragmatic syndication format for blogs, microblogs, and other time-based content.", feed.Description);
			Assert.Equal("https://jsonfeed.org/", feed.HomePageUrl);
			Assert.Equal("https://jsonfeed.org/feed.json", feed.FeedUrl);
			Assert.Equal("This feed allows you to read the posts from this site in any feed reader that supports the JSON Feed format. To add this feed to your reader, copy the following URL — https://jsonfeed.org/feed.json — and add it your reader.", feed.UserComment);
			Assert.Equal("https://jsonfeed.org/graphics/icon.png", feed.Favicon);
			Assert.Equal("Brent Simmons and Manton Reece", feed.Author.Name);
			Assert.Equal(1, feed.Items.Count());
			Assert.Equal("https://jsonfeed.org/2017/05/17/announcing_json_feed", feed.Items.First().Id);
			Assert.Equal("https://jsonfeed.org/2017/05/17/announcing_json_feed", feed.Items.First().Url);
			Assert.Equal("Announcing JSON Feed", feed.Items.First().Title);
			Assert.Equal("5/17/2017 11:02:12 AM", feed.Items.First().DatePublished.ToString());
		}

		[Fact]
		public void ParseTimetableJsonString()
		{
			var file = File.ReadAllText("../../Feeds/timetable.json");
			var feed = JsonFeed.Parse(file);

			Assert.Equal("Timetable", feed.Title);
			Assert.Equal("http://timetable.manton.org/", feed.HomePageUrl);
			Assert.Equal(5, feed.Items.Count());
			Assert.Equal("http://timetable.manton.org/2017/04/episode-45-launch-week/", feed.Items.First().Id);
			Assert.Equal("http://timetable.manton.org/2017/04/episode-45-launch-week/", feed.Items.First().Url);
			Assert.Equal("Episode 45: Launch week", feed.Items.First().Title);
			Assert.Equal(1, feed.Items.First().Attachments.Count());
			Assert.Equal("http://timetable.manton.org/podcast-download/139/episode-45-launch-week.mp3", feed.Items.First().Attachments.First().Url);
			Assert.Equal("audio/mpeg", feed.Items.First().Attachments.First().MimeType);
			Assert.Equal(5236920, feed.Items.First().Attachments.First().SizeInBytes);
			Assert.Equal("4/25/2017 9:09:45 PM", feed.Items.First().DatePublished.ToString());
		}

		[Fact]
		public void InvalidFeedWontSerialize()
		{
			var feed = new JsonFeed();

			var ex = Assert.Throws<ArgumentException>(() =>
			{
				JsonFeed.Serialize(feed);
			});

			Assert.Equal("Invalid version", ex.Message);

			feed.Version = Extensions.CurrentJsonFeedVersion;

			ex = Assert.Throws<ArgumentException>(() =>
			{
				JsonFeed.Serialize(feed);
			});

			Assert.Equal("Invalid title", ex.Message);

			feed.Title = "Feed Title";

			ex = Assert.Throws<ArgumentException>(() =>
			{
				JsonFeed.Serialize(feed);
			});

			Assert.Equal("No items found", ex.Message);
		}

		[Fact]
		public void AllowInvalidJsonWhenSpecifyingNonStrictParsing()
		{
			var file = File.ReadAllText("../../Feeds/invalid.json");

			Assert.Throws<ArgumentException>(() =>
			{
				JsonFeed.Parse(file);
			});

			var feed = JsonFeed.Parse(file, strictParsing: false);

			Assert.Equal(null, feed.Version);
			Assert.Equal(null, feed.Title);
			Assert.Equal(0, feed.Items.Count());
			Assert.Equal("JSON Feed is a pragmatic syndication format for blogs, microblogs, and other time-based content.", feed.Description);
		}
	}
}