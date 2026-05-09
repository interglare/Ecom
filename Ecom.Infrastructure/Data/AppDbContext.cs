using Microsoft.EntityFrameworkCore;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;

namespace Ecom.Infrastructure.Data;

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();

    public DbSet<ProductGroupItem> ProductGroupItems => Set<ProductGroupItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(x => x.Unit)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.IsProcessed)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<ProductGroup>(entity =>
        {
            entity.ToTable("product_groups");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasMany(x => x.Items)
                .WithOne(x => x.ProductGroup)
                .HasForeignKey(x => x.ProductGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductGroupItem>(entity =>
        {
            entity.ToTable("product_group_items");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(x => x.Unit)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Ignore(x => x.TotalPrice);
        });
    }
}