using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using COMP306.LibraryManagement.BUS.Service;
using COMP306.LibraryManagement.COM.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace COMP306.LibraryManagement.APP.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

        public RegisterController(IUserService userService
            , IAuthenticationSchemeProvider authenticationSchemeProvider)
        {
            _userService = userService;
            _authenticationSchemeProvider = authenticationSchemeProvider;
        }

        public IActionResult Index()
        {
            if (HttpContext?.User?.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            var result = await _userService.Register(model);
            if (!result.HasError)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.PrimarySid, result.Data?.ToString() ?? ""),
                };
                var scheme = await _authenticationSchemeProvider.GetDefaultAuthenticateSchemeAsync();
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, scheme?.Name));
                await HttpContext.SignInAsync(scheme?.Name, principal);
                await HttpContext.AuthenticateAsync(scheme?.Name);
            }
            result.Data = new();
            return Json(result);
        }
    }
}

