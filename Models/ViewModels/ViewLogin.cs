using System.ComponentModel.DataAnnotations;

namespace ASM.Models
{
    public class ViewLogin
    {
        [Required(ErrorMessage = "You need to enter email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You need to enter password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Make nullable to avoid implicit [Required] from nullable-reference-type validation
        public string? ReturnUrl { get; set; }
    }
}
