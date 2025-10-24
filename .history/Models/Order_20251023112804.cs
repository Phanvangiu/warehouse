using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class Order
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int StoreId { get; set; }
    public int OrderItem { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal DisCount { get; set; }
    public decimal Total { get; set; }
    public int Coupon { get; set; }
    [Required]
    // PENDING, PAID, SHIPPED, CANCELLED, COMPLETED, REFUNDED
    public string Status { get; set; } = string.Empty;
    [Required]
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<OrderItem>? OrderItems { get; set; }
    public virtual Store? Store { get; set; }
    public virtual User? User { get; set; }
  }
}