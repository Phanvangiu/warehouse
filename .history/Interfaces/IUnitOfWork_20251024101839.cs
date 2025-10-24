using warehouse.Data;

namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {

    void SaveChanges();
  }
  public class UnitOfWork : IUnitOfWork
  {
    public UnitOfWork(DataContext dataContext)
    {
      _dataContext = dataContext;

    }
    public void Dispose()
    {
      throw new NotImplementedException();
    }

    public void SaveChanges()
    {
      throw new NotImplementedException();
    }
  }
}