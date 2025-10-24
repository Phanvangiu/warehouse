namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {

    void SaveChanges();
  }
}