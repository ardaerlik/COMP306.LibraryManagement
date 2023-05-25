using System;
using System.Linq;
using System.Threading.Tasks;
using COMP306.LibraryManagement.BUS.Model;
using COMP306.LibraryManagement.BUS.Service;
using COMP306.LibraryManagement.COM.Model;
using Microsoft.AspNetCore.Mvc;

namespace COMP306.LibraryManagement.APP.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public CollectionsController(IUserService userService,IBookService bookService)
        {
            _userService = userService;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            if (HttpContext?.User?.Identity?.IsAuthenticated ?? false)
            {
                var data = _userService.CreateViewBagModel(Convert.ToInt32(HttpContext.User.Claims.First().Value));
                ViewBag.Email = data.Email;
                ViewBag.FullName = data.FullName;
                ViewBag.RoleName = data.RoleName;
            }

            if (!HttpContext?.User?.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Login");
            else
                return View();
        }

        public IActionResult BookList()
        {
            return Json(_bookService.BookList());
        }
        public IActionResult BookListTry([FromBody] SearchBookModel model)
        {
            return Json(_bookService.BookListTry(model));
        }
        public IActionResult ContentsList()
        {
            return Json(_bookService.ContentsList());
        }

        public IActionResult SubjectsList()
        {
            return Json(_bookService.SubjectsList());
        }

        public IActionResult LanguagesList()
        {
            return Json(_bookService.LanguagesList());
        }

        public IActionResult AuthorsList()
        {
            return Json(_bookService.AuthorsList());
        }
    }
}

