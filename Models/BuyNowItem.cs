namespace ASM.Models
{
    public class BuyNowItem
    {
        public int GiayId { get; set; }
        public string TenGiay { get; set; }
        public string Hinh { get; set; }
        public double GiaGiay { get; set; }
        public int SoLuong { get; set; }
        public string Size { get; set; }
        
        public double ThanhTien => GiaGiay * SoLuong;
    }
}
