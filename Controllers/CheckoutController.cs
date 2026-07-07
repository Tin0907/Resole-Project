using ASM.Constants;
using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASM.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartSvc _cartSvc;
        private readonly IKhachhangSvc _khachhangSvc;
        private readonly IGiaySvc _giaySvc;
        private readonly DataContext _context;

        public CheckoutController(ICartSvc cartSvc, IKhachhangSvc khachhangSvc, IGiaySvc giaySvc, DataContext context)
        {
            _cartSvc = cartSvc;
            _khachhangSvc = khachhangSvc;
            _giaySvc = giaySvc;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var khIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(khIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int khId = int.Parse(khIdStr);

            var buyNowJson = HttpContext.Session.GetString("BuyNowItem");

            if (!string.IsNullOrEmpty(buyNowJson))
            {
                var buyNowItem = JsonConvert.DeserializeObject<BuyNowItem>(buyNowJson);
                var items = new List<dynamic> { new {
                    GiayId = buyNowItem.GiayId,
                    TenGiay = buyNowItem.TenGiay,
                    Hinh = buyNowItem.Hinh,
                    GiaGiay = buyNowItem.GiaGiay,
                    SoLuong = buyNowItem.SoLuong,
                    Size = buyNowItem.Size,
                    ThanhTien = buyNowItem.ThanhTien
                }};

                var khachHang = _khachhangSvc.GetKhachhang(khId);
                ViewBag.KhachHang = khachHang;
                ViewBag.CartItems = items;
                ViewBag.Total = buyNowItem.ThanhTien;
                ViewBag.IsBuyNow = true;

                HttpContext.Session.Remove("BuyNowItem");

                var checkoutItems = new List<CheckoutItemData>
                {
                    new CheckoutItemData
                    {
                        GiayId = buyNowItem.GiayId,
                        SoLuong = buyNowItem.SoLuong,
                        Size = buyNowItem.Size,
                        ThanhTien = buyNowItem.ThanhTien
                    }
                };
                HttpContext.Session.SetString("CheckoutItems", JsonConvert.SerializeObject(checkoutItems));
                HttpContext.Session.SetString("CheckoutTotal", buyNowItem.ThanhTien.ToString());
                HttpContext.Session.SetString("CheckoutIsBuyNow", "true");

                return View();
            }
            else
            {
                var carts = _cartSvc.GetCartByKhachHangId(khId);

                if (!carts.Any())
                {
                    TempData["Message"] = "Your cart is empty!";
                    return RedirectToAction("Index", "Cart");
                }

                var cartItems = carts.Select(c => {
                    var giay = c.Giay ?? _giaySvc.Get(c.GiayId);
                    return new {
                        GiayId = c.GiayId,
                        TenGiay = giay?.Ten,
                        Hinh = giay?.Hinh,
                        GiaGiay = giay?.Gia ?? 0,
                        SoLuong = c.SoLuong,
                        Size = c.Size,
                        ThanhTien = (giay?.Gia ?? 0) * c.SoLuong
                    };
                }).ToList();

                var khachHang = _khachhangSvc.GetKhachhang(khId);
                ViewBag.KhachHang = khachHang;
                ViewBag.CartItems = cartItems;
                ViewBag.Total = cartItems.Sum(x => x.ThanhTien);
                ViewBag.IsBuyNow = false;

                var checkoutItems = cartItems.Select(c => new CheckoutItemData
                {
                    GiayId = c.GiayId,
                    SoLuong = c.SoLuong,
                    Size = c.Size,
                    ThanhTien = c.ThanhTien
                }).ToList();
                HttpContext.Session.SetString("CheckoutItems", JsonConvert.SerializeObject(checkoutItems));
                HttpContext.Session.SetString("CheckoutTotal", cartItems.Sum(x => x.ThanhTien).ToString());
                HttpContext.Session.SetString("CheckoutIsBuyNow", "false");

                return View();
            }
        }

        [HttpPost]
        public IActionResult BuyNow([FromBody] BuyNowItem item)
        {
            var khIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(khIdStr))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            int khId = int.Parse(khIdStr);

            try
            {
                var giay = _giaySvc.Get(item.GiayId);
                if (giay == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                var buyNowItem = new BuyNowItem
                {
                    GiayId = giay.Id,
                    TenGiay = giay.Ten,
                    Hinh = giay.Hinh,
                    GiaGiay = giay.Gia,
                    SoLuong = item.SoLuong,
                    Size = item.Size
                };

                var json = JsonConvert.SerializeObject(buyNowItem);
                HttpContext.Session.SetString("BuyNowItem", json);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(string deliveryAddress, string phoneNumber, string email, string notes)
        {
            var khIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(khIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int khId = int.Parse(khIdStr);

            try
            {
                var checkoutItemsJson = HttpContext.Session.GetString("CheckoutItems");
                var checkoutTotalStr = HttpContext.Session.GetString("CheckoutTotal");
                var checkoutIsBuyNow = HttpContext.Session.GetString("CheckoutIsBuyNow") == "true";

                if (string.IsNullOrEmpty(checkoutItemsJson))
                {
                    TempData["Error"] = "Checkout session expired. Please try again.";
                    return RedirectToAction("Index", "Cart");
                }

                var checkoutItems = JsonConvert.DeserializeObject<List<CheckoutItemData>>(checkoutItemsJson);
                double tongTien = double.Parse(checkoutTotalStr ?? "0");

                if (checkoutItems == null || !checkoutItems.Any())
                {
                    TempData["Error"] = "No items to checkout.";
                    return RedirectToAction("Index", "Cart");
                }

                // Validate inventory and deduct inside a transaction
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    // Check availability
                    foreach (var item in checkoutItems)
                    {
                        var giay = _context.Giays.Find(item.GiayId);
                        if (giay == null)
                        {
                            transaction.Rollback();
                            TempData["Error"] = $"Product (ID {item.GiayId}) not found.";
                            return RedirectToAction("Index", "Checkout");
                        }

                        if (giay.Status == ProductStatus.Locked)
                        {
                            transaction.Rollback();
                            TempData["Error"] = $"Product '{giay.Ten}' is currently locked.";
                            return RedirectToAction("Index", "Checkout");
                        }

                        if (giay.Quantity < item.SoLuong)
                        {
                            transaction.Rollback();
                            TempData["Error"] = $"Product '{giay.Ten}' only has {giay.Quantity} left. Please adjust quantity.";
                            return RedirectToAction("Index", "Checkout");
                        }
                    }

                    // Create order
                    var donHang = new DonHang
                    {
                        KhachHangId = khId,
                        NgayDatHang = DateTime.Now,
                        TongTien = tongTien,
                        TrangThai = TrangThaiDonHang.ChoXacNhan,
                        GhiChu = $"Address: {deliveryAddress} | Phone: {phoneNumber} | Email: {email} | Notes: {notes}"
                    };

                    _context.DonHangs.Add(donHang);
                    _context.SaveChanges();

                    // Create order details and deduct stock
                    foreach (var item in checkoutItems)
                    {
                        var chitiet = new DonhangChitiet
                        {
                            DonHangId = donHang.Id,
                            GiayId = item.GiayId,
                            SoLuong = item.SoLuong,
                            ThanhTien = item.ThanhTien,
                            GhiChu = $"Size: {item.Size}"
                        };
                        _context.DonhangChitiets.Add(chitiet);

                        var giay = _context.Giays.Find(item.GiayId);
                        // Deduct
                        giay.Quantity -= item.SoLuong;
                        // Auto-lock when zero
                        giay.Status = giay.Quantity > 0 ? ProductStatus.InStock : ProductStatus.Locked;
                        giay.UpdatedAt = DateTime.Now;
                        _context.Giays.Update(giay);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    // Clear cart if checkout from cart (not buy now)
                    if (!checkoutIsBuyNow)
                    {
                        _cartSvc.ClearCart(khId);
                    }

                    // Clean up session
                    HttpContext.Session.Remove("CheckoutItems");
                    HttpContext.Session.Remove("CheckoutTotal");
                    HttpContext.Session.Remove("CheckoutIsBuyNow");

                    return RedirectToAction("Success", new { orderId = donHang.Id });
                }
                catch
                {
                    try { transaction.Rollback(); } catch { }
                    TempData["Error"] = "An error occurred while placing your order. Please try again.";
                    return RedirectToAction("Index", "Checkout");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while placing your order. Please try again.";
                return RedirectToAction("Index", "Checkout");
            }
        }

        [HttpGet]
        public IActionResult Success(int orderId)
        {
            var khIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            if (string.IsNullOrEmpty(khIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int khId = int.Parse(khIdStr);

            var donHang = _context.DonHangs
                .Where(d => d.Id == orderId && d.KhachHangId == khId)
                .Select(d => new {
                    d.Id,
                    d.NgayDatHang,
                    d.TongTien,
                    d.GhiChu
                })
                .FirstOrDefault();

            if (donHang == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.OrderId = donHang.Id;
            ViewBag.OrderDate = donHang.NgayDatHang;
            ViewBag.OrderTotal = donHang.TongTien;
            ViewBag.OrderNotes = donHang.GhiChu;

            return View();
        }
    }

    public class CheckoutItemData
    {
        public int GiayId { get; set; }
        public int SoLuong { get; set; }
        public string Size { get; set; }
        public double ThanhTien { get; set; }
    }
}
