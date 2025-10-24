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
    // Task<User> ManagerAuthenticate(RequestLogin account);
    // Task<CustomResult> AdminLogin(RequestLogin account);

  }
  public class UserRepository : GenericRepository<User>, IUserRepository
  {
    public UserRepository(DataContext dataContext) : base(dataContext)
    {

    }
  }


}