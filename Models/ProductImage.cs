using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class ProductImage
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public required string Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Product? Product { get; set; }
  }
}