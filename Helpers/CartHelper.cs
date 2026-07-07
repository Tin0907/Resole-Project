using ASM.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ASM.Helpers
{
    public static class CartHelper
    {
        private const string CartSessionKey = "ShoppingCart";

        public static List<CartItem> GetCart(ISession session)
        {
            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJson);
        }

        public static void AddToCart(ISession session, CartItem item)
        {
            var cart = GetCart(session);
            
            // Ki?m tra xem s?n ph?m ð? có trong gi? hàng chýa (cùng ID và size)
            var existingItem = cart.FirstOrDefault(x => x.GiayId == item.GiayId && x.Size == item.Size);
            
            if (existingItem != null)
            {
                // N?u ð? có th? tãng s? lý?ng
                existingItem.SoLuong += item.SoLuong;
            }
            else
            {
                // N?u chýa có th? thêm m?i
                cart.Add(item);
            }
            
            SaveCart(session, cart);
        }

        public static void RemoveFromCart(ISession session, int giayId, string size)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(x => x.GiayId == giayId && x.Size == size);
            
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(session, cart);
            }
        }

        public static void UpdateQuantity(ISession session, int giayId, string size, int quantity)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(x => x.GiayId == giayId && x.Size == size);
            
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.SoLuong = quantity;
                }
                else
                {
                    cart.Remove(item);
                }
                SaveCart(session, cart);
            }
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }

        public static int GetCartCount(ISession session)
        {
            var cart = GetCart(session);
            return cart.Sum(x => x.SoLuong);
        }

        public static decimal GetCartTotal(ISession session)
        {
            var cart = GetCart(session);
            return cart.Sum(x => x.ThanhTien);
        }
    }
}
