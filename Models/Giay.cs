using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public enum PhanLoai
    {
        [Display(Name="Leather Shoes")]
        GiayDa = 1,
        [Display(Name = "Sneakers")]
        GiaySneaker = 2,
        [Display(Name = "Sports Shoes")]
        GiayTheThao = 3,
        [Display(Name = "Special Shoe")]
        SpecialShoe = 4
    }

    public enum ProductStatus
    {
        InStock = 1,    // in_stock
        OutOfStock = 2, // out_of_stock
        Locked = 3      // locked by admin
    }

    [Table("MonAns")]
    public class Giay
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Shoe name is required")]
        [StringLength(100, ErrorMessage = "Shoe name cannot exceed 100 characters")]
        [Display(Name = "Shoe Name")]
        public string Ten { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 1000000, ErrorMessage = "Price must be between 0 and 1,000,000")]
        [Display(Name = "Price")]
        public float Gia { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public PhanLoai PhanLoai { get; set; }

        [StringLength(200, ErrorMessage = "Image path cannot exceed 200 characters")]
        [Display(Name = "Image")]
        public string? Hinh { get; set; }

        [NotMapped]
        [Display(Name = "Select Image")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "In Service")]
        public bool TrangThai { get; set; }

        // Inventory fields
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Product Status")]
        public ProductStatus Status { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}
