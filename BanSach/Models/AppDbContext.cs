using BanSach.Models;
using Microsoft.EntityFrameworkCore;

namespace BanSach.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaiLieu> TaiLieus { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaiLieu>().ToTable("TaiLieu");
            modelBuilder.Entity<DanhMuc>().ToTable("DanhMuc");
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            modelBuilder.Entity<DonHang>().ToTable("DonHang");
            modelBuilder.Entity<ChiTietDonHang>().ToTable("ChiTietDonHang");

            modelBuilder.Entity<TaiLieu>()
                .Property(t => t.GiaSoc)
                .HasPrecision(18, 2);
                
            modelBuilder.Entity<TaiLieu>()
                .Property(t => t.GiaKhuyenMai)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ChiTietDonHang>()
                .Property(c => c.Gia)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DonHang>()
                .Property(d => d.TongTien)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DonHang>()
           .Property(d => d.NgayDat)
           .HasColumnType("datetime");


            base.OnModelCreating(modelBuilder);
        }
    }
}
