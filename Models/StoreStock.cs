using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class StoreStock
  {
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int EmployeeId { get; set; }
    public required string BatchCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Store? Store { get; set; }
    public virtual ICollection<StoreStockItem>? StoreStockItems { get; set; }
  }
}