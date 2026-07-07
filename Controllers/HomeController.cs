using System.Diagnostics;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using ASM.Constants;
using ASM.Services;

namespace ASM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGiaySvc _giaySvc;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, IGiaySvc giaySvc, DataContext context)
        {
            _logger = logger;
            _giaySvc = giaySvc;
            _context = context;
        }

        public IActionResult Index()
        {
            string adminUser = HttpContext.Session.GetString(SessionKey.NguoiDung.Username);
            string customerEmail = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Email);

            if (!string.IsNullOrEmpty(adminUser))
            {
                return View();
            }

            // Khách ho?c khách v?ng lai => trang UserHome
            return RedirectToAction("UserHome");
        }

        public IActionResult UserHome()
        {
            // L?y s?n ph?m có phân lo?i "Special Shoe" cho Special Collection
            var specialCollection = _giaySvc.GetByCategory(PhanLoai.SpecialShoe);
            
            // L?y 4 s?n ph?m M?I NH?T KHÔNG ph?i "SpecialShoe" cho ph?n Latest Products
            var allProducts = _giaySvc.GetAll()
                .Where(g => g.PhanLoai != PhanLoai.SpecialShoe)
                .OrderByDescending(g => g.Id)
                .Take(4)
                .ToList();
            
            // T?o ViewModel ð? truy?n c? 2 danh sách
            ViewBag.SpecialCollection = specialCollection;
            ViewBag.AllProducts = allProducts;

            // Current logged-in customer ID (null if guest)
            var khIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            int? currentKhId = !string.IsNullOrEmpty(khIdStr) ? int.Parse(khIdStr) : null;

            // Load shop reviews with reaction counts
            var reviews = _context.ShopReviews
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt,
                    CustomerName = r.KhachHang.Fullname,
                    ProductName = r.DonHang.DonhangChitiets.Select(ct => ct.Giay.Ten).FirstOrDefault(),
                    ProductImage = r.DonHang.DonhangChitiets.Select(ct => ct.Giay.Hinh).FirstOrDefault(),
                    LikeCount = _context.ReviewReactions.Count(rr => rr.ShopReviewId == r.Id && rr.IsLike),
                    DislikeCount = _context.ReviewReactions.Count(rr => rr.ShopReviewId == r.Id && !rr.IsLike),
                    CurrentUserReaction = currentKhId.HasValue
                        ? _context.ReviewReactions
                            .Where(rr => rr.ShopReviewId == r.Id && rr.KhachHangId == currentKhId.Value)
                            .Select(rr => (bool?)rr.IsLike)
                            .FirstOrDefault()
                        : null
                })
                .ToList();
            ViewBag.ShopReviews = reviews;
            ViewBag.IsLoggedIn = currentKhId.HasValue;

            return View();
        }

        [HttpPost]
        public IActionResult ReactReview([FromBody] ReviewReactionRequest request)
        {
            var khIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(khIdStr))
            {
                return Json(new { success = false, message = "Please login to react" });
            }

            int khachHangId = int.Parse(khIdStr);

            var existing = _context.ReviewReactions
                .FirstOrDefault(r => r.ShopReviewId == request.ReviewId && r.KhachHangId == khachHangId);

            if (existing != null)
            {
                if (existing.IsLike == request.IsLike)
                {
                    // Same reaction clicked again ? remove it (toggle off)
                    _context.ReviewReactions.Remove(existing);
                }
                else
                {
                    // Switch reaction
                    existing.IsLike = request.IsLike;
                }
            }
            else
            {
                // New reaction
                _context.ReviewReactions.Add(new ReviewReaction
                {
                    ShopReviewId = request.ReviewId,
                    KhachHangId = khachHangId,
                    IsLike = request.IsLike
                });
            }

            _context.SaveChanges();

            // Return updated counts
            int likeCount = _context.ReviewReactions.Count(r => r.ShopReviewId == request.ReviewId && r.IsLike);
            int dislikeCount = _context.ReviewReactions.Count(r => r.ShopReviewId == request.ReviewId && !r.IsLike);
            var userReaction = _context.ReviewReactions
                .Where(r => r.ShopReviewId == request.ReviewId && r.KhachHangId == khachHangId)
                .Select(r => (bool?)r.IsLike)
                .FirstOrDefault();

            return Json(new { success = true, likeCount, dislikeCount, userReaction });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
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
}
