using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonFeed
{
	public class FeedItem
	{
		/// <summary>
		/// Unique ID for the item
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The URL of the resource described by the item
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// The URL of a page elsewhere
		/// </summary>
		public string ExternalUrl { get; set; }

		/// <summary>
		/// The title of the item
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The HTML of the item
		/// </summary>
		public string ContentHtml { get; set; }
		
		/// <summary>
		/// The plain text of the item
		/// </summary>
		public string ContentText { get; set; }

		/// <summary>
		/// A plain text sentence or two describing the item
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// The URL of the main image for the item
		/// </summary>
		public string Image { get; set; }

		/// <summary>
		/// The URL of an image to use as a banner
		/// </summary>
		public string BannerImage { get; set; }

		/// <summary>
		/// Date the item was published
		/// </summary>
		public DateTime? DatePublished { get; set; }

		/// <summary>
		/// Date the item was modified
		/// </summary>
		public DateTime? DateModified { get; set; }

		/// <summary>
		/// The author of the item
		/// </summary>
		public Author Author { get; set; }

		/// <summary>
		/// Tags for the item
		/// </summary>
		public IEnumerable<string> Tags { get; set; }

		/// <summary>
		/// Attachments for the item
		/// </summary>
		public IEnumerable<Attachment> Attachments { get; set; }

		public FeedItem()
		{
		}

		public FeedItem(IDictionary<string, object> json, bool strictParsing = true)
		{
			if (json == null)
			{
				return;
			}

			var id = json.GetValue<string>("id");
			var contentHtml = json.GetValue<string>("content_html");
			var contentText = json.GetValue<string>("content_text");

			if (strictParsing && string.IsNullOrWhiteSpace(id))
			{
				return;
			}

			if (strictParsing && (string.IsNullOrWhiteSpace(contentHtml) && string.IsNullOrWhiteSpace(contentText)))
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

			if (strictParsing)
			{
				Attachments = attachmentsJson
					.Select(s => new Attachment(s, strictParsing))
					.Where(w => w.Url != null && w.MimeType != null);
			}
			else
			{
				Attachments = attachmentsJson
					.Select(s => new Attachment(s, strictParsing));
			}
		}
	}
}