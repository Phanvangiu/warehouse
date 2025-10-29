using warehouse.Data;
using warehouse.Services;

namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository UserRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    Task SaveChangesAsync();
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DataContext _dataContext;
    public IUserRepository UserRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }

    public UnitOfWork(DataContext dataContext, IUserRepository userRepository, ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
      _dataContext = dataContext;
      UserRepository = userRepository;
      CategoryRepository = categoryRepository;
      ProductRepository = productRepository;
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