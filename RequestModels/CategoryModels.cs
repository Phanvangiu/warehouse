
namespace warehouse.RequestModels
{
  public class CategoryModel
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Descripton { get; set; }
    public IFormFile Image { get; set; }
  }
}