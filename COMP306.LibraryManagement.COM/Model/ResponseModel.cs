using System;
namespace COMP306.LibraryManagement.COM.Model
{
	public class ResponseModel
	{
		public object? Data { get; set; }
		public bool HasError { get; set; }
		public string? ExceptionMessage { get; set; }
	}
}

