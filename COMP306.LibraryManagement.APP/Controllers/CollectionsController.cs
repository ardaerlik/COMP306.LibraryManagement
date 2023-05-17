using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("Hello World from Collection Controller\n\n\n\n");
            return View(_bookService.BookList());
        }
    }
}

