using System.ComponentModel.DataAnnotations;

namespace warehouse.RequestModels
{
  public class ChangeAvatarModel
  {
    [Required]
    public IFormFile Image { get; set; }
  }
}
