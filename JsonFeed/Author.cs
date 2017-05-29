using System.Collections.Generic;

namespace JsonFeed
{
	public class Author
	{
		/// <summary>
		/// The author's name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The URL of a site owned by the author
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// The URL for an image for the author
		/// </summary>
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