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
    public string StoreType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    // 1 store - 1 manager (user)
    public int? ManagerId { get; set; }
    public virtual User? Manager { get; set; }
    // 1 store - many staff (users)
    public virtual ICollection<StoreUser> StoreUsers { get; set; }
    public virtual ICollection<StoreStock>? StoreStocks { get; set; }
  }
}