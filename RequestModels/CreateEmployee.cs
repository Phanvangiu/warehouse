using warehouse.Models;

namespace warehouse.RequestModels
{
  public class CreateEmployee
  {
    public string Email { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public int RoleId { get; set; }
    public virtual Role? Role { get; set; }
    public IFormFile? Avatar { get; set; }
  }
}