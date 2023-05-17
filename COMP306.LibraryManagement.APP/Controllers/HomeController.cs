using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COMP306.LibraryManagement.APP.Models;
using COMP306.LibraryManagement.BUS.Service;

namespace COMP306.LibraryManagement.APP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBookService _bookService;

    public HomeController(ILogger<HomeController> logger, IBookService bookService)
    {
        _logger = logger;
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ListBooksSubjectsPercentage()
    {
        return Json(_bookService.ListBooksSubjectsPercentage());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}

