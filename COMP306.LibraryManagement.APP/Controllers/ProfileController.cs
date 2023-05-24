using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMP306.LibraryManagement.BUS.Service;
using Microsoft.AspNetCore.Mvc;

namespace COMP306.LibraryManagement.APP.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
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
    }
}

