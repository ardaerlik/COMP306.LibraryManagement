using System;
namespace COMP306.LibraryManagement.COM.Model
{
	public class ReservationAddModel
	{
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string resource { get; set; }
        public string email { get; set; }
    }
}

