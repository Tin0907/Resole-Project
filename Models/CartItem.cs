namespace ASM.Models
{
    public class CartItem
    {
        public int GiayId { get; set; }
        public string Ten { get; set; }
        public string Hinh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string Size { get; set; }
        
        public decimal ThanhTien => Gia * SoLuong;
    }
}
