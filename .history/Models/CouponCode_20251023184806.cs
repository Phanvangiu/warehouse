using System.ComponentModel.DataAnnotations;

namespace warehouse.Models
{
  public class CouponCode
  {
    public int Id { get; set; }
    [Required]
    public string Code { get; set; } = string.Empty;
    public int PromotionId { get; set; }
    public int UsageLimit { get; set; }
    public int UseCount { get; set; }
    public int UserId { get; set; }
    public DateTime ExpriedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}