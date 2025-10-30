using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IStoreRepository : IRepository<Store>
  {
    Task<CustomResult> GetStores();
  }
  public class StoreRepository : GenericRepository<Store>, IStoreRepository
  {
    public StoreRepository(DataContext dataContext) : base(dataContext)
    {

    }
    public async Task<CustomResult> GetStores()
    {
      var list = await _context.Stores.ToListAsync();
      if (list.Count() == 0)
      {
        return new CustomResult(200, "list empty", null);
      }
      return new CustomResult(200, "list of store", null);
    }
  }
}