using ASM.Helpers;
using ASM.Models;
using ASM.Services;
using ASM.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    public class CartController : Controller
    {
        private readonly IGiaySvc _giaySvc;
        private readonly ICartSvc _cartSvc;
        private readonly ILogger<CartController> _logger;

        public CartController(IGiaySvc giaySvc, ICartSvc cartSvc, ILogger<CartController> logger)
        {
            _giaySvc = giaySvc;
            _cartSvc = cartSvc;
            _logger = logger;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
            
            if (string.IsNullOrEmpty(khachHangIdStr))
            {
                return RedirectToAction("Login", "Khachhang");
            }

            int khachHangId = int.Parse(khachHangIdStr);
            var carts = _cartSvc.GetCartByKhachHangId(khachHangId);
            
            // Convert to CartItem for view
            var cartItems = carts.Select(c => {
                var giay = c.Giay ?? _giaySvc.Get(c.GiayId);
                return new CartItem
                {
                    GiayId = c.GiayId,
                    Ten = giay?.Ten,
                    Hinh = giay?.Hinh,
                    Gia = (decimal)(giay?.Gia ?? 0),
                    SoLuong = c.SoLuong,
                    Size = c.Size
                };
            }).ToList();

            return View(cartItems);
        }

        // POST: Cart/Add
        [HttpPost]
        public IActionResult Add([FromBody] AddToCartRequest request)
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
                
                if (string.IsNullOrEmpty(khachHangIdStr))
                {
                    return Json(new { success = false, message = "Please login to add products to cart" });
                }

                int khachHangId = int.Parse(khachHangIdStr);

                _logger.LogInformation($"Add to cart - KhachHangId: {khachHangId}, GiayId: {request.GiayId}, Size: {request.Size}, Quantity: {request.Quantity}");
                
                var giay = _giaySvc.Get(request.GiayId);
                
                if (giay == null)
                {
                    _logger.LogWarning($"Product not found - GiayId: {request.GiayId}");
                    return Json(new { success = false, message = "Product not found" });
                }

                // Inventory checks
                if (giay.Status == ProductStatus.Locked)
                {
                    return Json(new { success = false, message = "Product is currently locked" });
                }

                if (giay.Quantity <= 0)
                {
                    return Json(new { success = false, message = "Product is out of stock" });
                }

                var existingCart = _cartSvc.GetCartByKhachHangId(khachHangId)
                    .FirstOrDefault(c => c.GiayId == request.GiayId && c.Size == request.Size);

                int existingQuantity = existingCart?.SoLuong ?? 0;
                int desiredTotal = existingQuantity + request.Quantity;

                if (desiredTotal > giay.Quantity)
                {
                    return Json(new { success = false, message = "Insufficient stock" });
                }

                if (existingCart != null)
                {
                    existingCart.SoLuong = desiredTotal;
                    _cartSvc.Update(existingCart);
                }
                else
                {
                    var cart = new Cart
                    {
                        KhachHangId = khachHangId,
                        GiayId = giay.Id,
                        Size = request.Size,
                        SoLuong = request.Quantity
                    };

                    _cartSvc.Add(cart);
                }
                
                var cartCount = _cartSvc.GetCartCount(khachHangId);
                
                _logger.LogInformation($"Added to cart successfully. Cart count: {cartCount}");
                
                return Json(new { success = true, cartCount = cartCount, message = "Added to cart successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to cart");
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        // POST: Cart/Remove
        [HttpPost]
        public IActionResult Remove([FromBody] RemoveFromCartRequest request)
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
                
                if (string.IsNullOrEmpty(khachHangIdStr))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                int khachHangId = int.Parse(khachHangIdStr);

                _cartSvc.DeleteByKhachHangIdAndGiayId(khachHangId, request.GiayId, request.Size);
                
                var cartCount = _cartSvc.GetCartCount(khachHangId);
                
                return Json(new { success = true, cartCount = cartCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from cart");
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
                
                if (string.IsNullOrEmpty(khachHangIdStr))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                int khachHangId = int.Parse(khachHangIdStr);

                var cart = _cartSvc.GetCartByKhachHangId(khachHangId)
                    .FirstOrDefault(c => c.GiayId == request.GiayId && c.Size == request.Size);

                if (cart != null)
                {
                    if (request.Quantity > 0)
                    {
                        // check stock
                        var giay = _giaySvc.Get(request.GiayId);
                        if (giay != null && request.Quantity > giay.Quantity)
                        {
                            return Json(new { success = false, message = "Insufficient stock" });
                        }

                        cart.SoLuong = request.Quantity;
                        _cartSvc.Update(cart);
                    }
                    else
                    {
                        _cartSvc.DeleteByKhachHangIdAndGiayId(khachHangId, request.GiayId, request.Size);
                    }
                }

                var cartCount = _cartSvc.GetCartCount(khachHangId);
                var carts = _cartSvc.GetCartByKhachHangId(khachHangId);
                var cartTotal = carts.Sum(c => {
                    var giay = _giaySvc.Get(c.GiayId);
                    return (decimal)(giay?.Gia ?? 0) * c.SoLuong;
                });
                
                return Json(new { success = true, cartCount = cartCount, cartTotal = cartTotal });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantity");
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        // POST: Cart/Clear
        [HttpPost]
        public IActionResult Clear()
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
                
                if (string.IsNullOrEmpty(khachHangIdStr))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                int khachHangId = int.Parse(khachHangIdStr);
                _cartSvc.ClearCart(khachHangId);
                
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        // GET: Cart/GetCount
        [HttpGet]
        public IActionResult GetCount()
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
                
                if (string.IsNullOrEmpty(khachHangIdStr))
                {
                    return Json(new { cartCount = 0 });
                }

                int khachHangId = int.Parse(khachHangIdStr);
                var cartCount = _cartSvc.GetCartCount(khachHangId);
                
                return Json(new { cartCount = cartCount });
            }
            catch
            {
                return Json(new { cartCount = 0 });
            }
        }
    }
    
    // Request models
    public class AddToCartRequest
    {
        public int GiayId { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
    
    public class RemoveFromCartRequest
    {
        public int GiayId { get; set; }
        public string Size { get; set; }
    }
    
    public class UpdateQuantityRequest
    {
        public int GiayId { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}
