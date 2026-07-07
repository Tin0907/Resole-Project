using ASM.Constants;
using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASM.Controllers
{
    public class AccountController : Controller
    {
        private readonly INguoidungSvc _nguoidungSvc;
        private readonly IKhachhangSvc _khachhangSvc;
        private readonly ILogger<AccountController> _logger;

        public AccountController(INguoidungSvc nguoidungSvc, IKhachhangSvc khachhangSvc, ILogger<AccountController> logger)
        {
            _nguoidungSvc = nguoidungSvc;
            _khachhangSvc = khachhangSvc;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKey.NguoiDung.Username)))
            {
                return RedirectToAction("Index", "Admin");
            }

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKey.KhachHang.KH_Email)))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new ViewLogin { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(ViewLogin model)
        {
            _logger.LogInformation("Unified login attempt for email: {Email}", model.Email);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Try admin account first
            var admin = _nguoidungSvc.Login(model);
            if (admin != null)
            {
                _logger.LogInformation("Admin login successful for user {User}", admin.User);

                ClearCustomerSession();
                HttpContext.Session.SetString(SessionKey.NguoiDung.Username, admin.User);
                HttpContext.Session.SetString(SessionKey.NguoiDung.FullName, admin.HoTen);
                HttpContext.Session.SetString(SessionKey.NguoiDung.NguoidungContext, JsonConvert.SerializeObject(admin));

                return RedirectToAction("Index", "Admin");
            }

            // Try customer account
            var customer = _khachhangSvc.Login(model);
            if (customer != null)
            {
                _logger.LogInformation("Customer login successful for email {Email}", customer.Email);

                ClearAdminSession();
                HttpContext.Session.SetString(SessionKey.KhachHang.KH_Email, customer.Email);
                HttpContext.Session.SetString(SessionKey.KhachHang.KH_FullName, customer.Fullname);
                HttpContext.Session.SetString(SessionKey.KhachHang.KH_Id, customer.Id.ToString());
                HttpContext.Session.SetString(SessionKey.KhachHang.KH_Context, JsonConvert.SerializeObject(customer));

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("UserHome", "Home");
            }

            ModelState.AddModelError(string.Empty, "Email ho?c m?t kh?u không chính xác.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        private void ClearAdminSession()
        {
            HttpContext.Session.Remove(SessionKey.NguoiDung.Username);
            HttpContext.Session.Remove(SessionKey.NguoiDung.FullName);
            HttpContext.Session.Remove(SessionKey.NguoiDung.NguoidungContext);
        }

        private void ClearCustomerSession()
        {
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_Email);
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_FullName);
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_Id);
            HttpContext.Session.Remove(SessionKey.KhachHang.KH_Context);
        }
    }
}
