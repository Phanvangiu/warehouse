using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;

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
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProductModel productModel)
    {
      var customResult = await _unitOfWork.ProductRepository.CreateProduct(productModel);
      return Ok(customResult);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateProductModel updateProductModel)
    {
      var customResult = await _unitOfWork.ProductRepository.UpdateProduct(updateProductModel);
      return Ok(customResult);
    }
  }
}