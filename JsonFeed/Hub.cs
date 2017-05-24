using System.Collections.Generic;

namespace JsonFeed
{
	public class Hub
	{
		public string Type { get; set; }
		public string Url { get; set; }

		public Hub()
		{
		}

		public Hub(IDictionary<string, object> json)
		{
			if (json == null)
			{
				return;
			}

			Type = json.GetValue<string>("type");
			Url = json.GetValue<string>("url");
		}
	}
}