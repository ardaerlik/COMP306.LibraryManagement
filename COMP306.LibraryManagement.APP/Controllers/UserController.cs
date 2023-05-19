using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMP306.LibraryManagement.BUS.Service;
using COMP306.LibraryManagement.COM.Model;
using Microsoft.AspNetCore.Mvc;

namespace COMP306.LibraryManagement.APP.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            return Json(await _userService.Register(model));
        }

        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            return Json(await _userService.Login(model));
        }
    }
}

