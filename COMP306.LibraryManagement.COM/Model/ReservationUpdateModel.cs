using System;
namespace COMP306.LibraryManagement.COM.Model
{
	public class ReservationUpdateModel
	{
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string resource { get; set; }
        public string email { get; set; }
        public string reservationId { get; set; }
    }
}

