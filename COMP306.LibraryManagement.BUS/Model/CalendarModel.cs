using System;
namespace COMP306.LibraryManagement.BUS.Model
{
	public class CalendarModel
	{
		public List<CalendarResource>? resources { get; set; }
		public List<Event>? events { get; set; }
	}

	public class CalendarResource
	{
		public string? name { get; set; }
		public string? id { get; set; }
		public bool expanded { get; set; }
		public List<Location>? children { get; set; }
	}

	public class Location
	{
		public string? name { get; set; }
		public string? id { get; set; }
	}

	public class Event
	{
		public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string? id { get; set; }
        public string? resource { get; set; }
        public string? text { get; set; }
        public string? bubbleHtml { get; set; }
    }
}

