using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IStockTransferRepository : IRepository<StockTransfer>
  {
    Task<CustomResult> CreateStockTransfer(CreateStockTransferModel model);
    Task<CustomResult> GetStockTransfers();
    Task<CustomResult> Cancel(int id);
    Task<CustomResult> GetStockTransfer(int id);
  }
  public class StockTransferRepository(DataContext dataContext) : GenericRepository<StockTransfer>(dataContext), IStockTransferRepository
  {
    private async Task<(Store? fromStore, Store? toStore, Product? product, CustomResult? error)> ValidateStockTransferModel(CreateStockTransferModel model)
    {
      if (model is null)
        return (null, null, null, new CustomResult(400, "Invalid request: store stock tranfer data cannot be null.", null!));

      if (model.FromStoreId <= 0 || model.ToStoreId <= 0 || model.ProductId <= 0 || model.Quantity <= 0)
        return (null, null, null, new CustomResult(400, "Invalid request: FromStoreId ,ToStoreId, ProductId and Quantity must be greater than 0!", null!));

      if (string.IsNullOrWhiteSpace(model.BatchCode) || string.IsNullOrWhiteSpace(model.StranferType))
        return (null, null, null, new CustomResult(400, "Invalid request: ", null!));

      var fromStore = await _context.Stores.FindAsync(model.FromStoreId);
      if (fromStore is null)
        return (null, null, null, new CustomResult(404, "From store not found!", null!));

      var toStore = await _context.Stores.FindAsync(model.ToStoreId);
      if (toStore is null)
        return (null, null, null, new CustomResult(404, "To store not found!", null!));

      var product = await _context.Products.FindAsync(model.ProductId);
      if (product is null)
        return (null, null, null, new CustomResult(404, "Product not found", null!));
      return (fromStore, toStore, product, null);
    }
    public async Task<CustomResult> CreateStockTransfer(CreateStockTransferModel model)
    {
      try
      {
        var (fromStore, toStore, product, error) = await ValidateStockTransferModel(model);
        if (error != null)
          return error;
        var stockTransfer = new StockTransfer
        {
          FromStoreId = fromStore!.Id,
          ToStoreId = toStore!.Id,
          ProductId = product!.Id,
          BatchCode = model.BatchCode,
          Quantity = model.Quantity,
          StranferType = model.StranferType,
          Status = "pendding",
          Note = model.Note
        };
        _context.StockTransfers.Add(stockTransfer);
        await _context.SaveChangesAsync();
        return new CustomResult(200, "Stock transfer created successfully! ", null!);
      }
      catch (Exception Ex)
      {
        return new CustomResult(500, $"An error occurred while transfering products: {Ex.Message}", null!);
      }
    }
    public async Task<CustomResult> GetStockTransfers()
    {
      var list = await _context.StockTransfers.ToListAsync();
      return new CustomResult(200, list.Count > 0 ? "Stock tranfers retrieved successfully" : "List empty", list);
    }

    public async Task<CustomResult> Cancel(int id)
    {
      if (id <= 0)
        return new CustomResult(400, "Invalid request: Id must be greater than 0!", null!);

      var stockTranfer = await _context.StockTransfers.FindAsync(id);
      if (stockTranfer is null)
        return new CustomResult(404, "Stock tranfer not found!", null!);

      if (stockTranfer.Status == "completed")
        return new CustomResult(400, "Stock tranfer was completed!", null!);

      stockTranfer.Status = "canceled";
      await _context.SaveChangesAsync();

      return new CustomResult(200, "Stock transfer was canceled successfully!", stockTranfer);
    }
    public async Task<CustomResult> GetStockTransfer(int id)
    {
      if (id <= 0)
        return new CustomResult(400, "Invalid request: Id must be greater than 0!", null!);

      var stockTranfer = await _context.StockTransfers.FindAsync(id);
      if (stockTranfer is null)
        return new CustomResult(404, "Stock tranfer not found!", null!);

      return new CustomResult(200, "Stock transfer was canceled successfully!", stockTranfer);
    }
  }
}
