
namespace warehouse.RequestModels
{
  public class CategoryModel
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Descripton { get; set; }
    public IFormFile? Image { get; set; }
  }
}