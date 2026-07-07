using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    // Controller này KHÔNG k? th?a BaseController nên không có AuthenticationFilter
    public class ThucDonController : Controller
    {
        private IGiaySvc _giaySvc;

        public ThucDonController(IGiaySvc giaySvc)
        {
            _giaySvc = giaySvc;
        }

        // Danh sách giày cho t?t c? m?i ngý?i (không c?n ðãng nh?p)
        public IActionResult Index(string searchTerm, int? category)
        {
            ViewData["CurrentFilter"] = searchTerm;
            ViewData["CurrentCategory"] = category?.ToString() ?? "";

            IEnumerable<Giay> giays = _giaySvc.GetAll();

            // If a category is provided and valid, use category filter first
            if (category.HasValue && Enum.IsDefined(typeof(PhanLoai), category.Value))
            {
                var catEnum = (PhanLoai)category.Value;
                giays = _giaySvc.GetByCategory(catEnum);

                // If there's also a search term, further filter within the category
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var term = searchTerm.Trim().ToLower();
                    giays = giays.Where(g => g.Ten.ToLower().Contains(term)
                                             || (g.MoTa != null && g.MoTa.ToLower().Contains(term)));
                }
            }
            else if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                // No category selected - perform global search
                giays = _giaySvc.Search(searchTerm);
            }

            return View(giays);
        }

        // Chi ti?t giày (không c?n ðãng nh?p)
        public IActionResult Details(int id)
        {
            var giay = _giaySvc.Get(id);
            if (giay == null)
            {
                return NotFound();
            }
            return View(giay);
        }
    }
}
