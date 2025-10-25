using warehouse.Data;
using warehouse.Services;

namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository UserRepository { get; }
    Task SaveChangesAsync();
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

    public async Task SaveChangesAsync()
    {
      await _dataContext.SaveChangesAsync();
    }
  }
}