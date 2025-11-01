namespace warehouse.RequestModels
{
  public class ChangePasswordModel
  {
    public required string PreviousPassword { get; set; }

    public required string NewPassword { get; set; }
  }
}