using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class Position
  {
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<PositionHistory>? PositionHistories { get; set; }
  }
}
