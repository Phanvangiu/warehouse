using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;

namespace warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrderController(IUnitOfWork unitOfWork) : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    // [HttpPost]
    // public async Task<IActionResult> CreateOrder(CreateOrderModel model)
    // {
    //   var customResult = await _unitOfWork.OrderRepository.CreateOrder(model);
    //   return Ok(customResult);
    // }

    // [HttpGet]
    // public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] decimal min, [FromQuery] decimal max)
    // {
    //   var customResult = await _unitOfWork.OrderRepository.GetAll(pageNumber, pageSize, min, max);
    //   return Ok(customResult);
    // }
  }
}