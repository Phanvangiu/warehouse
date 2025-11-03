using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class StockTransaction
  {
    public int Id { get; set; }
    public int StoreStockId { get; set; }

    // IMPORT, SALE, ADJUST, TRANSFER_OUT, TRANSFER_IN
    public required string Type { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual StoreStock? StoreStock { get; set; }
  }
}