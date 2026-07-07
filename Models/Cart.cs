using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KhachHangId { get; set; }

        [Required]
        public int GiayId { get; set; }

        [Required]
        [StringLength(10)]
        public string Size { get; set; }

        [Required]
        [Range(1, 999)]
        public int SoLuong { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("KhachHangId")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("GiayId")]
        public virtual Giay Giay { get; set; }
    }
}
