using System;
using System.Globalization;
using COMP306.LibraryManagement.BUS.Model;
using COMP306.LibraryManagement.COM.Enum;
using COMP306.LibraryManagement.DAL.Context;
using COMP306.LibraryManagement.DAL.Entity;
using Google.Protobuf;

namespace COMP306.LibraryManagement.BUS.Service
{
	public class BookService : IBookService
	{
		private readonly ApplicationContext _context;
        private readonly IUserService _userService;

		public BookService(ApplicationContext context, IUserService userService)
		{
			_context = context;
            _userService = userService;
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

        public IEnumerable<TftBook> BookList()
        {
            
            var data = (from obj in _context.TftBooks orderby obj.Rating descending select obj).Take(20).ToList();
            return data;

        }
        public IEnumerable<TftBook> BookListTry(SearchBookModel model)
        {

            var keyword = model.keyword;
            var contents = model.content.Select(int.Parse).ToList();
            var subjects = model.subject.Select(int.Parse).ToList();
            var authors = model.author.Select(int.Parse).ToList();
            var languages = model.language.Select(int.Parse).ToList();

            var data = (from obj in _context.TluContents
                        where contents.Contains(obj.Id)
                        from book in obj.Books
                        select book).Distinct().ToList();

            data = data.Union(_context.TluSubjects.Where(s => subjects.Contains(s.Id)).SelectMany(s => s.Books)).ToList();
            data = data.Union(_context.TftAuthors.Where(a => authors.Contains(a.Id)).SelectMany(a => a.Books)).ToList();
            data = data.Union(_context.TluLanguages.Where(l => languages.Contains(l.Id)).SelectMany(l => l.TftBooks)).ToList();
            data = data.Union(_context.TftBooks.Where(k => keyword.Contains(k.Title))).ToList();

            try
            {
                var startDate = DateTime.ParseExact(model.startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(model.endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                data = data.Union(_context.TftBooks.Where(b => b.PublicationDate >= startDate && b.PublicationDate <= endDate)).ToList();
            }
            catch (Exception ex)
            {
                
            }

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
        IEnumerable<TftBook> BookListTry(SearchBookModel model);
        IEnumerable<TluContent> ContentsList();
        IEnumerable<TluSubject> SubjectsList();
        IEnumerable<TluLanguage> LanguagesList();
        IEnumerable<TftAuthor> AuthorsList();
    }
}

