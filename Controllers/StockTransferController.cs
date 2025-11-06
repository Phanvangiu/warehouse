using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;

namespace warehouse.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class StockTransferController : ControllerBase
  {
    private IUnitOfWork _unitOfwork;
    public StockTransferController(UnitOfWork unitOfWork)
    {
      _unitOfwork = unitOfWork;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateStockTransferModel model)
    {
      var customResult = await _unitOfwork.StockTransferRepository.CreateStockTransfer(model);
      return Ok(customResult);
    }
  }
}