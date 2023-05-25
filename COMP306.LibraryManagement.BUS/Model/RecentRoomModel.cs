using System;
namespace COMP306.LibraryManagement.BUS.Model
{
	public class RecentRoomModel
	{
		public int Id { get; set; }
        public string userName { get; set; }
        public string roomName { get; set; }
        public DateTime createdTime { get; set; }
        public int status { get; set; }
    }
}

