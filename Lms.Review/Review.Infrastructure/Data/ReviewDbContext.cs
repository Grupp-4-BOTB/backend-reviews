using Microsoft.EntityFrameworkCore;
using Review.Domain.Entities;

namespace Review.Infrastructure.Data;

public class ReviewDbContext : DbContext
{
    public ReviewDbContext(DbContextOptions<ReviewDbContext> options): base(options)
    {

    }

    public DbSet<ReviewEntity> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ReviewEntity>()
            .ToTable("Reviews", "reviews");

        modelBuilder.Entity<ReviewEntity>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<ReviewEntity>()
            .Property(r => r.Comment)
            .IsRequired()
            .HasMaxLength(1000);
    }
}