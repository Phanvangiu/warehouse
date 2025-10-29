using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;

namespace warehouse.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var customResult = await _unitOfWork.ProductRepository.GetProducts();
      return Ok(customResult);
    }
  }
}