using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.Models;
using warehouse.RequestModels;
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
    public async Task<IActionResult> GetAll()
    {
      var customResult = await _unitOfWork.CategoryRepository.GetCategoriesAsync();
      return Ok(customResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var customResult = await _unitOfWork.CategoryRepository.GetCategory(id);
      return Ok(customResult);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] Category category)
    {
      var customResult = await _unitOfWork.CategoryRepository.CreateCategory(category);
      return Ok(customResult);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromForm] CategoryModel category)
    {
      var result = await _unitOfWork.CategoryRepository.UpdateCategory(category);
      return Ok(result);
    }
  }
}