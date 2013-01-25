using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using CsQuery;
namespace RiceAlumni.Theme.Helpers
{
	public class PciEventCalendarHelper
	{
		public static IList<PciEvent> GetEvents()
		{
			try
			{
				var feed = XDocument.Load("https://online.alumni.rice.edu/default.aspx?Page=RSSFeed&RSSFeedID=1");
				var items = feed.Element("rss").Element("channel").Elements("item");

				var events = new List<PciEvent>();
				foreach (var item in items)
				{
					var body = GetStringValue("description", item);
					DateTime startDate;
					DateTime endDate;
					ParseDates(body,out startDate,out endDate);
					var e = new PciEvent
					        {
						        Body = body,
						        StartDate = startDate,
						        EndDate = endDate,
						        Link = GetStringValue("link", item),
						        Title = GetStringValue("title", item)
					        };
					events.Add(e);
				}
				return events.OrderBy(e => e.StartDate).ToList();
			}
			catch (Exception)
			{
				//TODO: Log this
				return new List<PciEvent>();
			}
		}

		private static string GetStringValue(string elementName, XElement element)
		{
			var value = element.Element(elementName);
			return value==null ? null : value.Value;
		}
		private static void ParseDates(string value, out DateTime startDate, out DateTime endDate)
		{
			try
			{
				var dom = CQ.Create(value);
				var dateBlock = dom["table:first table:first tr:nth-child(2) > td:first"].Html();
				var justDateInfo = dateBlock.Replace("<b>Date:</b>&nbsp;", "");
				var startEnd = justDateInfo.Split(new[] { "to" }, StringSplitOptions.RemoveEmptyEntries);
				DateTime.TryParse(startEnd[0], out startDate);
				if (startEnd.Length > 0)
					DateTime.TryParse(startEnd[1], out endDate);
				else
					endDate = DateTime.MinValue;
			}
			catch (Exception)
			{
				startDate = DateTime.MinValue;
				endDate = DateTime.MinValue;
				//TODO: Log this
			}
		}
	}

	public class PciEvent
	{
		public string Title { get; set; }
		public DateTime StartDate { get; set; }
		public string Body { get; set; }
		public string Link { get; set; }

		public DateTime EndDate { get; set; }
	}

}