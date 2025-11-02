using warehouse.Models;

namespace warehouse.RequestModels
{
  public class CreateEmployee
  {
    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string Name { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public int RoleId { get; set; }
    public IFormFile? Avatar { get; set; }
  }
}