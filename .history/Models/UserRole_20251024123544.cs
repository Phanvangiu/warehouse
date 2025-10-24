namespace warehouse.Models
{

  public class UserRole
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    // ADMIN, MANAGER, STAFF
    public int RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<User>? Users { get; set; }
  }
}
