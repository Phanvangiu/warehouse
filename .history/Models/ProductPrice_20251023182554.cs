namespace warehouse.Models
{
  public class ProductPrice
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
  }
}