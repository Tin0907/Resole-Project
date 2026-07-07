using ASM.Helpers;
using ASM.Models;

namespace ASM.Services
{
    public interface IKhachhangSvc
    {
        List<KhachHang> GetKhachhangAll();
        KhachHang GetKhachhang(int id);
        int AddKhachhang(KhachHang khachHang);
        int EditKhachhang(int id, KhachHang khachHang);
        int ChangePassword(int id, string currentPassword, string newPassword);
        KhachHang Login(ViewLogin login);

    }

    public class KhachhangSvc : IKhachhangSvc
    {
        protected DataContext _context;
        protected IMahoaHelper _mahoaHelper;

        public KhachhangSvc(DataContext context, IMahoaHelper mahhoaHelper)
        {
            _context = context;
            _mahoaHelper = mahhoaHelper;
        }

        public List<KhachHang> GetKhachhangAll()
        {
            List<KhachHang> list = new List<KhachHang>();
            list = _context.KhachHangs.ToList();
            return list;
        }

        public KhachHang GetKhachhang(int id)
        {
            KhachHang khachHang = null;
            khachHang = _context.KhachHangs.Find(id);
            return khachHang;
        }

        public int AddKhachhang(KhachHang khachHang)
        {
            // 1. Kiểm tra email trùng
            bool emailExists = _context.KhachHangs
                .Any(k => k.Email.ToLower() == khachHang.Email.ToLower());

            if (emailExists)
            {
                // Email đã tồn tại -> trả về -1 để controller phân biệt được
                return -1;
            }

            int ret = 0;
            try
            {
                // 2. Hash mật khẩu
                khachHang.Password = _mahoaHelper.Mahoa(khachHang.Password);

                // 3. Thêm vào DB
                _context.KhachHangs.Add(khachHang);
                _context.SaveChanges();

                ret = khachHang.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                ret = 0;
            }
            return ret;
        }

        public int EditKhachhang(int id, KhachHang khachHang)
        {
            int ret = 0;
            try
            {
                KhachHang _khachHang = null;
                _khachHang = _context.KhachHangs.Find(id);

                if (_khachHang == null)
                {
                    return 0;
                }

                _khachHang.Fullname = khachHang.Fullname;
                _khachHang.NgaySinh = khachHang.NgaySinh;
                _khachHang.PhoneNumber = khachHang.PhoneNumber;
                _khachHang.Email = khachHang.Email;
                
                // Chỉ update password nếu có nhập mới
                if (!string.IsNullOrEmpty(khachHang.Password))
                {
                    _khachHang.Password = _mahoaHelper.Mahoa(khachHang.Password);
                }

                _context.SaveChanges();
                ret = khachHang.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing customer: {ex.Message}");
                ret = 0;
            }
            return ret;
        }

        public int ChangePassword(int id, string currentPassword, string newPassword)
        {
            var customer = _context.KhachHangs.Find(id);
            if (customer == null) return 0;

            var hashedCurrent = _mahoaHelper.Mahoa(currentPassword);
            if (!string.Equals(customer.Password, hashedCurrent, StringComparison.OrdinalIgnoreCase))
            {
                return -1; // sai mật khẩu hiện tại
            }

            customer.Password = _mahoaHelper.Mahoa(newPassword);
            _context.SaveChanges();
            return 1;
        }

        public KhachHang Login(ViewLogin login)
        {
            var u = _context.KhachHangs.Where(
                p=>p.Email.ToLower().Equals(login.Email.ToLower())
                && p.Password.Equals(_mahoaHelper.Mahoa(login.Password))
                ).FirstOrDefault();
            return u;
        }

    }
}
