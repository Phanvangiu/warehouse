using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;

namespace warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class StoreStockcontroller : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    public StoreStockcontroller(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var customResult = await _unitOfWork.StoreStockRepository.GetStoreStocks();
      return Ok(customResult);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateStoreStockModel model)
    {
      var customerResult = await _unitOfWork.StoreStockRepository.CreateStoreStock(model);
      return Ok(customerResult);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var customerResult = await _unitOfWork.StoreStockRepository.GetStoreStock(id);
      return Ok(customerResult);
    }
  }
}