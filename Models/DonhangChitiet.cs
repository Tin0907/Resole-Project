using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class DonhangChitiet
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("DonHang")]
        public int DonHangId { get; set; } 

        [Column("MonAnId")]
        [ForeignKey("Giay")]
        public int GiayId { get; set; }

        [Required, Range(0, int.MaxValue, ErrorMessage = "You need to enter quantity.")]
        [Display(Name = "Quantity")]
        public int SoLuong { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "You need to enter subtotal.")]
        [Display(Name = "Subtotal")]
        public double ThanhTien { get; set; }

        [StringLength(250)]
        [Display(Name = "Notes")]
        public string GhiChu { get; set; }

        public DonHang DonHang { get; set; }
        public Giay Giay { get; set; }

    }
}
