using System.ComponentModel.DataAnnotations;

namespace ASM.Models.ViewModels
{
    public class KhachHangProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string Fullname { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? NgaySinh { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; } = new();

        public List<DonHang> Orders { get; set; } = new();
    }
}
