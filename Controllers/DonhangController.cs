using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace ASM.Controllers
{
    public class DonhangController : Controller
    {
        private IDonHangSvc _donHangSvc;

        public DonhangController(IDonHangSvc donHangSvc)
        {
            _donHangSvc = donHangSvc;
        }

        public IActionResult Index()
        {
            return View(_donHangSvc.GetDonHangAll());
        }

        public IActionResult Details(int id)
        {
            return View(_donHangSvc.GetDonHangById(id));
        }

        public ActionResult Edit(int id)
        {
            var donHang = _donHangSvc.GetDonHangById(id);
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DonHang donHang) {
            try { 
                donHang.KhachHang = null;
                _donHangSvc.EditDonHang(id, donHang);
                return RedirectToAction(nameof(Details), new { id = donHang.Id });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            try
            {
                var (success, message) = _donHangSvc.ApproveDonHang(id);
                if (success)
                    return Json(new { success = true, message = message });

                return Json(new { success = false, message = message });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred" });
            }
        }

    }
}
