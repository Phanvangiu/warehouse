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
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionProduct> PromotionProducts { get; set; }
    public DbSet<StockStranfer> StockStranfers { get; set; }
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
      modelBuilder.Entity<User>().HasOne(u => u.Role)
      .WithMany(u => Users)
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

    }
  }
}