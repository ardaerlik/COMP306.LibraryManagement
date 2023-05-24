using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMP306.LibraryManagement.APP.Models;
using COMP306.LibraryManagement.BUS.Service;
using COMP306.LibraryManagement.COM.Model;
using Microsoft.AspNetCore.Mvc;

namespace COMP306.LibraryManagement.APP.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;

        public ReservationController(IReservationService reservationService
            , IUserService userService)
        {
            _reservationService = reservationService;
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

        [HttpPost]
        public async Task<IActionResult> ListAsync([FromBody] ReservationListModel model)
        {
            return Json(await _reservationService.GetAsync(model));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ReservationAddModel model)
        {
            return Json(await _reservationService.AddAsync(model));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync([FromBody] ReservationUpdateModel model)
        {
            return Json(await _reservationService.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync([FromBody] ReservationDeleteModel model)
        {
            return Json(await _reservationService.DeleteAsync(model));
        }
    }
}

