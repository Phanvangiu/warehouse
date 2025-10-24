using Microsoft.EntityFrameworkCore;
using warehouse.Models;

namespace warehouse.Data
{

  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CouponCode> CouponCodes { get; set; }
    public DbSet<CouponUsage> CouponUsages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<PositionHistory> PositionHistories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionProduct> PromotionProducts { get; set; }
    public DbSet<StockTransfer> StockTransfers { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreStock> StoreStocks { get; set; }
    public DbSet<StoreUser> StoreUsers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<UserRole>()
      .HasOne<User>()
      .WithOne()
      .HasForeignKey<UserRole>(ur => ur.UserId)
      .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<Store>()
        .HasOne<User>()
        .WithMany()
        .HasForeignKey(s => s.ManagerId)
        .OnDelete(DeleteBehavior.Restrict);
      modelBuilder.Entity<StoreUser>()
        .HasOne(su => su.Store)
        .WithMany()
        .HasForeignKey(su => su.StoreId)
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<StoreUser>()
       .HasOne(su => su.User)
       .WithMany()
       .HasForeignKey(su => su.UserId)
       .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Wallet>()
        .HasOne(w => w.User)
        .WithOne()
        .HasForeignKey<Wallet>(w => w.UserId)
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<WalletTransaction>()
        .HasOne(wt => wt.Wallet)
        .WithMany()
        .HasForeignKey(wt => wt.WalletId)
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<Product>()
        .HasOne(p => p.Category)
        .WithMany()
        .HasForeignKey(p => p.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<ProductImage>()
        .HasOne(pi => pi.Product)
        .WithMany()
        .HasForeignKey(pi => pi.ProductId)
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<ProductPrice>()
        .HasOne(pp => pp.Product)
        .WithMany()
        .HasForeignKey(pp => pp.ProductId)
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<ProductPrice>()
          .HasOne(pp => pp.Store)
          .WithMany()
          .HasForeignKey(pp => pp.StoreId)
          .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<StoreStock>()
              .HasOne(ss => ss.Store)
              .WithMany()
              .HasForeignKey(ss => ss.StoreId)
              .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<StoreStock>()
          .HasOne(ss => ss.Product)
          .WithMany()
          .HasForeignKey(ss => ss.ProductId)
          .OnDelete(DeleteBehavior.Restrict);
      modelBuilder.Entity<StockTransaction>()
                .HasOne(st => st.StoreStock)
                .WithMany()
                .HasForeignKey(st => st.StoreStockId)
                .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<StockTransaction>()
          .HasOne<User>()
          .WithMany()
          .HasForeignKey(st => st.CreatedBy)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<StockTransfer>()
          .HasOne(st => st.Product)
          .WithMany()
          .HasForeignKey(st => st.ProductId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<StockTransfer>()
          .HasOne<User>()
          .WithMany()
          .HasForeignKey(st => st.CreatedBy)
          .OnDelete(DeleteBehavior.Restrict);
      modelBuilder.Entity<User>()
        .HasOne(u => u.Role)
        .WithMany(u => u.Users)
        .HasForeignKey(r => r.RoleId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Product>()
        .HasOne(p => p.Category)
        .WithMany(c => c.Products)
        .HasForeignKey(p => p.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
        .HasOne(o => o.User)
        .WithMany(u => u.Orders)
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
        .HasMany(o => o.OrderItems)
        .WithOne(oi => oi.Order)
        .HasForeignKey(oi => oi.OrderId)
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<Order>(option =>
        {
          option.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
          option.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });
    }
  }
}