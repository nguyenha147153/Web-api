using Microsoft.EntityFrameworkCore;

namespace MyWebApiApp.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) {}

        #region DbSet
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<HangHoa> HangHoas { get; set; }
        public DbSet<Loai> Loais { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<DonHangChiTiet> DonHangChiTiets { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>(e =>
            {
                e.ToTable("DonHang");
                e.HasKey(dh => dh.MaDH);
                e.Property(dh => dh.NgayDat).HasDefaultValueSql("getutcdate()");
                e.Property(dh => dh.NguoiNhan).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<DonHangChiTiet>(e =>
            {
                e.ToTable("ChiTietDonHang");
                e.HasKey(e => new { e.MaDH, e.MaHH });
                e.HasOne(e => e.DonHang)
                 .WithMany(e=>e.DonHangChiTiets)
                 .HasForeignKey(e=>e.MaDH)
                 .HasConstraintName("FK_DonHangCT_DonHang");
                e.HasOne(e => e.HangHoa)
                 .WithMany(e => e.DonHangChiTiets)
                 .HasForeignKey(e => e.MaHH)
                 .HasConstraintName("FK_DonHangCT_HangHoa");
            });
            modelBuilder.Entity<NguoiDung>(e =>
            {
                e.HasIndex(e => e.UserName).IsUnique();
                e.Property(e => e.HoTen).IsRequired().HasMaxLength(150);
                e.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });
        }
    }
}
