using ASM.Models;

namespace ASM.Services
{
    public interface IDonhangChitietSvc
    {
        int AddDonhangChitiet(Models.DonhangChitiet donhangChitiet);
    }
    public class DonhangChitietSvc : IDonhangChitietSvc
    {
        protected DataContext _context;

        public DonhangChitietSvc(DataContext context)
        {
            _context = context;
        }

        public int AddDonhangChitiet(DonhangChitiet donhangChitiet)
        {
            int ret = 0;
            try
            {
                _context.DonhangChitiets.Add(donhangChitiet);
                _context.SaveChanges();
                ret = donhangChitiet.Id;
            }
            catch (Exception ex)
            {
                ret = 0;
            }
            return ret;
        }
    }
}
