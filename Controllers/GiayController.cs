using ASM.Helpers;
using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace ASM.Controllers
{
    public class GiayController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGiaySvc _giaySvc;
        private readonly IUploadHelper _uploadHelper;

        public GiayController(IWebHostEnvironment webHostEnvironment, IGiaySvc giaySvc, IUploadHelper uploadHelper)
        {
            _webHostEnvironment = webHostEnvironment;
            _giaySvc = giaySvc;
            _uploadHelper = uploadHelper;
        }

        // Danh sách giày cho Admin (CRUD)
        public ActionResult Index()
        {
            // Ensure statuses are consistent with quantities
            _giaySvc.EnsureStatusConsistency();
            return View(_giaySvc.GetAll());
        }

        // Inventory overview
        public ActionResult Inventory()
        {
            _giaySvc.EnsureStatusConsistency();
            var list = _giaySvc.GetAll();
            return View(list);
        }

        // Chi ti?t giày
        public ActionResult Details(int id)
        {
            var giay = _giaySvc.Get(id);
            return View(giay);
        }

        // GET: GiayController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GiayController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Giay giay)
        {
            if (!ModelState.IsValid)
            {
                return View(giay);
            }

            string folder = "Giay";
            try
            {
                if (giay.ImageFile != null && giay.ImageFile.Length > 0)
                {
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    _uploadHelper.UploadImage(giay.ImageFile, rootPath, folder);
                    giay.Hinh = giay.ImageFile.FileName;
                }

                // Determine status based on quantity
                giay.Status = giay.Quantity > 0 ? ProductStatus.InStock : ProductStatus.OutOfStock;

                var ret = _giaySvc.Add(giay);
                if (ret > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Add failed. Please try again.");
                return View(giay);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
                return View(giay);
            }
        }

        public ActionResult Edit(int id)
        {
            var giay = _giaySvc.Get(id);
            return View(giay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Giay giay)
        {
            if (!ModelState.IsValid)
            {
                return View(giay);
            }

            string folder = "Giay";
            try
            {
                if (giay.ImageFile != null && giay.ImageFile.Length > 0)
                {
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    _uploadHelper.UploadImage(giay.ImageFile, rootPath, folder);
                    giay.Hinh = giay.ImageFile.FileName;
                }

                // If admin set explicit Status via form, keep it; otherwise service will compute from Quantity
                var ret = _giaySvc.Edit(id, giay);
                if (ret > 0)
                {
                    return RedirectToAction(nameof(Details), new { id = giay.Id });
                }

                ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                return View(giay);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
                return View(giay);
            }
        }

        public ActionResult Delete(int id)
        {
            _giaySvc.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // Accept JSON payload for inventory updates
        [HttpPost]
        public IActionResult UpdateInventory([FromBody] UpdateInventoryRequest request)
        {
            if (request == null)
                return Json(new { success = false, message = "Invalid request" });

            var giay = _giaySvc.Get(request.Id);
            if (giay == null)
                return Json(new { success = false, message = "Product not found" });

            ProductStatus? status = null;
            if (request.Status.HasValue)
            {
                status = (ProductStatus)request.Status.Value;
            }

            var ret = _giaySvc.UpdateQuantity(request.Id, request.Quantity, status);
            if (ret > 0)
                return Json(new { success = true });

            return Json(new { success = false, message = "Update failed" });
        }

        public class UpdateInventoryRequest
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
            public int? Status { get; set; }
        }
    }
}
