using System;
namespace COMP306.LibraryManagement.BUS.Model
{
	public class SearchBookModel
	{
        public string keyword { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> content { get; set; }
        public List<string> subject { get; set; }
        public List<string> author { get; set; }
        public List<string> language { get; set; }
    }
}

