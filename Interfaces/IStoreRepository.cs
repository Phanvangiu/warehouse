using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IStoreRepository : IRepository<Store>
  {
    Task<CustomResult> GetStores();
    Task<CustomResult> CreateStore(CreateStoreModel createStoreModel);
  }
  public class StoreRepository : GenericRepository<Store>, IStoreRepository
  {
    public StoreRepository(DataContext dataContext) : base(dataContext)
    {

    }
    public async Task<CustomResult> GetStores()
    {
      var list = await _context.Stores.ToListAsync();
      if (list.Count == 0)
      {
        return new CustomResult(200, "list empty", null);
      }
      return new CustomResult(200, "list of store", null);
    }
    public async Task<CustomResult> CreateStore(CreateStoreModel createStoreModel)
    {
      if (createStoreModel == null)
      {
        return new CustomResult(400, "Invalid request: store data cannot be null.", null);
      }
      if (string.IsNullOrWhiteSpace(createStoreModel.Address) ||
          string.IsNullOrWhiteSpace(createStoreModel.Code) ||
          string.IsNullOrWhiteSpace(createStoreModel.Phone))
      {
        return new CustomResult(400, "Invalid request: address, code, and phone are required.", null);
      }
      var storeNew = new Store();
      storeNew.Address = createStoreModel.Address;
      storeNew.Code = createStoreModel.Address;
      storeNew.Phone = createStoreModel.Phone;
      storeNew.StoreType = "Branch";
      _context.Stores.Add(storeNew);
      await _context.SaveChangesAsync();

      return new CustomResult(200, "Store created successfully.", null);
    }


  }
}