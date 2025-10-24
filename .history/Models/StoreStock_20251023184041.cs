using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class StoreStock
  {
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    [Required]
    public string BatchCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int RemaininQuantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Store? Store { get; set; }

  }
}