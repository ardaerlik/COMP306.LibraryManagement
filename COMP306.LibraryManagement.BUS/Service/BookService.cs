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
						orderby obj.Books.Count() descending
					select new PieChartModel
					{
						value = obj.Books.Count(),
						name = obj.Name
					}).Take(10).ToList();

			return data;
		}

		public IEnumerable<RecentRoomModel> GetRecentReservations()
		{

			var data = (from obj in _context.TftLocationreservations
						orderby obj.CreatedDate descending
						select new RecentRoomModel
						{
							Id = obj.Id,
							userName = obj.User.Name,
							roomName = obj.Location.Name,
							createdTime = obj.CreatedDate,
							status =obj.ReservationStatus
						}).Take(25).ToList();

			return data;
		}
    }

	public interface IBookService
	{
		IEnumerable<TftBook> List();
		IEnumerable<PieChartModel> ListBooksSubjectsPercentage();
		IEnumerable<RecentRoomModel> GetRecentReservations();
    }
}

