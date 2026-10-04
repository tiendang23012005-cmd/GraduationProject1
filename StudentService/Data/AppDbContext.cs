using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SinhVien> SinhViens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.ToTable("SINHVIEN");

            entity.HasKey(x => x.MaSV);

            entity.Property(x => x.MaSV)
                  .HasColumnName("MaSV");

            entity.Property(x => x.HoTen)
                  .HasColumnName("HoTen");

            entity.Property(x => x.Lop)
                  .HasColumnName("Lop");

            entity.Property(x => x.Email)
                  .HasColumnName("Email");

            entity.Property(x => x.MatKhau)
                  .HasColumnName("MatKhau");

            entity.Property(x => x.Quyen)
                  .HasColumnName("Quyen");
        });
    }
}