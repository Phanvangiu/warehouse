namespace warehouse.Models
{

  public class UserRole
  {
    public int Id { get; set; }
    // ADMIN, MANAGER, STAFF
    public string RollName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<User>? Users { get; set; }
  }
}
