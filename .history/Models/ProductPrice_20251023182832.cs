namespace warehouse.Models
{
  public class ProductPrice
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int StoreId { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Store? Store { get; set; }
    public virtual Product? Product { get; set; }
  }
}