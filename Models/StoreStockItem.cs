namespace warehouse.Models
{
  public class StoreStockItem
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int StoreStockId { get; set; }
    public int RemaininQuantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Product? Product { get; set; }
    public virtual StoreStock? StoreStock { get; set; }
  }
}