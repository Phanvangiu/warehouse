using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class Product
  {
    public int Id { get; set; }
    public required string ProductName { get; set; }
    public int CategoryId { get; set; }
    public string? Descripton { get; set; }
    public decimal DefaultPrice { get; set; }
    public string? Unit { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<ProductImage> ProductImages { get; set; } = [];

  }
}