
namespace warehouse.RequestModels
{
  public class CreateCustomerModel
  {
    public required string Email { get; set; }

    public required string Password { get; set; }

    public string? Phone { get; set; }

    public string? Name { get; set; }

  }
}
