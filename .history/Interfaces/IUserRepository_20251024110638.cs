using warehouse.Data;
using warehouse.Models;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IUserRepository : IRepository<User>
  {
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int userId);
    void CreateOwner(User owner);
    void UpdateOwner(User owner);
    void DeleteOwner(User owner);
    Task<User> ManagerAuthenticate(RequestLogin account);
    Task<CustomResult> AdminLogin(RequestLogin account);

  }
  public class UserRepository : GenericRepository<User>, IUserRepository
  {
    private readonly ILogger<UserRepository> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;
    // private readonly IMailService _mailService;
    public UserRepository(DataContext dataContext, ILogger<UserRepository> logger, IConfiguration configuration, IWebHostEnvironment env) : base(dataContext)
    {
      _logger = logger;
      _config = configuration;
      _env = env;
      // _mailService = mailService;
    }
  }


}