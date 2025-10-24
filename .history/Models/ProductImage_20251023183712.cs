using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class ProductImage
  {
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    public string Image { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Product? Product { get; set; }
  }
}