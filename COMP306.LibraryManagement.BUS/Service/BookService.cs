using System;
using System.Globalization;
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

        public TftBook GetBestRankedBook()
        {
            var bestRankedBook = _context.TftBooks.OrderByDescending(t => t.Rating).FirstOrDefault();

            return bestRankedBook ?? new();
        }

        public IEnumerable<TftBook> ListNewComerBooks()
        {
            var data = _context.TftBooks
                               .OrderByDescending(book => book.AddedDate)
                               .Take(6)
                               .ToList();
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
                            status = obj.ReservationStatus
                        }).Take(25).ToList();

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

            return (double)(reservationsToday - reservationsYesterday) / reservationsYesterday;
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


        public IEnumerable<RoomReportModel> ListRoomReports()
        {
            var currentDate = DateTime.Now;
            var reservations = _context.TftLocationreservations.Where(r => r.CreatedDate.Day == currentDate.Day).ToList();
            var locations = _context.TftLocations.ToList();

            var groupedReservations = reservations
                .GroupBy(r => new { roomId = r.LocationId});

            List<RoomReportModel> roomReports = new List<RoomReportModel>();

            foreach (var group in groupedReservations)
            {
                var roomReport = new RoomReportModel();
                roomReport.roomName = locations[group.Key.roomId-1].Name;
                roomReport.roomId = group.Key.roomId;
                roomReport.hourlyRoomResList = new List<int>(new int[24]);

                foreach (var reservation in group)
                {
                    roomReport.hourlyRoomResList[reservation.CreatedDate.Hour]++;
                }

                roomReports.Add(roomReport);
            }

            return roomReports;
        }

    }

    public interface IBookService
    {
        IEnumerable<TftBook> List();
        IEnumerable<PieChartModel> ListBooksSubjectsPercentage();
        TftBook GetBestRankedBook();
        IEnumerable<TftBook> ListNewComerBooks();
        IEnumerable<RecentRoomModel> GetRecentReservations();
        int GetTotalUsers();
        double GetUserIncreaseRate();
        int GetRoomReservationCount();
        double GetRoomReservationRate();
        IEnumerable<TftBook> BookList();
        IEnumerable<TftBook> BookListTry(SearchBookModel model);
        IEnumerable<TluContent> ContentsList();
        IEnumerable<TluSubject> SubjectsList();
        IEnumerable<TluLanguage> LanguagesList();
        IEnumerable<TftAuthor> AuthorsList();
        IEnumerable<RoomReportModel> ListRoomReports();
    }
}
