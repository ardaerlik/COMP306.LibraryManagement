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

        public int GetRoomReservationCount()
        {
            var currentDate = DateTime.Now;
            var data = _context.TftLocationreservations.Count(res => res.ReservationStartDate.Day == currentDate.Day);

			return data;
		}

		public double GetRoomReservationRate()
		{
			var currentDate = DateTime.Now;
			var yesterday = currentDate.AddDays(-1);

			var reservationsToday = _context.TftLocationreservations.Count(res => res.ReservationStartDate.Day == currentDate.Day);
			var reservationsYesterday = _context.TftLocationreservations.Count(res => res.ReservationStartDate.Day == yesterday.Day);

            if (reservationsYesterday == 0)
            {
                return reservationsToday > 0 ? 1.0 : 0.0;
            }

			return (double) (reservationsToday - reservationsYesterday) / reservationsYesterday;
        }

    }


    public interface IBookService
	{
		IEnumerable<TftBook> List();
		IEnumerable<PieChartModel> ListBooksSubjectsPercentage();
		int GetRoomReservationCount();
		double GetRoomReservationRate();
    }
}

