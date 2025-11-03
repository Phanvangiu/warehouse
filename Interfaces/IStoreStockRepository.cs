using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IStoreStockRepository : IRepository<StoreStock>
  {
    Task<CustomResult> GetStoreStocks();
    Task<CustomResult> CreateStoreStock(CreateStoreStockModel model);
    Task<CustomResult> GetStoreStock(int id);
  }
  public class StoreStockRepository : GenericRepository<StoreStock>, IStoreStockRepository
  {
    public StoreStockRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public async Task<CustomResult> GetStoreStocks()
    {
      try
      {
        var list = await _context.StoreStocks.ToListAsync();
        if (list.Count == 0)
        {
          return new CustomResult(200, "List empty", null!);
        }
        return new CustomResult(200, "Store stocks retrieved successfully", null!);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving store stocks: {ex.Message}", null!);
      }
    }
    private async Task<(Product? user, Store? store, CustomResult? error)> ValidateStoreStockInput(CreateStoreStockModel model)
    {
      if (model is null)
        return (null, null, new CustomResult(400, "Invalid request: store stock data cannot be null.", null!));

      if (model.Quantity <= 0 || model.StoreId <= 0 || model.ProductId <= 0 || model.CostPrice <= 0)
        return (null, null, new CustomResult(400, "Invalid request: Quantity, ProductId, StoreId, and CostPrice must be greater than 0.", null!));

      if (model.ExpiryDate <= model.ManufactureDate)
        return (null, null, new CustomResult(400, "Invalid request: ExpiryDate must be greater than ManufactureDate.", null!));

      var product = await _context.Products.FindAsync(model.ProductId);
      if (product is null)
        return (null, null, new CustomResult(404, "Product not found", null!));

      var store = await _context.Stores.FindAsync(model.StoreId);
      if (store is null)
        return (null, null, new CustomResult(404, "Store not found", null!));

      return (product, store, null);
    }
    public async Task<CustomResult> CreateStoreStock(CreateStoreStockModel model)
    {
      try
      {
        var (product, store, error) = await ValidateStoreStockInput(model);
        if (error != null) return error;
        var storeStock = new StoreStock
        {
          ProductId = product!.Id,
          StoreId = store!.Id,
          Quantity = model.Quantity,
          ManufactureDate = model.ManufactureDate,
          ExpiryDate = model.ExpiryDate,
          BatchCode = model.BatchCode,
          CostPrice = model.CostPrice
        };
        _context.StoreStocks.Add(storeStock);
        await _context.SaveChangesAsync();
        return new CustomResult(200, "Store stock created successfully.", null!);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while creating store stock: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> GetStoreStock(int id)
    {
      try
      {
        if (id <= 0)
        {
          return new CustomResult(400, "Invalid request: Id must be greater than 0", null!);
        }
        var storeStock = await _context.StoreStocks.FindAsync(id);
        if (storeStock is null)
        {
          return new CustomResult(200, "Store stock not found", null!);
        }
        return new CustomResult(200, "Store stock retieved successfully", storeStock);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving store stock: {ex.Message}", null!);
      }
    }

  }
}