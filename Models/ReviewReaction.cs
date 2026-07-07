using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class ReviewReaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ShopReview")]
        public int ShopReviewId { get; set; }

        [ForeignKey("KhachHang")]
        public int KhachHangId { get; set; }

        /// <summary>
        /// true = Like, false = Dislike
        /// </summary>
        public bool IsLike { get; set; }

        public ShopReview ShopReview { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
