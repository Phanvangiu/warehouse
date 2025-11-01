namespace warehouse.RequestModels
{
  public class CreatePositionModel
  {
    public required string Title { get; set; }
    public string? Description { get; set; }
  }
}