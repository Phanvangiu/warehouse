namespace warehouse.Models
{
  public class WalletTransaction
  {
    public int Id { get; set; }
    public int WalletId { get; set; }
    public string? Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalenceAfter { get; set; }
    public int RelatedOrderId { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}