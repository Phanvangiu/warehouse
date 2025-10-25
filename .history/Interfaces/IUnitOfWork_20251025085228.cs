using warehouse.Data;
using warehouse.Services;

namespace warehouse.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository UserRepository { get; }
    IMailService MailService { get; }
    void SaveChanges();
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DataContext _dataContext;
    public IUserRepository UserRepository { get; }
    public IMailService MailService { get; }
    public UnitOfWork(DataContext dataContext, IUserRepository userRepository, IMailService mailService)
    {
      _dataContext = dataContext;
      UserRepository = userRepository;
      MailService = mailService;
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