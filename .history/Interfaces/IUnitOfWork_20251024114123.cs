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
    public IUserRepository UserRepository { get; }
    public UnitOfWork(DataContext dataContext, IUserRepository userRepository)
    {
      _dataContext = dataContext;
      UserRepository = userRepository;
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