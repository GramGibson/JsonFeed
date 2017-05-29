using System.Collections.Generic;

namespace JsonFeed
{
	public class Attachment
	{
		/// <summary>
		/// Specifies the location of the attachment
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Specifies the type of the attachment
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// Name for the attachment
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Specifies how large the file is
		/// </summary>
		public int? SizeInBytes { get; set; }

		/// <summary>
		/// Specifies how long it takes to listen to or watch, when played at normal speed
		/// </summary>
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