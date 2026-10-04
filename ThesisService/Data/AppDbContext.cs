using Microsoft.EntityFrameworkCore;
using ThesisService.Models;

namespace ThesisService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<DeTai> DeTais => Set<DeTai>();

    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeTai>(entity =>
        {
            entity.ToTable("DETAI");

            entity.HasKey(x => x.MaDT);

            entity.Property(x => x.MaDT)
                .HasColumnName("MaDT");

            entity.Property(x => x.TenDT)
                .HasColumnName("TenDT");

            entity.Property(x => x.MoTa)
                .HasColumnName("MoTa");

            entity.Property(x => x.Diem)
                .HasColumnName("Diem");
        });
    }
}