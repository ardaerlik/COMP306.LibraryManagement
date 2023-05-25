using System;
namespace COMP306.LibraryManagement.COM.Model
{
	public class RegisterModel
	{
		public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public List<int> roles { get; set; }
    }
}

