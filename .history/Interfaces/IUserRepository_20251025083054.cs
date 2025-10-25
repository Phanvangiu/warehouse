using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
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
    Task<bool> CheckEmailExist(string email);
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
      Console.WriteLine($"Login attempt for email: {account.Email}");
      Console.WriteLine($"Login attempt for email: {account.Password}");
      string hash = BCrypt.Net.BCrypt.HashPassword(account.Password, 12);

      var verified = await _context.Users.Include(u => u.Role)
      .Where(u => u.Email == account.Email && (u.RoleId == 1 || u.RoleId == 2)).SingleOrDefaultAsync();


      if (verified != null)
      {
        // Console.WriteLine($"account.Password: {account.Password}");
        // Console.WriteLine($"verified.Password: {verified.Password}");
        // Console.WriteLine($"Checkpasss: {BCrypt.Net.BCrypt.Verify(account.Password, verified.Password)}");
        // Console.WriteLine($"hashpass: {hash}");
        if (BCrypt.Net.BCrypt.Verify(account.Password, verified.Password))
        {
          return verified;
        }

      }

      return null;
    }
    public async Task<CustomResult> AdminLogin(RequestLogin account)
    {
      var user = await ManagerAuthenticate(account);
      if (user == null)
      {
        return new CustomResult(404, "Not Found", null);
      }

      if (user.Role!.RoleName == "Admin" && user.IsActive == false)
      {
        return new CustomResult(401, "Account is not active", null);
      }

      var token = CreateToken(user);

      return new CustomResult(200, "token", token);

    }
    private string CreateToken(User user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Role, user.Role.RoleName),
                  new Claim("Id", user.Id.ToString()),
              };

      var token = new JwtSecurityToken(
              issuer: _config["JwtSettings:Issuer"],
              audience: _config["JwtSettings:Audience"],
              signingCredentials: credentials,
              claims: claims,
              expires: DateTime.Now.AddDays(7)
          );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<bool> CheckEmailExist(string email)
    {
      try
      {
        var verified = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (verified == null)
        {
          return false;
        }
        return true;
      }
      catch (System.Exception)
      {
        return false;
      }
    }
  }
}


