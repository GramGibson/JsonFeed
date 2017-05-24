using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonFeed.Tests
{
	public class HubTests
	{
		[Fact]
		public void ParseJson()
		{
			var json = new Dictionary<string, object>
			{
				{ "url", "https://jsonfeed.org/endpoint" },
				{ "type", "WebSub" }
			};
			var hub = new Hub(json);

			Assert.Equal("https://jsonfeed.org/endpoint", hub.Url);
			Assert.Equal("WebSub", hub.Type);
		}
	}
}