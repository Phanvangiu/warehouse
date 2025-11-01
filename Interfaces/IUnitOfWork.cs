using warehouse.Data;
using warehouse.Services;

namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository UserRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    IStoreRepository StoreRepository { get; }
    IPositionRepository PositionRepository { get; }
    Task SaveChangesAsync();
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DataContext _dataContext;
    public IUserRepository UserRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IStoreRepository StoreRepository { get; }
    public IPositionRepository PositionRepository { get; }


    public UnitOfWork(DataContext dataContext, IUserRepository userRepository, ICategoryRepository categoryRepository, IProductRepository productRepository, IStoreRepository storeRepository, IPositionRepository positionRepository)
    {
      _dataContext = dataContext;
      UserRepository = userRepository;
      CategoryRepository = categoryRepository;
      ProductRepository = productRepository;
      StoreRepository = storeRepository;
      PositionRepository = positionRepository;
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