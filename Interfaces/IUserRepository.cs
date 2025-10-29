using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;
using warehouse.Services;

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
    Task<CustomResult> Login(RequestLogin account);
    Task<bool> CheckEmailExist(string email);
    Task<bool> CheckPhoneExist(string phone);
    Task<CustomResult> CreateCustomer(CreateCustomerModel account);
    Task<CustomResult> CreateEmployee(CreateEmployee account);
    Task<CustomResult> GetAllCustomer();
    Task<CustomResult> GetAllEmployee();
    Task<CustomResult> GetUser(string email);
    Task<CustomResult> ChangePassword(int userId, ChangePasswordModel changePasswordRequest);
    Task<CustomResult> ActivateEmployee(int userId);
    Task<CustomResult> DeactivateEmployee(int userId);
    Task<CustomResult> ChangeUserImage(string email, IFormFile image);

  }
  public class UserRepository : GenericRepository<User>, IUserRepository
  {

    private readonly ILogger<UserRepository> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;
    private readonly IMailService _mailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    public UserRepository(DataContext dataContext, ILogger<UserRepository> logger, IConfiguration configuration, IWebHostEnvironment env, IMailService mailService, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(dataContext)
    {
      _logger = logger;
      _config = configuration;
      _env = env;
      _mailService = mailService;
      _httpContextAccessor = httpContextAccessor;
      _mapper = mapper;
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
      .Where(u => u.Email == account.Email).SingleOrDefaultAsync();
      if (verified != null)
      {
        if (BCrypt.Net.BCrypt.Verify(account.Password, verified.Password))
        {
          return verified;
        }
      }
      return null;
    }
    public async Task<CustomResult> Login(RequestLogin account)
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
      if (email == "")
      {
        return true;
      }
      var verified = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
      if (verified == null)
      {
        return false;
      }
      return true;
    }
    public async Task<bool> CheckPhoneExist(string phone)
    {
      var verified = await _context.Users.SingleOrDefaultAsync(u => u.Phone == phone);
      if (verified == null)
      {
        return false;
      }
      return true;
    }
    public async Task<CustomResult> CreateCustomer(CreateCustomerModel account)
    {
      var verifiedEmail = await CheckEmailExist(account.Email);

      if (verifiedEmail == true)
      {
        return new CustomResult(400, "Email already exist", null);
      }

      var verifiedPhone = await CheckPhoneExist(account.Phone);

      if (verifiedPhone == true)
      {
        return new CustomResult(400, "Phone number already exist", null);
      }
      var customerRole = await _context.Roles.SingleOrDefaultAsync(ur => ur.RoleName == "Customer");
      var customer = new User()
      {
        Email = account.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(account.Password),
        IsActive = true,
        Phone = account.Phone,
        Name = account.Name,
        Role = customerRole,
        RoleId = customerRole.Id
      };
      _context.Users.Add(customer);
      await _context.SaveChangesAsync();
      var token = CreateToken(customer);
      _ = _mailService.SendMail(new MailRequest(customer.Email, "Verify Email",
                  $"<h1>Thank you for registering</h1>" +
                         $"<p>Please verify your email by clicking the following link: </p>" +
                         $"<h1>{token}</h1>"
                         //  $"<a href='{_config["AppSettings:ClientURL"]}?token={token}'>Verify Email</a></p>"
                         ))
              .ContinueWith(t =>
              {
                if (t.IsFaulted)
                {
                  _logger.LogError(t.Exception, "Some Exception in Test");
                }
              });
      return new CustomResult(200, "Account created successfully. Please verify your email.", customer);
    }
    public async Task<CustomResult> CreateEmployee(CreateEmployee account)
    {
      var verifiedEmail = await CheckEmailExist(account.Email);

      if (verifiedEmail == true)
      {
        return new CustomResult(400, "Email already exist", null);
      }
      if (account.Phone != null)
      {
        var verifiedPhone = await CheckPhoneExist(account.Phone);

        if (verifiedPhone == true)
        {
          return new CustomResult(400, "Phone number already exist", null);
        }
      }
      return new CustomResult(200, "Success", null);
    }

    public async Task<CustomResult> GetAllCustomer()
    {
      var customer = await _context.Users.Where(u => u.Role.RoleName == "Customer").ToListAsync();
      if (customer == null)
      {
        return new CustomResult(200, "Not found", null);
      }
      return new CustomResult(200, "List of customers", customer);
    }
    public async Task<CustomResult> GetAllEmployee()
    {
      var employees = await _context.Users.Where(u => u.Role!.RoleName == "Employee").ToListAsync();
      if (employees == null)
      {
        return new CustomResult(200, "Not found", null);
      }
      return new CustomResult(200, "List of customers", employees);
    }
    public async Task<CustomResult> GetUser(string email)
    {
      var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
      var userResult = _mapper.Map<UserResult>(user);
      return new CustomResult(200, "user", userResult);
    }
    public async Task<CustomResult> ChangePassword(int userId, ChangePasswordModel model)
    {
      var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
      if (user == null)
        return new CustomResult(404, "User not found", null);

      if (!BCrypt.Net.BCrypt.Verify(model.PreviousPassword, user.Password))
        return new CustomResult(400, "Wrong password", null);

      if (BCrypt.Net.BCrypt.Verify(model.NewPassword, user.Password))
        return new CustomResult(400, "New password cannot be same as old password", null);

      user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
      _context.Users.Update(user);
      await _context.SaveChangesAsync();

      return new CustomResult(200, "Password changed successfully", null);
    }
    public async Task<CustomResult> ActivateEmployee(int userId)
    {
      var employee = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
      if (employee == null)
      { return new CustomResult(404, "User not found", null); }
      employee.IsActive = true;
      _context.Users.Update(employee);
      await _context.SaveChangesAsync();
      return new CustomResult(200, "Success", employee);

    }
    public async Task<CustomResult> DeactivateEmployee(int userId)
    {
      var employee = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
      if (employee == null)
      { return new CustomResult(404, "User not found", null); }
      employee.IsActive = false;
      _context.Users.Update(employee);
      await _context.SaveChangesAsync();
      return new CustomResult(200, "Success", employee);
    }
    public async Task<CustomResult> ChangeUserImage(string email, IFormFile image)
    {
      try
      {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
          return new CustomResult(404, "User not found", null);

        if (image == null || image.Length == 0)
          return new CustomResult(400, "No image file uploaded", null);

        var folderName = "avatars";
        var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(image.FileName)}";

        // Đường dẫn thư mục gốc lưu ảnh
        var uploadPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images", folderName);

        if (!Directory.Exists(uploadPath))
          Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await image.CopyToAsync(stream);
        }

        // Lấy URL public
        string imageUrl;
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request != null)
          imageUrl = $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}";
        else
          imageUrl = $"/images/{folderName}/{fileName}";

        // Cập nhật user
        user.Avatar = imageUrl;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Success", user);
      }
      catch (Exception ex)
      {
        return new CustomResult(400, "Failed", ex.Message);
      }
    }

  }
}


