using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using COMP306.LibraryManagement.BUS.Service;
using Microsoft.AspNetCore.Mvc;

namespace COMP306.LibraryManagement.APP.Controllers
{
    public class CollectionsController : Controller
    {

        private readonly ILogger<CollectionsController> _logger;
        private readonly IBookService _bookService;

        public CollectionsController(ILogger<CollectionsController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BookList()
        {
            return Json(_bookService.BookList());
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

