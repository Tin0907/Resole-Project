using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class KhachHang
    {
        // key
        [Key]
        public int Id { get; set; }
        // full name
        [StringLength(150)]
        [Required(ErrorMessage ="You need to enter name")]
        [Display(Name ="Full Name")]
        public string Fullname { get; set; }
        // date of birth
        [Display(Name ="Date of Birth")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime? NgaySinh { get; set; }
        // phone number
        [Required(ErrorMessage ="You need to enter phone number")]
        [Display(Name ="Phone Number")]
        [Column(TypeName ="varchar(15)"),MaxLength(15)]
        [RegularExpression(@"^[0-9\-\+]{9,15}$", ErrorMessage ="Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "You need to enter email")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        // password
        [Required(ErrorMessage = "You need to enter password")]
        [Display(Name = "Password")]
        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        public string Password { get; set; }
        
        // confirmpassword - NOT SAVED TO DATABASE
        [Required(ErrorMessage = "You need to re-enter password")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [NotMapped]  // Important: do not map to database
        public string ConfirmPassword { get; set; }
    }
}
