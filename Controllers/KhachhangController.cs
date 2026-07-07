using ASM.Constants;
using ASM.Models;
using ASM.Models.ViewModels;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASM.Controllers
{
    public class KhachhangController : Controller
    {
        private IKhachhangSvc _khachHangSvc;
        private IDonHangSvc _donHangSvc;
        private ILogger<KhachhangController> _logger;
        private DataContext _context;

        public KhachhangController(IKhachhangSvc khachHangSvc, IDonHangSvc donHangSvc, ILogger<KhachhangController> logger, DataContext context)
        {
            _khachHangSvc = khachHangSvc;
            _donHangSvc = donHangSvc;
            _logger = logger;
            _context = context;
        }

        // Trang đăng nhập khách hàng
        public IActionResult Login(string returnUrl)
        {
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(ViewLogin viewLogin)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = viewLogin.ReturnUrl });
        }

        // Đăng xuất khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_Email);
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_FullName);
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_Id);
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_Context);
            return RedirectToAction(nameof(Login));
        }

        // Trang đăng ký khách hàng
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(KhachHang khachHang)
        {
            _logger.LogInformation($"Registration attempt for email: {khachHang.Email}");

            if (ModelState.IsValid)
            {
                try
                {
                    int result = _khachHangSvc.AddKhachhang(khachHang);

                    if (result > 0)
                    {
                        _logger.LogInformation($"Registration successful! Customer ID: {result}");
                        TempData["SuccessMessage"] = "Registration successful! Please login.";
                        return RedirectToAction(nameof(Login));
                    }
                    else if (result == -1)
                    {
                        ModelState.AddModelError(nameof(KhachHang.Email), "This email is already in use. Please choose another email.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Registration failed due to system error. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Registration exception: {ex.Message}");
                    _logger.LogError($"Inner exception: {ex.InnerException?.Message}");
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid");
            }

            return View(khachHang);
        }

        // Quản lý khách hàng (chỉ dành cho Admin)
        public IActionResult Index()
        {
            return View(_khachHangSvc.GetKhachhangAll());
        }

        public IActionResult Details(int id)
        {
            return View(_khachHangSvc.GetKhachhang(id));
        }

        // Hồ sơ khách hàng (self-service)
        public IActionResult Profile()
        {
            var idStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(idStr))
            {
                var returnUrl = Url.Action("Profile", "Khachhang");
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            int id = int.Parse(idStr);
            var customer = _khachHangSvc.GetKhachhang(id);
            if (customer == null) return RedirectToAction("Login", "Account");

            var vm = new KhachHangProfileViewModel
            {
                Id = customer.Id,
                Fullname = customer.Fullname,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                NgaySinh = customer.NgaySinh,
                ChangePassword = new ChangePasswordViewModel(),
                Orders = _donHangSvc.GetDonHangByKhachHang(id)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(KhachHangProfileViewModel model)
        {
            var idStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(idStr))
            {
                var returnUrl = Url.Action("Profile", "Khachhang");
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            int id = int.Parse(idStr);
            var result = _khachHangSvc.ChangePassword(id, model.ChangePassword.CurrentPassword, model.ChangePassword.NewPassword);
            if (result == -1)
            {
                ModelState.AddModelError("ChangePassword.CurrentPassword", "Current password is incorrect");
                return View("Profile", model);
            }
            if (result == 0)
            {
                ModelState.AddModelError(string.Empty, "Password change failed. Please try again.");
                return View("Profile", model);
            }

            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitReview(int donHangId, int rating, string comment)
        {
            var idStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(idStr))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            int khId = int.Parse(idStr);

            try
            {
                var donHang = _donHangSvc.GetDonHangById(donHangId);
                if (donHang == null || donHang.KhachHangId != khId)
                {
                    return Json(new { success = false, message = "Order not found" });
                }

                if (donHang.TrangThai != TrangThaiDonHang.DaGiao)
                {
                    return Json(new { success = false, message = "This order cannot be reviewed" });
                }

                var review = new ShopReview
                {
                    DonHangId = donHangId,
                    KhachHangId = khId,
                    Rating = rating,
                    Comment = comment ?? "",
                    CreatedAt = DateTime.Now
                };

                _context.ShopReviews.Add(review);

                donHang.TrangThai = TrangThaiDonHang.DaDanhGia;
                _context.SaveChanges();

                return Json(new { success = true, message = "Thank you for your review!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
