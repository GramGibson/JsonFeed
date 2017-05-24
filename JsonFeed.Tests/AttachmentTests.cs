using System;
using System.Collections.Generic;
using Xunit;

namespace JsonFeed.Tests
{
	public class AttachmentTests
	{
		[Fact]
		public void ParseJson()
		{
			var json = new Dictionary<string, object>
			{
				{ "url", "https://jsonfeed.org/audio.mp3" },
				{ "mime_type", "audio/mpeg" },
				{ "title", "Podcast Title" },
				{ "size_in_bytes", "28817" },
				{ "duration_in_seconds", "38" }
			};
			var attachment = new Attachment(json);

			Assert.Equal("https://jsonfeed.org/audio.mp3", attachment.Url);
			Assert.Equal("audio/mpeg", attachment.MimeType);
			Assert.Equal("Podcast Title", attachment.Title);
			Assert.Equal(28817, attachment.SizeInBytes);
			Assert.Equal(38, attachment.DurationInSeconds);
		}
	}
}