using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class StockTransfer
  {
    public int Id { get; set; }
    public int FromStoreId { get; set; }
    public int ToStoreId { get; set; }
    public int ProductId { get; set; }
    public string? BatchCode { get; set; }
    public int Quantity { get; set; }
    [Required]
    // PENDING, COMPLETED, CANCELLED
    public string Status { get; set; } = string.Empty;
    [Required]
    // WAREHOUSE_TO_STORE, STORE_TO_STORE
    public string StranferType { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CraetedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    public virtual Product? Product
    {
      get;
      set;
    }
  }
}