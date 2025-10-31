namespace warehouse.RequestModels
{
  public class CreateStoreModel
  {
    public string Code { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? StoreType { get; set; }
  }
}