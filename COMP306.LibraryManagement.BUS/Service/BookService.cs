using System;
using COMP306.LibraryManagement.BUS.Model;
using COMP306.LibraryManagement.DAL.Context;
using COMP306.LibraryManagement.DAL.Entity;

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

		public IEnumerable<PieChartModel> ListBooksSubjectsPercentage()
		{
			var data = (from obj in _context.TluSubjects
					select new PieChartModel
					{
						value = obj.Books.Count(),
						name = obj.Name
					}).ToList();

			return data;
		}

		public TftBook GetBestRankedBook()
		{
            var bestRankedBook = _context.TftBooks.OrderByDescending(book => book.Ranking).FirstOrDefault();

            return bestRankedBook;
        }
    }

	public interface IBookService
	{
		IEnumerable<TftBook> List();
		IEnumerable<PieChartModel> ListBooksSubjectsPercentage();
		TftBook GetBestRankedBook();
    }
}

