namespace warehouse.RequestModels
{
  public class CreateStockTransferModel
  {
    public int FromStoreId { get; set; }
    public int ToStoreId { get; set; }
    public int ProductId { get; set; }
    public required string BatchCode { get; set; }
    public int Quantity { get; set; }
    // PENDING, COMPLETED, CANCELLED
    public required string Status { get; set; }
    // WAREHOUSE_TO_STORE, STORE_TO_STORE
    public required string StranferType { get; set; }
    public string? Note { get; set; }
  }
}