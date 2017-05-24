using System.Collections.Generic;
using Xunit;

namespace JsonFeed.Tests
{
	public class AuthorTests
	{
		[Fact]
		public void ParseEmptyJson()
		{
			var json = new Dictionary<string, object>();
			var author = new Author(json);

			Assert.Equal(null, author.Name);
			Assert.Equal(null, author.Url);
			Assert.Equal(null, author.Avatar);

		}

		[Fact]
		public void ParseJson()
		{
			var json = new Dictionary<string, object>();
			json.Add("name", "JSON Feed");
			json.Add("url", "https://jsonfeed.org");
			json.Add("avatar", "https://jsonfeed.org/graphics/icon.png");

			var author = new Author(json);

			Assert.Equal("JSON Feed", author.Name);
			Assert.Equal("https://jsonfeed.org", author.Url);
			Assert.Equal("https://jsonfeed.org/graphics/icon.png", author.Avatar);
		}
	}
}