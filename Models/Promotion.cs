namespace warehouse.Models
{
  public class Promotion
  {
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Tilte { get; set; }
    // PERCENT, FIXED, BUY_X_GET_Y, FLASH
    public string Type { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public int BuyQuantity { get; set; }
    public int GetQuantity { get; set; }
    public string ApplyTo { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MinOrderValue { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

  }
}