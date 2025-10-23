namespace warehouse.Models
{
  public class PositionHistory
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int PositionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public virtual User? Employee { get; set; }
    public virtual Position? Position { get; set; }

  }
}