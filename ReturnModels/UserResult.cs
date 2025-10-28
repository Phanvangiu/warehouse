using System.ComponentModel.DataAnnotations;

namespace warehouse.ReturnModels
{
  public class UserResult
  {
    public int? Id { get; set; }
    public string? Email { get; set; }
    [Required]
    public string? Avatar { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Sex { get; set; }
    public bool IsActive { get; set; }
    public DateTime Dob { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}