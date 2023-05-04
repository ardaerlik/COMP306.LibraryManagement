using System;
using COMP306.LİbraryManagement.DAL.Context;
using COMP306.LİbraryManagement.DAL.Entity;

namespace COMP306.LibraryManagement.BUS.Service
{
	public class BookService : IBookService
	{
		private readonly ApplicationContext _context;

		public BookService(ApplicationContext context)
		{
			_context = context;
		}

		public IEnumerable<TftBook> List()
		{
			var data = (from obj in _context.TftBooks select obj).ToList();
            return data;
		}
	}

	public interface IBookService
	{
		IEnumerable<TftBook> List();
    }
}

