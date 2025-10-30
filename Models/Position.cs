using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class Position
  {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<PositionHistory>? PositionHistories { get; set; }
  }
}
