namespace warehouse.RequestModels
{
  public class CreateStoreStockModel
  {
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public required string BatchCode { get; set; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpiryDate { get; set; }

  }
}