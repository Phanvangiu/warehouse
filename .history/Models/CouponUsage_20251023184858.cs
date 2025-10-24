namespace warehouse.Models
{
  public class CouponUsage
  {
    public int Id { get; set; }
    public int CouponCode { get; set; }
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime UseAt { get; set; }

  }
}