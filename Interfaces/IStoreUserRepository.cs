using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IStoreUserRepository : IRepository<StoreUser>
  {
    Task<CustomResult> GetStoreUsers();
    Task<CustomResult> CreateStoreUser(CreateStoreUserModel createStoreUserModel);
    Task<CustomResult> TransferStoreUser(CreateStoreUserModel createStoreUserModel);
  }

  public class StoreUserRepository : GenericRepository<StoreUser>, IStoreUserRepository
  {
    public StoreUserRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<CustomResult> GetStoreUsers()
    {
      try
      {
        var list = await _context.StoreUsers.ToListAsync();
        if (list.Count == 0)
          return new CustomResult(400, "List is empty", null!);

        return new CustomResult(200, "Store users retrieved successfully", list);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving store users: {ex.Message}", null!);
      }
    }
    private async Task<(User? user, Store? store, CustomResult? error)> ValidateStoreUserInput(CreateStoreUserModel model)
    {
      if (model is null)
        return (null, null, new CustomResult(400, "Invalid request: store user data cannot be null.", null!));

      if (model.UserId <= 0 || model.StoreId <= 0)
        return (null, null, new CustomResult(400, "Invalid request: StoreId and UserId must be greater than 0.", null!));

      var user = await _context.Users.FindAsync(model.UserId);
      if (user is null)
        return (null, null, new CustomResult(404, "User not found", null!));

      var store = await _context.Stores.FindAsync(model.StoreId);
      if (store is null)
        return (null, null, new CustomResult(404, "Store not found", null!));

      return (user, store, null);
    }
    public async Task<CustomResult> CreateStoreUser(CreateStoreUserModel model)
    {
      try
      {
        var (user, store, error) = await ValidateStoreUserInput(model);
        if (error != null) return error;

        var storeUser = new StoreUser
        {
          StoreId = store!.Id,
          UserId = user!.Id,
          EndDate = null
        };

        _context.StoreUsers.Add(storeUser);
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Store user created successfully", storeUser);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while creating store user: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> TransferStoreUser(CreateStoreUserModel model)
    {
      try
      {
        var (user, store, error) = await ValidateStoreUserInput(model);
        if (error != null) return error;

        var currentStoreUser = await _context.StoreUsers
          .Where(st => st.UserId == model.UserId && st.EndDate == null)
          .SingleOrDefaultAsync();

        if (currentStoreUser != null)
        {
          currentStoreUser.EndDate = DateTime.Now;
          await _context.SaveChangesAsync();
        }

        return await CreateStoreUser(model);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while transferring store user: {ex.Message}", null!);
      }
    }
  }
}
