using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COMP306.LibraryManagement.APP.Models;
using COMP306.LibraryManagement.BUS.Service;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using COMP306.LibraryManagement.DAL.Entity;
using Microsoft.AspNetCore.Identity;

namespace COMP306.LibraryManagement.APP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IBookService bookService
        , IUserService userService
        , IAuthenticationSchemeProvider authenticationSchemeProvider)
    {
        _logger = logger;
        _bookService = bookService;
        _userService = userService;
    }

    public ActionResult Index()
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

