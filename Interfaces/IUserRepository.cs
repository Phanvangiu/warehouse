using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
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
    Task<User?> ManagerAuthenticate(RequestLogin account);
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
    Task<CustomPaging> GetAllCustomer(int pageNumber, int pageSize, string? searchValue);

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

    public async Task<User?> ManagerAuthenticate(RequestLogin account)
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
        return new CustomResult(404, "Email or password is wrong", null!);
      }

      if (user.IsActive == false)
      {
        return new CustomResult(401, "Account is not active", null!);
      }

      var token = CreateToken(user);

      return new CustomResult(200, "Token", token);

    }
    private string CreateToken(User user)
    {
      var key = _config["JwtSettings:Key"];
      if (string.IsNullOrWhiteSpace(key))
        throw new InvalidOperationException("JWT key is missing in configuration.");

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var roleName = user.Role?.RoleName ?? "User"; // fallback an toàn

      var claims = new[]
      {
        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        new Claim(ClaimTypes.Role, roleName),
        new Claim("Id", user.Id.ToString())
    };

      // create token
      var token = new JwtSecurityToken(
          issuer: _config["JwtSettings:Issuer"],
          audience: _config["JwtSettings:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddDays(7),
          signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<bool> CheckEmailExist(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return false;

      return await _context.Users.AnyAsync(u => u.Email == email);
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
      try
      {
        var verifiedEmail = await CheckEmailExist(account.Email!);
        if (verifiedEmail)
          return new CustomResult(400, "Email already exists.", null!);

        var verifiedPhone = await CheckPhoneExist(account.Phone!);
        if (verifiedPhone)
          return new CustomResult(400, "Phone number already exists.", null!);

        var customerRole = await _context.Roles.SingleOrDefaultAsync(ur => ur.RoleName == "Customer");
        if (customerRole == null)
          return new CustomResult(404, "Customer role not found.", null!);

        var customer = new User
        {
          Email = account.Email!,
          Password = BCrypt.Net.BCrypt.HashPassword(account.Password),
          IsActive = true,
          Phone = account.Phone,
          Name = account.Name,
          RoleId = customerRole.Id,
        };

        _context.Users.Add(customer);
        await _context.SaveChangesAsync();

        var token = CreateToken(customer);

        _ = _mailService.SendMail(new MailRequest(
                    customer.Email,
                    "Verify Email",
                    $"<h1>Thank you for registering</h1>" +
                    $"<p>Please verify your email by clicking the following link:</p>" +
                    $"<h3>{token}</h3>"
                // $"<a href='{_config["AppSettings:ClientURL"]}?token={token}'>Verify Email</a></p>"
                ))
            .ContinueWith(t =>
            {
              if (t.IsFaulted)
                _logger.LogError(t.Exception, "Error sending verification email to {Email}", customer.Email);
            });

        return new CustomResult(200, "Account created successfully. Please verify your email.", customer);
      }
      catch (DbUpdateException dbEx)
      {
        _logger.LogError(dbEx, "Database error while creating customer");
        return new CustomResult(500, "A database error occurred while creating the account.", null!);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unexpected error while creating customer");
        return new CustomResult(500, "An unexpected error occurred while creating the account.", null!);
      }
    }
    public async Task<CustomResult> CreateEmployee(CreateEmployee account)
    {
      try
      {
        if (account == null)
          return new CustomResult(400, "Invalid request: employee data is missing.", null!);

        if (string.IsNullOrWhiteSpace(account.Email))
          return new CustomResult(400, "Email is required.", null!);

        if (string.IsNullOrWhiteSpace(account.Phone))
          return new CustomResult(400, "Phone is required.", null!);


        if (await CheckEmailExist(account.Email))
          return new CustomResult(400, "Email already exists.", null!);

        if (await CheckPhoneExist(account.Phone))
          return new CustomResult(400, "Phone number already exists.", null!);
        var employeeNew = new User
        {
          Email = account.Email,
          Dob = account.Dob,
          Password = BCrypt.Net.BCrypt.HashPassword(account.Password),
          Name = account.Name,
          RoleId = account.RoleId,
          Phone = account.Phone,
          IsActive = true,
        };

        if (account.Avatar != null && account.Avatar.Length > 0)
        {
          var folderName = "avatars";
          var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(account.Avatar.FileName)}";

          var uploadPath = Path.Combine(
              _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
              "images", folderName);

          if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

          var filePath = Path.Combine(uploadPath, fileName);
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await account.Avatar.CopyToAsync(stream);
          }

          var request = _httpContextAccessor.HttpContext?.Request;
          employeeNew.Avatar = request != null
              ? $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}"
              : $"/images/{folderName}/{fileName}";
        }

        _context.Users.Add(employeeNew);
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Employee created successfully.", employeeNew);
      }
      catch (DbUpdateException ex)
      {
        return new CustomResult(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}", null!);
      }
      catch (IOException ex)
      {
        return new CustomResult(500, $"File upload error: {ex.Message}", null!);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"Unexpected error: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> GetAllCustomer()
    {
      try
      {
        var customer = await _context.Users.Where(u => u.Role!.RoleName == "Customer").ToListAsync();
        if (customer == null)
        {
          return new CustomResult(200, "Not found", null!);
        }
        return new CustomResult(200, "List of customers", customer);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving customers: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> GetAllEmployee()
    {
      try
      {
        var employees = await _context.Users
          .Include(u => u.Role)
          .Where(u => u.Role!.RoleName == "Employee")
          .ToListAsync();

        if (employees.Count == 0)
        {
          return new CustomResult(200, "List empty", null!);
        }

        return new CustomResult(200, "Employees retrieved successfully.", employees);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving employees: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> GetUser(string email)
    {
      try
      {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        if (user == null)
          return new CustomResult(404, "User not found.", null!);

        var userResult = _mapper.Map<UserResult>(user);

        return new CustomResult(200, "User retrieved successfully.", userResult);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving the user: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> ChangePassword(int userId, ChangePasswordModel model)
    {
      try
      {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
          return new CustomResult(404, "User not found.", null!);

        if (!BCrypt.Net.BCrypt.Verify(model.PreviousPassword, user.Password))
          return new CustomResult(400, "Incorrect current password.", null!);

        if (BCrypt.Net.BCrypt.Verify(model.NewPassword, user.Password))
          return new CustomResult(400, "New password cannot be the same as the current password.", null!);

        user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Password changed successfully.", null!);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while changing the password: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> ActivateEmployee(int userId)
    {
      try
      {
        if (userId <= 0)
          return new CustomResult(400, " Invalid request: Employee Id must be greater than 0 ", null!);

        var employee = await _context.Users.FindAsync(userId);

        if (employee is null)
          return new CustomResult(404, "Employee not found.", null!);

        if (employee.IsActive)
          return new CustomResult(400, "Employee is already active.", employee);

        employee.IsActive = true;
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Employee activated successfully.", employee);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while activating the employee: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> DeactivateEmployee(int userId)
    {
      try
      {
        if (userId <= 0)
          return new CustomResult(400, " Invalid request: Employee Id must be greater than 0 ", null!);

        var employee = await _context.Users.FindAsync(userId);

        if (employee is null)
          return new CustomResult(404, "Employee not found.", null!);

        if (!employee.IsActive)
          return new CustomResult(400, "Employee is already deactivated.", employee);

        employee.IsActive = false;
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Employee deactivated successfully.", employee);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while deactivating the employee: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> ChangeUserImage(string email, IFormFile image)
    {
      try
      {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
          return new CustomResult(404, "User not found.", null!);

        if (image == null || image.Length == 0)
          return new CustomResult(400, "No image file uploaded.", null!);

        var folderName = "avatars";
        var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(image.FileName)}";

        // Tạo đường dẫn upload (wwwroot/images/avatars)
        var uploadPath = Path.Combine(
            _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            "images", folderName);

        if (!Directory.Exists(uploadPath))
          Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);

        // Upload file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await image.CopyToAsync(stream);
        }

        // Lấy URL public
        var request = _httpContextAccessor.HttpContext?.Request;
        var imageUrl = request != null
            ? $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}"
            : $"/images/{folderName}/{fileName}";

        // Cập nhật avatar
        user.Avatar = imageUrl;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new CustomResult(200, "User avatar updated successfully.", user);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while updating the user image: {ex.Message}", null!);
      }
    }
    public async Task<CustomPaging> GetAllCustomer(int pageNumber, int pageSize, string? searchValue)
    {
      if (pageNumber <= 0) pageNumber = 1;
      if (pageSize <= 0) pageSize = 10;
      try
      {
        var query = _context.Users.AsNoTracking().Include(u => u.Orders).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchValue))
        {
          string normalizedSearch = searchValue.Trim().ToLower();
          query = query.Where(u => u.Name!.ToLower().Contains(normalizedSearch));
        }
        var total = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
        return new CustomPaging
        {
          Status = 200,
          Message = items.Any() ? "Users retrieved successfully." : "List empty",
          CurrentPage = pageNumber,
          PageSize = pageSize,
          TotalCount = total,
          TotalPages = (int)Math.Ceiling((double)total / pageSize),
          Data = items
        };

      }
      catch (Exception ex)
      {
        return new CustomPaging
        {
          Status = 500,
          Message = $"An error occurred while retrieving users: {ex.Message}",
          CurrentPage = 0,
          PageSize = pageSize,
          TotalCount = 0,
          Data = null
        };
      }

    }
  }
}


