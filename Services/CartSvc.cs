using ASM.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM.Services
{
    public interface ICartSvc
    {
        List<Cart> GetCartByKhachHangId(int khachHangId);
        Cart GetById(int id);
        void Add(Cart cart);
        void Update(Cart cart);
        void Delete(int id);
        void DeleteByKhachHangIdAndGiayId(int khachHangId, int giayId, string size);
        void ClearCart(int khachHangId);
        int GetCartCount(int khachHangId);
    }

    public class CartSvc : ICartSvc
    {
        private readonly DataContext _context;

        public CartSvc(DataContext context)
        {
            _context = context;
        }

        public List<Cart> GetCartByKhachHangId(int khachHangId)
        {
            return _context.Carts
                .Include(c => c.Giay)
                .Include(c => c.KhachHang)
                .Where(c => c.KhachHangId == khachHangId)
                .ToList();
        }

        public Cart GetById(int id)
        {
            return _context.Carts.Find(id);
        }

        public void Add(Cart cart)
        {
            // Ki?m tra xem s?n ph?m ð? có trong gi? hàng chýa
            var existingCart = _context.Carts
                .FirstOrDefault(c => c.KhachHangId == cart.KhachHangId 
                                  && c.GiayId == cart.GiayId 
                                  && c.Size == cart.Size);

            if (existingCart != null)
            {
                // N?u ð? có th? tãng s? lý?ng
                existingCart.SoLuong += cart.SoLuong;
                _context.SaveChanges();
            }
            else
            {
                // N?u chýa có th? thêm m?i
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public void DeleteByKhachHangIdAndGiayId(int khachHangId, int giayId, string size)
        {
            var cart = _context.Carts
                .FirstOrDefault(c => c.KhachHangId == khachHangId 
                                  && c.GiayId == giayId 
                                  && c.Size == size);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public void ClearCart(int khachHangId)
        {
            var carts = _context.Carts.Where(c => c.KhachHangId == khachHangId).ToList();
            _context.Carts.RemoveRange(carts);
            _context.SaveChanges();
        }

        public int GetCartCount(int khachHangId)
        {
            return _context.Carts
                .Where(c => c.KhachHangId == khachHangId)
                .Sum(c => c.SoLuong);
        }
    }
}
