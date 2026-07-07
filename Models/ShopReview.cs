using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class ShopReview
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DonHang")]
        public int DonHangId { get; set; }

        [ForeignKey("KhachHang")]
        public int KhachHangId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DonHang DonHang { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
