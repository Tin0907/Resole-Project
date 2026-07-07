using ASM.Helpers;
using ASM.Models;
using System.Collections.Generic;
namespace ASM.Services
{
    public interface INguoidungSvc
    {
        List<NguoiDung> GetNguoiDungAll();
        public NguoiDung GetNguoiDung(int id);
        public int AddNguoiDung(NguoiDung nguoiDung);
        public int EditNguoiDung(int id, NguoiDung nguoiDung);
        public int DeleteNguoiDung(int id);
        public NguoiDung Login(ViewLogin viewLogin);

    }
    public class NguoidungSvc : INguoidungSvc
    {
        protected DataContext _context;
        protected IMahoaHelper _mahoaHelper;
        public NguoidungSvc(DataContext context, IMahoaHelper mahoaHelper)
        {
            _context = context;
            _mahoaHelper = mahoaHelper;
        }

        public List<NguoiDung> GetNguoiDungAll()
        {
            List<NguoiDung> list = new List<NguoiDung>();
            list = _context.Nguoidungs.ToList();
            return list;
        }

        public NguoiDung GetNguoiDung(int id)
        {
            NguoiDung nguoidung = null;
            nguoidung = _context.Nguoidungs.Find(id); //cách này chỉ dùng cho Khóa chính
            //product = _context.Products.Where(e=>e.Id==id).FirstOrDefault(); //cách tổng quát
            return nguoidung;
        }

        public int AddNguoiDung(NguoiDung nguoiDung)
        {
            int ret = 0;
            try
            {
                nguoiDung.Password = _mahoaHelper.Mahoa(nguoiDung.Password);
                _context.Add(nguoiDung);
                _context.SaveChanges();
                ret = nguoiDung.NguoiDungID;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public int EditNguoiDung(int id, NguoiDung nguoiDung)
        {
            int ret = 0;
            try
            {
                NguoiDung _nguoidung = null;
                _nguoidung = _context.Nguoidungs.Find(id); //cách này chỉ dùng cho Khóa chính

                _nguoidung.User = nguoiDung.User;
                _nguoidung.HoTen = nguoiDung.HoTen;
                _nguoidung.ChucDanh = nguoiDung.ChucDanh;
                _nguoidung.NgaySinh = nguoiDung.NgaySinh;
                _nguoidung.Email = nguoiDung.Email;
                _nguoidung.Admin = nguoiDung.Admin;
                _nguoidung.Locked = nguoiDung.Locked;
                if (nguoiDung.Password != null)
                {
                    nguoiDung.Password = _mahoaHelper.Mahoa(nguoiDung.Password);
                    _nguoidung.Password = nguoiDung.Password;
                }
                _context.Update(_nguoidung);
                _context.SaveChanges();
                ret = nguoiDung.NguoiDungID;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public int DeleteNguoiDung(int id)
        {
            int ret = 0;
            try
            {
                var current = _context.Nguoidungs.Find(id);
                if (current == null) return 0;
                _context.Nguoidungs.Remove(current);
                _context.SaveChanges();
                ret = id;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public NguoiDung Login(ViewLogin viewLogin)
        {
           var u = _context.Nguoidungs.Where(
               p => p.Email.ToLower().Equals(viewLogin.Email.ToLower())
               && p.Password.Equals(_mahoaHelper.Mahoa(viewLogin.Password))
               ).FirstOrDefault();
            return u;
        }

    }

}

