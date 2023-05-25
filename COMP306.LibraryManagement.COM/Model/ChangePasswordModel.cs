using System;
namespace COMP306.LibraryManagement.COM.Model
{
	public class ChangePasswordModel
	{
		public string currentPassword { get; set; }
        public string newPassword { get; set; }
        public string newPasswordAgain { get; set; }
    }
}

