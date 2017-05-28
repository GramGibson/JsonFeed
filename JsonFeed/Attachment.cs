using System.Collections.Generic;

namespace JsonFeed
{
	public class Attachment
	{
		public string Url { get; set; }
		public string MimeType { get; set; }
		public string Title { get; set; }
		public int? SizeInBytes { get; set; }
		public int? DurationInSeconds { get; set; }

		public Attachment()
		{
		}

		public Attachment(IDictionary<string, object> json, bool strictParsing = true)
		{
			if (json == null)
			{
				return;
			}

			Url = json.GetValue<string>("url");

			if (strictParsing && string.IsNullOrWhiteSpace(Url))
			{
				return;
			}

			MimeType = json.GetValue<string>("mime_type");

			if (strictParsing && string.IsNullOrWhiteSpace(MimeType))
			{
				return;
			}

			Title = json.GetValue<string>("title");

			if (int.TryParse(json.GetValue<object>("size_in_bytes")?.ToString(), out int sizeInBytes))
			{
				SizeInBytes = sizeInBytes;
			}

			if (int.TryParse(json.GetValue<object>("duration_in_seconds")?.ToString(), out int durationInSeconds))
			{
				DurationInSeconds = durationInSeconds;
			}
		}
	}
}