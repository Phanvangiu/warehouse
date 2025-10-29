using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IProductRepository : IRepository<Product>
  {
    Task<CustomResult> GetProducts();
    Task<CustomResult> DeleteProduct(int Id);
  }


  public class ProductRepository : GenericRepository<Product>, IProductRepository
  {
    public ProductRepository(DataContext dataContext) : base(dataContext)
    {
      _context = dataContext;
    }
    public async Task<CustomResult> GetProducts()
    {
      var products = await _context.Products.ToListAsync();
      if (products.Count == 0)
      {
        return new CustomResult(200, "list empty", new List<Product>());
      }
      return new CustomResult(200, "list of products", products);
    }
    public async Task<CustomResult> DeleteProduct(int id)
    {
      try
      {
        var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
          return new CustomResult(404, "not found", null);
        }
        product.IsActive = false;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return new CustomResult(200, "success", product);
      }
      catch (System.Exception)
      {

        throw;
      }
    }
  }

}