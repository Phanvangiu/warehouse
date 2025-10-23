namespace warehouse.Models
{
  public class StoreUser
  {
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int UserId { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime EndDate { get; set; }
    public virtual Store? Store { get; set; }
    public virtual User? User { get; set; }



  }
}