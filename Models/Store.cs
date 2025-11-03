using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class Store
  {
    public int Id { get; set; }
    [Required]
    public string Code { get; set; } = string.Empty;

    public string? Address { get; set; }
    public string? Phone { get; set; }
    public required string StoreType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<StoreUser>? StoreUsers { get; set; }
    public virtual ICollection<StoreStock>? StoreStocks { get; set; }
  }
}
