using warehouse.Data;

namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {

    void SaveChanges();
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DataContext _dataContext;
    public UnitOfWork(DataContext dataContext)
    {
      _dataContext = dataContext;

    }
    public void Dispose()
    {
      _dataContext.Dispose();
    }

    public void SaveChanges()
    {
      _dataContext.SaveChanges();
    }
  }
}