using Microsoft.EntityFrameworkCore;
using RegistrationService.Models;

namespace RegistrationService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<DangKy> DangKys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DangKy>(entity =>
        {
            entity.ToTable("DANGKY");

            entity.HasKey(x => x.MaDK);

            entity.Property(x => x.MaDK)
                .HasColumnName("MaDK");

            entity.Property(x => x.MaSV)
                .HasColumnName("MaSV");

            entity.Property(x => x.MaDT)
                .HasColumnName("MaDT");

            entity.Property(x => x.NgayDangKy)
                .HasColumnName("NgayDangKy");

            entity.Property(x => x.Diem)
                .HasColumnName("Diem");
        });
    }
}