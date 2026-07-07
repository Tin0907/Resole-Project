using ASM.Constants;
using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ASM.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ASM.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private INguoidungSvc _nguoidungSvc;
        private IMahoaHelper _mahoaHelper;
        private readonly ILogger<AdminController> _logger;
        private readonly DataContext _db;

        public AdminController(IWebHostEnvironment webHostEnviroment, INguoidungSvc nguoidungSvc, IMahoaHelper mahoaHelper, ILogger<AdminController> logger, DataContext db)
        {
            _env = webHostEnviroment;
            _nguoidungSvc = nguoidungSvc;
            _mahoaHelper = mahoaHelper;
            _logger = logger;
            _db = db;
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKey.NguoiDung.Username);
            HttpContext.Session.Remove(SessionKey.NguoiDung.FullName);
            HttpContext.Session.Remove(SessionKey.NguoiDung.NguoidungContext);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Index()
        {
            return View();
        }

        // API: realtime dashboard data
        [HttpGet]
        [Route("api/admin/dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            // Load orders and shoes from database
            var orders = await _db.DonHangs.ToListAsync();

            // Count by relevant statuses (exclude pending/packing etc.)
            // Treat both DaGiao and DaDanhGia as delivered for dashboard counts
            var delivered = orders.Count(o => o.TrangThai == TrangThaiDonHang.DaGiao || o.TrangThai == TrangThaiDonHang.DaDanhGia);
            var cancelled = orders.Count(o => o.TrangThai == TrangThaiDonHang.DaHuy);
            var shipping = orders.Count(o => o.TrangThai == TrangThaiDonHang.ChoGiaoHang);

            // Only consider delivered and reviewed orders for revenue
            var revenueOrders = orders.Where(o => o.TrangThai == TrangThaiDonHang.DaGiao || o.TrangThai == TrangThaiDonHang.DaDanhGia).ToList();

            // Revenue grouped by year and month (only from delivered/reviewed)
            var revenueByYear = new Dictionary<string, double[]>();
            var groupedByYear = revenueOrders.GroupBy(o => o.NgayDatHang.Year);
            foreach (var g in groupedByYear)
            {
                var year = g.Key.ToString();
                var months = new double[12];
                for (int m = 1; m <= 12; m++)
                {
                    months[m - 1] = g.Where(o => o.NgayDatHang.Month == m).Sum(o => o.TongTien);
                }
                revenueByYear[year] = months;
            }

            // Inventory total (sum quantity)
            var inventoryTotal = await _db.Giays.SumAsync(g => (int?)g.Quantity) ?? 0;

            // Purchased counts for current year/month (based on delivered/reviewed orders)
            var now = DateTime.Now;
            var purchasedThisYear = revenueOrders.Count(o => o.NgayDatHang.Year == now.Year);
            var purchasedThisMonth = revenueOrders.Count(o => o.NgayDatHang.Year == now.Year && o.NgayDatHang.Month == now.Month);

            var result = new
            {
                orderStatus = new { delivered, cancelled, shipping },
                revenueByYear,
                inventory = new { totalQuantity = inventoryTotal },
                purchasedThisYear,
                purchasedThisMonth
            };

            return Ok(result);
        }
    }
}
