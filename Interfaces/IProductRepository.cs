using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IProductRepository : IRepository<Product>
  {
    Task<CustomResult> GetProducts();
    Task<CustomResult> DeleteProduct(int Id);
    Task<CustomResult> CreateProduct(CreateProductModel productCreateModel);
    Task<CustomResult> UpdateProduct(UpdateProductModel productUpdateModel);
  }


  public class ProductRepository : GenericRepository<Product>, IProductRepository
  {
    private IWebHostEnvironment _env;
    private IHttpContextAccessor _httpContextAccessor;
    public ProductRepository(DataContext dataContext, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(dataContext)
    {
      _context = dataContext;
      _env = env;
      _httpContextAccessor = httpContextAccessor;
    }
    public async Task<CustomResult> GetProducts()
    {
      var products = await _context.Products
          .Select(p => new
          {
            p.Id,
            p.ProductName,
            p.CategoryId,
            p.Descripton,
            p.DefaultPrice,
            p.Unit,
            p.IsActive,
            p.CreatedAt,
            p.UpdatedAt,
            ProductImages = p.ProductImages.Select(img => img.Image).ToList()
          }).ToListAsync();

      if (products.Count == 0)
      {
        return new CustomResult(200, "List empty", new List<Product>());
      }

      return new CustomResult(200, "Products retrieved successfully", products);
    }

    public async Task<CustomResult> DeleteProduct(int id)
    {
      try
      {
        var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
          return new CustomResult(404, "Product not found", null);
        }
        product.IsActive = false;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return new CustomResult(200, "Product deleted successfully", product);
      }
      catch (System.Exception)
      {
        throw;
      }
    }
    public async Task<CustomResult> CreateProduct(CreateProductModel productCreateModel)
    {
      if (productCreateModel == null)
      {
        return new CustomResult(400, "Invalid request: product data cannot be null.", null);
      }
      if (string.IsNullOrWhiteSpace(productCreateModel.ProductName) ||
       string.IsNullOrWhiteSpace(productCreateModel.Descripton) ||
       string.IsNullOrWhiteSpace(productCreateModel.Unit) ||
       productCreateModel.DefaultPrice <= 0
       )
      {
        return new CustomResult(400, "Invalid request: productName, Code, Unit and DefaultPrice are required and must be valid.", null);
      }
      var productNew = new Product
      {
        ProductName = productCreateModel.ProductName,
        Descripton = productCreateModel.Descripton,
        DefaultPrice = productCreateModel.DefaultPrice,
        CategoryId = productCreateModel.CategoryId,
        Unit = productCreateModel.Unit,
        IsActive = productCreateModel.IsActive,
      };

      _context.Products.Add(productNew);
      await _context.SaveChangesAsync();

      if (productCreateModel.Images != null && productCreateModel.Images.Count > 0)
      {
        var folderName = "products";
        var uploadPath = Path.Combine(
            _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            "images", folderName
        );

        Directory.CreateDirectory(uploadPath);

        var request = _httpContextAccessor.HttpContext?.Request;
        foreach (var item in productCreateModel.Images)
        {
          var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(item.FileName)}";
          var filePath = Path.Combine(uploadPath, fileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await item.CopyToAsync(stream);
          }

          string imageUrl = request != null
              ? $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}"
              : $"images/{folderName}/{fileName}";

          var productImage = new ProductImage
          {
            Image = imageUrl,
            ProductId = productNew.Id
          };

          _context.ProductImages.Add(productImage);
        }

        await _context.SaveChangesAsync();
      }

      productNew.ProductImages = await _context.ProductImages
        .Where(img => img.ProductId == productNew.Id)
        .ToListAsync();

      return new CustomResult(200, "Product created successfully", productNew);
    }

    public async Task<CustomResult> UpdateProduct(UpdateProductModel productUpdateModel)
    {
      var productOld = await _context.Products.SingleOrDefaultAsync(p => p.Id == productUpdateModel.Id);
      if (productOld == null)
      {
        return new CustomResult(200, "Product not found", null);
      }
      productOld.DefaultPrice = productUpdateModel.DefaultPrice;
      productOld.CategoryId = productUpdateModel.CategoryId;
      productOld.Descripton = productUpdateModel.Descripton;
      productOld.ProductName = productUpdateModel.ProductName;

      var folderName = "products";
      var uploadPath = Path.Combine(
          _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
          "images", folderName
      );
      var request = _httpContextAccessor.HttpContext?.Request;

      if (productUpdateModel.Images != null && productUpdateModel.Images.Count() > 0)
      {
        foreach (var item in productUpdateModel.Images)
        {
          var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(item.FileName)}";
          var filePath = Path.Combine(uploadPath, fileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await item.CopyToAsync(stream);
          }

          string imageUrl = request != null
              ? $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}"
              : $"images/{folderName}/{fileName}";

          var productImage = new ProductImage
          {
            Image = imageUrl,
            ProductId = productOld.Id
          };
          _context.ProductImages.Add(productImage);
        }
      }
      return new CustomResult(200, "Product updated successfully", null);
    }

  }

}