using Microsoft.EntityFrameworkCore;
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
    public void CreateOwner(User owner)
    {
      Add(owner);
    }

    public void DeleteOwner(User owner)
    {
      Delete(owner);
    }
    public void UpdateOwner(User owner)
    {
      Update(owner);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int ownerId)
    {
      return await _context.Users.FindAsync(ownerId);
    }
    public async Task<User> ManagerAuthenticate(RequestLogin account)
    {

      var verified = await _context.Users.Include(u => u.Role)
      .Where(u => u.Email == account.Email && (u.Role!.RoleName == "Admin" || u.Role.RoleName == "Employee")).SingleOrDefaultAsync();

      if (verified != null)
      {
        if (BCrypt.Net.BCrypt.Verify(account.Password, verified.Password))
        {
          return verified;
        }
      }

      return null;
    }
    public async Task<CustomResult> AdminLogin(RequestLogin account)
    {
      if (user == null)
      {
        return new CustomResult(404, "Not Found", null);
      }

      if (user.RoleTypeId == 5 && user.Active == false)
      {
        return new CustomResult(401, "Account is not active", null);
      }

      var token = CreateToken(user);

      return new CustomResult(200, "token", token);

    }
  }
}


