﻿using System;
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


        public int GetTotalUsers()
        {
            return _context.AppUsers.Count();
        }


        public double GetUserIncreaseRate()
        {
            var currentDate = DateTime.Now;
            var startOfCurrentYear = new DateTime(currentDate.Year, 1, 1);
            var startOfLastYear = startOfCurrentYear.AddYears(-1);

            var usersThisYear = _context.AppUsers.Count(user => user.RegisteredDate >= startOfCurrentYear);
            var usersLastYear = _context.AppUsers.Count(user => user.RegisteredDate >= startOfLastYear && user.RegisteredDate < startOfCurrentYear);

            if (usersLastYear == 0)
            {
                return usersThisYear > 0 ? 1.0 : 0.0;
            }

            return (double)(usersThisYear - usersLastYear) / usersLastYear;
        }
    }


	public interface IBookService
	{
		IEnumerable<TftBook> List();
		IEnumerable<PieChartModel> ListBooksSubjectsPercentage();
		int GetTotalUsers();
        double GetUserIncreaseRate();
    }
}

