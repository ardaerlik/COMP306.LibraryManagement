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
        public IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IActionResult Index()
        {
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

