using System.Collections.ObjectModel;
using MyWebApi.Namespace;

namespace warehouse.RequestModels
{
  public class CreateOrderModel
  {
    public int UserId { get; set; }
    public int EmployeeId { get; set; }
    public int StoreId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public decimal DisCount { get; set; }
    public int? Coupon { get; set; }
    //(PENDING, PAID, SHIPPED, CANCELLED, COMPLETED, REFUNDED)
    public required string Status { get; set; }
    public required string PaymentMethod { get; set; }
    public required IReadOnlyCollection<CreateOrderItemModel> OrderItems { get; set; }
  }
}
