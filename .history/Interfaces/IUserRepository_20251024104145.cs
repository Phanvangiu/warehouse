using warehouse.Models;

namespace warehouse.Interfaces
{
  public interface IUserRepository : IRepository<User>
  {
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int userId);
    void CreateOwner(User owner);
    void UpdateOwner(User owner);
    void DeleteOwner(User owner);
  }


}