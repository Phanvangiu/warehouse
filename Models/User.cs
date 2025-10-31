using System.Collections;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace warehouse.Models
{
  public class User
  {
    public int Id { get; set; }
    [EmailAddress]
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Sex { get; set; }
    public bool IsActive { get; set; }
    public DateTime? Dob { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int RoleId { get; set; }
    //1 staff(user) - many store
    public virtual Role? Role { get; set; }
    public virtual ICollection<StoreUser>? StoreUsers { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
  }

}