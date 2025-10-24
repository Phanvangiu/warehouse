namespace warehouse.Models
{
  public class Role
  {
    public int Id { get; set; }
    public string? RoleName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}