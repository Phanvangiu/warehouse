using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;

namespace warehouse.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("get-all-categories")]
    public async Task<IActionResult> GetCategories()
    {
      var customResult = await _unitOfWork.CategoryRepository.GetCategoriesAsync();
      return Ok(customResult);
    }
  }
}