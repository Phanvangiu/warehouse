using Microsoft.AspNetCore.Mvc;
using warehouse.Data;
using warehouse.Interfaces;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class StoreUserController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    public StoreUserController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var customResult = await _unitOfWork.StoreUserRepository.GetStoreUsers();
      return Ok(customResult);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateStoreUserModel createStoreUserModel)
    {
      if (!ModelState.IsValid)
        return Ok(new CustomResult(400, "Invalid input format.", ModelState));
      var customResult = await _unitOfWork.StoreUserRepository.CreateStoreUser(createStoreUserModel);
      return Ok(customResult);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> StranferStoreUser([FromForm] CreateStoreUserModel createStoreUserModel)
    {
      var customResult = await _unitOfWork.StoreUserRepository.TransferStoreUser(createStoreUserModel);
      return Ok(customResult);
    }
  }
}