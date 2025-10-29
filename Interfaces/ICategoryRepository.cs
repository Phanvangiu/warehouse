using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface ICategoryRepository : IRepository<Category>
  {
    Task<CustomResult> GetCategoriesAsync();
    Task<Category?> GetCategoryAsync(int categoryId);
    void CreateCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(Category category);
  }

  public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
  {
    private readonly DataContext _context;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(DataContext context, ILogger<CategoryRepository> logger) : base(context)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<CustomResult> GetCategoriesAsync()
    {
      var categories = await _context.Categories.ToListAsync();
      return new CustomResult(200, "list of categories", categories);
    }

    public async Task<Category?> GetCategoryAsync(int categoryId)
    {
      return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public void CreateCategory(Category category)
    {
      Add(category);
    }

    public void UpdateCategory(Category category)
    {
      Update(category);
    }

    public void DeleteCategory(Category category)
    {
      Delete(category);
    }
  }
}
