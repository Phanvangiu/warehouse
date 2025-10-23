using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class Category
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Descripton { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
  }
}