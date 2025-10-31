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
    Task<CustomResult> CreateCategory(CategoryModel category);
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
      try
      {
        var categories = await _context.Categories.ToListAsync();
        if (categories.Count == 0)
        {
          return new CustomResult(200, "list empty", categories);

        }
        return new CustomResult(200, "Categories retrieved successfully", categories);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving categories: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> GetCategory(int categoryId)
    {
      try
      {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category is null)
        {
          return new CustomResult(404, "Category not found.", null!);
        }
        return new CustomResult(200, "Category retrieved successfully.", category);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving category: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> CreateCategory(CategoryModel categoryModel)
    {
      try
      {
        if (categoryModel is null)
        {
          return new CustomResult(400, "Invalid request: category data cannot be null.", null!);
        }
        if (string.IsNullOrWhiteSpace(categoryModel.Name))
        {
          return new CustomResult(400, "Category name is required", null!);
        }
        var categoryNew = new Category
        {
          Descripton = categoryModel.Descripton,
          Name = categoryModel.Name,
        };
        if (categoryModel.Image is null || categoryModel.Image.Length <= 0)
        {
          return new CustomResult(400, "Image category is required", null!);
        }
        else
        {
          var folderName = "categories";
          var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(categoryModel.Image.FileName)}";

          var uploadPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory()), "images", folderName);

          if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

          var filePath = Path.Combine(uploadPath, fileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await categoryModel.Image.CopyToAsync(stream);
          }

          var request = _httpContextAccessor.HttpContext?.Request;
          var imageUrl = request != null
              ? $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}"
              : $"/images/{folderName}/{fileName}";
          categoryNew.Image = imageUrl;
        }

        _context.Categories.Add(categoryNew);
        await _context.SaveChangesAsync();
        return new CustomResult(200, "Category created successfully", categoryNew);

      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while creating category: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> UpdateCategory(CategoryModel categoryModel)
    {
      try
      {
        var categoryOld = await _context.Categories.FindAsync(categoryModel.Id);
        if (categoryOld == null)
          return new CustomResult(404, "Category not found", null!);

        categoryOld.Name = categoryModel.Name;
        categoryOld.Descripton = categoryModel.Descripton;
        categoryOld.UpdatedAt = DateTime.Now;

        // Nếu có ảnh mới -> upload
        if (categoryModel.Image != null && categoryModel.Image.Length > 0)
        {
          var folderName = "categories";
          var fileName = $"{DateTime.Now.Ticks}_{Path.GetFileName(categoryModel.Image.FileName)}";

          var uploadPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images", folderName);

          if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

          var filePath = Path.Combine(uploadPath, fileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await categoryModel.Image.CopyToAsync(stream);
          }

          var request = _httpContextAccessor.HttpContext?.Request;
          var imageUrl = request != null
              ? $"{request.Scheme}://{request.Host}/images/{folderName}/{fileName}"
              : $"/images/{folderName}/{fileName}";

          categoryOld.Image = imageUrl;
        }

        _context.Categories.Update(categoryOld);
        await _context.SaveChangesAsync();

        return new CustomResult(200, "Category updated successfully", categoryOld);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An unexpected error occurred while updating the category: {ex.Message}", null!);
      }
    }

  }
}
