using warehouse.Data;
using warehouse.Services;

namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository UserRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    Task SaveChangesAsync();
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DataContext _dataContext;
    public IUserRepository UserRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public UnitOfWork(DataContext dataContext, IUserRepository userRepository, ICategoryRepository categoryRepository)
    {
      _dataContext = dataContext;
      UserRepository = userRepository;
      CategoryRepository = categoryRepository;
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