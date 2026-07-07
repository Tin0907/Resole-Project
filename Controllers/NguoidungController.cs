using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    public class NguoiDungController : BaseController
    {
        private readonly INguoidungSvc _nguoidungSvc;

        public NguoiDungController(INguoidungSvc nguoidungSvc)
        {
            _nguoidungSvc = nguoidungSvc;
        }
        public IActionResult Index()
        {
            var data = _nguoidungSvc.GetNguoiDungAll();
            return View(data);
        }
        public IActionResult Details(int id)
        {
            var model = _nguoidungSvc.GetNguoiDung(id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NguoiDung model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _nguoidungSvc.AddNguoiDung(model);

            if (result > 0)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Không thể tạo người dùng.");
            return View(model);
        }
        public IActionResult Update(int id)
        {
            var model = _nguoidungSvc.GetNguoiDung(id);
            if (model == null) return NotFound();

            // Khi load Edit, không hiển thị mật khẩu
            model.Password = "";
            model.ConfirmPassword = "";

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, NguoiDung model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _nguoidungSvc.EditNguoiDung(id, model);

            if (result > 0)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Không thể cập nhật người dùng.");
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            _nguoidungSvc.DeleteNguoiDung(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
