using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;

namespace warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class StoreController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    public StoreController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var customResult = await _unitOfWork.StoreRepository.GetStores();
      return Ok(customResult);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateStoreModel createStoreModel)
    {
      var customResult = await _unitOfWork.StoreRepository.CreateStore(createStoreModel);
      return Ok(customResult);
    }
  }
}