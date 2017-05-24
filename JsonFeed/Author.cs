using System.Collections.Generic;

namespace JsonFeed
{
	public class Author
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Avatar { get; set; }

		public Author()
		{
		}

		public Author(IDictionary<string, object> json)
		{
			if (json == null)
			{
				return;
			}

			Name = json.GetValue<string>("name");
			Url = json.GetValue<string>("url");
			Avatar = json.GetValue<string>("avatar");
		}
	}
}