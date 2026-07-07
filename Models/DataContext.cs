using Microsoft.EntityFrameworkCore;
using System;

namespace ASM.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Giay> Giays { get; set; }
        public DbSet<NguoiDung> Nguoidungs{ get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<DonhangChitiet> DonhangChitiets { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ShopReview> ShopReviews { get; set; }
        public DbSet<ReviewReaction> ReviewReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.KhachHang)
                .WithMany()
                .HasForeignKey(c => c.KhachHangId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Giay)
                .WithMany()
                .HasForeignKey(c => c.GiayId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShopReview>()
                .HasOne(r => r.DonHang)
                .WithMany()
                .HasForeignKey(r => r.DonHangId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShopReview>()
                .HasOne(r => r.KhachHang)
                .WithMany()
                .HasForeignKey(r => r.KhachHangId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReviewReaction>()
                .HasOne(r => r.ShopReview)
                .WithMany()
                .HasForeignKey(r => r.ShopReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewReaction>()
                .HasOne(r => r.KhachHang)
                .WithMany()
                .HasForeignKey(r => r.KhachHangId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReviewReaction>()
                .HasIndex(r => new { r.ShopReviewId, r.KhachHangId })
                .IsUnique();
        }
    }
}