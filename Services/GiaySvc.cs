using ASM.Models;
using System.Linq;

namespace ASM.Services
{
    public interface IGiaySvc
    {
        List<Giay> GetAll();
        Giay Get(int id);
        int Add(Giay giay);
        int Edit(int id, Giay giay);
        int Delete(int id);
        List<Giay> GetLatest(int count);
        List<Giay> GetByCategory(PhanLoai category);
        List<Giay> Search(string searchTerm);
        int UpdateQuantity(int id, int quantity, ProductStatus? status = null);
        void EnsureStatusConsistency();
    }

    public class GiaySvc : IGiaySvc
    {
        protected DataContext _context;
        public GiaySvc(DataContext context)
        {
            _context = context;
        }

        public List<Giay> GetAll()
        {
            return _context.Giays.ToList();
        }

        public List<Giay> GetLatest(int count)
        {
            return _context.Giays
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToList();
        }

        public List<Giay> GetByCategory(PhanLoai category)
        {
            return _context.Giays
                .Where(x => x.PhanLoai == category)
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        public Giay Get(int id)
        {
            return _context.Giays.Find(id);
        }

        public int Add(Giay giay)
        {
            int ret = 0;
            try
            {
                // set timestamps
                giay.CreatedAt = DateTime.Now;
                giay.UpdatedAt = DateTime.Now;

                // determine status from quantity
                giay.Status = giay.Quantity > 0 ? ProductStatus.InStock : ProductStatus.Locked;

                _context.Add(giay);
                _context.SaveChanges();
                ret = giay.Id;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public int Edit(int id, Giay giay)
        {
            int ret = 0;
            try
            {
                var current = _context.Giays.Find(id);
                if (current == null) return 0;

                current.Ten = giay.Ten;
                current.Gia = giay.Gia;
                current.PhanLoai = giay.PhanLoai;
                current.Hinh = giay.Hinh;
                current.MoTa = giay.MoTa;
                current.TrangThai = giay.TrangThai;

                // update quantity if provided
                current.Quantity = giay.Quantity;

                // if caller explicitly set Status (enum value > 0), use it; otherwise compute from quantity
                if ((int)giay.Status != 0)
                {
                    current.Status = giay.Status;
                }
                else
                {
                    current.Status = current.Quantity > 0 ? ProductStatus.InStock : ProductStatus.Locked;
                }

                current.UpdatedAt = DateTime.Now;

                _context.Update(current);
                _context.SaveChanges();
                ret = current.Id;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public int Delete(int id)
        {
            int ret = 0;
            try
            {
                var current = _context.Giays.Find(id);
                if (current == null) return 0;
                _context.Giays.Remove(current);
                _context.SaveChanges();
                ret = id;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public List<Giay> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAll();

            var term = searchTerm.Trim().ToLower();
            return _context.Giays
                .Where(g => g.Ten.ToLower().Contains(term)
                         || (g.MoTa != null && g.MoTa.ToLower().Contains(term))
                         || g.PhanLoai.ToString().ToLower().Contains(term))
                .ToList();
        }

        public int UpdateQuantity(int id, int quantity, ProductStatus? status = null)
        {
            int ret = 0;
            try
            {
                var current = _context.Giays.Find(id);
                if (current == null) return 0;

                current.Quantity = quantity;
                if (status.HasValue)
                {
                    current.Status = status.Value;
                }
                else
                {
                    // Auto-lock when quantity is zero, otherwise in stock
                    current.Status = current.Quantity > 0 ? ProductStatus.InStock : ProductStatus.Locked;
                }
                current.UpdatedAt = DateTime.Now;

                _context.Update(current);
                _context.SaveChanges();
                ret = current.Id;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        // Ensure DB status reflects quantity (lock items with 0 quantity)
        public void EnsureStatusConsistency()
        {
            try
            {
                var list = _context.Giays.Where(g => (g.Quantity <= 0 && g.Status != ProductStatus.Locked)
                                                    || (g.Quantity > 0 && g.Status == ProductStatus.Locked)).ToList();
                if (!list.Any()) return;

                foreach (var g in list)
                {
                    if (g.Quantity <= 0)
                    {
                        g.Status = ProductStatus.Locked;
                    }
                    else
                    {
                        g.Status = ProductStatus.InStock;
                    }
                    g.UpdatedAt = DateTime.Now;
                    _context.Giays.Update(g);
                }
                _context.SaveChanges();
            }
            catch
            {
                // ignore errors here to avoid breaking controllers
            }
        }
    }
}
