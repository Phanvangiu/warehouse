using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace warehouse.Models
{
  public class ChangeAvatarModel
  {
    [Required]
    public IFormFile Image { get; set; }
  }
}
