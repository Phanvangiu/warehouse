namespace warehouse.Models
{
  public class Wallet
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User? User { get; set; }

  }
}