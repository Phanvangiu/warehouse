using System.ComponentModel.DataAnnotations;

namespace warehouse.RequestModels
{
  public class ChangeAvatarModel
  {
    public required IFormFile Image { get; set; }
  }
}
