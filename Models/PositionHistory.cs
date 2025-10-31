namespace warehouse.Models
{
  public class PositionHistory
  {
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public int StoreId { get; set; } // thêm dòng này để biết làm ở store nào
    public int PositionId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // null = vẫn đang làm

    public virtual User? Employee { get; set; }
    public virtual Store? Store { get; set; }
    public virtual Position? Position { get; set; }
  }
}
