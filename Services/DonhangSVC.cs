using ASM.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM.Services
{
    public interface IDonHangSvc
    {
        List<DonHang> GetDonHangAll();
        List<DonHang> GetDonHangByKhachHang(int khachhangId);
        DonHang GetDonHangById(int id);
        int AddDonHang(DonHang donHang);
        int EditDonHang(int id, DonHang donHang);
        (bool success, string message) ApproveDonHang(int id);
    }
    public class DonHangSvc : IDonHangSvc
    {
        protected DataContext _context;

        public DonHangSvc(DataContext context)
        {
            _context = context;
        }

        public List<DonHang> GetDonHangAll()
        {
            List<DonHang> list = new List<DonHang>();
            list = _context.DonHangs.OrderByDescending(x => x.NgayDatHang)
                .Include(x=>x.KhachHang)
                .Include(x=>x.DonhangChitiets)
                .ThenInclude(d => d.Giay)
                .ToList();
            return list;
        }

        public List<DonHang> GetDonHangByKhachHang(int khachhangId)
        {
            return _context.DonHangs
                .Where(x => x.KhachHangId == khachhangId)
                .Include(x => x.KhachHang)
                .Include(x => x.DonhangChitiets)
                .ThenInclude(d => d.Giay)
                .OrderByDescending(x => x.NgayDatHang)
                .ToList();
        }

        public DonHang GetDonHangById(int id)
        {
            return _context.DonHangs
                .Where(x => x.Id == id)
                .Include(x => x.KhachHang)
                .Include(x => x.DonhangChitiets)
                .ThenInclude(d => d.Giay)
                .FirstOrDefault();
        }

        public DonHang GetDonHang(int id)
        {
            DonHang donHang = null;
            donHang = _context.DonHangs.Where(x => x.Id==id)
                .Include(x =>x.KhachHang)
                .Include(x => x.DonhangChitiets)
                .ThenInclude(d => d.Giay)
                .FirstOrDefault();
            return donHang;
        }

        public int AddDonHang(DonHang donHang)
        {
           int ret = 0;
            try
            {
                _context.Add(donHang);
                _context.SaveChanges();
                ret = donHang.Id;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public int EditDonHang(int id, DonHang donHang)
        {
            int ret = 0;
            try
            {
                var existingDonHang = _context.DonHangs.Find(id);
                if (existingDonHang != null)
                {
                    // Update properties
                    existingDonHang.KhachHangId = donHang.KhachHangId;
                    existingDonHang.NgayDatHang = donHang.NgayDatHang;
                    existingDonHang.TongTien = donHang.TongTien;
                    existingDonHang.TrangThai = donHang.TrangThai;
                    existingDonHang.GhiChu = donHang.GhiChu;
                    // Optionally update navigation properties if needed

                    _context.Update(existingDonHang);
                    _context.SaveChanges();
                    ret = existingDonHang.Id;
                }
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public (bool success, string message) ApproveDonHang(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var donHang = _context.DonHangs
                    .Include(d => d.DonhangChitiets)
                    .FirstOrDefault(d => d.Id == id);

                if (donHang == null)
                    return (false, "Order not found");

                if (donHang.TrangThai != TrangThaiDonHang.ChoXacNhan)
                    return (false, "Order is not pending");

                // Check inventory for each item
                foreach (var item in donHang.DonhangChitiets)
                {
                    var giay = _context.Giays.Find(item.GiayId);
                    if (giay == null)
                    {
                        transaction.Rollback();
                        return (false, $"Product with id {item.GiayId} not found");
                    }

                    if (giay.Status == ProductStatus.Locked)
                    {
                        transaction.Rollback();
                        return (false, $"Product '{giay.Ten}' is locked");
                    }

                    if (giay.Quantity < item.SoLuong)
                    {
                        transaction.Rollback();
                        return (false, "Insufficient stock for product");
                    }
                }

                // Deduct quantities
                foreach (var item in donHang.DonhangChitiets)
                {
                    var giay = _context.Giays.Find(item.GiayId);
                    giay.Quantity -= item.SoLuong;
                    // Auto-lock when quantity reaches zero to keep consistent behavior
                    giay.Status = giay.Quantity > 0 ? ProductStatus.InStock : ProductStatus.Locked;
                    giay.UpdatedAt = DateTime.Now;
                    _context.Giays.Update(giay);
                }

                // Update order status to approved (Ready to pickup)
                donHang.TrangThai = TrangThaiDonHang.ChoLayHang;
                _context.DonHangs.Update(donHang);

                _context.SaveChanges();
                transaction.Commit();

                return (true, "Order approved successfully");
            }
            catch (Exception ex)
            {
                try { transaction.Rollback(); } catch { }
                return (false, "An error occurred while approving order");
            }
        }
    }
}
