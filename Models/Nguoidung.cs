using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class NguoiDung
    {
        [Key]
        public int NguoiDungID { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "You need to enter username")]
        public string User { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "You need to enter full name")]
        [Column(TypeName = "nvarchar(100)")]
        public string HoTen { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Display(Name = "Job Title")]
        [Column(TypeName = "nvarchar(50)")]
        public string ChucDanh { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Administrator")]
        public bool Admin { get; set; }

        [Display(Name = "Active")]
        public bool Locked { get; set; }

        [Display(Name = "Password")]
        [Column(TypeName = "nvarchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Column(TypeName = "nvarchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [NotMapped]
        public string ConfirmPassword { get; set; }


    }
}
