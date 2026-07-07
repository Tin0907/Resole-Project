using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public enum TrangThaiDonHang
    {
        [Display(Name = "Pending Confirmation")]
        ChoXacNhan = 1,
        [Display(Name = "Ready to Pickup")]
        ChoLayHang = 2,
        [Display(Name = "Shipping")]
        ChoGiaoHang = 3,
        [Display(Name = "Delivered")]
        DaGiao = 4,
        [Display(Name = "Reviewed")]
        DaDanhGia = 5,
        [Display(Name = "Cancelled")]
        DaHuy = 6,
    }

    public class DonHang
    {
        // key 
        [Key]  
        public int Id { get; set; }
        // foreign key
        [ForeignKey("KhachHang")]
        public int KhachHangId { get; set; }

        // order date
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage ="You need to select date.")]
        [Display(Name ="Order Date")]
        public DateTime NgayDatHang { get; set; }

        // total amount
        [Required,Range(0, double.MaxValue, ErrorMessage ="You need to enter price")]
        [Display(Name ="Total Amount")]
        public double TongTien { get; set; }

        // status
        [Display(Name ="Order Status")]
        public TrangThaiDonHang TrangThai { get; set; }

        // notes
        [StringLength(250)]
        [Display(Name ="Notes")]
        public string GhiChu { get; set; }

        public KhachHang KhachHang { get; set; }
        public List<DonhangChitiet> DonhangChitiets { get; set; }
    }
}
