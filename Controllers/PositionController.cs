using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PositionController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    public PositionController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var customResult = await _unitOfWork.PositionRepository.GetPositions();
      return Ok(customResult);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPosition(int id)
    {
      if (id <= 0)
      {
        return Ok(new CustomResult(400, "Invalid ID. ID must be greater than 0.", null!));
      }
      var customResult = await _unitOfWork.PositionRepository.GetPosition(id);
      return Ok(customResult);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePositionModel createPositionModel)
    {
      var customResult = await _unitOfWork.PositionRepository.CreatePosition(createPositionModel);
      return Ok(customResult);
    }
  }
}