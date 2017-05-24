using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonFeed
{
	public class FeedItem
	{
		public string Id { get; set; }
		public string Url { get; set; }
		public string ExternalUrl { get; set; }
		public string Title { get; set; }
		public string ContentHtml { get; set; }
		public string ContentText { get; set; }
		public string Summary { get; set; }
		public string Image { get; set; }
		public string BannerImage { get; set; }
		public DateTime? DatePublished { get; set; }
		public DateTime? DateModified { get; set; }
		public Author Author { get; set; }
		public IEnumerable<string> Tags { get; set; }
		public IEnumerable<Attachment> Attachments { get; set; }

		public FeedItem()
		{
		}

		public FeedItem(IDictionary<string, object> json)
		{
			if (json == null)
			{
				return;
			}

			var id = json.GetValue<string>("id");
			var contentHtml = json.GetValue<string>("content_html");
			var contentText = json.GetValue<string>("content_text");

			if (string.IsNullOrWhiteSpace(id))
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(contentHtml) && string.IsNullOrWhiteSpace(contentText))
			{
				return;
			}

			Id = id;
			Url = json.GetValue<string>("url");
			ExternalUrl = json.GetValue<string>("external_url");
			Title = json.GetValue<string>("title");
			ContentHtml = contentHtml;
			ContentText = contentText;
			Summary = json.GetValue<string>("summary");
			Image = json.GetValue<string>("image");
			BannerImage = json.GetValue<string>("banner_image");
			Tags = json.GetValue<IEnumerable<string>>("tags");

			if (json.ContainsKey("author"))
			{
				Author = new Author(json.GetValue<IDictionary<string, object>>("author"));
			}

			DateTime.TryParse(json.GetValue<string>("date_published"), out DateTime published);
			DateTime.TryParse(json.GetValue<string>("date_modified"), out DateTime modified);

			DatePublished = (published == DateTime.MinValue) ? DateTime.Now : published;
			DateModified = (modified == DateTime.MinValue) ? DateTime.Now : modified;

			var attachmentsJson = json.GetList<IDictionary<string, object>>("attachments");

			Attachments = attachmentsJson
				.Select(s => new Attachment(s))
				.Where(w => w.Url != null && w.MimeType != null);
		}
	}
}