using System.Collections.Generic;

namespace JsonFeed
{
	public class Feed
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
	}
}