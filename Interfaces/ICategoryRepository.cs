using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface ICategoryRepository : IRepository<Category>
  {
    Task<CustomResult> GetCategories();
    Task<CustomResult> GetCategory(int categoryId);
    Task<CustomResult> CreateCategory(Category category);
    Task<CustomResult> UpdateCategory(CategoryModel category);

  }

  public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
  {
    private readonly ILogger<CategoryRepository> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CategoryRepository(DataContext dataContext, ILogger<CategoryRepository> logger, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(dataContext)
    {
      _context = dataContext;
      _logger = logger;
      _env = env;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CustomResult> GetCategories()
    {
      var categories = await _context.Categories.ToListAsync();
      return new CustomResult(200, "list of categories", categories);
    }

    public async Task<CustomResult> GetCategory(int categoryId)
    {
      var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
      if (category == null)
      {
        return new CustomResult(404, "not found", null);
      }
      return new CustomResult(200, "category", category);

    }
    public async Task<CustomResult> CreateCategory(Category category)
    {
      try
      {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return new CustomResult(200, "create success", category);

      }
      catch (Exception ex)
      {
        return new CustomResult(400, "failed", ex.Message);
      }
    }
    public async Task<CustomResult> UpdateCategory(CategoryModel category)
    {
      try
      {
        var categoryOld = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        if (categoryOld == null)
          return new CustomResult(404, "Category not found", null);

        categoryOld.Name = category.Name;
        categoryOld.Descripton = category.Descripton;
        categoryOld.UpdatedAt = DateTime.Now;

        // Nếu có ảnh mới -> upload
        if (category.Image != null && category.Image.Length > 0)
        {
          var folderName = "categories";
          var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(category.Image.FileName)}";

          // Đường dẫn thư mục gốc lưu ảnh
          var uploadPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images", folderName);

          // Tạo folder nếu chưa có
          if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

          // Đường dẫn đầy đủ tới file
          var filePath = Path.Combine(uploadPath, fileName);

          // Ghi file vào ổ đĩa
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await category.Image.CopyToAsync(stream);
          }

          // Tạo URL ảnh public
          string imageUrl;
          var request = _httpContextAccessor.HttpContext?.Request;
          if (request != null)
            imageUrl = $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}";
          else
            imageUrl = $"/images/{folderName}/{fileName}";

          categoryOld.Image = imageUrl;
        }

        _context.Categories.Update(categoryOld);
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Update success", categoryOld);
      }
      catch (Exception ex)
      {
        return new CustomResult(400, "Failed", ex.Message);
      }
    }

  }
}
