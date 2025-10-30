namespace warehouse.RequestModels
{
  public class CreateProductModel
  {
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? Descripton { get; set; }
    public decimal DefaultPrice { get; set; }
    public string? Unit { get; set; }
    public bool IsActive { get; set; }
    public ICollection<IFormFile> Images { get; set; }
  }
}