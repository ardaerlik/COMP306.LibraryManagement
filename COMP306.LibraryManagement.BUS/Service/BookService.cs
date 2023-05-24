﻿using System;
using System.Text.Json.Nodes;
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

        public IEnumerable<TftBook> BookList()
        {
            var data = (from obj in _context.TftBooks select obj).ToList();
            return data;
            
        }
        public IEnumerable<TluContent> ContentsList()
        {
            var data = (from obj in _context.TluContents select obj).ToList();
			return data;
        }
        public IEnumerable<TluSubject> SubjectsList()
        {
            var data = (from obj in _context.TluSubjects select obj).ToList();
            return data;
        }
        public IEnumerable<TluLanguage> LanguagesList()
        {
            var data = (from obj in _context.TluLanguages select obj).ToList();
            return data;
        }
        public IEnumerable<TftAuthor> AuthorsList()
        {
            var data = (from obj in _context.TftAuthors select obj).ToList();
            return data;
        }

    }

	public interface IBookService
	{
		IEnumerable<TftBook> List();
		IEnumerable<PieChartModel> ListBooksSubjectsPercentage();
		IEnumerable<TftBook> BookList();
		IEnumerable<TluContent> ContentsList();
		IEnumerable<TluSubject> SubjectsList();
        IEnumerable<TluLanguage> LanguagesList();
        IEnumerable<TftAuthor> AuthorsList();
    }
}

