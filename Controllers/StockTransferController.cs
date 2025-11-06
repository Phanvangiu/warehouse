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
    public StockTransferController(IUnitOfWork unitOfWork)
    {
      _unitOfwork = unitOfWork;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateStockTransferModel model)
    {
      var customResult = await _unitOfwork.StockTransferRepository.CreateStockTransfer(model);
      return Ok(customResult);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var customResult = await _unitOfwork.StockTransferRepository.GetStockTransfers();
      return Ok(customResult);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var customResult = await _unitOfwork.StockTransferRepository.GetStockTransfer(id);
      return Ok(customResult);
    }
    [HttpPut]
    [Route("cancel/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
      var customResult = await _unitOfwork.StockTransferRepository.Cancel(id);
      return Ok(customResult);
    }

  }
}